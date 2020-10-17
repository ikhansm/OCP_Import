using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Net;
using System.IO;
using Renci.SshNet;
using Renci.SshNet.Sftp;
using ImportService.Wrapper;
using System.Xml.Serialization;
using System.Xml;
using System.Text;

namespace ImportService.Service
{
    public class Service:IService.IService,IDisposable
    {
    
        public Wrapper.ProductCatalogImport ReadCSV(int sellerId)
        {
            string appPath = AppDomain.CurrentDomain.BaseDirectory;

            var h = new Wrapper.Helper();
          //  var appPath = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath;
            var filePath = Path.Combine(appPath, "DownloadFile/Downloaded_" + sellerId + ".xml");
            //var filePath = HttpContext.Current.Server.MapPath("~/DownloadFile/Downloaded_"+ sellerId + ".xml");
            var products = h.DeserializeToObject<Wrapper.ProductCatalogImport>(filePath);

            return products;
        }

        public Tuple<bool,string> DownloadFileSFTP(string host, string username,string password, string pathRemoteFile,int sellerId)
        {
            string appPath = AppDomain.CurrentDomain.BaseDirectory;
            bool downloadresult = false;
        
            var pathLocalFile = Path.Combine(appPath, "DownloadFile/Downloaded_" + sellerId + ".xml");
            if (!System.IO.File.Exists(pathLocalFile))
            {
                try
                {
                    System.IO.File.Create(pathLocalFile).Dispose();
                }catch(Exception ex) { }
            }

            using (SftpClient sftp = new SftpClient(host, username, password))
            {
                try
                {
                    sftp.Connect();

                    var files = sftp.ListDirectory(pathRemoteFile).Where(f => !f.IsDirectory);
                    var brandFiles  =files.Where(x => x.Name.Contains(".xml.sm")).ToList();
                    var lastUpdatedFile = files.OrderByDescending(x => x.LastWriteTime).FirstOrDefault();
                    string fullPath = pathRemoteFile + "/";
                    if (lastUpdatedFile != null)
                    {
                        fullPath += lastUpdatedFile.Name;
                        using (Stream fileStream = File.OpenWrite(pathLocalFile))
                        {

                            sftp.DownloadFile(fullPath, fileStream);
                            downloadresult = true;
                        }


                    }

        //            sftp.DeleteFile(fullPath);
                    sftp.Disconnect();

                    return Tuple.Create(downloadresult, fullPath);
                }
                catch (Exception ex)
                {
                    return     Tuple.Create(downloadresult, ex.Message);
                   
                }
            }
         
          }


     

        public bool CreateFileSFTP(string host, string username, string password,string _path ,string folder ,string fileName, ProductCatalogImport products)
        {

            var _stream = ConvertObjectToStream(products);

            bool downloadresult = false;

            string fullfolderPath = _path + "/" + folder;
            string currentTime = DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year + "-" + DateTime.Now.Hour+"-"+DateTime.Now.Minute+"-"+DateTime.Now.Second;
            string fullPath = fullfolderPath + "/" + fileName+"-"+currentTime.Trim()+".xml";
            
            using (SftpClient sftp = new SftpClient(host, username, password))
            {
                try
                {
                        sftp.Connect();

                     if (!sftp.Exists(fullfolderPath))
                      {
                        sftp.CreateDirectory(fullfolderPath);

                     }
                       // byte[] results = System.Text.Encoding.ASCII.GetBytes("hello");
                        var stream = new MemoryStream();
                        using (var ms = new MemoryStream(_stream))
                        {
                            sftp.BufferSize = (uint)ms.Length; // bypass Payload error large files
                            sftp.UploadFile(ms, fullPath);
                        }


                    sftp.Disconnect();
                }
                catch (Exception er)
                {
                    //   Console.WriteLine("An exception has been caught " + er.ToString());
                }
            }
            return downloadresult;
            //       String LocalDestinationPath = HttpContext.Current.Server.MapPath("~/DownloadFile");
            //         Helper.Utility.FTPDownload(LocalDestinationPath, "test.txt", ApplicationEngine.FTP_Host+ ApplicationEngine.FTP_File_Path+ "/test.txt", ApplicationEngine.FTP_User_Name, ApplicationEngine.FTP_Password);
        }
        public  string SerializeObject<t>(ProductCatalogImport obj)
        {
            try
            {
                string xmlString = null;
                MemoryStream memoryStream = new MemoryStream();
                XmlSerializer xs = new XmlSerializer(typeof(ProductCatalogImport));
                XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
                xs.Serialize(xmlTextWriter, obj);
                memoryStream = (MemoryStream)xmlTextWriter.BaseStream;
                xmlString = ByteArrayToUTF8String(memoryStream.ToArray()); return xmlString;
            }
            catch
            {
                return string.Empty;
            }
        }
        public  string ByteArrayToUTF8String(byte[] characters)
        {
            UTF8Encoding encoding = new UTF8Encoding();
            string constructedString = encoding.GetString(characters);
            return (constructedString);
        }
        public  Byte[] StringToUTF8ByteArray(string stringVal)
        {
            System.Text.UTF8Encoding encoding = new UTF8Encoding();
            byte[] byteArray = encoding.GetBytes(stringVal);
            return byteArray;
        }


        public Byte[] ConvertObjectToStream(ProductCatalogImport o)
        {
          string str =  SerializeObject<ProductCatalogImport>(o);

            var _byte = StringToUTF8ByteArray(str);
            return _byte;
        }


        public ColorList GetColorFamily(string filePath) {
            string appPath = AppDomain.CurrentDomain.BaseDirectory;

            var h = new Wrapper.Helper();
            var completePath = Path.Combine(appPath, filePath);
            var color = h.DeserializeToObject<Wrapper.ColorList>(completePath);
            return color;
        }


        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~VerifyFohf_Cash() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }





        #endregion



    }
}