// Copyright 2010-2011 Miguel de Icaza
//
// TODO:
//   Make the LRUcache also track image sizes, and limit the size of the cache that way
//
// Based on the TweetStation specific ImageStore
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.CoreGraphics;
using MonoTouch.Dialog.Utilities;
using System.Security.Cryptography;

namespace MonoTouch.Dialog.Utilities 
{
	public interface IImageUpdated {
		void UpdatedImage (Uri uri);
	}
	
	//
	// Provides an interface to download pictures in the background
	// and keep a local cache of the original files + rounded versions
	// 

	public static class ImageLoader
	{
        public readonly static string BaseDir = Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.Personal), "..");
		const int MaxRequests = 6;
		static string PicDir; 
		
		// Cache of recently used images
		static LRUCache<Uri,UIImage> cache;
		
		// A list of requests that have been issues, with a list of objects to notify.
		static Dictionary<Uri, List<IImageUpdated>> pendingRequests;
		
		// A list of updates that have completed, we must notify the main thread about them.
		static HashSet<Uri> queuedUpdates;
		
		// A queue used to avoid flooding the network stack with HTTP requests
		static Stack<Uri> requestQueue;
		
		static NSString nsDispatcher = new NSString ("x");
		
		//static MD5CryptoServiceProvider checksum = new MD5CryptoServiceProvider ();
		
		static ImageLoader ()
		{
			PicDir = Path.Combine (BaseDir, "Library/Caches/Pictures.MonoTouch.Dialog/");
			
			if (!Directory.Exists (PicDir))
				Directory.CreateDirectory (PicDir);
			
			cache = new LRUCache<Uri,UIImage> (50);
			pendingRequests = new Dictionary<Uri,List<IImageUpdated>> ();
			queuedUpdates = new HashSet<Uri>();
			requestQueue = new Stack<Uri> ();
		}
		
		public static void Purge ()
		{
			cache.Purge ();
		}

		static int hex (int v)
		{
			if (v < 10)
				return '0' + v;
			return 'a' + v-10;
		}

		static string md5 (string input)
		{
			var checksum  = new MD5CryptoServiceProvider ();
			var bytes = checksum.ComputeHash (Encoding.UTF8.GetBytes (input));
			var ret = new char [32];
			for (int i = 0; i < 16; i++){
				ret [i*2] = (char)hex (bytes [i] >> 4);
				ret [i*2+1] = (char)hex (bytes [i] & 0xf);
			}
			return new string (ret);
		}
		
		public static UIImage RequestImage (Uri uri, IImageUpdated notify)
		{
			UIImage ret;
			
			lock (cache){
				ret = cache [uri];
				if (ret != null)
					return ret;
			}

			lock (requestQueue){
				if (pendingRequests.ContainsKey (uri))
					return null;
			}

			string picfile = PicDir + md5 (uri.AbsoluteUri);
			if (File.Exists (picfile)){
				ret = UIImage.FromFileUncached (picfile);
				if (ret != null){
					lock (cache)
						cache [uri] = ret;
					return ret;
				}
			} 
			QueueRequest (uri, picfile, notify);
			return null;
		}
		
		static void QueueRequest (Uri uri, string target, IImageUpdated notify)
		{
			if (notify == null)
				throw new ArgumentNullException ("notify");
			
			lock (requestQueue){
				if (pendingRequests.ContainsKey (uri)){
					//Util.Log ("pendingRequest: added new listener for {0}", id);
					pendingRequests [uri].Add (notify);
					return;
				}
				var slot = new List<IImageUpdated> (4);
				slot.Add (notify);
				pendingRequests [uri] = slot;
				
				if (requestQueue.Count >= MaxRequests)
					requestQueue.Push (uri);
				else {
					ThreadPool.QueueUserWorkItem (delegate { 
							try {
								StartPicDownload (uri, target); 
							} catch (Exception e){
								Console.WriteLine (e);
							}
						});
				}
			}
		}
		
		static bool Download (Uri uri, string target)
		{
			var buffer = new byte [4*1024];
			
			try {
				var tmpfile = target + ".tmp";
				using (var file = new FileStream (tmpfile, FileMode.Create, FileAccess.Write, FileShare.Read)) {
	                	var req = WebRequest.Create (uri) as HttpWebRequest;
					
	                using (var resp = req.GetResponse()) {
						using (var s = resp.GetResponseStream()) {
							int n;
							while ((n = s.Read (buffer, 0, buffer.Length)) > 0){
								file.Write (buffer, 0, n);
	                        }
						}
	                }
				}
				File.Move (tmpfile, target);
				return true;
			} catch (Exception e) {
				Console.WriteLine ("Problem with {0} {1}", uri, e);
				return false;
			}
		}
		
		static long picDownloaders;
		
		static void StartPicDownload (Uri uri, string target)
		{
			Interlocked.Increment (ref picDownloaders);
			try {
				_StartPicDownload (uri, target);
			} catch (Exception e){
				Console.Error.WriteLine ("CRITICAL: should have never happened {0}", e);
			}
			//Util.Log ("Leaving StartPicDownload {0}", picDownloaders);
			Interlocked.Decrement (ref picDownloaders);
		}
		
		static void _StartPicDownload (Uri uri, string target)
		{
			do {
				bool downloaded = false;
				
				System.Threading.Thread.Sleep (5000);
				downloaded = Download (uri, target);
				if (!downloaded)
					Console.WriteLine ("Error fetching picture for {0} to {1}", uri, target);
				
				// Cluster all updates together
				bool doInvoke = false;
				
				lock (requestQueue){
					if (downloaded){
						queuedUpdates.Add (uri);
					
						// If this is the first queued update, must notify
						if (queuedUpdates.Count == 1)
							doInvoke = true;
					} else
						pendingRequests.Remove (uri);

					// Try to get more jobs.
					if (requestQueue.Count > 0){
						uri = requestQueue.Pop ();
						if (uri == null){
							Console.Error.WriteLine ("Dropping request {0} because url is null", uri);
							pendingRequests.Remove (uri);
							uri = null;
						}
					} else {
						//Util.Log ("Leaving because requestQueue.Count = {0} NOTE: {1}", requestQueue.Count, pendingRequests.Count);
						uri = null;
					}
				}	
				if (doInvoke)
					nsDispatcher.BeginInvokeOnMainThread (NotifyImageListeners);
				
			} while (uri != null);
		}
		
		// Runs on the main thread
		static void NotifyImageListeners ()
		{
			lock (requestQueue){
				foreach (var quri in queuedUpdates){
					var list = pendingRequests [quri];
					pendingRequests.Remove (quri);
					foreach (var pr in list){
						try {
							pr.UpdatedImage (quri);
						} catch (Exception e){
							Console.WriteLine (e);
						}
					}
				}
				queuedUpdates.Clear ();
			}
		}
	}
}
