using System;
using System.Collections.Generic;
using xAPI.Dao;
using xAPI.Entity;
using xAPI.Entity.Base;

namespace xAPI.BL
{
    public class BalotarioBl
    {
        #region Singleton
        private static BalotarioBl instance = null;
        public static BalotarioBl Instance
        {
            get
            {
                if (instance == null)
                    instance = new BalotarioBl();
                return instance;
            }
        }
        #endregion

        public List<Balotario> Obtener_ListaBalotario(ref BaseEntity objBase, Int32 categoriaId)
        {
            objBase = new BaseEntity();
            List<Balotario> lstBalotario = new List<Balotario>();
            try
            {
                lstBalotario = BalotarioDao.Instance.Obtener_ListaBalotario(ref objBase, categoriaId);
            }
            catch (Exception ex)
            {
                lstBalotario = null;
                objBase.Errors.Add(new BaseEntity.ListError(ex, "An error occurred  on application level 2"));
            }

            return lstBalotario;
        }
    }
}
