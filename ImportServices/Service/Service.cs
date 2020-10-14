using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Net;
using System.IO;
using Renci.SshNet;
using Renci.SshNet.Sftp;
using ImportServices.Wrapper;


namespace ImportServices.Service
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

        public bool DownloadFileSFTP(string host, string username,string password, string pathRemoteFile,int sellerId)
        {
            string appPath = AppDomain.CurrentDomain.BaseDirectory;
            bool downloadresult = false;
           // var appPath = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath;
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
                    var lastUpdatedFile = files.OrderByDescending(x => x.LastWriteTime).FirstOrDefault();
                    if (lastUpdatedFile != null)
                    { 
                        
                        using (Stream fileStream = File.OpenWrite(pathLocalFile))
                        {

                            sftp.DownloadFile(pathRemoteFile+"/"+ lastUpdatedFile.Name, fileStream);
                            downloadresult = true;
                        }



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