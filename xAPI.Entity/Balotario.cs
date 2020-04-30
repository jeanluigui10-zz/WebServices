using System;
using System.Data;

namespace xAPI.Entity
{
    [Serializable]
    public class Balotario
    {
        public Int32 BalotarioId { get; set; }
        public Int32 ModalidadId { get; set; }
        public Int32 RespuestaId { get; set; }        
        public Int32 CategoriaId { get; set; }
        public String BalotarioDescripcion { get; set; }
        public Int16 BalotarioEstado { get; set; }
        
        public Balotario() { }
        public Balotario(DataRow dr)
        {
            
        }
    }

}
