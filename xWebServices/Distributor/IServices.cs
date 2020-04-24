using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace xWebServices.Distributor
{
    [ServiceContract]
    public interface IServices
    {


        //[OperationContract]
        //[WebInvoke(Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        //svcResponseData Distributor_GetDistributor(string inKey, string cultureName);

        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        svcDistributorObject Distributor_GetList_ByQuery(String queryString, String connection);

    }
    [DataContract(Name = "svcResponseData", Namespace = "")]
    [Serializable]
    public class svcResponseData
    {
        private string _result;
        [DataMember]
        public string Result
        {
            get
            {
                if (_result == null) { _result = String.Empty; }
                return _result;
            }
            set { _result = value; }
        }

        private string _message;
        [DataMember]
        public string Message
        {
            get
            {
                if (_message == null) { _message = String.Empty; }
                return _message;
            }
            set { _message = value; }
        }

    }
    [DataContract(Name = "svcDistributorObject", Namespace = "")]
    public class svcDistributorObject
    {
        [DataMember]
        public String Message { get; set; } = String.Empty;

        [DataMember]
        public String Result { get; set; } = String.Empty;

        [DataMember]
        public List<svcDistributorEntity> ListDistributor { get; set; } = new List<svcDistributorEntity>();


    }

    [Serializable]
    [DataContract(Name = "svcDistributorEntity", Namespace = "")]
    public class svcDistributorEntity
    {
        [DataMember]
        public String Distributorid { get; set; }
        [DataMember]
        public String Legacynumber { get; set; }
        [DataMember]
        public String Firstname { get; set; }
        [DataMember]
        public String Lastname { get; set; }
        [DataMember]
        public String Emailaddress { get; set; }
        [DataMember]
        public String Cellphone { get; set; }
        [DataMember]
        public String Homephone { get; set; }
        [DataMember]
        public String Enrolldate { get; set; }
        [DataMember]
        public String Languageid { get; set; }
        [DataMember]
        public String Languagename { get; set; }
        [DataMember]
        public String Marketid { get; set; }
        [DataMember]
        public String Marketname { get; set; }
        [DataMember]
        public String Countryid { get; set; }
        [DataMember]
        public String Countryname { get; set; }
        [DataMember]
        public String Rank { get; set; }
        [DataMember]
        public String Lastorderdate { get; set; }
        [DataMember]
        public String UrlOrder { get; set; }

    }
}

