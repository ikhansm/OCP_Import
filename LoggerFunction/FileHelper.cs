
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Threading.Tasks;


namespace LoggerFunctions
{
    public static class FileHelper
    {
        private static object lockObject = new object();
        private static int fileIndex { get; set; } = 1;

        public static void WriteExceptionMessage(string Content, string brandName)
        {
            
            var logViewModel = new LogViewModel()
            {
                LogStatus = "Error",
                LogFrom = "OnHandleException",
                LogBrand = brandName,
                LogErrorMessage = Content,
            };

            WriteExceptionMessage(logViewModel, brandName);
        }

        public static void WriteExceptionMessage(string brandName, string LogFrom = "OnAction", string LogFromActoin = "", string LogFromController = "", string LogStatus = "", string LogErrorMessage = "", string LogInnerErrorMessage = "")
        {
            var logViewModel = new LogViewModel()
            {
                LogStatus = LogStatus.ToUpper(),
                LogFrom = LogFrom,
                LogBrand = brandName,
                LogErrorMessage = LogErrorMessage,
                LogFromActoin = LogFromActoin,
                LogFromController = LogFromController,
                LogInnerErrorMessage = LogInnerErrorMessage
            };

            WriteExceptionMessage(logViewModel, brandName);
        }

        public static void WriteExceptionMessage(LogViewModel Content, string brandName)
        {
            List<string> vsAttr = new List<string>();
            vsAttr.Add(DateTime.Now.ToString("MM:dd:yyyy hh:mm:ss tt,fff "));

            vsAttr.Add(Content.LogStatus);

            vsAttr.Add(Content.LogFromController);

            vsAttr.Add(Content.LogFromActoin);

            vsAttr.Add(Environment.NewLine + "Message => " + Content.LogErrorMessage);

            if (!string.IsNullOrEmpty(Content.LogInnerErrorMessage))
                vsAttr.Add(Environment.NewLine + "Inner Exception Message => " + Content.LogInnerErrorMessage);

            string message = String.Join(":", vsAttr);

            bool IsFromException = (Content.LogFrom == "OnHandleException" || Content.LogFrom == "OnException");

            WriteToFile(message, brandName, IsFromException);
        }

        public static string ReadFileToEnd(string filePath)
        {
            string contents = string.Empty;
            using (StreamReader sr = new StreamReader(filePath))
                contents = sr.ReadToEnd();
            return contents;
        }

        public static void WriteToFile(string content, string brandName, bool IsFromException = false)
        {
            bool append = true;
            string FilePathWithName = "";
            string ExcpFilePathWithName = "";
            FileGenerate(brandName, ref FilePathWithName, ref ExcpFilePathWithName, IsFromException);
            lock (lockObject)
                using (StreamWriter sw = new StreamWriter(FilePathWithName, append))
                    sw.WriteLine(content);

            if (IsFromException)
            {
                lock (lockObject)
                    using (StreamWriter sw = new StreamWriter(ExcpFilePathWithName, append))
                        sw.WriteLine(content);
            }

        }

        public static async Task RemoveLogsFile()
        {
            RemoveFiles(System.Configuration.ConfigurationManager.AppSettings["RemoveFileDays"].ToString(), AppDomain.CurrentDomain.BaseDirectory + System.Configuration.ConfigurationManager.AppSettings["LogFilePath"].ToString());
            RemoveFiles(System.Configuration.ConfigurationManager.AppSettings["RemoveJsonFileDays"].ToString(), AppDomain.CurrentDomain.BaseDirectory + System.Configuration.ConfigurationManager.AppSettings["JsonFilePath"].ToString());
            WriteExceptionMessage("", LogStatus: "INFO", LogErrorMessage: "over remove File" + DateTime.Now);
        }

        private static void RemoveFiles(string NumberOfDays, string RemoveFilePath)
        {
            try
            {
                if (!Directory.Exists(RemoveFilePath))
                    Directory.CreateDirectory(RemoveFilePath);
                if (!string.IsNullOrEmpty(RemoveFilePath))
                {
                    var directoryList = Directory.GetDirectories(RemoveFilePath, "*").ToList();
                    foreach (var dir in directoryList)
                    {
                        string[] files = Directory.GetFiles(dir);
                        foreach (string file in files)
                            DeleteFiles(file, Convert.ToInt32(NumberOfDays), dir);
                    }
                }
            }
            catch (Exception ex)
            {
                WriteExceptionMessage("Error:Delete log file " + ex.Message);
                if (ex.InnerException != null)
                    WriteExceptionMessage("Error:Delete Log file" + ex.InnerException.Message);
            }
        }

