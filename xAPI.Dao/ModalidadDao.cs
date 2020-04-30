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
    public class ModalidadDao
    {
        #region Singleton
        private static ModalidadDao instance = null;
        public static ModalidadDao Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ModalidadDao();
                }

                return instance;
            }
        }
        #endregion
        public List<Modalidad> Obtener_ListaModalidades(ref BaseEntity objBase, Int32 bancoAnualId)
        {
            SqlCommand ObjCmd = null;
            List<Modalidad> lstModalidad = null;
            SqlDataReader dr = null;
            try
            {
                ObjCmd = new SqlCommand("ListarModalidades_Sp", clsConnection.GetConnection());
                ObjCmd.Parameters.AddWithValue("@prmBancoAnualId", bancoAnualId);
                ObjCmd.CommandType = CommandType.StoredProcedure;
                lstModalidad = new List<Modalidad>();
                dr = ObjCmd.ExecuteReader();
                while (dr.Read())
                {
                    lstModalidad.Add(new Modalidad()
                    {
                        ModalidadId = dr.GetColumnValue<Int32>("ModalidadId"),
                        BancoAnualId = dr.GetColumnValue<Int32>("BancoAnualId"),
                        ModalidadNombre = dr.GetColumnValue<String>("ModalidadNombre"),
                        ModalidadDescripcion = dr.GetColumnValue<String>("ModalidadDescripcion"),
                        ModalidadEstado = dr.GetColumnValue<Int16>("ModalidadEstado"),
                    });
                }
            }
            catch (Exception ex)
            {
                lstModalidad = null;
                objBase.Errors.Add(new BaseEntity.ListError(ex, "Modalidades not found."));
            }
            finally
            {
                clsConnection.DisposeCommand(ObjCmd);
            }
            return lstModalidad;
        }
    }
}
