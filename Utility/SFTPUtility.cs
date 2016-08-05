using UtilityHouse.Entity;
using Renci.SshNet;
using System;
using System.IO;

namespace UtilityHouse.Service.Helper.SFTP
{
    public class SFTPUtility
    {

        private static SFTPUtility instance = null;
        private static ConnectionInfo conncectionInfo = null;

        private SFTPUtility()
        {   //Public SFTP credential : http://www.sftp.net/public-online-sftp-servers
            conncectionInfo = new ConnectionInfo(ApplicationConstants.SFTPHost, ApplicationConstants.SFTPPort, ApplicationConstants.SFTPUsername,
            new AuthenticationMethod[]{
                new PasswordAuthenticationMethod(ApplicationConstants.SFTPUsername, ApplicationConstants.SFTPPassword)
            });
        }

        public static SFTPUtility Instance
        {
            get
            {
                if (instance == null || conncectionInfo == null)
                {
                    instance = new SFTPUtility();
                }
                return instance;
            }
        }

        public bool UploadFile(Stream fileStream, string filename)
        {
            try
            {
                using (var sftp = new SftpClient(conncectionInfo))
                {
                    sftp.Connect();

                    sftp.UploadFile(fileStream, ApplicationConstants.SFTPUploadFileLocation + filename);

                    sftp.Disconnect();
                }
                return true;
            }
            
            catch (Exception ex)
            {
                return false;
            }
        }

        public Stream DownloadFile(string fileName)
        {
            try
            {
                using (var sftp = new SftpClient(conncectionInfo))
                {
                    sftp.Connect();

                    //fileName = "readme.txt";

                    Stream fileToSave = File.OpenWrite(fileName);
                    sftp.DownloadFile(ApplicationConstants.SFTPDownloadFileLocation + fileName, fileToSave);

                    sftp.Disconnect();

                    return fileToSave;
                }
            }
            catch(Exception ex)
            {
                return null;
            }
        }
    }
}
