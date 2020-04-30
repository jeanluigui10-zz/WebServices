using System;
using System.Collections.Generic;
using xAPI.Dao;
using xAPI.Entity;
using xAPI.Entity.Base;

namespace xAPI.BL
{
    public class ModalidadBl
    {
        #region Singleton
        private static ModalidadBl instance = null;
        public static ModalidadBl Instance
        {
            get
            {
                if (instance == null)
                    instance = new ModalidadBl();
                return instance;
            }
        }
        #endregion

        public List<Modalidad> Obtener_ListaModalidad(ref BaseEntity objBase, Int32 bancoAnualId)
        {
            objBase = new BaseEntity();
            List<Modalidad> lstModalidad = new List<Modalidad>();
            try
            {
                lstModalidad = ModalidadDao.Instance.Obtener_ListaModalidades(ref objBase, bancoAnualId);
            }
            catch (Exception ex)
            {
                lstModalidad = null;
                objBase.Errors.Add(new BaseEntity.ListError(ex, "An error occurred  on application level 2"));
            }

            return lstModalidad;
        }
    }
}
