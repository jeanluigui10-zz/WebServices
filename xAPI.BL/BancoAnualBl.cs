using System;
using System.Collections.Generic;
using xAPI.Dao;
using xAPI.Entity;
using xAPI.Entity.Base;

namespace xAPI.BL
{
    public class BancoAnualBl
    {
        #region Singleton
        private static BancoAnualBl instance = null;
        public static BancoAnualBl Instance
        {
            get
            {
                if (instance == null)
                    instance = new BancoAnualBl();
                return instance;
            }
        }
        #endregion

        public List<BancoAnual> Obtener_ListaBancoAnual(ref BaseEntity objBase)
        {
            objBase = new BaseEntity();
            List<BancoAnual> lstBancoAnual = new List<BancoAnual>();
            try
            {
                lstBancoAnual = BancoAnualDao.Instance.Obtener_ListaBancoAnual(ref objBase);
            }
            catch (Exception ex)
            {
                lstBancoAnual = null;
                objBase.Errors.Add(new BaseEntity.ListError(ex, "An error occurred  on application level 2"));
            }

            return lstBancoAnual;
        }
    }
}
