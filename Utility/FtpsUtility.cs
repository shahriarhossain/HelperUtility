using Ftps.Entity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace Ftps.Service.Helper
{
    public static class FtpsUtility
    {
        public static TextReader DownloadFile(string location, string fileName)
        {
            try
            {
                Console.WriteLine("Attempting to download the file :"+ fileName);
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ApplicationConstants.SFTPHost + location + fileName);
                request.Credentials = new NetworkCredential(ApplicationConstants.SFTPUsername, ApplicationConstants.SFTPPassword);
                request.UseBinary = true;
                request.UsePassive = true;
                request.EnableSsl = true;

                request.Method = WebRequestMethods.Ftp.DownloadFile;

                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
               
                string fileSaveLocation = String.Format(@"{0}\{1}\{2}", ApplicationConstants.WebJobsTempLocation, ApplicationConstants.WebJobsName, fileName);

                Console.WriteLine("Before Saving the file in Disk");
                //Save the stream in DISK
                using (var fileStream = new FileStream(fileSaveLocation, FileMode.Create, FileAccess.Write))
                {
                    Stream responseStream = response.GetResponseStream();
                    responseStream.CopyTo(fileStream);
                    Console.WriteLine("After Saving the file in Disk");
                }

                Console.WriteLine("Download Complete, status {0}", response.StatusDescription);

                response.Close();

                TextReader reader = File.OpenText(fileSaveLocation);
                return reader;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool UploadFile(byte[] fileContents, string location, string fileName)
        {
            try
            {
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ApplicationConstants.SFTPHost + location + fileName);
                request.Credentials = new NetworkCredential(ApplicationConstants.SFTPUsername, ApplicationConstants.SFTPPassword);
                request.UseBinary = true;
                request.UsePassive = true;
                request.EnableSsl = true;

                request.Method = WebRequestMethods.Ftp.UploadFile;

                request.ContentLength = fileContents.Length;

                Stream requestStream = request.GetRequestStream();
                requestStream.Write(fileContents, 0, fileContents.Length);
                requestStream.Close();

                FtpWebResponse response = (FtpWebResponse)request.GetResponse();

                Console.WriteLine("Upload File Complete, status {0}", response.StatusDescription);

                response.Close();

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<string> DirectoryList(string fileName)
        {
            try
            {
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ApplicationConstants.SFTPHost + fileName);
                request.Credentials = new NetworkCredential(ApplicationConstants.SFTPUsername, ApplicationConstants.SFTPPassword);
                request.UseBinary = true;
                request.UsePassive = true;
                request.EnableSsl = true;

                request.Method = WebRequestMethods.Ftp.ListDirectory;
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();

                StreamReader streamReader = new StreamReader(response.GetResponseStream());

                List<string> directories = new List<string>();

                string line = streamReader.ReadLine();
                while (!string.IsNullOrEmpty(line))
                {
                    directories.Add(line);
                    Console.WriteLine(line);

                    line = streamReader.ReadLine();
                }

                streamReader.Close();

                return directories;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public static bool FileExistsInFtps(string location, string fileName)
        {
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ApplicationConstants.SFTPHost + location + fileName);
            request.Credentials = new NetworkCredential(ApplicationConstants.SFTPUsername, ApplicationConstants.SFTPPassword);
            request.UseBinary = true;
            request.UsePassive = true;
            request.EnableSsl = true;

            request.Method = WebRequestMethods.Ftp.GetFileSize;

            bool res = false;
            try
            {
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                res = true;
                return res;
            }
            catch (WebException ex)
            {
                FtpWebResponse response = (FtpWebResponse)ex.Response;
                if (response.StatusCode ==
                    FtpStatusCode.ActionNotTakenFileUnavailable)
                {
                    res = false;
                    return res;
                }
            }
            return res;
        }

        public static Tuple<bool, string> FileExistsInDirectory(List<string> directoryList, string pattern)
        {
            bool res = false;
            string fileName = "";
            foreach (var directory in directoryList)
            {
                res = directory.Contains(pattern);
                fileName = directory;
                if (res)
                {
                    break;
                }
            }

            return new Tuple<bool, string>(res, fileName);
        }
    }
}
