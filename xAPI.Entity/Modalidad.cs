using System;
using System.Data;

namespace xAPI.Entity
{
    [Serializable]
    public class Modalidad
    {
        public Int32  ModalidadId { get; set; }
        public Int32 BancoAnualId { get; set; }        
        public String ModalidadNombre { get; set; }
        public String ModalidadDescripcion { get; set; }
        public Int16  ModalidadEstado { get; set; }
        
        public Modalidad() { }
        public Modalidad(DataRow dr)
        {
            
        }
    }

}
