using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogMaker
{
    public class Log
    {
        private static readonly Log _instance = new Log();
        protected ILog monitoringLogger;
        protected static ILog debugLogger;

        private Log()
        {
           JobScheduler.Start();
            monitoringLogger = LogManager.GetLogger("MonitoringLogger");
            debugLogger = LogManager.GetLogger("DebugLogger");
           
        }



        /// <summary>  
        /// Used to log Debug messages in an explicit Debug Logger  
        /// </summary>  
        /// <param name="message">The object message to log</param>  
        public static void Debug(string message)
        {
            debugLogger.Debug(message);
        }


        /// <summary>  
        ///  
        /// </summary>  
        /// <param name="message">The object message to log</param>  
        /// <param name="exception">The exception to log, including its stack trace </param>  
        public static void Debug(string message, System.Exception exception)
        {
            debugLogger.Debug(message, exception);
        }


        public static void Info(string message)
        {
            log4net.GlobalContext.Properties["Folder"] = "Global";
            log4net.GlobalContext.Properties["LogFileName"] = DateTime.Now.ToString("dd-MM-yyyy") + "_Info";
            log4net.Config.XmlConfigurator.Configure();
            _instance.monitoringLogger.Error(message);
        }
        ///  
        /// </summary>  
        /// <param name="message">The object message to log</param>  
        /// <param name="exception">The exception to log, including its stack trace </param>  
        public static void Info(string message, System.Exception exception)
        {
            log4net.GlobalContext.Properties["Folder"] = "Global";
            log4net.GlobalContext.Properties["LogFileName"] =DateTime.Now.ToString("dd-MM-yyyy") +"_Info";
            log4net.Config.XmlConfigurator.Configure();
            _instance.monitoringLogger.Error(message);
        }


        public static void Info(string message, string brand = "Global")
        {
            log4net.GlobalContext.Properties["Folder"] = brand;
            log4net.GlobalContext.Properties["LogFileName"] = DateTime.Now.ToString("dd-MM-yyyy") + "_Info";
            log4net.Config.XmlConfigurator.Configure();
            _instance.monitoringLogger.Error(message);
        }


        /// <summary>  
        ///  
        /// </summary>  
        /// <param name="message">The object message to log</param>  
        public static void Info(string message, System.Exception ex, string brand = "Global")
        {
            log4net.GlobalContext.Properties["Folder"] = brand;
            log4net.GlobalContext.Properties["LogFileName"] = DateTime.Now.ToString("dd-MM-yyyy") + "_Info";
            log4net.Config.XmlConfigurator.Configure();
            _instance.monitoringLogger.Error(message, ex);
        }




        public static void Warn(string message)
        {
            log4net.GlobalContext.Properties["Folder"] = "Global";
            log4net.GlobalContext.Properties["LogFileName"] = DateTime.Now.ToString("dd-MM-yyyy") + "_Warn";
            log4net.Config.XmlConfigurator.Configure();
            _instance.monitoringLogger.Error(message);
        }
        ///  
        /// </summary>  
        /// <param name="message">The object message to log</param>  
        /// <param name="exception">The exception to log, including its stack trace </param>  
        public static void Warn(string message, System.Exception exception)
        {
            log4net.GlobalContext.Properties["Folder"] = "Global";
            log4net.GlobalContext.Properties["LogFileName"] = DateTime.Now.ToString("dd-MM-yyyy") + "_Warn";
            log4net.Config.XmlConfigurator.Configure();
            _instance.monitoringLogger.Error(message);
        }


        public static void Warn(string message, string brand = "Global")
        {
            log4net.GlobalContext.Properties["Folder"] = brand;
            log4net.GlobalContext.Properties["LogFileName"] = DateTime.Now.ToString("dd-MM-yyyy") + "_Warn";
            log4net.Config.XmlConfigurator.Configure();
            _instance.monitoringLogger.Error(message);
        }


        /// <summary>  
        ///  
        /// </summary>  
        /// <param name="message">The object message to log</param>  
        public static void Warn(string message, System.Exception ex, string brand = "Global")
        {
            log4net.GlobalContext.Properties["Folder"] = brand;
            log4net.GlobalContext.Properties["LogFileName"] = DateTime.Now.ToString("dd-MM-yyyy") + "_Warn";
            log4net.Config.XmlConfigurator.Configure();
            _instance.monitoringLogger.Error(message, ex);
        }





        //public static void Warn(string message, System.Exception exception)
        //{
        //    _instance.monitoringLogger.Warn(message, exception);
        //}
        public static void Error(string message)
        {
            log4net.GlobalContext.Properties["Folder"] = "Global";
            log4net.GlobalContext.Properties["LogFileName"] = DateTime.Now.ToString("dd-MM-yyyy") + "_Error";
            log4net.Config.XmlConfigurator.Configure();
            _instance.monitoringLogger.Error(message);
        }
        ///  
        /// </summary>  
        /// <param name="message">The object message to log</param>  
        /// <param name="exception">The exception to log, including its stack trace </param>  
        public static void Error(string message, System.Exception exception)
        {
            log4net.GlobalContext.Properties["Folder"] = "Global";
            log4net.GlobalContext.Properties["LogFileName"] = DateTime.Now.ToString("dd-MM-yyyy") + "_Error";
            log4net.Config.XmlConfigurator.Configure();
            _instance.monitoringLogger.Error(message);
        }


        public static void Error(string message,  string brand = "Global")
        {
            log4net.GlobalContext.Properties["Folder"] = brand;
            log4net.GlobalContext.Properties["LogFileName"] = DateTime.Now.ToString("dd-MM-yyyy") + "_Error";
            log4net.Config.XmlConfigurator.Configure();
            _instance.monitoringLogger.Error(message);
        }


        /// <summary>  
        ///  
        /// </summary>  
        /// <param name="message">The object message to log</param>  
        public static void Error( string message, System.Exception ex, string brand = "Global")
        {
             log4net.GlobalContext.Properties["Folder"] = brand;
             log4net.GlobalContext.Properties["LogFileName"] = DateTime.Now.ToString("dd-MM-yyyy") + "_Error";
             log4net.Config.XmlConfigurator.Configure();
            _instance.monitoringLogger.Error(message,ex);
        }

        /// <summary>  
      

        /// <summary>  
        ///  
        /// </summary>  
        /// <param name="message">The object message to log</param>  
        public static void Fatal(string message)
        {
            _instance.monitoringLogger.Fatal(message);
        }

        /// <summary>  
        ///  
        /// </summary>  
        /// <param name="message">The object message to log</param>  
        /// <param name="exception">The exception to log, including its stack trace </param>  
        public static void Fatal(string message, System.Exception exception)
        {
            _instance.monitoringLogger.Fatal(message, exception);
        }

        public static void DeleteFiles(string file, int NumberOfDays)
        {
            FileInfo objFileInfo = new FileInfo(file);
            //WriteExceptionMessage(LogStatus: "INFO", LogErrorMessage: "start remove File | file Name:" + objFileInfo.Name);
            if (objFileInfo.Extension.ToLower().Contains("log"))
            {
                if (objFileInfo.CreationTime < DateTime.Now.AddDays(-NumberOfDays))
                {                    
                    objFileInfo.Delete();
                }
            }
          
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
                            DeleteFiles(file, Convert.ToInt32(NumberOfDays));
                    }
                }
            }
            catch (Exception ex)
            {
                Error("Error:Delete log file " + ex.Message);
                if (ex.InnerException != null)
                Error("InnerException" + ex.InnerException.Message);
            }
        }

        public static void RemoveLogsFile() {

            string strBasepath = AppDomain.CurrentDomain.BaseDirectory;
            string logfolder = strBasepath + System.Configuration.ConfigurationManager.AppSettings["LogFilePath"].ToString();
            string deleteIntervalDays = System.Configuration.ConfigurationManager.AppSettings["RemoveFileDays"].ToString();
            RemoveFiles(deleteIntervalDays, logfolder);


        }


    }
}
