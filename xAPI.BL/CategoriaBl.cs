using System;
using System.Collections.Generic;
using xAPI.Dao;
using xAPI.Entity;
using xAPI.Entity.Base;

namespace xAPI.BL
{
    public class CategoriaBl
    {
        #region Singleton
        private static CategoriaBl instance = null;
        public static CategoriaBl Instance
        {
            get
            {
                if (instance == null)
                    instance = new CategoriaBl();
                return instance;
            }
        }
        #endregion

        public List<Categoria> Obtener_ListaCategoria(ref BaseEntity objBase, int modalidad)
        {
            objBase = new BaseEntity();
            List<Categoria> lstCategoria = new List<Categoria>();
            try
            {
                lstCategoria = CategoriaDao.Instance.Obtener_ListaCategoria(ref objBase, modalidad);
            }
            catch (Exception ex)
            {
                lstCategoria = null;
                objBase.Errors.Add(new BaseEntity.ListError(ex, "An error occurred  on application level 2"));
            }

            return lstCategoria;
        }
    }
}
