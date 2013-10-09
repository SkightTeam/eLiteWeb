using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using FluentBuild.Utilities;

namespace FluentBuild.Publishing
{
    //This code was adapted from the nant-googlecode project http://code.google.com/p/nant-googlecode/ 
    ///<summary>
    /// Publishes a file to GoogleCode
    ///</summary>
    public class GoogleCode : InternalExecutable
    {
        private static readonly byte[] NewLineAsciiBytes = Encoding.ASCII.GetBytes("\r\n");
        private static readonly string Boundary = Guid.NewGuid().ToString();
        private string _localFilePath;
        private string _password;
        private string _projectName;
        private string _summary;
        private string _targetFileName;
        private string _username;

        public GoogleCode()
        {
        }

        /// <summary>
        /// Gets or sets Google user name to authenticate as (this is just the username part, don't include the @gmail.com part.
        /// </summary>
        public GoogleCode UserName(string username)
        {
            _username = username;
            return this;
        }


        /// <summary>
        /// Gets or sets the Google Code password (not the same as the gmail password).
        /// </summary>
        public GoogleCode Password(string password)
        {
            _password = password;
            return this;
        }

        /// <summary>
        /// Gets or sets the Google Code project name to upload to.
        /// </summary>
        public GoogleCode ProjectName(string name)
        {
            _projectName = name;
            return this;
        }

        /// <summary>
        /// Gets or sets the local path of the file to upload.
        /// </summary>
        public GoogleCode LocalFileName(string path)
        {
            _localFilePath = path;
            return this;
        }

        /// <summary>
        /// Gets or sets the file name that this file will be given on Google Code.
        /// </summary>
        public GoogleCode TargetFileName(string targetName)
        {
            _targetFileName = targetName;
            return this;
        }

        /// <summary>
        /// Gets or sets the summary of the upload.
        /// </summary>
        public GoogleCode Summary(string data)
        {
            _summary = data;
            return this;
        }

        internal void ValidateString(string value, string friendlyName)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException(String.Format("{0} is required", friendlyName));
            }
        }

        internal HttpWebRequest CreateRequest()
        {
            var request = (HttpWebRequest)WebRequest.Create(String.Format("https://{0}.googlecode.com/files", _projectName));
            request.Method = "POST";
            request.ContentType = String.Concat("multipart/form-data; boundary=" + Boundary);
            request.UserAgent = String.Concat("Google Code Upload FluentBuild Task ",Assembly.GetExecutingAssembly().GetName().Version.ToString());
            request.Headers.Add("Authorization", String.Concat("Basic ", CreateAuthorizationToken(_username, _password)));
            return request;
        }

        ///<summary>
        /// Executes the upload of the file via a http post
        ///</summary>
        [Obsolete("Replaced with Task.Publish", true)]
        public void Upload()
        {
            InternalExecute();
        }

        internal override void InternalExecute()
        {
            Validate();
            var request= CreateRequest();
            using (var requestStream = request.GetRequestStream())
                CreateRequestContent(requestStream);    
            request.GetResponse();
        }

        private void CreateRequestContent(Stream stream)
        {
                WriteLine(stream, String.Concat("--", Boundary));
                WriteLine(stream, @"content-disposition: form-data; name=""summary""");
                WriteLine(stream, "");
                WriteLine(stream, _summary);


                WriteLine(stream, String.Concat("--", Boundary));
                WriteLine(stream,
                          String.Format(@"content-disposition: form-data; name=""filename""; filename=""{0}""",
                                        _targetFileName));
                WriteLine(stream, "Content-Type: application/octet-stream");
                WriteLine(stream, "");
                WriteFile(stream, _localFilePath);
                WriteLine(stream, "");
                WriteLine(stream, String.Concat("--", Boundary, "--"));   
        }

        internal void Validate()
        {
            ValidateString(_localFilePath, "LocalFilePath");
            ValidateString(_password, "Password");
            ValidateString(_projectName, "ProjectName");
            ValidateString(_summary, "Summary");
            ValidateString(_targetFileName, "TargetFileName");
            ValidateString(_username, "Username");
        }

        /// <summary>
        /// Writes the specified file to the specified stream.
        /// </summary>
        internal void WriteFile(Stream outputStream, string fileToWrite)
        {
            using (var fileStream = new FileStream(_localFilePath, FileMode.Open))
            {
                var buffer = new byte[1024];
                int count;
                while ((count = fileStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    outputStream.Write(buffer, 0, count);
                }
            }
        }

        /// <summary>
        /// Writes the string to the specified stream and concatenates a newline.
        /// </summary>
        internal static void WriteLine(Stream outputStream, string valueToWrite)
        {
            if (valueToWrite == null)
                throw new ArgumentNullException("valueToWrite");

            var bytesToWrite = new List<byte>(Encoding.ASCII.GetBytes(valueToWrite));
            bytesToWrite.AddRange(NewLineAsciiBytes);
            outputStream.Write(bytesToWrite.ToArray(), 0, bytesToWrite.Count);
        }

        /// <summary>
        /// Creates the authorization token.
        /// </summary>
        internal static string CreateAuthorizationToken(string username, string password)
        {
            var authBytes = Encoding.ASCII.GetBytes(String.Concat(username, ":", password));
            return Convert.ToBase64String(authBytes);
        }
    }
}