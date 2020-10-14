using Newtonsoft.Json;
using RestSharp;
using Shopify.Request;
using Shopify.Request.TrackingNumber;
using Shopify.Request.variants;
using Shopify.Response;
using ShopifyAPIAdapterLibrary;
using LoggerFunctions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Script.Serialization;

namespace Shopify
{
    public class shopifyClass
    {
        const string _BaseUrl = "{yourshop}/admin/";
        string BaseURL = string.Empty;
        public ShopifyAPIClient _shopify;
        public string AuthenticationToken { get; set; }

        const string SingleOrder = "/orders.json?ids={0}";
        const string CapturePayment = "/orders/{0}/transactions.json";
        const string UpdateInventory = "/inventory_levels/set.json";
        const string Location = "/locations.json";
        //const string ProductCount = "/products/count.json";
        const string OrderList = "/api/2020-01/orders.json?limit={0}";
        //const string OrderCount = "/orders/count.json";
        const string getVariants = "/variants/{0}.json";
        const string AddNewProduct = "/products.json";
        const string AddNewVariants = "/products/{0}/variants.json";
        const string AddNewVariantsMetafields = "/products/{0}/variants/{1}/metafields.json";
        const string MetafieldsUpdate = "/metafields/{0}.json";

        const string createPriceRule = "/price_rules.json";
        const string createDiscount = "/price_rules/{0}/discount_codes.json";
        const string productBycollection = "/api/2019-10/products.json?limit={0}&collection_id={1}";
        const string ProductList = "/api/2020-01/products.json?limit={0}";
        const string ProductListByTitle = "/products.json?title={0}";
        const string ProductListByhandle = "/products.json?handle={0}";
        const string ProductVariants = "/variants/{0}.json";
        const string InventoryLevel = "/inventory_levels.json?inventory_item_ids={0}";

        const string InventoryItems = "/inventory_items/{0}.json";
        const string publishProduct = "/products/{0}.json";
        const string PutProduct = "/products/{0}.json";
        const string GetTransaction = "/orders/{0}/transactions.json";
        const string InventoryConnect = "/inventory_levels/connect.json";
        const string Getrefunds = "/orders/{0}/refunds.json";
        const string DiscountLookup = "discount_codes/lookup.json?code={0}";
        const string DeleteDiscountCode = "/api/2020-01/price_rules/{0}/discount_codes/{1}.json";

        const string CancelFulfil = "/api/2020-01/orders/{0}/fulfillments/{1}/cancel.json";
        const string fulfillment = "/orders/{0}/fulfillments.json";
        const string openOrder = "/api/2020-01/orders/{0}/open.json";
        const string putFulfillment = "/api/2020-01/orders/{0}/fulfillments/{1}.json";

        const string PriceRules = "/api/2020-04/price_rules.json?limit={0}";
        const string discountCodeList = "/api/2020-04/price_rules/{0}/discount_codes.json?limit={1}";
        const string searchCustomer = "/customers/search.json?query={0}";