        public static void DeleteFiles(string file, int NumberOfDays, string brandName)
        {
            FileInfo objFileInfo = new FileInfo(file);
            //WriteExceptionMessage(LogStatus: "INFO", LogErrorMessage: "start remove File | file Name:" + objFileInfo.Name);
            if (objFileInfo.Extension.ToLower().Contains("txt"))
            {
                if (objFileInfo.CreationTime < DateTime.Now.AddDays(-NumberOfDays))
                {
                    WriteExceptionMessage(brandName, LogStatus: "INFO", LogErrorMessage: "File Extension | file Name:" + objFileInfo.Name + " | Extension" + objFileInfo.Extension);
                    objFileInfo.Delete();
                    WriteExceptionMessage(brandName, LogStatus: "INFO", LogErrorMessage: "File Removed Successfully | file Name:" + objFileInfo.Name);
                }
            }
            if (objFileInfo.Extension.ToLower().Contains("json"))
            {
                if (objFileInfo.CreationTime < DateTime.Now.AddDays(-NumberOfDays))
                {
                    WriteExceptionMessage(brandName, LogStatus: "INFO", LogErrorMessage: "File Extension | file Name:" + objFileInfo.Name + " | Extension" + objFileInfo.Extension);
                    objFileInfo.Delete();
                    WriteExceptionMessage(brandName, LogStatus: "INFO", LogErrorMessage: "File Removed Successfully | file Name:" + objFileInfo.Name);
                }
            }
        }


        private static void FileGenerate(string brandName, ref string FilePathWithName, ref string ExcpFilePathWithName, bool IsFromException = false)
        {
            string folder = System.Configuration.ConfigurationManager.AppSettings["LogFilePath"].ToString();
            string FilePath = AppDomain.CurrentDomain.BaseDirectory + System.Configuration.ConfigurationManager.AppSettings["LogFilePath"].ToString() +"\\" + brandName + "\\";
            string FileDate = DateTime.Now.ToString("dd.MM.yyyy");
            string FileName = brandName + "SyncServices";
            string ExcpFileName = "Exception_" + brandName + "_SyncServices";
            FilePathWithName = string.Format("{0}{1}_{2}_{3}.txt", FilePath, FileName, fileIndex, FileDate);
            ExcpFilePathWithName = string.Format("{0}{1}_{2}_{3}.txt", FilePath, ExcpFileName, fileIndex, FileDate);
            try
            {
                if (!Directory.Exists(FilePath))
                    Directory.CreateDirectory(FilePath);

                if (!File.Exists(FilePathWithName))
                {
                    fileIndex = 1;
                    FilePathWithName = FileCreateForLog(FilePath, FileDate, FileName);
                }
                else
                {
                    long length = new System.IO.FileInfo(FilePathWithName).Length;
                    string FileMaxSize = System.Configuration.ConfigurationManager.AppSettings["ApplicationUserFileMaxSizeLimitMB"].ToString();
                    FileMaxSize = "100";
                    if (length >= (Convert.ToInt64(FileMaxSize) * 1024 * 1024))
                    {
                        fileIndex++;
                        FilePathWithName = FileCreateForLog(FilePath, FileDate, FileName);
                    }
                }

                if (IsFromException && !File.Exists(ExcpFilePathWithName))
                {
                    ExcpFilePathWithName = FileCreateForLog(FilePath, FileDate, ExcpFileName);
                }

            }
            catch (Exception ex)
            {

            }
        }

        private static string FileCreateForLog(string FilePath, string FileDate, string FileName)
        {
            string FilePathWithName = string.Format("{0}{1}_{2}_{3}.txt", FilePath, FileName, fileIndex, FileDate);
            string Header = "DateTime:Status:Controller:Action:Method" + Environment.NewLine + "Message" + Environment.NewLine + "Inner Exception Message" + Environment.NewLine + "-----------------------------------------------------" + Environment.NewLine;

            using (StreamWriter sw = new StreamWriter(FilePathWithName, true))
                sw.WriteLine(Header);
            return FilePathWithName;
        }
    }
}