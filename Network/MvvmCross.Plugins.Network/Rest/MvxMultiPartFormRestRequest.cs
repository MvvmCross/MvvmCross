// MvxMultiPartFormRestRequest.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using MvvmCross.Platform;
using MvvmCross.Platform.Exceptions;
using MvvmCross.Plugins.File;
using System;
using System.Collections.Generic;
using System.IO;

namespace MvvmCross.Plugins.Network.Rest
{
    public class MvxMultiPartFormRestRequest
        : MvxTextBasedRestRequest
    {
        protected virtual string GenerateBoundary()
        {
            return "---------------------------" + DateTime.UtcNow.Ticks.ToString("x");
        }

        protected virtual string GenerateContentType(string boundary)
        {
            return MvxContentType.MultipartFormWithBoundary + boundary;
        }

        public MvxMultiPartFormRestRequest(string url, string verb = MvxVerbs.Post, string accept = MvxContentType.Json,
                                           string tag = null)
            : base(url, verb, accept, tag)
        {
            Initialize();
        }

        public MvxMultiPartFormRestRequest(Uri uri, string verb = MvxVerbs.Post, string accept = MvxContentType.Json,
                                           string tag = null)
            : base(uri, verb, accept, tag)
        {
            Initialize();
        }

        private void Initialize()
        {
            SetBoundary(GenerateBoundary());
            StreamsToSend = new List<IStreamForUpload>();
            FieldsToSend = new Dictionary<string, string>();

            // Note: by default we suppress WindowsPhone compression on these uploads
            // - as they are quite often image, audio or video files
            Options[MvxKnownOptions.ForceWindowsPhoneToUseCompression] = false;
        }

        protected virtual void SetBoundary(string boundary)
        {
            Boundary = boundary;
            ContentType = GenerateContentType(boundary);
        }

        protected string Boundary { get; private set; }

        public interface IStreamForUpload
        {
            string FieldName { get; }
            string FileName { get; }
            string ContentType { get; }

            void WriteTo(Stream stream);
        }

        public abstract class StreamForUpload
            : IStreamForUpload
        {
            protected StreamForUpload(string fieldName, string fileName, string contentType)
            {
                ContentType = contentType;
                FileName = fileName;
                FieldName = fieldName;
            }

            public string FieldName { get; private set; }
            public string FileName { get; private set; }
            public string ContentType { get; private set; }

            public abstract void WriteTo(Stream stream);
        }

        public class MemoryStreamForUpload
            : StreamForUpload
        {
            public MemoryStreamForUpload(string fieldName, string fileName, string contentType, byte[] bytes)
                : this(fieldName, fileName, contentType, new MemoryStream(bytes))
            {
            }

            public MemoryStreamForUpload(string fieldName, string fileName, string contentType,
                                         MemoryStream memoryStream)
                : base(fieldName, fileName, contentType)
            {
                MemoryStream = memoryStream;
            }

            public MemoryStream MemoryStream { get; set; }

            public override void WriteTo(Stream stream)
            {
                MemoryStream.CopyTo(stream);
                stream.Flush();
            }
        }

        public class FileStreamForUpload
            : StreamForUpload
        {
            public FileStreamForUpload(string fieldName, string fileName, string contentType, string path)
                : base(fieldName, fileName, contentType)
            {
                Path = path;
            }

            public string Path { get; set; }

            public override void WriteTo(Stream stream)
            {
                var file = Mvx.Resolve<IMvxFileStore>();
                var result = file.TryReadBinaryFile(Path, (fileStream) =>
                    {
                        fileStream.CopyTo(stream);
                        stream.Flush();
                        return true;
                    });

                if (!result)
                    throw new MvxException("Failed to read file for upload at {0}", Path);
            }
        }

        public List<IStreamForUpload> StreamsToSend { get; set; }
        public Dictionary<string, string> FieldsToSend { get; set; }

        public override bool NeedsRequestStream => StreamsToSend != null && StreamsToSend.Count > 0;

        public override void ProcessRequestStream(Stream stream)
        {
            UploadFields(stream);
            UploadStreams(stream);
            byte[] trailer = System.Text.Encoding.UTF8.GetBytes("\r\n--" + Boundary + "--\r\n");
            stream.Write(trailer, 0, trailer.Length);
        }

        protected virtual void UploadFields(Stream stream)
        {
            if (FieldsToSend == null || FieldsToSend.Count == 0)
                return;

            byte[] boundarybytes = System.Text.Encoding.UTF8.GetBytes("\r\n--" + Boundary + "\r\n");

            const string formDataTemplate = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";
            foreach (var kvp in FieldsToSend)
            {
                stream.Write(boundarybytes, 0, boundarybytes.Length);
                string content = string.Format(formDataTemplate, kvp.Key, kvp.Value);
                WriteTextToStream(stream, content);
            }
        }

        protected virtual void UploadStreams(Stream stream)
        {
            if (StreamsToSend == null || StreamsToSend.Count == 0)
                return;

            byte[] boundarybytes = System.Text.Encoding.UTF8.GetBytes("\r\n--" + Boundary + "\r\n");

            const string fileHeaderTemplate =
                "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n";
            foreach (var toSend in StreamsToSend)
            {
                stream.Write(boundarybytes, 0, boundarybytes.Length);
                string header = string.Format(fileHeaderTemplate, toSend.FieldName, toSend.FileName, toSend.ContentType);
                byte[] headerbytes = System.Text.Encoding.UTF8.GetBytes(header);
                stream.Write(headerbytes, 0, headerbytes.Length);
                toSend.WriteTo(stream);
            }
        }
    }
}