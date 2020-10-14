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
        bool DownloadFileSFTP(string host, string username, string password, string pathRemoteFile, int sellerId);

        Wrapper.ColorList GetColorFamily(string filePath);

    }
}
