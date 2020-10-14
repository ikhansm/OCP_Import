using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopify.Response
{

    public class RootobjectRefund
    {
        public Refund refund { get; set; }
        public List<Refund> refunds { get; set; }
    }
    public class Rootobject
    {
        public Transaction transaction { get; set; }
        public List<Transaction> transactions { get; set; }
    }

    public class Transaction
    {
        public long id { get; set; }
        public long order_id { get; set; }
        public string amount { get; set; }
        public string kind { get; set; }
        public string gateway { get; set; }
        public string status { get; set; }
        public string message { get; set; }
        public string created_at { get; set; }
        public bool test { get; set; }
        public string authorization { get; set; }
        public string currency { get; set; }
        public object location_id { get; set; }
        public object user_id { get; set; }
        public long? parent_id { get; set; }
        public object device_id { get; set; }
        public Receipt receipt { get; set; }
        public object error_code { get; set; }
        public string source_name { get; set; }
    }

    public class Receipt
    {
        public string id { get; set; }
        public string _object { get; set; }
        public int amount { get; set; }
        public int amount_refunded { get; set; }
        public string application { get; set; }
        public string application_fee { get; set; }
        public Balance_Transaction balance_transaction { get; set; }
        public bool captured { get; set; }
        public int created { get; set; }
        public string currency { get; set; }
        public object customer { get; set; }
        public string description { get; set; }
        public object destination { get; set; }
        public object dispute { get; set; }
        public object failure_code { get; set; }
        public object failure_message { get; set; }
        public Fraud_Details fraud_details { get; set; }
        public object invoice { get; set; }
        public bool livemode { get; set; }
        public Metadata metadata { get; set; }
        public object on_behalf_of { get; set; }
        public object order { get; set; }
        public Outcome outcome { get; set; }
        public bool paid { get; set; }
        public object receipt_email { get; set; }
        public object receipt_number { get; set; }
        public bool refunded { get; set; }
        public Refunds refunds { get; set; }
        public object review { get; set; }
        public object shipping { get; set; }
        public Source source { get; set; }
        public object source_transfer { get; set; }
        public object statement_descriptor { get; set; }
        public string status { get; set; }
        public object transfer_group { get; set; }
        public string message { get; set; }
        public string error_codes { get; set; }
        public Error error { get; set; }
    }

    public class Error
    {
        public string code { get; set; }
        public string doc_url { get; set; }
        public string message { get; set; }
        public string type { get; set; }
    }

    public class Balance_Transaction
    {
        public string id { get; set; }
        public string _object { get; set; }
        public int amount { get; set; }
        public int available_on { get; set; }
        public int created { get; set; }
        public string currency { get; set; }
        public string description { get; set; }
        public int fee { get; set; }
        public Fee_Details[] fee_details { get; set; }
        public int net { get; set; }
        public string source { get; set; }
        public Sourced_Transfers sourced_transfers { get; set; }
        public string status { get; set; }
        public string type { get; set; }
    }

    public class Sourced_Transfers
    {
        public string _object { get; set; }
        public object[] data { get; set; }
        public bool has_more { get; set; }
        public int total_count { get; set; }
        public string url { get; set; }
    }

    public class Fee_Details
    {
        public int amount { get; set; }
        public string application { get; set; }
        public string currency { get; set; }
        public string description { get; set; }
        public string type { get; set; }
    }

    public class Fraud_Details
    {
    }

    public class Metadata
    {
        public string payments_charge_id { get; set; }
        public string order_transaction_id { get; set; }
        public string email { get; set; }
        public string order_id { get; set; }
    }

    public class Outcome
    {
        public string network_status { get; set; }
        public object reason { get; set; }
        public string seller_message { get; set; }
        public string type { get; set; }
    }

    public class Refunds
    {
        public string _object { get; set; }
        public object[] data { get; set; }
        public bool has_more { get; set; }
        public int total_count { get; set; }
        public string url { get; set; }
    }

    public class Source
    {
        public string id { get; set; }
        public string _object { get; set; }
        public string address_city { get; set; }
        public string address_country { get; set; }
        public string address_line1 { get; set; }
        public string address_line1_check { get; set; }
        public string address_line2 { get; set; }
        public string address_state { get; set; }
        public string address_zip { get; set; }
        public string address_zip_check { get; set; }
        public string brand { get; set; }
        public string country { get; set; }
        public object customer { get; set; }
        public string cvc_check { get; set; }
        public object dynamic_last4 { get; set; }
        public int exp_month { get; set; }
        public int exp_year { get; set; }
        public string fingerprint { get; set; }
        public string funding { get; set; }
        public string last4 { get; set; }
        public Metadata1 metadata { get; set; }
        public string name { get; set; }
        public object tokenization_method { get; set; }
    }

    public class Metadata1
    {
    }
}
