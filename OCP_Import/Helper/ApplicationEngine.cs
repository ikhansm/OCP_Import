using Microsoft.Extensions.Primitives;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace OCP_Import.Helper
{
    public static class ApplicationEngine
    {
        public static string ShopifySecretKeyPublicApp { get; } =
            ConfigurationManager.AppSettings.Get("Shopify_Secret_Key_PublicApp");

        public static string ShopifyApiKeyPublicApp { get; } =
            ConfigurationManager.AppSettings.Get("Shopify_API_Key_PublicApp");

        public static string RedirectUrl_HandShake { get; } =
            ConfigurationManager.AppSettings.Get("RedirectUrl_HandShake");

        public static string ReturnUrl_Charge { get; } =
            ConfigurationManager.AppSettings.Get("ReturnUrl_Charge");

        public static string Address_ChargeResult_UnInstall { get; } =
            ConfigurationManager.AppSettings.Get("Address_ChargeResult_UnInstall");
        public static string Url_Path { get; } = ConfigurationManager.AppSettings.Get("Url_Path");
        public static List<KeyValuePair<string, StringValues>> ToKvps(this System.Collections.Specialized.NameValueCollection qs)
            {
                Dictionary<string, string> parameters = qs.Keys.Cast<string>().ToDictionary(key => key, value => qs[value]);
                var kvps = new List<KeyValuePair<string, StringValues>>();

                parameters.ToList().ForEach(x =>
                {
                    kvps.Add(new KeyValuePair<string, StringValues>(x.Key, new StringValues(x.Value)));
                });

                return kvps;
            }
        
    }
}