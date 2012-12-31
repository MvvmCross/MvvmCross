#region Copyright

// <copyright file="Forwarding.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
//  
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com

#endregion

using System.Runtime.CompilerServices;

/*
 * not available in MonoTouch and/or MonoDroid :/
[assembly: TypeForwardedTo(typeof(System.Net.Sockets.UdpAnySourceMulticastClient))]
[assembly: TypeForwardedTo(typeof(System.Net.Sockets.UdpSingleSourceMulticastClient))]
[assembly: TypeForwardedTo(typeof(System.Net.DnsEndPoint))]
[assembly: TypeForwardedTo(typeof(System.Net.WriteStreamClosedEventHandler))]
[assembly: TypeForwardedTo(typeof(System.Net.WriteStreamClosedEventArgs))]
*/

[assembly: TypeForwardedTo(typeof (System.Net.Cookie))]
[assembly: TypeForwardedTo(typeof (System.Net.CookieCollection))]
[assembly: TypeForwardedTo(typeof (System.Net.CookieContainer))]
[assembly: TypeForwardedTo(typeof (System.Net.CookieException))]
[assembly: TypeForwardedTo(typeof (System.Net.Dns))]
[assembly: TypeForwardedTo(typeof (System.Net.WebProxy))]
[assembly: TypeForwardedTo(typeof (System.Net.HttpRequestHeader))]
[assembly: TypeForwardedTo(typeof (System.Net.HttpStatusCode))]
[assembly: TypeForwardedTo(typeof (System.Net.HttpVersion))]
[assembly: TypeForwardedTo(typeof (System.Net.DecompressionMethods))]
[assembly: TypeForwardedTo(typeof (System.Net.WebRequest))]
[assembly: TypeForwardedTo(typeof (System.Net.HttpWebRequest))]
[assembly: TypeForwardedTo(typeof (System.Net.WebResponse))]
[assembly: TypeForwardedTo(typeof (System.Net.HttpWebResponse))]
[assembly: TypeForwardedTo(typeof (System.Net.ICertificatePolicy))]
[assembly: TypeForwardedTo(typeof (System.Net.ICredentials))]
[assembly: TypeForwardedTo(typeof (System.Net.HttpContinueDelegate))]
[assembly: TypeForwardedTo(typeof (System.Net.IPAddress))]
[assembly: TypeForwardedTo(typeof (System.Net.IPEndPoint))]
[assembly: TypeForwardedTo(typeof (System.Net.IPHostEntry))]
[assembly: TypeForwardedTo(typeof (System.Net.IWebRequestCreate))]
[assembly: TypeForwardedTo(typeof (System.Net.NetworkCredential))]
[assembly: TypeForwardedTo(typeof (System.Net.ProtocolViolationException))]
[assembly: TypeForwardedTo(typeof (System.Net.ServicePoint))]
[assembly: TypeForwardedTo(typeof (System.Net.ServicePointManager))]
[assembly: TypeForwardedTo(typeof (System.Net.WebHeaderCollection))]
[assembly: TypeForwardedTo(typeof (System.Net.SocketAddress))]
[assembly: TypeForwardedTo(typeof (System.Net.Sockets.SocketException))]
[assembly: TypeForwardedTo(typeof (System.Net.WebClient))]
[assembly: TypeForwardedTo(typeof (System.Net.OpenReadCompletedEventHandler))]
[assembly: TypeForwardedTo(typeof (System.Net.OpenReadCompletedEventArgs))]
[assembly: TypeForwardedTo(typeof (System.Net.OpenWriteCompletedEventHandler))]
[assembly: TypeForwardedTo(typeof (System.Net.OpenWriteCompletedEventArgs))]
[assembly: TypeForwardedTo(typeof (System.Net.DownloadStringCompletedEventHandler))]
[assembly: TypeForwardedTo(typeof (System.Net.DownloadStringCompletedEventArgs))]
[assembly: TypeForwardedTo(typeof (System.Net.UploadStringCompletedEventHandler))]
[assembly: TypeForwardedTo(typeof (System.Net.UploadStringCompletedEventArgs))]
[assembly: TypeForwardedTo(typeof (System.Net.DownloadProgressChangedEventHandler))]
[assembly: TypeForwardedTo(typeof (System.Net.DownloadProgressChangedEventArgs))]
[assembly: TypeForwardedTo(typeof (System.Net.UploadProgressChangedEventHandler))]
[assembly: TypeForwardedTo(typeof (System.Net.UploadProgressChangedEventArgs))]
[assembly: TypeForwardedTo(typeof (System.Net.WebException))]
[assembly: TypeForwardedTo(typeof (System.Net.WebExceptionStatus))]
[assembly: TypeForwardedTo(typeof (System.Net.NetworkInformation.NetworkAddressChangedEventHandler))]
[assembly: TypeForwardedTo(typeof (System.Net.NetworkInformation.NetworkChange))]
[assembly: TypeForwardedTo(typeof (System.Net.NetworkInformation.NetworkInterface))]
[assembly: TypeForwardedTo(typeof (System.Net.Sockets.AddressFamily))]
[assembly: TypeForwardedTo(typeof (System.Net.Sockets.LingerOption))]
[assembly: TypeForwardedTo(typeof (System.Net.Sockets.MulticastOption))]
[assembly: TypeForwardedTo(typeof (System.Net.Sockets.IPv6MulticastOption))]
[assembly: TypeForwardedTo(typeof (System.Net.Sockets.Socket))]
[assembly: TypeForwardedTo(typeof (System.Net.Sockets.ProtocolType))]
[assembly: TypeForwardedTo(typeof (System.Net.Sockets.SelectMode))]
[assembly: TypeForwardedTo(typeof (System.Net.Sockets.SocketAsyncOperation))]
[assembly: TypeForwardedTo(typeof (System.Net.Sockets.SocketAsyncEventArgs))]
[assembly: TypeForwardedTo(typeof (System.Net.Sockets.SocketError))]
[assembly: TypeForwardedTo(typeof (System.Net.Sockets.SocketFlags))]
[assembly: TypeForwardedTo(typeof (System.Net.Sockets.SocketOptionLevel))]
[assembly: TypeForwardedTo(typeof (System.Net.Sockets.SocketOptionName))]
[assembly: TypeForwardedTo(typeof (System.Net.Sockets.SocketShutdown))]
[assembly: TypeForwardedTo(typeof (System.Net.Sockets.SocketType))]