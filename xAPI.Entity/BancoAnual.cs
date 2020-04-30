using System;
using System.Data;
using System.Data.SqlClient;
using xAPI.Entity.General;

namespace xAPI.Entity
{
    [Serializable]
    public class BancoAnual
    {
        public Int32 BancoAnualId { get; set; }
        public String BancoAnualNombre { get; set; }
        public String BancoAnualDescripcion { get; set; }
        public Int16 BancoAnualEstado { get; set; }
        
        public BancoAnual() { }
        public BancoAnual(DataRow dr)
        {
            
        }
    }

}
