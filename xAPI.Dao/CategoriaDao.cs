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
    public class CategoriaDao
    {
        #region Singleton
        private static CategoriaDao instance = null;
        public static CategoriaDao Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new CategoriaDao();
                }

                return instance;
            }
        }
        #endregion
        public List<Categoria> Obtener_ListaCategoria(ref BaseEntity objBase, int modalidad)
        {
            SqlCommand ObjCmd = null;
            List<Categoria> lstCategoria = null;
            SqlDataReader dr = null;
            try
            {
                ObjCmd = new SqlCommand("Sp_ListarCategoriasxModalidad", clsConnection.GetConnection());
                ObjCmd.Parameters.AddWithValue("@modalidadId", modalidad);
                ObjCmd.CommandType = CommandType.StoredProcedure;
                lstCategoria = new List<Categoria>();
                dr = ObjCmd.ExecuteReader();
                while (dr.Read())
                {
                    lstCategoria.Add(new Categoria() 
                    {
                         CategoriaId = dr.GetColumnValue<Int32>("CategoriaId"),
                         CategoriaNombre = dr.GetColumnValue<String>("CategoriaNombre"),
                         CategoriaDescripcion = dr.GetColumnValue<String>("CategoriaDescripcion"),
                         ModalidadId = dr.GetColumnValue<Int32>("ModalidadId"),
                         ModalidadNombre= dr.GetColumnValue<String>("ModalidadNombre"),
                         TotalPreguntas= dr.GetColumnValue<Int32>("TotalPreguntas"),
                    });
                }
            }
            catch (Exception ex)
            {
                lstCategoria = null;
                objBase.Errors.Add(new BaseEntity.ListError(ex, "Categoria not found."));
            }
            finally
            {
                clsConnection.DisposeCommand(ObjCmd);
            }
            return lstCategoria;
        }
    }
}
