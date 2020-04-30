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
    public class BancoAnualDao
    {
        #region Singleton
        private static BancoAnualDao instance = null;
        public static BancoAnualDao Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new BancoAnualDao();
                }

                return instance;
            }
        }
        #endregion
        public List<BancoAnual> Obtener_ListaBancoAnual(ref BaseEntity objBase)
        {
            SqlCommand ObjCmd = null;
            List<BancoAnual> lstBancoAnual = null;
            SqlDataReader dr = null;
            try
            {
                ObjCmd = new SqlCommand("ListarBancosAnuales_Sp", clsConnection.GetConnection());
                ObjCmd.CommandType = CommandType.StoredProcedure;
                lstBancoAnual = new List<BancoAnual>();
                dr = ObjCmd.ExecuteReader();
                while (dr.Read())
                {
                    lstBancoAnual.Add(new BancoAnual() 
                    {
                         BancoAnualId = dr.GetColumnValue<Int32>("BancoAnualId"),
                         BancoAnualNombre = dr.GetColumnValue<String>("BancoAnualNombre"),
                         BancoAnualDescripcion = dr.GetColumnValue<String>("BancoAnualDescripcion"),
                         BancoAnualEstado = dr.GetColumnValue<Int16>("BancoAnualEstado"),
                   });
                }
            }
            catch (Exception ex)
            {
                lstBancoAnual = null;
                objBase.Errors.Add(new BaseEntity.ListError(ex, "Banco Anual not found."));
            }
            finally
            {
                clsConnection.DisposeCommand(ObjCmd);
            }
            return lstBancoAnual;
        }
    }
}
