using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace xWebServices.BancoPreguntas
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

        [OperationContract]
        [WebInvoke(Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        svcBancoAnualList Obtener_ListaBancoAnual();

        [OperationContract]
        [WebInvoke(Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        svcModalidadList Obtener_ListaModalidades(Int32 bancoAnualId);

        [OperationContract]
        [WebInvoke(Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        svcBalotarioList Obtener_ListaBalotario(Int32 categoriaId);

        [OperationContract]
        [WebInvoke(Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        svcCategoriaList Obtener_ListaCategoria(Int32 modalidadId);
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

    [Serializable]
    [DataContract(Name = "svcBancoAnual", Namespace = "")]
    public class svcBancoAnual
    {
        private string _BancoAnualId;
        [DataMember]
        public string BancoAnualId
        {
            get
            {
                if (_BancoAnualId == null) { _BancoAnualId = String.Empty; }
                return _BancoAnualId;
            }
            set { _BancoAnualId = value; }
        }
        private string _BancoAnualNombre;
        [DataMember]
        public string BancoAnualNombre
        {
            get
            {
                if (_BancoAnualNombre == null) { _BancoAnualNombre = String.Empty; }
                return _BancoAnualNombre;
            }
            set { _BancoAnualNombre = value; }
        }
        private string _BancoAnualDescripcion;
        [DataMember]
        public string BancoAnualDescripcion
        {
            get
            {
                if (_BancoAnualDescripcion == null) { _BancoAnualDescripcion = String.Empty; }
                return _BancoAnualDescripcion;
            }
            set { _BancoAnualDescripcion = value; }
        }
        private string _BancoAnualEstado;
        [DataMember]
        public string BancoAnualEstado
        {
            get
            {
                if (_BancoAnualEstado == null) { _BancoAnualEstado = String.Empty; }
                return _BancoAnualEstado;
            }
            set { _BancoAnualEstado = value; }
        }

    }
    public class svcBancoAnualList
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
        private List<svcBancoAnual> _items;
        [DataMember]
        public List<svcBancoAnual> ListaBancoAnual
        {
            get { return _items ?? (_items = new List<svcBancoAnual>()); }
            set { _items = value; }
        }
    }

    [Serializable]
    [DataContract(Name = "svcModalidad", Namespace = "")]
    public class svcModalidad
    {
        private string _ModalidadId;
        [DataMember]
        public string ModalidadId
        {
            get
            {
                if (_ModalidadId == null) { _ModalidadId = String.Empty; }
                return _ModalidadId;
            }
            set { _ModalidadId = value; }
        }

        private string _BancoAnualId;
        [DataMember]
        public string BancoAnualId
        {
            get
            {
                if (_BancoAnualId == null) { _BancoAnualId = String.Empty; }
                return _BancoAnualId;
            }
            set { _BancoAnualId = value; }
        }
        private string _ModalidadNombre;
        [DataMember]
        public string ModalidadNombre
        {
            get
            {
                if (_ModalidadNombre == null) { _ModalidadNombre = String.Empty; }
                return _ModalidadNombre;
            }
            set { _ModalidadNombre = value; }
        }
        private string _ModalidadDescripcion;
        [DataMember]
        public string ModalidadDescripcion
        {
            get
            {
                if (_ModalidadDescripcion == null) { _ModalidadDescripcion = String.Empty; }
                return _ModalidadDescripcion;
            }
            set { _ModalidadDescripcion = value; }
        }
        private string _ModalidadEstado;
        [DataMember]
        public string ModalidadEstado
        {
            get
            {
                if (_ModalidadEstado == null) { _ModalidadEstado = String.Empty; }
                return _ModalidadEstado;
            }
            set { _ModalidadEstado = value; }
        }

    }

    public class svcModalidadList
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
        private List<svcModalidad> _items;
        [DataMember]
        public List<svcModalidad> ListaModalidad
        {
            get { return _items ?? (_items = new List<svcModalidad>()); }
            set { _items = value; }
        }
    }

    [Serializable]
    [DataContract(Name = "svcBalotario", Namespace = "")]
    public class svcBalotario
    {
        private string _BalotarioId;
        [DataMember]
        public string BalotarioId
        {
            get
            {
                if (_BalotarioId == null) { _BalotarioId = String.Empty; }
                return _BalotarioId;
            }
            set { _BalotarioId = value; }
        }

        private string _ModalidadId;
        [DataMember]
        public string ModalidadId
        {
            get
            {
                if (_ModalidadId == null) { _ModalidadId = String.Empty; }
                return _ModalidadId;
            }
            set { _ModalidadId = value; }
        }

        private string _RespuestaId;
        [DataMember]
        public string RespuestaId
        {
            get
            {
                if (_RespuestaId == null) { _RespuestaId = String.Empty; }
                return _RespuestaId;
            }
            set { _RespuestaId = value; }
        }

        private string _CategoriaId;
        [DataMember]
        public string CategoriaId
        {
            get
            {
                if (_CategoriaId == null) { _CategoriaId = String.Empty; }
                return _CategoriaId;
            }
            set { _CategoriaId = value; }
        }

        private string _BalotarioDescripcion;
        [DataMember]
        public string BalotarioDescripcion
        {
            get
            {
                if (_BalotarioDescripcion == null) { _BalotarioDescripcion = String.Empty; }
                return _BalotarioDescripcion;
            }
            set { _BalotarioDescripcion = value; }
        }
 
        private string _BalotarioEstado;
        [DataMember]
        public string BalotarioEstado
        {
            get
            {
                if (_BalotarioEstado == null) { _BalotarioEstado = String.Empty; }
                return _BalotarioEstado;
            }
            set { _BalotarioEstado = value; }
        }

    }

    public class svcBalotarioList
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
        private List<svcBalotario> _items;
        [DataMember]
        public List<svcBalotario> ListaBalotario
        {
            get { return _items ?? (_items = new List<svcBalotario>()); }
            set { _items = value; }
        }
    }

    [Serializable]
    [DataContract(Name = "svcCategoria", Namespace = "")]
    public class svcCategoria
    {
        private string _CategoriaId;
        [DataMember]
        public string CategoriaId
        {
            get
            {
                if (_CategoriaId == null) { _CategoriaId = String.Empty; }
                return _CategoriaId;
            }
            set { _CategoriaId = value; }
        }

        private string _CategoriaNombre;
        [DataMember]
        public string CategoriaNombre
        {
            get
            {
                if (_CategoriaNombre == null) { _CategoriaNombre = String.Empty; }
                return _CategoriaNombre;
            }
            set { _CategoriaNombre = value; }
        }

        private string _CategoriaDescripcion;
        [DataMember]
        public string CategoriaDescripcion
        {
            get
            {
                if (_CategoriaDescripcion == null) { _CategoriaDescripcion = String.Empty; }
                return _CategoriaDescripcion;
            }
            set { _CategoriaDescripcion = value; }
        }

        private string _ModalidadId;

        [DataMember]
        public string ModalidadId
        {
            get
            {
                if (_ModalidadId == null) { _ModalidadId = String.Empty; }
                return _ModalidadId;
            }
            set { _ModalidadId = value; }
        }

        private string _ModalidadNombre;
        [DataMember]
        public string ModalidadNombre
        {
            get
            {
                if (_ModalidadNombre == null) { _ModalidadNombre = String.Empty; }
                return _ModalidadNombre;
            }
            set { _ModalidadNombre = value; }
        }

        private string _TotalPreguntas;

        [DataMember]
        public string TotalPreguntas
        {
            get
            {
                if (_TotalPreguntas == null) { _TotalPreguntas = String.Empty; }
                return _TotalPreguntas;
            }
            set { _TotalPreguntas = value; }
        }


    }
    public class svcCategoriaList
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
        private List<svcCategoria> _items;
        [DataMember]
        public List<svcCategoria> ListaCategoria
        {
            get { return _items ?? (_items = new List<svcCategoria>()); }
            set { _items = value; }
        }
    }



}

