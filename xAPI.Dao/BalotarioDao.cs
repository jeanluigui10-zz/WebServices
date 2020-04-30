using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using xAPI.Dao.Connection;
using xAPI.Entity;
using xAPI.Entity.Base;
using xAPI.Entity.General;

namespace xAPI.Dao
{
    public class BalotarioDao
    {
        #region Singleton
        private static BalotarioDao instance = null;
        public static BalotarioDao Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new BalotarioDao();
                }

                return instance;
            }
        }
        #endregion
        public List<Balotario> Obtener_ListaBalotario(ref BaseEntity objBase, Int32 categoriaId)
        {
            SqlCommand ObjCmd = null;
            List<Balotario> lstBalotario = null;
            SqlDataReader dr = null;
            try
            {
                ObjCmd = new SqlCommand("Sp_ListaPreguntasxCategoria", clsConnection.GetConnection());
                ObjCmd.Parameters.AddWithValue("@categoriaid", categoriaId);
                ObjCmd.CommandType = CommandType.StoredProcedure;
                lstBalotario = new List<Balotario>();
                dr = ObjCmd.ExecuteReader();
                while (dr.Read())
                {
                    lstBalotario.Add(new Balotario() 
                    {
                        BalotarioId = dr.GetColumnValue<Int32>("BalotarioId"),
                        ModalidadId = dr.GetColumnValue<Int32>("ModalidadId"),
                        RespuestaId = dr.GetColumnValue<Int32>("RespuestaId"),
                        CategoriaId = dr.GetColumnValue<Int32>("CategoriaId"),
                        BalotarioDescripcion = dr.GetColumnValue<String>("BalotarioDescripcion"),
                        BalotarioEstado = dr.GetColumnValue<Int16>("BalotarioEstado"),
                   });
                }
            }
            catch (Exception ex)
            {
                lstBalotario = null;
                objBase.Errors.Add(new BaseEntity.ListError(ex, "Balotario Anual not found."));
            }
            finally
            {
                clsConnection.DisposeCommand(ObjCmd);
            }
            return lstBalotario;
        }
    }
}
