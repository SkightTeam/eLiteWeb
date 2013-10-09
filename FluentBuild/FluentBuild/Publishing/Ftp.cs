using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using FluentBuild.Utilities;

namespace FluentBuild.Publishing
{
    public class Ftp : InternalExecutable
    {
        private string _serverName;
        private string _username;
        private string _password;
        private string _remoteFilePath;
        private string _localFilePath;
        private TimeSpan _timeout;

        public Ftp Server(string serverName)
        {
            _serverName = serverName;
            return this;
        }

        public Ftp UserName(string username)
        {
            _username = username;
            return this;
        }

        public Ftp Password(string password)
        {
            _password = password;
            return this;
        }

        public Ftp RemoteFilePath(string path)
        {
            _remoteFilePath = path;
            return this;
        }

        public Ftp LocalFilePath(string path)
        {
            _localFilePath = path;
            return this;
        }

        public Ftp Timeout(TimeSpan timespan)
        {
            _timeout = timespan;
            return this;
        }



        internal override void InternalExecute()
        {
            Defaults.Logger.Write("FTP", String.Format("Uploading {0} to ftp://{1}/{2}/{3}", _localFilePath, _serverName, _remoteFilePath, Path.GetFileName(_localFilePath)));
            var request = (FtpWebRequest)WebRequest.Create(String.Format("ftp://{0}/{1}/{2}", _serverName, _remoteFilePath, Path.GetFileName(_localFilePath)));
            request.Method = WebRequestMethods.Ftp.UploadFile;

            request.Credentials = new NetworkCredential(_username, _password);
            request.UseBinary = true;

            if (_timeout.TotalMilliseconds > 0)
            {
                request.ReadWriteTimeout = (int) _timeout.TotalMilliseconds;
                request.Timeout = (int) _timeout.TotalMilliseconds;
            }

            request.KeepAlive = false;

            byte[] fileContents= new byte[0];
            

        using(var s = new FileStream(_localFilePath, FileMode.Open, FileAccess.Read))
        {
            Array.Resize(ref fileContents, (int) (s.Length));
            s.Read(fileContents, 0, (int) s.Length);
        }


            using (var requestStream = request.GetRequestStream())
            {
                requestStream.Write(fileContents, 0, fileContents.Length);
                requestStream.Close();
            }

            using (var response = (FtpWebResponse)request.GetResponse())
            {
                if (response.StatusCode != FtpStatusCode.ClosingData)
                    BuildFile.SetErrorState();

                Defaults.Logger.Write("FTP", "Upload File Complete, status {0}", response.StatusDescription);
                    response.Close();
                
            }

        }
    }
}