        public shopifyClass(string StoreName, string Token)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            // shopifyErroLogErrorLogPath = shopifyErroLogErrorLogPath + "//" + StoreName + "//Errors//";
            BaseURL = _BaseUrl.Replace("{yourshop}", StoreName);
            AuthenticationToken = Token;
            _shopify = new ShopifyAPIClient(new ShopifyAuthorizationState { AccessToken = Token, ShopName = StoreName });
        }
        #region Add Header
        private void setHeaders(RestRequest request)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            if (!string.IsNullOrEmpty(AuthenticationToken))
            {
                request.AddHeader("X-Shopify-Access-Token", AuthenticationToken);
            }
        }
        #endregion
        public IRestResponse<T> Execute<T>(RestRequest request) where T : new()
        {
            var client = new RestClient(BaseURL);
            setHeaders(request);
            //request.Credentials = new NetworkCredential("8503de4de7def9be2c097ca17aaca859", "daaa60353c96c9cf3a21e766cec17a59");
            request.RequestFormat = DataFormat.Json; // Or DataFormat.Xml, if you prefer
            IRestResponse<T> response = client.Execute<T>(request);
            return response;
        }
        #region Add Header graphql
        private void AddHeaderGraphQl(RestRequest request)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            if (!string.IsNullOrEmpty(AuthenticationToken))
            {
                request.AddHeader("X-Shopify-Access-Token", AuthenticationToken);
            }
            request.AddHeader("content-type", "application/graphql");
        }
        #endregion

        #region pricerule and Discount
        public bool CreateDiscount(string brandName, string priceRuleId, string code)
        {
            bool status = false;
            Restart:
            try
            {
                FileHelper.WriteExceptionMessage(brandName, LogStatus: "INFO", LogErrorMessage: "ShopifyClass.CreateDiscount - Start");
                var client = new RestClient(BaseURL + string.Format(createDiscount, priceRuleId));
                var request = new RestRequest(Method.POST);
                request.AddHeader("x-shopify-access-token", AuthenticationToken);
                request.AddHeader("Content-Type", "application/json");
                request.AddParameter("application/json", "{\r\n  \"discount_code\": {\r\n    \"code\": \"" + code + "\"\r\n  }\r\n}", ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                if (Convert.ToString(response.StatusCode) == "429")
                {
                    System.Threading.Thread.Sleep(2000);
                    goto Restart;
                }
                if (response.StatusCode == HttpStatusCode.Created)
                {
                    status = true;
                }
                FileHelper.WriteExceptionMessage(brandName, LogStatus: "INFO", LogErrorMessage: "ShopifyClass.CreateDiscount - End ");
            }
            catch (Exception ex)
            {
                FileHelper.WriteExceptionMessage(brandName, LogStatus: "ERROR", LogErrorMessage: "ShopifyClass.CreateDiscount - Error" + ex.Message, LogInnerErrorMessage: ex.InnerException == null ? "" : ex.InnerException.Message);
            }
            return status;
        }

        public void putstyleInCollection(string collectionId, string productId, string position)
        {
            Restart:
            var client = new RestClient("https://stevemadden.myshopify.com/admin/custom_collections/" + collectionId + ".json");
            var request = new RestRequest(Method.PUT);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("X-Shopify-Access-Token", "93051b6d3993880a343af9b48a9f8608");
            request.AddParameter("application/json", "{\r\n  \"custom_collection\": {\r\n    \"id\": " + collectionId + ",\r\n    \"collects\": [\r\n      {\r\n        \"product_id\": " + productId + ",\r\n        \"position\": " + position + "\r\n      }\r\n    ]\r\n  }\r\n}", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            if (Convert.ToString(response.StatusCode) == "429")
            {
                System.Threading.Thread.Sleep(5000);
                goto Restart;
            }
        }

        public Response.RootPriceRule CreatePriceRule(string brandName, Request.RootPriceRule rootPriceRule)
        {
            FileHelper.WriteExceptionMessage(brandName, LogStatus: "INFO", LogErrorMessage: "shopifyClass.CreatePriceRule - Start");
            Response.RootPriceRule priceRule = new Response.RootPriceRule();
            Restart:
            try
            {
                var request = new RestRequest(createPriceRule, Method.POST);
                var Json = JsonConvert.SerializeObject(rootPriceRule, Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                //request.AddJsonBody(Json);
                request.AddParameter("application/json", Json, ParameterType.RequestBody);
                //request.AddJsonBody(rootPriceRule);
                var tResponse = Execute<Response.RootPriceRule>(request);
                if (Convert.ToString(tResponse.StatusCode) == "429")
                {
                    System.Threading.Thread.Sleep(2000);
                    goto Restart;
                }
                priceRule = JsonConvert.DeserializeObject<Response.RootPriceRule>(tResponse.Content);
            }
            catch (Exception ex)
            {
                string InnerException = ex.ToString();
                if (ex.InnerException != null)
                    InnerException = ex.InnerException.ToString();
                FileHelper.WriteExceptionMessage(brandName, LogStatus: "Error", LogErrorMessage: "shopifyClass.CreatePriceRule : " + InnerException);

                FileHelper.WriteExceptionMessage(brandName, LogStatus: "INFO", LogErrorMessage: "shopifyClass.CreatePriceRule - End");
                throw ex;
            }
            FileHelper.WriteExceptionMessage(brandName, LogStatus: "INFO", LogErrorMessage: "shopifyClass.CreatePriceRule - End");
            return priceRule;
        }
        #endregion

        #region add varinats metafield

        public bool UpdateVariantsPolicy(string brandName, string id, string body)
        {
            bool result = false;
            Restart:
            try
            {
                FileHelper.WriteExceptionMessage(brandName, LogStatus: "INFO", LogErrorMessage: "ShopifyClass.UpdateVariantsPolicy - Start");
                var client = new RestClient(BaseURL + string.Format(getVariants, id));
                var request = new RestRequest(Method.PUT);
                request.AddHeader("x-shopify-access-token", AuthenticationToken);
                request.AddHeader("Content-Type", "application/json");
                request.AddParameter("application/json", body, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                if (Convert.ToString(response.StatusCode) == "429")
                {
                    System.Threading.Thread.Sleep(2000);
                    goto Restart;
                }
                if (response.StatusCode == HttpStatusCode.OK)
                    result = true;

                FileHelper.WriteExceptionMessage(brandName, LogStatus: "INFO", LogErrorMessage: "ShopifyClass.UpdateVariantsPolicy - End ");
            }
            catch (Exception ex)
            {
                FileHelper.WriteExceptionMessage(brandName, LogStatus: "ERROR", LogErrorMessage: "ShopifyClass.UpdateVariantsPolicy - Error" + ex.Message, LogInnerErrorMessage: ex.InnerException != null ? "" : ex.InnerException.Message);
            }
            return result;
        }

        public bool UpdateMetafields(string brandName, string id, string body)
        {
            bool result = false;
            Restart:
            try
            {
                FileHelper.WriteExceptionMessage(brandName, LogStatus: "INFO", LogErrorMessage: "ShopifyClass.UpdateMetafields - Start");
                var client = new RestClient(BaseURL + string.Format(MetafieldsUpdate, id));
                var request = new RestRequest(Method.PUT);
                request.AddHeader("x-shopify-access-token", AuthenticationToken);
                request.AddHeader("Content-Type", "application/json");
                request.AddParameter("application/json", body, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                if (Convert.ToString(response.StatusCode) == "429")
                {
                    System.Threading.Thread.Sleep(2000);
                    goto Restart;
                }
                if (response.StatusCode == HttpStatusCode.OK)
                    result = true;

                FileHelper.WriteExceptionMessage(brandName, LogStatus: "INFO", LogErrorMessage: "ShopifyClass.UpdateMetafields - End ");
            }
            catch (Exception ex)
            {
                FileHelper.WriteExceptionMessage(brandName, LogStatus: "ERROR", LogErrorMessage: "ShopifyClass.RegisterCustomer - Error" + ex.Message, LogInnerErrorMessage: ex.InnerException != null ? "" : ex.InnerException.Message);
            }
            return result;
        }

        public ProductVariantMetafieldResponse AddVariantMetafields(string brandName, string productId, string variantId, ProductVariantMetafieldRequest varianstMetafieldsRequest)
        {
            Restart:
            try
            {
                var client = new RestClient(BaseURL + string.Format(AddNewVariantsMetafields, productId, variantId));
                var request = new RestRequest(Method.POST);
                AddHeader(request);
                var json = new JavaScriptSerializer().Serialize(varianstMetafieldsRequest).Replace("_namespace", "namespace");
                request.AddParameter("application/json", json, ParameterType.RequestBody);
                //  request.AddJsonBody(customerMetafieldsRequest);
                IRestResponse response = client.Execute(request);
                if (Convert.ToString(response.StatusCode) == "429")
                {
                    System.Threading.Thread.Sleep(2000);
                    goto Restart;
                }
                return JsonConvert.DeserializeObject<ProductVariantMetafieldResponse>(response.Content);
            }
            catch (Exception)
            { }

            return null;
        }
        #endregion

        #region get graphql product Variants 
        public GraphQlResponse.PreOrder.GraphQlPreOrderProductVariantsResponse GetGraphQlVariants(string graphqlOrderRequestString, string brandName)
        {
            Restart:
            try
            {
                FileHelper.WriteExceptionMessage(brandName, LogStatus: "Info", LogErrorMessage: "Shopify Class Graph Ql Order  " + graphqlOrderRequestString);
                var client = new RestClient(BaseURL + "/api/graphql");
                var request = new RestRequest(Method.POST);
                AddHeaderGraphQl(request);
                request.AddParameter("application/graphql", graphqlOrderRequestString, ParameterType.RequestBody);
                IRestResponse<GraphQlResponse.PreOrder.GraphQlPreOrderProductVariantsResponse> response = client.Execute<GraphQlResponse.PreOrder.GraphQlPreOrderProductVariantsResponse>(request);
                if (Convert.ToString(response.StatusCode) == "429")
                {
                    System.Threading.Thread.Sleep(2000);
                    goto Restart;
                }
                return JsonConvert.DeserializeObject<GraphQlResponse.PreOrder.GraphQlPreOrderProductVariantsResponse>(response.Content);
            }
            catch (Exception ex)
            {
                string InnerException = ex.ToString();
                if (ex.InnerException != null)
                    InnerException = ex.InnerException.ToString();
                // EmailManager.SendMail(InnerException, "Steve Madden Sync Customer Job Error in GetCustomerBySinceId");
                FileHelper.WriteExceptionMessage(brandName, LogStatus: "ERROR", LogErrorMessage: "Shopify Class Graph Ql Order Error" + InnerException);
                return null;
            }
        }
        #endregion


        #region get graphql Orders linkshare
        public GraphQlResponse.linkShare.GraphQlOrderResponseLinkShare GetGraphQlOrderLinkShare(string graphqlOrderRequestString, string brandName)
        {
            Restart:
            try
            {
                FileHelper.WriteExceptionMessage(brandName, LogStatus: "Info", LogErrorMessage: "Shopify Class Graph Ql Order  " + graphqlOrderRequestString);
                var client = new RestClient(BaseURL + "/api/graphql");
                var request = new RestRequest(Method.POST);
                AddHeaderGraphQl(request);
                request.AddParameter("application/graphql", graphqlOrderRequestString, ParameterType.RequestBody);
                IRestResponse<GraphQlResponse.linkShare.GraphQlOrderResponseLinkShare> response = client.Execute<GraphQlResponse.linkShare.GraphQlOrderResponseLinkShare>(request);
                if (Convert.ToString(response.StatusCode) == "429")
                {
                    System.Threading.Thread.Sleep(2000);
                    goto Restart;
                }
                return JsonConvert.DeserializeObject<GraphQlResponse.linkShare.GraphQlOrderResponseLinkShare>(response.Content);
            }
            catch (Exception ex)
            {
                string InnerException = ex.ToString();
                if (ex.InnerException != null)
                    InnerException = ex.InnerException.ToString();
                // EmailManager.SendMail(InnerException, "Steve Madden Sync Customer Job Error in GetCustomerBySinceId");
                FileHelper.WriteExceptionMessage(brandName, LogStatus: "ERROR", LogErrorMessage: "Shopify Class Graph Ql Order Error" + InnerException);
                return null;
            }
        }
        #endregion


        #region get graphql Orders Fulfillment status
        public GraphQlResponse.ShippingState.GraphQlOrderFulfillmentStateResponse GetGraphQlOrderShipmentStatus(string graphqlOrderRequestString, string brandName)
        {
            Restart:
            try
            {
                FileHelper.WriteExceptionMessage(brandName, LogStatus: "Info", LogErrorMessage: "Shopify Class Graph Ql Order  " + graphqlOrderRequestString);
                var client = new RestClient(BaseURL + "/api/graphql");
                var request = new RestRequest(Method.POST);
                AddHeaderGraphQl(request);
                request.AddParameter("application/graphql", graphqlOrderRequestString, ParameterType.RequestBody);
                IRestResponse<GraphQlResponse.ShippingState.GraphQlOrderFulfillmentStateResponse> response = client.Execute<GraphQlResponse.ShippingState.GraphQlOrderFulfillmentStateResponse>(request);
                if (Convert.ToString(response.StatusCode) == "429")
                {
                    System.Threading.Thread.Sleep(2000);
                    goto Restart;
                }
                return JsonConvert.DeserializeObject<GraphQlResponse.ShippingState.GraphQlOrderFulfillmentStateResponse>(response.Content);
            }
            catch (Exception ex)
            {
                string InnerException = ex.ToString();
                if (ex.InnerException != null)
                    InnerException = ex.InnerException.ToString();
                // EmailManager.SendMail(InnerException, "Steve Madden Sync Customer Job Error in GetCustomerBySinceId");
                FileHelper.WriteExceptionMessage(brandName, LogStatus: "ERROR", LogErrorMessage: "Shopify Class Graph Ql Order Error" + InnerException);
                return null;
            }
        }
        #endregion

        #region get graphql Orders
        public GraphQlResponse.GraphQlOrderResponse GetGraphQlOrder(string graphqlOrderRequestString, string brandName)
        {
            Restart:
            try
            {
                FileHelper.WriteExceptionMessage(brandName, LogStatus: "Info", LogErrorMessage: "Shopify Class Graph Ql Order  " + graphqlOrderRequestString);
                var client = new RestClient(BaseURL + "/api/graphql");
                var request = new RestRequest(Method.POST);
                AddHeaderGraphQl(request);
                request.AddParameter("application/graphql", graphqlOrderRequestString, ParameterType.RequestBody);
                IRestResponse<GraphQlResponse.GraphQlOrderResponse> response = client.Execute<GraphQlResponse.GraphQlOrderResponse>(request);
                if (Convert.ToString(response.StatusCode) == "429")
                {
                    System.Threading.Thread.Sleep(2000);
                    goto Restart;
                }
                return JsonConvert.DeserializeObject<GraphQlResponse.GraphQlOrderResponse>(response.Content);
            }
            catch (Exception ex)
            {
                string InnerException = ex.ToString();
                if (ex.InnerException != null)
                    InnerException = ex.InnerException.ToString();
                // EmailManager.SendMail(InnerException, "Steve Madden Sync Customer Job Error in GetCustomerBySinceId");
                FileHelper.WriteExceptionMessage(brandName, LogStatus: "ERROR", LogErrorMessage: "Shopify Class Graph Ql Order Error" + InnerException);
                return null;
            }
        }
        #endregion

        public GraphQlResponse.MexicoCost.GraphQLVariantResponse GetVariantIdBybarcode(string graphqlProductRequestString)
        {
            restart:
            try
            {

                var client = new RestClient(BaseURL + "/api/graphql");
                var request = new RestRequest(Method.POST);
                AddHeaderGraphQl(request);
                request.AddParameter("application/graphql", graphqlProductRequestString, ParameterType.RequestBody);
                IRestResponse<GraphQlResponse.MexicoCost.GraphQLVariantResponse> response = client.Execute<GraphQlResponse.MexicoCost.GraphQLVariantResponse>(request);
                if (Convert.ToString(response.StatusCode) == "429")
                    goto restart;
                return JsonConvert.DeserializeObject<GraphQlResponse.MexicoCost.GraphQLVariantResponse>(response.Content);
            }
            catch (Exception ex)
            {
                string InnerException = ex.ToString();
                if (ex.InnerException != null)
                    InnerException = ex.InnerException.ToString();
                //EmailManager.SendMail(InnerException, "Steve Madden Sync Customer Job Error in GetCustomerBySinceId");
                //sFileHelper.WriteExceptionMessage(brandName, LogStatus: "ERROR", LogErrorMessage: "Shopify Class Graph Ql Product Error" + InnerException);
                return null;
            }
        }

        #region get graphql InventoryItemId by Upc
        public GraphQlResponse.Inventory.GraphQlInventoryItemIdByUPC GetGraphQlInventoryItemId(string graphqlProductRequestString, string brandName)
        {
            restart:
            try
            {
                FileHelper.WriteExceptionMessage(brandName, LogStatus: "Info", LogErrorMessage: "Shopify Class Graph Ql product  " + graphqlProductRequestString);
                var client = new RestClient(BaseURL + "/api/graphql");
                var request = new RestRequest(Method.POST);
                AddHeaderGraphQl(request);
                request.AddParameter("application/graphql", graphqlProductRequestString, ParameterType.RequestBody);
                IRestResponse<GraphQlResponse.Inventory.GraphQlInventoryItemIdByUPC> response = client.Execute<GraphQlResponse.Inventory.GraphQlInventoryItemIdByUPC>(request);
                if (Convert.ToString(response.StatusCode) == "429")
                {
                    System.Threading.Thread.Sleep(2000);
                    goto restart;
                }
                return JsonConvert.DeserializeObject<GraphQlResponse.Inventory.GraphQlInventoryItemIdByUPC>(response.Content);
            }
            catch (Exception ex)
            {
                string InnerException = ex.ToString();
                if (ex.InnerException != null)
                    InnerException = ex.InnerException.ToString();
                // EmailManager.SendMail(InnerException, "Steve Madden Sync Customer Job Error in GetCustomerBySinceId");
                FileHelper.WriteExceptionMessage(brandName, LogStatus: "ERROR", LogErrorMessage: "Shopify Class Graph Ql Product Error" + InnerException);
                return null;
            }
        }
        #endregion

        #region get graphql Product id
        public GraphQlResponse.CollectionProduct.GraphQlProductIdResponse GetGraphQlProductIdResponse(string graphqlRequestString, string brandName)
        {
            restart:
            try
            {
                FileHelper.WriteExceptionMessage(brandName, LogStatus: "Info", LogErrorMessage: "Shopify Class Graph Ql GetCollectionByQtyResponse  " + graphqlRequestString);
                var client = new RestClient(BaseURL + "/api/graphql");
                var request = new RestRequest(Method.POST);
                AddHeaderGraphQl(request);
                request.AddParameter("application/graphql", graphqlRequestString, ParameterType.RequestBody);
                IRestResponse<GraphQlResponse.Product.GraphQlProductResponse> response = client.Execute<GraphQlResponse.Product.GraphQlProductResponse>(request);
                if (Convert.ToString(response.StatusCode) == "429")
                {
                    System.Threading.Thread.Sleep(2000);
                    goto restart;
                }
                return JsonConvert.DeserializeObject<GraphQlResponse.CollectionProduct.GraphQlProductIdResponse>(response.Content);
            }
            catch (Exception ex)
            {
                string InnerException = ex.ToString();
                if (ex.InnerException != null)
                    InnerException = ex.InnerException.ToString();
                // EmailManager.SendMail(InnerException, "Steve Madden Sync Customer Job Error in GetCustomerBySinceId");
                FileHelper.WriteExceptionMessage(brandName, LogStatus: "ERROR", LogErrorMessage: "Shopify Class Graph Ql GetCollectionByQtyResponse Error" + InnerException);
                return null;
            }
        }
        #endregion

        #region get graphql Variants Qty
        public GraphQlResponse.Collection.CollectionByQtyResponse GetCollectionByQtyResponse(string graphqlRequestString, string brandName)
        {
            restart:
            try
            {
                FileHelper.WriteExceptionMessage(brandName, LogStatus: "Info", LogErrorMessage: "Shopify Class Graph Ql GetCollectionByQtyResponse  " + graphqlRequestString);
                var client = new RestClient(BaseURL + "/api/graphql");
                var request = new RestRequest(Method.POST);
                AddHeaderGraphQl(request);
                request.AddParameter("application/graphql", graphqlRequestString, ParameterType.RequestBody);
                IRestResponse<GraphQlResponse.Product.GraphQlProductResponse> response = client.Execute<GraphQlResponse.Product.GraphQlProductResponse>(request);
                if (Convert.ToString(response.StatusCode) == "429")
                {
                    System.Threading.Thread.Sleep(2000);
                    goto restart;
                }
                return JsonConvert.DeserializeObject<GraphQlResponse.Collection.CollectionByQtyResponse>(response.Content);
            }
            catch (Exception ex)
            {
                string InnerException = ex.ToString();
                if (ex.InnerException != null)
                    InnerException = ex.InnerException.ToString();
                // EmailManager.SendMail(InnerException, "Steve Madden Sync Customer Job Error in GetCustomerBySinceId");
                FileHelper.WriteExceptionMessage(brandName, LogStatus: "ERROR", LogErrorMessage: "Shopify Class Graph Ql GetCollectionByQtyResponse Error" + InnerException);
                return null;
            }
        }
        #endregion

        #region 
        public void ExpireDiscountCode(string brandName, string DiscountCode)
        {
            FileHelper.WriteExceptionMessage(brandName, LogStatus: "INFO", LogErrorMessage: "shopifyClass.DiscountCodeCheck - Start");
            restart:
            try
            {
                var request = new RestRequest(string.Format(DiscountLookup, DiscountCode), Method.GET);
                var tResponse = Execute<Response.RootDiscount>(request);
                FileHelper.WriteExceptionMessage(brandName, LogStatus: "INFO", LogErrorMessage: "ShopifyClass.UpdateDiscountUsage - " + Convert.ToString(tResponse.StatusCode));
                if (Convert.ToString(tResponse.StatusCode) == "429")
                {
                    System.Threading.Thread.Sleep(2000);
                    goto restart;
                }
                if (tResponse.StatusCode == HttpStatusCode.OK)
                {
                    var rootdiscount = JsonConvert.DeserializeObject<Response.RootDiscount>(tResponse.Content);
                    if (rootdiscount != null && rootdiscount.discount_code != null)
                    {
                        UpdateDiscountUsage(brandName, Convert.ToString(rootdiscount.discount_code.price_rule_id), Convert.ToString(rootdiscount.discount_code.id));
                        FileHelper.WriteExceptionMessage(brandName, LogStatus: "INFO", LogErrorMessage: "PriceRule was called completed");
                    }
                }
            }
            catch (Exception ex)
            {
                string InnerException = ex.ToString();
                if (ex.InnerException != null)
                    InnerException = ex.InnerException.ToString();
                FileHelper.WriteExceptionMessage(brandName, LogStatus: "ERROR", LogErrorMessage: "ExpireDiscountCode Error " + InnerException);

            }

            FileHelper.WriteExceptionMessage(brandName, LogStatus: "INFO", LogErrorMessage: "shopifyClass.DiscountCodeCheck - End");
        }

        public bool UpdateDiscountUsage(string brandName, string price_rule_id, string discount_code_id)
        {
            FileHelper.WriteExceptionMessage(brandName, LogStatus: "INFO", LogErrorMessage: "ShopifyClass.UpdateCustomer - Start");
            restart:
            try
            {
                var client = new RestClient(BaseURL + string.Format(DeleteDiscountCode, price_rule_id, discount_code_id));
                var request = new RestRequest(Method.DELETE);
                AddHeader(request);
                //var Json = JsonConvert.SerializeObject(rootDiscount, Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

                //request.AddParameter("application/json", Json, ParameterType.RequestBody);
                FileHelper.WriteExceptionMessage(brandName, LogStatus: "INFO", LogErrorMessage: "client request execute :  client.BaseUrl : " + client.BaseUrl + " clientURL : " + request.Resource);
                IRestResponse response = client.Execute(request);
                FileHelper.WriteExceptionMessage(brandName, LogStatus: "INFO", LogErrorMessage: "get Client Response Status Code : " + response.StatusCode);
                FileHelper.WriteExceptionMessage(brandName, LogStatus: "INFO", LogErrorMessage: "ShopifyClass.UpdateDiscountUsage - " + Convert.ToString(response.StatusCode));
                if (Convert.ToString(response.StatusCode) == "429")
                {
                    System.Threading.Thread.Sleep(2000);
                    goto restart;
                }
                if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Accepted)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                string InnerException = ex.ToString();
                if (ex.InnerException != null)
                    InnerException = ex.InnerException.ToString();
                FileHelper.WriteExceptionMessage(brandName, LogStatus: "ERROR", LogErrorMessage: "Shopify Class UpdateDiscountUsage Error" + InnerException);
            }
            return false;
        }
        
        #endregion


        #region get graphql Products SKU By Tags
        public GraphQlResponse.SoldOut.GraphQlProductResponseByTags GetGraphQlProductSKUByTags(string graphqlProductRequestString, string brandName)
        {
            restart:
            try
            {
                FileHelper.WriteExceptionMessage(brandName, LogStatus: "Info", LogErrorMessage: "Shopify Class Graph Ql product  " + graphqlProductRequestString);
                var client = new RestClient(BaseURL + "/api/graphql");
                var request = new RestRequest(Method.POST);
                AddHeaderGraphQl(request);
                request.AddParameter("application/graphql", graphqlProductRequestString, ParameterType.RequestBody);
                IRestResponse<GraphQlResponse.SoldOut.GraphQlProductResponseByTags> response = client.Execute<GraphQlResponse.SoldOut.GraphQlProductResponseByTags>(request);
                if (Convert.ToString(response.StatusCode) == "429")
                {
                    System.Threading.Thread.Sleep(2000);
                    goto restart;
                }
                return JsonConvert.DeserializeObject<GraphQlResponse.SoldOut.GraphQlProductResponseByTags>(response.Content);
            }
            catch (Exception ex)
            {
                string InnerException = ex.ToString();
                if (ex.InnerException != null)
                    InnerException = ex.InnerException.ToString();
                // EmailManager.SendMail(InnerException, "Steve Madden Sync Customer Job Error in GetCustomerBySinceId");
                FileHelper.WriteExceptionMessage(brandName, LogStatus: "ERROR", LogErrorMessage: "Shopify Class Graph Ql Product Error" + InnerException);
                return null;
            }
        }
        #endregion

        #region get graphql Products By Tags
        public GraphQlResponse.SoldOut.GraphQlProductResponseByTags GetGraphQlProductByTags(string graphqlProductRequestString, string brandName)
        {
            restart:
            try
            {
                FileHelper.WriteExceptionMessage(brandName, LogStatus: "Info", LogErrorMessage: "Shopify Class Graph Ql product  " + graphqlProductRequestString);
                var client = new RestClient(BaseURL + "/api/graphql");
                var request = new RestRequest(Method.POST);
                AddHeaderGraphQl(request);
                request.AddParameter("application/graphql", graphqlProductRequestString, ParameterType.RequestBody);
                IRestResponse<GraphQlResponse.SoldOut.GraphQlProductResponseByTags> response = client.Execute<GraphQlResponse.SoldOut.GraphQlProductResponseByTags>(request);
                if (Convert.ToString(response.StatusCode) == "429")
                {
                    System.Threading.Thread.Sleep(2000);
                    goto restart;
                }
                return JsonConvert.DeserializeObject<GraphQlResponse.SoldOut.GraphQlProductResponseByTags>(response.Content);
            }
            catch (Exception ex)
            {
                string InnerException = ex.ToString();
                if (ex.InnerException != null)
                    InnerException = ex.InnerException.ToString();
                // EmailManager.SendMail(InnerException, "Steve Madden Sync Customer Job Error in GetCustomerBySinceId");
                FileHelper.WriteExceptionMessage(brandName, LogStatus: "ERROR", LogErrorMessage: "Shopify Class Graph Ql Product Error" + InnerException);
                return null;
            }
        }
        #endregion

        #region get graphql Products
        public GraphQlResponse.Product.GraphQlProductResponse GetGraphQlProduct(string graphqlProductRequestString, string brandName)
        {
            restart:
            try
            {
                FileHelper.WriteExceptionMessage(brandName, LogStatus: "Info", LogErrorMessage: "Shopify Class Graph Ql product  " + graphqlProductRequestString);
                var client = new RestClient(BaseURL + "/api/graphql");
                var request = new RestRequest(Method.POST);
                AddHeaderGraphQl(request);
                request.AddParameter("application/graphql", graphqlProductRequestString, ParameterType.RequestBody);
                IRestResponse<GraphQlResponse.Product.GraphQlProductResponse> response = client.Execute<GraphQlResponse.Product.GraphQlProductResponse>(request);
                if (Convert.ToString(response.StatusCode) == "429")
                {
                    System.Threading.Thread.Sleep(2000);
                    goto restart;
                }
                return JsonConvert.DeserializeObject<GraphQlResponse.Product.GraphQlProductResponse>(response.Content);
            }
            catch (Exception ex)
            {
                string InnerException = ex.ToString();
                if (ex.InnerException != null)
                    InnerException = ex.InnerException.ToString();
                // EmailManager.SendMail(InnerException, "Steve Madden Sync Customer Job Error in GetCustomerBySinceId");
                FileHelper.WriteExceptionMessage(brandName, LogStatus: "ERROR", LogErrorMessage: "Shopify Class Graph Ql Product Error" + InnerException);
                return null;
            }
        }
        #endregion
        #region Get Price Rule List
        public PriceRuleListViewModel GetPriceRules(out string next_page_info, int limit,string brandName, string url = "")
        {
            next_page_info = "";
            Restart:
            try
            {
                if (string.IsNullOrEmpty(url))
                {
                    url = string.Format(PriceRules, limit);
                }
                FileHelper.WriteExceptionMessage(brandName, LogStatus: "INFO", LogErrorMessage: "URL : " + url);
                var request = new RestRequest(url, Method.GET);
                request.AddHeader("content-type", "application/json");
                IRestResponse<PriceRuleListViewModel> response = Execute<PriceRuleListViewModel>(request);
                if (Convert.ToString(response.StatusCode) == "429")
                {
                    System.Threading.Thread.Sleep(2000);
                    goto Restart;
                }
                next_page_info = GetNextPageInfofromHeader(next_page_info, response.Headers, brandName);
                return JsonConvert.DeserializeObject<PriceRuleListViewModel>(response.Content);
            }
            catch (Exception ex)
            {
                string InnerException = ex.ToString();
                if (ex.InnerException != null)
                    InnerException = ex.InnerException.ToString();
                FileHelper.WriteExceptionMessage(brandName, LogStatus: "ERROR", LogErrorMessage: "Exception" + InnerException);
                return null;
            }
        }
        #endregion

        #region Get Discount Code List

        public DiscountCodeListViewModel GetDiscountCodeList(out string next_page_info,string price_rule_id, int limit, string brandName, string url = "")
        {
            next_page_info = "";
            Restart:
            try
            {
                if (string.IsNullOrEmpty(url))
                {
                    url = string.Format(discountCodeList,price_rule_id, limit);
                }
                FileHelper.WriteExceptionMessage(brandName, LogStatus: "INFO", LogErrorMessage: "URL : " + url);
                var request = new RestRequest(url, Method.GET);
                request.AddHeader("content-type", "application/json");
                IRestResponse<DiscountCodeListViewModel> response = Execute<DiscountCodeListViewModel>(request);
                if (Convert.ToString(response.StatusCode) == "429")
                {
                    System.Threading.Thread.Sleep(2000);
                    goto Restart;
                }
                next_page_info = GetNextPageInfofromHeader(next_page_info, response.Headers, brandName);
                return JsonConvert.DeserializeObject<DiscountCodeListViewModel>(response.Content);
            }
            catch (Exception ex)
            {
                string InnerException = ex.ToString();
                if (ex.InnerException != null)
                    InnerException = ex.InnerException.ToString();
                FileHelper.WriteExceptionMessage(brandName, LogStatus: "ERROR", LogErrorMessage: "Exception" + InnerException);
                return null;
            }
        }
        #endregion

        #region Get Order refunds
        public RootobjectRefund GetOrderRefundById(string OrderId)
        {
            try
            {
                int retry = 0;
                Retry:
                string url = string.Format(Getrefunds, OrderId);
                var request = new RestRequest(url, Method.GET);
                request.AddHeader("content-type", "application/json");
                IRestResponse<RootobjectRefund> response = Execute<RootobjectRefund>(request);
                if (Convert.ToString(response.StatusCode) == "429")
                {
                    System.Threading.Thread.Sleep(2000);
                    goto Retry;
                }
                if (response.StatusCode == HttpStatusCode.OK)
                    return JsonConvert.DeserializeObject<RootobjectRefund>(response.Content);
                else
                {
                    if (retry != 2)
                    {
                        retry = retry + 1;
                        FileHelper.WriteExceptionMessage("SMRefund", LogStatus: "INFO", LogErrorMessage: " Retry " + retry);
                        goto Retry;
                    }
                }
            }
            catch (Exception ex)
            {
                string InnerException = ex.ToString();
                if (ex.InnerException != null)
                    InnerException = ex.InnerException.ToString();
                FileHelper.WriteExceptionMessage("SMRefund", LogStatus: "ERROR", LogErrorMessage: InnerException);
                return null;
            }
            return null;
        }
        #endregion
        public bool publishProductById(string brandName, string query, dynamic productId, out string message)
        {
            bool status = false; message = "";
            int index = 0;
            retry:
            try
            {
                FileHelper.WriteExceptionMessage(brandName, LogStatus: "INFO", LogErrorMessage: "ShopifyClass.publishProductById - Start");

                var client = new RestClient(BaseURL + string.Format(publishProduct, productId));
                var request = new RestRequest(Method.PUT);
                AddHeader(request);
                request.AddParameter("application/json", query, ParameterType.RequestBody);
                FileHelper.WriteExceptionMessage(brandName, LogStatus: "INFO", LogErrorMessage: "client request execute :  client.BaseUrl : " + client.BaseUrl + " clientURL : " + request.Resource);
                IRestResponse response = client.Execute(request);
                FileHelper.WriteExceptionMessage(brandName, LogStatus: "INFO", LogErrorMessage: "get Client Response Status Code : " + response.StatusCode);
                if (response.StatusDescription.Contains("Too Many Requests"))
                {
                    System.Threading.Thread.Sleep(5000);
                    goto retry;
                }
                if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Created || response.StatusCode == HttpStatusCode.Accepted)
                {
                    status = true;
                    message = "Successfully Updated.";
                }
                FileHelper.WriteExceptionMessage(brandName, LogStatus: "INFO", LogErrorMessage: "ShopifyClass.UpdateCustomer - End : message : " + message + " status : " + status);
                return status;
            }
            catch (Exception ex)
            {
                if (index <= 2)
                {
                    string innnere = ex.ToString();
                    if (ex.InnerException != null)
                        innnere = ex.InnerException.Message;
                    FileHelper.WriteExceptionMessage(brandName, LogStatus: "Error", LogErrorMessage: " GetProductListByHandle " + innnere);
                    index = index + 1;
                    System.Threading.Thread.Sleep(10000);
                    goto retry;
                }
            }
            return status;
        }
        public Response.InventoryLevelResponse GetInventoryLevel(string brandName, string inventory_item_id, string locationid = "")
        {
            int index = 0;
            FileHelper.WriteExceptionMessage(brandName, LogStatus: "INFO", LogErrorMessage: "shopifyClass.GetInventoryLevel - Start");
            restart:
            try
            {

                string inventoryUrl = string.Format(InventoryLevel, inventory_item_id);
                if (!string.IsNullOrEmpty(locationid))
                    inventoryUrl = inventoryUrl + "&location_ids=" + locationid;
                FileHelper.WriteExceptionMessage(brandName, LogStatus: "INFO", LogErrorMessage: "shopifyClass.GetInventoryLevel - Url " + inventoryUrl);
                var request = new RestRequest(inventoryUrl, Method.GET);
                request.AddHeader("content-type", "application/json");
                FileHelper.WriteExceptionMessage(brandName, LogStatus: "INFO", LogErrorMessage: "shopifyClass.GetInventoryLevel - BaseUrl " + BaseURL);
                IRestResponse<Response.InventoryLevelResponse> response = Execute<Response.InventoryLevelResponse>(request);
                FileHelper.WriteExceptionMessage(brandName, LogStatus: "INFO", LogErrorMessage: "shopifyClass.GetInventoryLevel - response.StatusCode " + response.StatusCode);
                if (Convert.ToString(response.StatusCode) == "429")
                {
                    if (index <= 2)
                    {
                        index = index + 1;
                        System.Threading.Thread.Sleep(1000);
                        goto restart;
                    }
                    return null;
                }
                FileHelper.WriteExceptionMessage(brandName, LogStatus: "INFO", LogErrorMessage: "shopifyClass.GetInventoryLevel - response.Content " + response.Content);
                FileHelper.WriteExceptionMessage(brandName, LogStatus: "INFO", LogErrorMessage: "shopifyClass.GetInventoryLevel - End");
                return JsonConvert.DeserializeObject<Response.InventoryLevelResponse>(response.Content);
            }
            catch (Exception ex)
            {
                string InnerException = ex.ToString();
                if (ex.InnerException != null)
                    InnerException = ex.InnerException.ToString();
                FileHelper.WriteExceptionMessage(brandName, LogStatus: "Error", LogErrorMessage: "shopifyClass.GetInventoryLevel : " + InnerException);
                FileHelper.WriteExceptionMessage(brandName, LogStatus: "INFO", LogErrorMessage: "shopifyClass.GetInventoryLevel - End");
                if (index <= 2)
                {
                    index = index + 1;
                    System.Threading.Thread.Sleep(1000);
                    goto restart;
                }
                return null;
            }
        }


        #region Get Order Transaction
        public Rootobject GetOrderTransactionById(string OrderId)
        {
            try
            {
                int retry = 0;
                Retry:
                string url = string.Format(GetTransaction, OrderId);
                var request = new RestRequest(url, Method.GET);
                request.AddHeader("content-type", "application/json");
                IRestResponse<Rootobject> response = Execute<Rootobject>(request);
                if (Convert.ToString(response.StatusCode) == "429")
                {
                    System.Threading.Thread.Sleep(2000);
                    goto Retry;
                }
                if (response.StatusCode == HttpStatusCode.OK)
                    return JsonConvert.DeserializeObject<Rootobject>(response.Content);
                else
                {
                    if (retry != 2)
                    {
                        retry = retry + 1;
                        FileHelper.WriteExceptionMessage("SMTransactions", LogStatus: "INFO", LogErrorMessage: " Retry " + retry);
                        goto Retry;
                    }
                }
            }
            catch (Exception ex)
            {
                string InnerException = ex.ToString();
                if (ex.InnerException != null)
                    InnerException = ex.InnerException.ToString();
                FileHelper.WriteExceptionMessage("SMTransactions", LogStatus: "ERROR", LogErrorMessage: InnerException);
                return null;
            }
            return null;
        }
        #endregion
        #region CapturePaymentMethod
        public ShopifyOrders GetOrderById(string Ids)
        {
            Restart:
            try
            {
                string url = string.Format(SingleOrder, Ids) + "&status=any";
                var request = new RestRequest(url, Method.GET);
                request.AddHeader("content-type", "application/json");
                IRestResponse<ShopifyOrders> response = Execute<ShopifyOrders>(request);
                if (Convert.ToString(response.StatusCode) == "429")
                {
                    System.Threading.Thread.Sleep(2000);
                    goto Restart;
                }
                return JsonConvert.DeserializeObject<ShopifyOrders>(response.Content);
            }
            catch (Exception ex)
            {
                string InnerException = ex.ToString();
                if (ex.InnerException != null)
                    InnerException = ex.InnerException.ToString();
                return null;
            }
        }
        public Rootobject GetCapturePayment(long strOrderNumber, string brandName)
        {
            Restart:
            try
            {
                var request = new RestRequest(string.Format(CapturePayment, strOrderNumber), Method.GET);
                request.AddHeader("content-type", "application/json");

                IRestResponse<Rootobject> response = Execute<Rootobject>(request);
                if (Convert.ToString(response.StatusCode) == "429")
                {
                    System.Threading.Thread.Sleep(2000);
                    goto Restart;
                }
                return JsonConvert.DeserializeObject<Rootobject>(response.Content);
            }
            catch (Exception ex)
            {
                string InnerException = ex.ToString();
                if (ex.InnerException != null)
                    InnerException = ex.InnerException.ToString();
                FileHelper.WriteExceptionMessage(brandName, LogStatus: "ERROR", LogErrorMessage: "Shopify Class Transaction Error" + InnerException);
                return null;
            }
        }
        public Transaction CapturePaymentMethod(long strOrderNumber, decimal finalAmount, string kindstatus)
        {
            Restart:
            try
            {
                var json = JsonConvert.SerializeObject(new { transaction = new { kind = kindstatus, amount = finalAmount } });
                var request = new RestRequest(string.Format(CapturePayment, strOrderNumber), Method.POST);
                request.AddHeader("content-type", "application/json");
                request.AddParameter("application/json", json, ParameterType.RequestBody);
                IRestResponse<Transaction> response = Execute<Transaction>(request);
                if (Convert.ToString(response.StatusCode) == "429")
                {
                    System.Threading.Thread.Sleep(2000);
                    goto Restart;
                }
                return JsonConvert.DeserializeObject<Rootobject>(response.Content).transaction;
            }
            catch (Exception ex)
            {
                string InnerException = ex.ToString();
                if (ex.InnerException != null)
                    InnerException = ex.InnerException.ToString();
                return null;
            }
        }
        #endregion


        #region Order Count & List
        //public int GetOrderCount(string created_at_min = "", string created_at_max = "", string status = "", string financialStatus = "", string fileds = "", long since_id = 0, string updated_at_min = "", string updated_at_max = "", string fulfillment_status = "")
        //{
        //    restart:
        //    try
        //    {
        //        string url = OrderCount;
        //        if (!String.IsNullOrWhiteSpace(status))
        //            url += (url.IndexOf('?') < 0 ? "?" : "&") + "status=" + status;
        //        if (!String.IsNullOrWhiteSpace(created_at_min))
        //            url += (url.IndexOf('?') < 0 ? "?" : "&") + "created_at_min=" + created_at_min;
        //        if (!String.IsNullOrWhiteSpace(created_at_max))
        //            url += (url.IndexOf('?') < 0 ? "?" : "&") + "created_at_max=" + created_at_max;
        //        if (!String.IsNullOrEmpty(financialStatus))
        //            url += (url.IndexOf('?') < 0 ? "?" : "&") + "financial_status=" + financialStatus;
        //        if (!String.IsNullOrEmpty(updated_at_min))
        //            url += (url.IndexOf('?') < 0 ? "?" : "&") + "updated_at_min=" + updated_at_min;
        //        if (!String.IsNullOrEmpty(updated_at_max))
        //            url += (url.IndexOf('?') < 0 ? "?" : "&") + "updated_at_max=" + updated_at_max;
        //        if (!String.IsNullOrEmpty(fulfillment_status))
        //            url += (url.IndexOf('?') < 0 ? "?" : "&") + "fulfillment_status=" + fulfillment_status;
        //        if (since_id != 0)
        //            url += (url.IndexOf('?') < 0 ? "?" : "&") + "since_id=" + since_id;
        //        var request = new RestRequest(url, Method.GET);
        //        request.AddHeader("content-type", "application/json");
        //        IRestResponse<TotalOrderCount> response = Execute<TotalOrderCount>(request);
        //        if (response.StatusCode == HttpStatusCode.OK)
        //        {

        //            var count = JsonConvert.DeserializeObject<TotalOrderCount>(response.Content).count;
        //            FileHelper.WriteExceptionMessage("SMReport", LogStatus: "Info", LogErrorMessage: "Total Count :: " + count);
        //            return (count / 250);
        //        }
        //        else
        //            goto restart;


        //    }
        //    catch (Exception ex)
        //    {

        //        string InnerException = ex.ToString();
        //        if (ex.InnerException != null)
        //            InnerException = ex.InnerException.ToString();
        //        return 0;
        //    }

        //}
        public Shopify.Request.Variant GetVariantsById(string variantsId)
        {
            Restart:
            try
            {
                string url = string.Format(getVariants, variantsId);

                var request = new RestRequest(url, Method.GET);
                request.AddHeader("content-type", "application/json");
                IRestResponse<VariantRootobject> response = Execute<VariantRootobject>(request);
                if (Convert.ToString(response.StatusCode) == "429")
                {
                    System.Threading.Thread.Sleep(2000);
                    goto Restart;
                }
                return JsonConvert.DeserializeObject<VariantRootobject>(response.Content).variant;
            }
            catch (Exception ex)
            {
                string InnerException = ex.ToString();
                if (ex.InnerException != null)
                    InnerException = ex.InnerException.ToString();
                return null;
            }
        }
        public string GetOrderListJson(out string next_page_info, int limit, string created_at_min = "", string created_at_max = "", string status = "", string financialStatus = "", string fileds = "", long since_id = 0, string updated_at_min = "", string updated_at_max = "", string fulfillment_status = "", string url = "")
        {
            next_page_info = "";
            Restart:
            try
            {
                if (string.IsNullOrEmpty(url))
                {
                    url = string.Format(OrderList, limit);
                    if (!String.IsNullOrWhiteSpace(status))
                        url += (url.IndexOf('?') < 0 ? "?" : "&") + "status=" + status;
                    if (!string.IsNullOrWhiteSpace(created_at_min))
                        url += (url.IndexOf('?') < 0 ? "?" : "&") + "created_at_min=" + created_at_min;
                    if (!string.IsNullOrWhiteSpace(created_at_max))
                        url += (url.IndexOf('?') < 0 ? "?" : "&") + "created_at_max=" + created_at_max;
                    if (!String.IsNullOrEmpty(financialStatus))
                        url += (url.IndexOf('?') < 0 ? "?" : "&") + "financial_status=" + financialStatus;
                    if (!String.IsNullOrEmpty(updated_at_min))
                        url += (url.IndexOf('?') < 0 ? "?" : "&") + "updated_at_min=" + updated_at_min;
                    if (!String.IsNullOrEmpty(updated_at_max))
                        url += (url.IndexOf('?') < 0 ? "?" : "&") + "updated_at_max=" + updated_at_max;
                    if (!String.IsNullOrEmpty(fileds))
                        url += (url.IndexOf('?') < 0 ? "?" : "&") + "fields=" + fileds;
                    if (!String.IsNullOrEmpty(fulfillment_status))
                        url += (url.IndexOf('?') < 0 ? "?" : "&") + "fulfillment_status=" + fulfillment_status;
                    if (since_id != 0)
                        url += (url.IndexOf('?') < 0 ? "?" : "&") + "since_id=" + since_id;
                }
                FileHelper.WriteExceptionMessage("SMReport", LogStatus: "INFO", LogErrorMessage: "URL : " + url);
                var request = new RestRequest(url, Method.GET);
                request.AddHeader("content-type", "application/json");
                IRestResponse<ShopifyOrders> response = Execute<ShopifyOrders>(request);
                if (Convert.ToString(response.StatusCode) == "429")
                {
                    System.Threading.Thread.Sleep(2000);
                    goto Restart;
                }
                next_page_info = GetNextPageInfofromHeader(next_page_info, response.Headers, "GetOrderList");

                return response.Content;
            }
            catch (Exception ex)
            {
                string InnerException = ex.ToString();
                if (ex.InnerException != null)
                    InnerException = ex.InnerException.ToString();
                FileHelper.WriteExceptionMessage("SMReport", LogStatus: "ERROR", LogErrorMessage: "Exception" + InnerException);
                return null;
            }
        }
        private static string GetNextPageInfofromHeader(string next_page_info, IList<Parameter> headerList, string apiCallName)
        {
            try
            {
                var header = headerList.ToList().Where(X => X.Name == "Link").FirstOrDefault();
                if (header != null)
                {
                    if (header.Name == "Link")
                    {
                        string[] linkArray = Convert.ToString(header.Value).Split(';');
                        if (linkArray.Length > 0)
                        {
                            if (linkArray[1].ToLower().Contains("next"))
                            {
                                string St = Convert.ToString(header.Value);
                                next_page_info = St.Trim().Substring(1, St.LastIndexOf(">") - 1);
                            }
                            else
                            {
                                string[] subWords = linkArray[1].Split(',');
                                if (subWords.Length > 1)
                                {
                                    string St = Convert.ToString(subWords[1]);
                                    next_page_info = St.Trim().Substring(1, St.Trim().LastIndexOf(">") - 1);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                FileHelper.WriteExceptionMessage("HeadersPageInfo", LogStatus: "INFO", LogErrorMessage: "shopifyClass.GetNextPageInfor Error on API Call of : " + apiCallName + " _ " + ex.ToString());
            }
            return next_page_info;
        }

        #region CancelFulfillment
        public bool CancelFulfillment(string brandName, long orderId, long fulfillmentId)
        {
            Restart:
            try
            {
                var client = new RestClient(BaseURL + string.Format(CancelFulfil, orderId, fulfillmentId));
                var request = new RestRequest(Method.POST);

                AddHeader(request);
                IRestResponse response = client.Execute(request);
                if (Convert.ToString(response.StatusCode) == "429")
                {
                    System.Threading.Thread.Sleep(2000);
                    goto Restart;
                }
                else if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Created || response.StatusCode == HttpStatusCode.Accepted)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;

                if (ex.InnerException != null)
                    ErrorMessage = ex.InnerException.Message;
                FileHelper.WriteExceptionMessage(brandName, LogStatus: "INFO", LogErrorMessage: "ShopifyClass.CancelFulfillment - End : message : " + ErrorMessage);
                return false;
            }

        }
        #endregion

        public ShopifyOrders GetOrderList(out string next_page_info, int limit, string created_at_min = "", string created_at_max = "", string status = "", string financialStatus = "", string fileds = "", long since_id = 0, string updated_at_min = "", string updated_at_max = "", string fulfillment_status = "", string url = "")
        {
            next_page_info = "";
            Restart:
            try
            {
                if (string.IsNullOrEmpty(url))
                {
                    url = string.Format(OrderList, limit);
                    if (!String.IsNullOrWhiteSpace(status))
                        url += (url.IndexOf('?') < 0 ? "?" : "&") + "status=" + status;
                    if (!string.IsNullOrWhiteSpace(created_at_min))
                        url += (url.IndexOf('?') < 0 ? "?" : "&") + "created_at_min=" + created_at_min;
                    if (!string.IsNullOrWhiteSpace(created_at_max))
                        url += (url.IndexOf('?') < 0 ? "?" : "&") + "created_at_max=" + created_at_max;
                    if (!String.IsNullOrEmpty(financialStatus))
                        url += (url.IndexOf('?') < 0 ? "?" : "&") + "financial_status=" + financialStatus;
                    if (!String.IsNullOrEmpty(updated_at_min))
                        url += (url.IndexOf('?') < 0 ? "?" : "&") + "updated_at_min=" + updated_at_min;
                    if (!String.IsNullOrEmpty(updated_at_max))
                        url += (url.IndexOf('?') < 0 ? "?" : "&") + "updated_at_max=" + updated_at_max;
                    if (!String.IsNullOrEmpty(fileds))
                        url += (url.IndexOf('?') < 0 ? "?" : "&") + "fields=" + fileds;
                    if (!String.IsNullOrEmpty(fulfillment_status))
                        url += (url.IndexOf('?') < 0 ? "?" : "&") + "fulfillment_status=" + fulfillment_status;
                    if (since_id != 0)
                        url += (url.IndexOf('?') < 0 ? "?" : "&") + "since_id=" + since_id;
                }
                FileHelper.WriteExceptionMessage("SMReport", LogStatus: "INFO", LogErrorMessage: "URL : " + url);
                var request = new RestRequest(url, Method.GET);
                request.AddHeader("content-type", "application/json");
                IRestResponse<ShopifyOrders> response = Execute<ShopifyOrders>(request);
                if (Convert.ToString(response.StatusCode) == "429")
                {
                    System.Threading.Thread.Sleep(2000);
                    goto Restart;
                }
                next_page_info = GetNextPageInfofromHeader(next_page_info, response.Headers, "GetOrderList");
                return JsonConvert.DeserializeObject<ShopifyOrders>(response.Content);
            }
            catch (Exception ex)
            {
                string InnerException = ex.ToString();
                if (ex.InnerException != null)
                    InnerException = ex.InnerException.ToString();
                FileHelper.WriteExceptionMessage("SMReport", LogStatus: "ERROR", LogErrorMessage: "Exception" + InnerException);
                return null;
            }
        }


        #endregion

        #region Order Open
        public bool OpenArchiveOrder(string brandName, long orderId)
        {
            Restart:
            try
            {
                var request = new RestRequest(string.Format(openOrder, orderId), Method.POST);
                request.AddHeader("content-type", "application/json");

                IRestResponse<Order> response = Execute<Order>(request);
                if (Convert.ToString(response.StatusCode) == "429")
                {
                    System.Threading.Thread.Sleep(2000);
                    goto Restart;
                }
                else if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Created || response.StatusCode == HttpStatusCode.Accepted)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                string InnerException = ex.ToString();
                if (ex.InnerException != null)
                    InnerException = ex.InnerException.ToString();
                FileHelper.WriteExceptionMessage(brandName, LogStatus: "INFO", LogErrorMessage: "ShopifyClass.OpenArchiveOrder - End : message : " + InnerException);

            }
            return false;
        }
        #endregion

        #region PostFulfillment
        public void PutFulfillment(string brandName, string json, string fulfillment_id, string orderId)
        {
            Restart:
            try
            {
                var client = new RestClient(BaseURL + string.Format(putFulfillment, orderId, fulfillment_id));
                var request = new RestRequest(Method.PUT);

                request.AddParameter("application/json", json, ParameterType.RequestBody);
                AddHeader(request);
                FileHelper.WriteExceptionMessage(brandName, LogStatus: "INFO", LogErrorMessage: "PutFulfillment client request execute :  client.BaseUrl : " + client.BaseUrl + " clientURL : " + request.Resource);
                IRestResponse response = client.Execute(request);
                FileHelper.WriteExceptionMessage(brandName, LogStatus: "INFO", LogErrorMessage: "PutFulfillment get Client Response Status Code : " + response.StatusCode);
                if (Convert.ToString(response.StatusCode) == "429")
                {
                    System.Threading.Thread.Sleep(2000);
                    goto Restart;
                }
                if (response.StatusCode == HttpStatusCode.Created)
                {
                    JsonConvert.DeserializeObject<Shopify.Response.CustomerResponse>(response.Content);
                }
                else
                {
                    FileHelper.WriteExceptionMessage(brandName, LogStatus: "INFO", LogErrorMessage: "ShopifyClass.PutFulfillment - Error - STatus False", LogInnerErrorMessage: "Fulfillment Status False");
                }
            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                string InnerErrorMessage = "";
                if (ex.InnerException != null)
                    InnerErrorMessage = ex.InnerException.Message;
                FileHelper.WriteExceptionMessage(brandName, LogStatus: "ERROR", LogErrorMessage: "ShopifyClass.PostFulfillment - Error" + ErrorMessage, LogInnerErrorMessage: InnerErrorMessage);
            }
        }

        public void PostFulfillment(string brandName, FulfillmentRequestWithTrackingNumber fulfillmentRequest, string orderId)
        {
            int index = 0;
            Restart:
            try
            {
                var client = new RestClient(BaseURL + string.Format(fulfillment, orderId));
                var request = new RestRequest(Method.POST);
                request.AddJsonBody(fulfillmentRequest);
                AddHeader(request);
                FileHelper.WriteExceptionMessage(brandName, LogStatus: "INFO", LogErrorMessage: "PostFulfillment client request execute :  client.BaseUrl : " + client.BaseUrl + " clientURL : " + request.Resource);
                IRestResponse response = client.Execute(request);
                FileHelper.WriteExceptionMessage(brandName, LogStatus: "INFO", LogErrorMessage: "PostFulfillment get Client Response Status Code : " + response.StatusCode);
                if (Convert.ToString(response.StatusCode) == "429")
                {
                    System.Threading.Thread.Sleep(2000);
                    goto Restart;
                }
                if (response.StatusCode == HttpStatusCode.Created)
                {
                    JsonConvert.DeserializeObject<Shopify.Response.CustomerResponse>(response.Content);
                }
                else
                {
                    if (index == 0)
                    {
                        index = index + 1;
                        goto Restart;
                    }

                    FileHelper.WriteExceptionMessage(brandName, LogStatus: "INFO", LogErrorMessage: "ShopifyClass.PostFulfillment - Error - STatus False", LogInnerErrorMessage: "Fulfillment Status False");
                }
            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                string InnerErrorMessage = "";
                if (ex.InnerException != null)
                    InnerErrorMessage = ex.InnerException.Message;
                FileHelper.WriteExceptionMessage(brandName, LogStatus: "ERROR", LogErrorMessage: "ShopifyClass.PostFulfillment - Error" + ErrorMessage, LogInnerErrorMessage: InnerErrorMessage);
            }
        }
        #endregion

        #region get graphql Update Bulk Inventory
        public GraphQlResponse.BulkInventoryUpdate.GraphQlBulkUpdateInventoryResponse GetUpdateBulkInventory(string graphqlRequestString, string brandName)
        {
            restart:
            try
            {
                FileHelper.WriteExceptionMessage(brandName, LogStatus: "Info", LogErrorMessage: "Shopify Class Graph Ql GetInventoryItemIdByStyleName  " + graphqlRequestString);
                var client = new RestClient(BaseURL + "/api/graphql");
                var request = new RestRequest(Method.POST);
                AddHeaderGraphQl(request);
                request.AddParameter("application/graphql", graphqlRequestString, ParameterType.RequestBody);
                IRestResponse<GraphQlResponse.BulkInventoryUpdate.GraphQlBulkUpdateInventoryResponse> response = client.Execute<GraphQlResponse.BulkInventoryUpdate.GraphQlBulkUpdateInventoryResponse>(request);
                if (Convert.ToString(response.StatusCode) == "429")
                {
                    System.Threading.Thread.Sleep(2000);
                    goto restart;
                }
                return JsonConvert.DeserializeObject<GraphQlResponse.BulkInventoryUpdate.GraphQlBulkUpdateInventoryResponse>(response.Content);
            }
            catch (Exception ex)
            {
                string InnerException = ex.ToString();
                if (ex.InnerException != null)
                    InnerException = ex.InnerException.ToString();
                // EmailManager.SendMail(InnerException, "Steve Madden Sync Customer Job Error in GetCustomerBySinceId");
                FileHelper.WriteExceptionMessage(brandName, LogStatus: "ERROR", LogErrorMessage: "Shopify Class Graph Ql GetInventoryItemIdByStyleName Error" + InnerException);
                return null;
            }
        }
        #endregion

        #region Post Inventory Level
        public InventoryLevelResponse SetInventoryLevel(InventoryLevel inventoryLevel,string brandName)
        {
            retry:
            try
            {
                var request = new RestRequest(string.Format(UpdateInventory), Method.POST);
                request.AddHeader("Content-Type", "application/json");
                request.AddJsonBody(inventoryLevel);
                IRestResponse<InventoryLevelResponse> response = Execute<InventoryLevelResponse>(request);
                if (response.StatusDescription.Contains("Too Many Requests"))
                {
                    FileHelper.WriteExceptionMessage(brandName, LogStatus: "ERROR", LogErrorMessage: response.StatusDescription);
                    System.Threading.Thread.Sleep(2000);
                    goto retry;
                }
                return JsonConvert.DeserializeObject<InventoryLevelResponse>(response.Content);
            }
            catch (Exception ex)
            {
                string InnerException = ex.ToString();
                if (ex.InnerException != null)
                    InnerException = ex.InnerException.ToString();
                FileHelper.WriteExceptionMessage("inventoryLevel", LogStatus: "ERROR", LogErrorMessage: "Exception" + InnerException);
                return null;
            }

        }
        public InventoryItemResponse GetInventoryItem(string inventoryItem)
        {
            Restart:
            var request = new RestRequest(string.Format(InventoryItems, inventoryItem), Method.GET);
            request.AddHeader("Content-Type", "application/json");
            //request.AddJsonBody(inventoryItem);
            IRestResponse<InventoryItemResponse> response = Execute<InventoryItemResponse>(request);
            if (Convert.ToString(response.StatusCode) == "429")
            {
                System.Threading.Thread.Sleep(2000);
                goto Restart;
            }
            return JsonConvert.DeserializeObject<InventoryItemResponse>(response.Content);
        }
        public InventoryItemResponse SetInventoryItem(InventoryItemRootobject inventoryItem)
        {
            Restart:
            var request = new RestRequest(string.Format(InventoryItems, inventoryItem.inventory_item.id), Method.PUT);
            request.AddHeader("Content-Type", "application/json");
            request.AddJsonBody(inventoryItem);
            IRestResponse<InventoryItemResponse> response = Execute<InventoryItemResponse>(request);
            if (Convert.ToString(response.StatusCode) == "429")
            {
                System.Threading.Thread.Sleep(2000);
                goto Restart;
            }
            return JsonConvert.DeserializeObject<InventoryItemResponse>(response.Content);
        }

        public GetInventoryLevel GetLastInventoryLevel(long inventory_item_ids, string locationid = "")
        {
            retry:
            try
            {
                string inventoryUrl = string.Format(InventoryLevel, inventory_item_ids);
                if (!string.IsNullOrEmpty(locationid))
                    inventoryUrl = inventoryUrl + "&location_ids=" + locationid;
                var request = new RestRequest(inventoryUrl, Method.GET);
                request.AddHeader("Content-Type", "application/json");
                IRestResponse<GetInventoryLevel> response = Execute<GetInventoryLevel>(request);
                if (response.StatusDescription.Contains("Too Many Requests"))
                {
                    System.Threading.Thread.Sleep(2000);
                    goto retry;
                }
                return JsonConvert.DeserializeObject<GetInventoryLevel>(response.Content);
            }
            catch (Exception ex)
            {
                string innnere = ex.ToString();
                if (ex.InnerException != null)
                    innnere = ex.InnerException.Message;
                FileHelper.WriteExceptionMessage("inventoryLevel", LogStatus: "Error", LogErrorMessage: " GetProductListByHandle " + innnere);
                return null;
            }

        }

        #endregion

        #region Get LocationId
        public LocationObject GetLocation()
        {
            Restart:
            var request = new RestRequest(string.Format(Location), Method.GET);
            request.AddHeader("Content-Type", "application/json");
            IRestResponse<LocationObject> response = Execute<LocationObject>(request);
            if (Convert.ToString(response.StatusCode) == "429")
            {
                System.Threading.Thread.Sleep(2000);
                goto Restart;
            }
            return JsonConvert.DeserializeObject<LocationObject>(response.Content);
        }

        #endregion

        #region Product
        private void AddHeader(RestRequest request)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            request.AddHeader("X-Shopify-Access-Token", AuthenticationToken);
            request.AddHeader("content-type", "application/json");
        }
        public bool UpdateProductTags(string productId, string tags)
        {
            Restart:
            try
            {
                var client = new RestClient(BaseURL + string.Format(PutProduct, productId));
                var request = new RestRequest(Method.PUT);
                AddHeader(request);
                request.AddParameter("application/json", "{\"product\": {\"id\": " + productId + ",\"tags\": \"" + tags + "\"}}", ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                if (Convert.ToString(response.StatusCode) == "429")
                {
                    System.Threading.Thread.Sleep(2000);
                    goto Restart;
                }
                if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Created)
                    return true;
            }
            catch (Exception ex)
            {
                return false;
            }
            return false;
        }
        public bool UpdateProductVendor(string productId, string vendor)
        {
            restart:
            try
            {
                var client = new RestClient(BaseURL + string.Format(PutProduct, productId));
                var request = new RestRequest(Method.PUT);
                AddHeader(request);
                request.AddParameter("application/json", "{\"product\": {\"id\": " + productId + ",\"vendor\": \"" + vendor + "\"}}", ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                if (Convert.ToString(response.StatusCode) == "429")
                {
                    System.Threading.Thread.Sleep(2000);
                    goto restart;
                }
                if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Created)
                    return true;
            }
            catch (Exception ex)
            {
                return false;
            }
            return false;
        }
        //public ProductCount GetProductCount()
        //{
        //    Restart:
        //    var request = new RestRequest(string.Format(ProductCount), Method.GET);
        //    request.AddHeader("Content-Type", "application/json");
        //    IRestResponse<ProductCount> response = Execute<ProductCount>(request);
        //    if (Convert.ToString(response.StatusCode) == "429")
        //    {
        //        System.Threading.Thread.Sleep(5000);
        //        goto Restart;
        //    }
        //    return JsonConvert.DeserializeObject<ProductCount>(response.Content);
        //}
        public string GetProductListJson(int Page, int Limit)
        {
            int index = 0;
            Retry:
            try
            {
                var request = new RestRequest(string.Format(ProductList, Limit), Method.GET);
                request.AddHeader("Content-Type", "application/json");
                IRestResponse<Productobject> response = Execute<Productobject>(request);
                if (response.StatusDescription.Contains("Too Many Requests"))
                {
                    System.Threading.Thread.Sleep(2000);
                    goto Retry;
                }
                if (response.StatusCode == HttpStatusCode.OK)
                    return response.Content;
                else
                {
                    if (index <= 2)
                    {
                        index = index + 1;
                        goto Retry;
                    }
                    return null;
                }
            }
            catch (Exception ex)
            {
                if (index <= 2)
                    goto Retry;
                return null;
            }

        }


        public Productobject GetProductListByCollectionId(out string next_page_info,string collectionId, int Limit, string url = "")
        {
            next_page_info = "";
            int index = 0;
            Retry:
            try
            {
                string RequestURL = string.Format(productBycollection, Limit, collectionId);
                if (!string.IsNullOrEmpty(url))
                {
                    RequestURL = url;
                }
                var request = new RestRequest(RequestURL, Method.GET);
                request.AddHeader("Content-Type", "application/json");
                IRestResponse<Productobject> response = Execute<Productobject>(request);
                if (response.StatusDescription.Contains("Too Many Requests"))
                {
                    System.Threading.Thread.Sleep(5000);
                    goto Retry;
                }
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    next_page_info = GetNextPageInfofromHeader(next_page_info, response.Headers, "GetOrderList");
                    return JsonConvert.DeserializeObject<Productobject>(response.Content);
                }
                else
                {
                    if (index <= 2)
                    {
                        index = index + 1;
                        goto Retry;
                    }
                    return null;
                }
            }
            catch (Exception ex)
            {
                if (index <= 2)
                    goto Retry;
                return null;
            }

        }
        public Productobject GetProductList(out string next_page_info, int Limit, string url = "")
        {
            next_page_info = "";
            int index = 0;
            Retry:
            try
            {
                string RequestURL = string.Format(ProductList, Limit);
                if (!string.IsNullOrEmpty(url))
                {
                    RequestURL = url;
                }
                var request = new RestRequest(RequestURL, Method.GET);
                request.AddHeader("Content-Type", "application/json");
                IRestResponse<Productobject> response = Execute<Productobject>(request);
                if (response.StatusDescription.Contains("Too Many Requests"))
                {
                    System.Threading.Thread.Sleep(5000);
                    goto Retry;
                }
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    next_page_info = GetNextPageInfofromHeader(next_page_info, response.Headers, "GetOrderList");
                    return JsonConvert.DeserializeObject<Productobject>(response.Content);
                }
                else
                {
                    if (index <= 2)
                    {
                        index = index + 1;
                        goto Retry;
                    }
                    return null;
                }
            }
            catch (Exception ex)
            {
                if (index <= 2)
                    goto Retry;
                return null;
            }

        }
        public Productobject GetProductListByTitle(string Title)
        {
            int index = 0;
            Retry:
            try
            {
                var request = new RestRequest(string.Format(ProductListByTitle, Title), Method.GET);
                request.AddHeader("Content-Type", "application/json");
                IRestResponse<Productobject> response = Execute<Productobject>(request);
                if (Convert.ToString(response.StatusCode) == "429")
                {
                    System.Threading.Thread.Sleep(2000);
                    goto Retry;
                }
                return JsonConvert.DeserializeObject<Productobject>(response.Content);
            }
            catch (System.Exception ex)
            {
                if (index <= 2)
                    goto Retry;
                return null;
            }
        }
        public Productobject GetProductById(string productId, string brandName)
        {
            int index = 0;
            retry:
            try
            {
                var request = new RestRequest(string.Format(PutProduct, productId), Method.GET);
                request.AddHeader("Content-Type", "application/json");
                IRestResponse<Productobject> response = Execute<Productobject>(request);
                if (response.StatusDescription.Contains("Too Many Requests"))
                {
                    System.Threading.Thread.Sleep(3000);
                    goto retry;
                }
                FileHelper.WriteExceptionMessage(brandName, LogStatus: "Error", LogErrorMessage: " GetProductById Status " + response.StatusDescription + " " + response.StatusCode);
                return JsonConvert.DeserializeObject<Productobject>(response.Content);
            }
            catch (System.Exception ex)
            {
                if (index <= 2)
                {
                    string innnere = ex.ToString();
                    if (ex.InnerException != null)
                        innnere = ex.InnerException.Message;
                    FileHelper.WriteExceptionMessage(brandName, LogStatus: "Error", LogErrorMessage: " GetProductById " + innnere);
                    index = index + 1;
                    System.Threading.Thread.Sleep(10000);
                    goto retry;
                }

                return null;
            }
        }
        public Productobject GetProductListByHandle(string Handle, string brandName)
        {
            int index = 0;
            retry:
            try
            {
                var request = new RestRequest(string.Format(ProductListByhandle, Handle), Method.GET);
                request.AddHeader("Content-Type", "application/json");
                IRestResponse<Productobject> response = Execute<Productobject>(request);
                if (response.StatusDescription.Contains("Too Many Requests"))
                {
                    System.Threading.Thread.Sleep(10000);
                    goto retry;
                }
                FileHelper.WriteExceptionMessage(brandName, LogStatus: "Error", LogErrorMessage: " GetProductListByHandle Status " + response.StatusDescription + " " + response.StatusCode);
                return JsonConvert.DeserializeObject<Productobject>(response.Content);
            }
            catch (System.Exception ex)
            {
                if (index <= 2)
                {
                    string innnere = ex.ToString();
                    if (ex.InnerException != null)
                        innnere = ex.InnerException.Message;
                    FileHelper.WriteExceptionMessage(brandName, LogStatus: "Error", LogErrorMessage: " GetProductListByHandle " + innnere);
                    index = index + 1;
                    System.Threading.Thread.Sleep(10000);
                    goto retry;
                }

                return null;
            }
        }
        public VariantRootobject PostProductVariants(Shopify.Products.ProductVariantsRoot productVariant, string brandName)
        {
            Restart:
            try
            {
                var request = new RestRequest(string.Format(AddNewVariants, productVariant.variant.ProductId), Method.POST);
                request.AddHeader("Content-Type", "application/json");
                var Json = JsonConvert.SerializeObject(productVariant, Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                //request.AddJsonBody(Json);
                request.AddParameter("application/json", Json, ParameterType.RequestBody);
                IRestResponse<VariantRootobject> response = Execute<VariantRootobject>(request);
                if (Convert.ToString(response.StatusCode) == "429")
                {
                    System.Threading.Thread.Sleep(2000);
                    goto Restart;
                }
                return JsonConvert.DeserializeObject<VariantRootobject>(response.Content);
            }
            catch (System.Exception ex)
            {
                string InnerException = ex.ToString();
                if (ex.InnerException != null)
                    InnerException = ex.InnerException.ToString();
                FileHelper.WriteExceptionMessage(brandName, LogStatus: "ERROR", LogErrorMessage: "Shopify Class PostProductVariants Error" + InnerException);
                return null;
            }
        }
        public Productobject PostProduct(Shopify.Products.ProductRoot product, string brandName)
        {
            retry:
            try
            {
                var request = new RestRequest(string.Format(AddNewProduct), Method.POST);
                request.AddHeader("Content-Type", "application/json");
                var Json = JsonConvert.SerializeObject(product, Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                //request.AddJsonBody(Json);
                request.AddParameter("application/json", Json, ParameterType.RequestBody);
                IRestResponse<Productobject> response = Execute<Productobject>(request);
                if (response.StatusDescription.Contains("Too Many Requests"))
                {
                    System.Threading.Thread.Sleep(10000);
                    goto retry;
                }
                return JsonConvert.DeserializeObject<Productobject>(response.Content);
            }
            catch (System.Exception ex)
            {
                string InnerException = ex.ToString();
                if (ex.InnerException != null)
                    InnerException = ex.InnerException.ToString();
                FileHelper.WriteExceptionMessage(brandName, LogStatus: "ERROR", LogErrorMessage: "Shopify Class PostProduct Error" + InnerException);
                return null;
            }
        }

        #region graphql Product Publish
        public GraphQlResponse.Publish.GraphQLProductPublish UpdateProductPublish(string graphqlProductPublishString, string brandName)
        {
            retry:
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                var client = new RestClient(BaseURL + "/api/graphql");
                var request = new RestRequest(Method.POST);
                AddHeaderGraphQl(request);
                request.AddParameter("application/graphql", graphqlProductPublishString, ParameterType.RequestBody);
                IRestResponse<GraphQlResponse.Publish.GraphQLProductPublish> response = client.Execute<GraphQlResponse.Publish.GraphQLProductPublish>(request);
                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                };
                FileHelper.WriteExceptionMessage(brandName, LogStatus: "INFO", LogErrorMessage: "Shopify Class Graph Ql Product publish Response : " + response.Content);
                if (response.StatusDescription.Contains("Too Many Requests"))
                {
                    System.Threading.Thread.Sleep(10000);
                    goto retry;
                }
                return JsonConvert.DeserializeObject<GraphQlResponse.Publish.GraphQLProductPublish>(response.Content, settings);
            }
            catch (Exception ex)
            {
                string InnerException = ex.ToString();
                if (ex.InnerException != null)
                    InnerException = ex.InnerException.ToString();
                FileHelper.WriteExceptionMessage(brandName, LogStatus: "ERROR", LogErrorMessage: "Shopify Class Graph Ql Product publish Error" + InnerException);
                return null;
            }
        }

        public GraphQlResponse.UnPublish.GraphQLProductUnpublish UpdateProductUnpublish(string graphqlProductUnpublish, string brandName)
        {
            retry:
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                var client = new RestClient(BaseURL + "/api/graphql");
                var request = new RestRequest(Method.POST);
                AddHeaderGraphQl(request);
                request.AddParameter("application/graphql", graphqlProductUnpublish, ParameterType.RequestBody);
                IRestResponse<GraphQlResponse.UnPublish.GraphQLProductUnpublish> response = client.Execute<GraphQlResponse.UnPublish.GraphQLProductUnpublish>(request);
                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                };
                FileHelper.WriteExceptionMessage(brandName, LogStatus: "INFO", LogErrorMessage: "Shopify Class Graph Ql Product Unpublish Response : " + response.Content);
                if (response.StatusDescription.Contains("Too Many Requests"))
                {
                    System.Threading.Thread.Sleep(10000);
                    goto retry;
                }
                return JsonConvert.DeserializeObject<GraphQlResponse.UnPublish.GraphQLProductUnpublish>(response.Content, settings);
            }
            catch (Exception ex)
            {
                string InnerException = ex.ToString();
                if (ex.InnerException != null)
                    InnerException = ex.InnerException.ToString();
                FileHelper.WriteExceptionMessage(brandName, LogStatus: "ERROR", LogErrorMessage: "Shopify Class Graph Ql Product Unpublish Error" + InnerException);
                return null;
            }
        }

        #endregion

        public bool ShopifyInventoryConnect(string location_id, string inventory_item_id)
        {
            int index = 0;
            Retry:
            try
            {
                var request = new RestRequest(string.Format(InventoryConnect), Method.POST);
                request.AddHeader("Content-Type", "application/json");
                //var Json = JsonConvert.SerializeObject(variants, Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                //request.AddJsonBody(Json);
                request.AddParameter("application/json", "{  \"location_id\": " + location_id + ",  \"inventory_item_id\": " + inventory_item_id + "}", ParameterType.RequestBody);
                IRestResponse<dynamic> response = Execute<dynamic>(request);

                if (response.StatusCode == HttpStatusCode.Created || response.StatusCode == HttpStatusCode.OK)
                {
                    JsonConvert.DeserializeObject<dynamic>(response.Content);
                    return true;
                }
                else if (Convert.ToString(response.StatusCode) == "NotFound")
                {
                    return false;
                }
                else if (Convert.ToString(response.StatusCode) == "429")
                {
                    index = index + 1;
                    System.Threading.Thread.Sleep(1000);
                    goto Retry;
                }
                return true;
            }
            catch (Exception ex)
            {
                if (index != 2)
                {
                    index = index + 1;
                    System.Threading.Thread.Sleep(1000);
                    goto Retry;
                }
            }
            return true;
        }

        public void ProductInventoryConnect(string location_id, string inventory_item_id)
        {
            int index = 0;
            Retry:
            try
            {
                var request = new RestRequest(string.Format(InventoryConnect), Method.POST);
                request.AddHeader("Content-Type", "application/json");
                //var Json = JsonConvert.SerializeObject(variants, Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                //request.AddJsonBody(Json);
                request.AddParameter("application/json", "{  \"location_id\": " + location_id + ",  \"inventory_item_id\": " + inventory_item_id + "}", ParameterType.RequestBody);
                IRestResponse<dynamic> response = Execute<dynamic>(request);

                if (response.StatusCode == HttpStatusCode.Created || response.StatusCode == HttpStatusCode.OK)
                    JsonConvert.DeserializeObject<dynamic>(response.Content);
                else if (Convert.ToString(response.StatusCode) == "429")
                {
                    index = index + 1;
                    System.Threading.Thread.Sleep(1000);
                    goto Retry;

                }
            }
            catch (Exception ex)
            {
                if (index != 2)
                {
                    index = index + 1;
                    System.Threading.Thread.Sleep(1000);
                    goto Retry;
                }
            }
        }
        public Productobject PostProductWithVariants(Shopify.Response.Product product)
        {
            Restart:
            try
            {
                var request = new RestRequest(string.Format(AddNewProduct), Method.POST);
                request.AddHeader("Content-Type", "application/json");
                var Json = JsonConvert.SerializeObject(product, Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                //request.AddJsonBody(Json);
                request.AddParameter("application/json", Json, ParameterType.RequestBody);
                IRestResponse<Productobject> response = Execute<Productobject>(request);
                if (Convert.ToString(response.StatusCode) == "429")
                {
                    System.Threading.Thread.Sleep(2000);
                    goto Restart;
                }
                return JsonConvert.DeserializeObject<Productobject>(response.Content);
            }
            catch (System.Exception ex)
            {
                return null;
            }
        }
        #endregion
        #region Product Variants

        public void PutProductVariantsTaxable(string id)
        {
            int index = 0;
            Retry:
            try
            {
                var request = new RestRequest(string.Format(ProductVariants, id), Method.PUT);
                request.AddHeader("Content-Type", "application/json");
                //var Json = JsonConvert.SerializeObject(variants, Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                //request.AddJsonBody(Json);
                request.AddParameter("application/json", "{\"variant\":{\"id\":" + id + ",\"taxable\":true}}", ParameterType.RequestBody);
                IRestResponse<dynamic> response = Execute<dynamic>(request);
                if (Convert.ToString(response.StatusCode) == "429")
                {
                    System.Threading.Thread.Sleep(2000);
                    goto Retry;
                }
                if (response.StatusCode == HttpStatusCode.OK)
                    JsonConvert.DeserializeObject<dynamic>(response.Content);
                else
                {
                    if (index != 2)
                    {
                        index = index + 1;
                        System.Threading.Thread.Sleep(1000);
                        goto Retry;
                    }
                }
            }
            catch (Exception ex)
            {
                if (index != 2)
                {
                    index = index + 1;
                    System.Threading.Thread.Sleep(1000);
                    goto Retry;
                }
            }

        }

        public Shopify.Request.Variant PutProductVariants(VariantRootobject variants)
        {
            int index = 0;
            Restart:
            try
            {
                var request = new RestRequest(string.Format(ProductVariants, variants.variant.id), Method.PUT);
                request.AddHeader("Content-Type", "application/json");
                var Json = JsonConvert.SerializeObject(variants, Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                //request.AddJsonBody(Json);
                request.AddParameter("application/json", Json, ParameterType.RequestBody);
                IRestResponse<dynamic> response = Execute<dynamic>(request);

                if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Created)
                    JsonConvert.DeserializeObject<Shopify.Request.Variant>(response.Content);
                else if (Convert.ToString(response.StatusCode) == "429")
                {
                    index = index + 1;
                    System.Threading.Thread.Sleep(1000);
                    goto Restart;
                }
            }
            catch (Exception ex)
            {
                if (index != 2)
                {
                    index = index + 1;
                    System.Threading.Thread.Sleep(1000);
                    goto Restart;
                }
            }
            return null;
        }
        #endregion

        #region Search Customer

        public SearchCustomerResponse SearchCustomer(string brandName, string query, string fields = "")
        {
            Restart:
            FileHelper.WriteExceptionMessage(brandName, LogStatus: "INFO", LogErrorMessage: "ShopifyClass.SearchCustomer - Start");
            SearchCustomerResponse searchCustomerResponse = new SearchCustomerResponse();
            var client = new RestClient(BaseURL + string.Format(searchCustomer, query));
            var request = new RestRequest(Method.GET);
            AddHeader(request);
            FileHelper.WriteExceptionMessage(brandName, LogStatus: "INFO", LogErrorMessage: "client request execute :  client.BaseUrl : " + client.BaseUrl + " clientURL : " + request.Resource);
            IRestResponse response = client.Execute(request);
            FileHelper.WriteExceptionMessage(brandName, LogStatus: "INFO", LogErrorMessage: "get Client Response Status Code : " + response.StatusCode);
            if (Convert.ToString(response.StatusCode) == "429")
            {
                System.Threading.Thread.Sleep(2000);
                goto Restart;
            }
            searchCustomerResponse = JsonConvert.DeserializeObject<SearchCustomerResponse>(response.Content);

            FileHelper.WriteExceptionMessage(brandName, LogStatus: "INFO", LogErrorMessage: "ShopifyClass.SearchCustomer - End");
            return searchCustomerResponse;
        }

        #endregion
    }
}
