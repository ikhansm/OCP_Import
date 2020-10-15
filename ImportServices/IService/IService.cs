using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImportServices.IService
{
   public interface IService
    {
        Wrapper.ProductCatalogImport ReadCSV(int sellerId);
        Tuple<bool, string> DownloadFileSFTP(string host, string username, string password, string pathRemoteFile, int sellerId);

        Wrapper.ColorList GetColorFamily(string filePath);
         bool CreateFileSFTP(string host, string username, string password, string _path, string folder, string fileName, Wrapper.ProductCatalogImport products);

    }
}
