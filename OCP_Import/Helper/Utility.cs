using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace OCP_Import.Helper
{
    public class Utility
    {
        public static string nextUrl { get; set; }
        public T DeserializeToObject<T>(string filepath) where T : class
        {
            System.Xml.Serialization.XmlSerializer ser = new System.Xml.Serialization.XmlSerializer(typeof(T));

            using (StreamReader sr = new StreamReader(filepath))
            {
                return (T)ser.Deserialize(sr);
            }
        }


        public static HttpResponse GetHttpResponse()

        {

            return HttpContext.Current.Response;

        }

        public static void PassFileStreamDataToStream(FtpWebResponse response, FileStream outputStream)

        {

            try

            {

                Stream ftpStream = response.GetResponseStream();

                int bufferSize = 2048;

                int readCount;

                byte[] buffer = new byte[bufferSize];



                //reads the stream of response based on request.

                readCount = ftpStream.Read(buffer, 0, bufferSize);



                //Writing Stream to the Path specified in the File stream.

                while (readCount > 0)

                {

                    outputStream.Write(buffer, 0, readCount);

                    readCount = ftpStream.Read(buffer, 0, bufferSize);

                }



                ftpStream.Close();

                outputStream.Close();

                response.Close();

            }

            catch (Exception ex)

            {

                GetHttpResponse().Write(ex.Message + " Download Error");

            }

        }


        //Download

        public static FtpWebRequest GetDownloadFTPWebRequest(string path, string fileName, string fullFtpPath,

              string ftpUserID, string ftpPassword, out FileStream fs)

        {

            //Creates file stream based on the passed path.

            fs = new FileStream(path + "\\" + fileName, FileMode.Create);



            //Creating FTP request.

            FtpWebRequest reqFTP = CreateFTPRequest(fullFtpPath, fileName);



            //Setting FTP Credentials.

            reqFTP.Credentials = CreateNetworkCredentials(ftpUserID, ftpPassword);

            reqFTP.KeepAlive = false;



            // What mechanism you need to perform like download,upload etc in the FTP path.

            reqFTP.Method = WebRequestMethods.Ftp.DownloadFile;


            // Specify the data transfer type.

            reqFTP.UseBinary = true;



            // Notify the server about the size of the uploaded file

            reqFTP.ContentLength = fs.Length;



            return reqFTP;

        }

        //Helps to download file from FTP and save it to local folder.

        public static void FTPDownload(string path, string fileName, string fullFtpPath,

                string ftpUserID, string ftpPassword)

        {

            FileStream outputStream;



            //Now the FTP web request completed and ready for response.

            FtpWebRequest ftpWebRequest = GetDownloadFTPWebRequest(path, fileName, fullFtpPath, ftpUserID, ftpPassword, out outputStream);

            System.Net.ServicePointManager.Expect100Continue = false;


            //Getting response from the request.
            try
            {
                FtpWebResponse response = (FtpWebResponse)ftpWebRequest.GetResponse();

                PassFileStreamDataToStream(response, outputStream);
            }
            catch (Exception ex)
            { }
        }
        ///Creates the FTP web request

        /// </summary>

        public static FtpWebRequest CreateFTPRequest(string fullFtpPath, string fileName)

        {

            return (FtpWebRequest)FtpWebRequest.Create(new Uri(fullFtpPath + "/" + fileName));

        }
        public static NetworkCredential CreateNetworkCredentials(string ftpUserID, string ftpPassword)

        {

            return new NetworkCredential(ftpUserID, ftpPassword);

        }
        public static void appendToFile(string myfile)
        {
            using (StreamWriter sw = File.AppendText(myfile))
            {
                sw.WriteLine("Gfg");
                sw.WriteLine("GFG");
                sw.WriteLine("GeeksforGeeks");
            }

            // Opening the file for reading 
            using (StreamReader sr = File.OpenText(myfile))
            {
                string s = "";
                while ((s = sr.ReadLine()) != null)
                {
                    Console.WriteLine(s);
                }
            }
            using (StreamWriter sw = File.AppendText(myfile))
            {
                sw.WriteLine("Gfg");
                sw.WriteLine("GFG");
                sw.WriteLine("GeeksforGeeks");
            }

            
        }

    }
}