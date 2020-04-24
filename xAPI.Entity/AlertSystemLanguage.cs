using System;
using System.Data;
namespace xAPI.Entity
{
    [Serializable]
    public class AlertSystemLanguage
    {
        public Int32 AlertId { get; set; }
        public Int32 LanguageId { get; set; }
        public Int16 Status { get; set; }
        
        public AlertSystemLanguage() { }
        public AlertSystemLanguage(DataRow data)
        {
            //AlertId = AlertId.ValidateDataRowKey(data,"Alertid");
            //LanguageId = LanguageId.ValidateDataRowKey(data, "LanguageId");
            //Status = Status.ValidateDataRowKey(data, "Status");
        }
    }

}
