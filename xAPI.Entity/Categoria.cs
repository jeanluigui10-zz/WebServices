using System;
using System.Data;

namespace xAPI.Entity
{
    [Serializable]
    public class Categoria
    {
        public Int32 CategoriaId { get; set; }
        public String CategoriaNombre { get; set; }
        public String CategoriaDescripcion { get; set; }
        public Int32 ModalidadId { get; set; }
        public String ModalidadNombre { get; set; }
        public Int32 TotalPreguntas { get; set; }
        
        public Categoria() { }
        public Categoria(DataRow dr)
        {
            
        }
    }

}
