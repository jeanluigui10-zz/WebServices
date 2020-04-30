using System;
using System.Collections.Generic;
using System.Data;
using System.ServiceModel.Activation;
using xAPI.BL;
using xAPI.Entity;
using xAPI.Entity.Base;

namespace xWebServices.BancoPreguntas
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    public class Services : IServices
    {
        public svcDistributorObject Distributor_GetList_ByQuery(String queryString, String connection)
        {

            BaseEntity objBase = new BaseEntity();
            svcDistributorObject svcResponse = new svcDistributorObject();
            List<svcDistributorEntity> lstDistributor = new List<svcDistributorEntity>();
            try
            {

                if (!String.IsNullOrEmpty(queryString) && !String.IsNullOrEmpty(connection))
                {
                    DataTable dt = ReportsBl.Instance.Distributor_GetList_ByQuery(ref objBase, queryString, connection);

                    if (objBase.Errors.Count > 0)
                    {
                        svcResponse = new svcDistributorObject()
                        {
                            Result = "NoOk",
                            ListDistributor = lstDistributor
                        };
                    }
                    else
                    {
                        if (dt != null)
                        {
                            if (dt.Rows.Count > 0)
                            {
                                foreach (DataRow item in dt.Rows)
                                {
                                    lstDistributor.Add(new svcDistributorEntity()
                                    {
                                        //Distributorid = item["distributorid"].ToString(),
                                    });
                                }

                                svcResponse = new svcDistributorObject()
                                {
                                    Result = "Ok",
                                    Message = "Successfully",
                                    ListDistributor = lstDistributor
                                };
                            }
                            else
                            {
                                svcResponse = new svcDistributorObject()
                                {
                                    Result = "Ok",
                                    ListDistributor = lstDistributor
                                };
                            }
                        }
                        else
                        {
                            svcResponse = new svcDistributorObject()
                            {
                                Result = "NoOk",
                                ListDistributor = lstDistributor
                            };
                        }
                    }
                }
                else
                {
                    svcResponse = new svcDistributorObject()
                    {
                        Result = "NoOk",
                    };
                }
            }
            catch (Exception ex)
            {
                svcResponse = new svcDistributorObject()
                {
                    Result = "NoOk",
                };

                return svcResponse;
            }
            return svcResponse;
        }

        public svcBalotarioList Obtener_ListaBalotario(Int32 categoriaId)
        {
            BaseEntity objBase = new BaseEntity();
            svcBalotarioList svcResponse = new svcBalotarioList()
            {
                Result = "Ok",
            };

            try
            {
                List<Balotario> lstBalotario = BalotarioBl.Instance.Obtener_ListaBalotario(ref objBase,categoriaId);
                if (objBase.Errors.Count == 0 && lstBalotario != null)
                {
                    if (lstBalotario.Count > 0)
                    {
                        foreach (Balotario item in lstBalotario)
                        {
                            svcResponse.ListaBalotario.Add(new svcBalotario
                            {
                                BalotarioId = item.BalotarioId.ToString(),
                                ModalidadId = item.ModalidadId.ToString(),
                                RespuestaId = item.RespuestaId.ToString(),
                                CategoriaId = item.CategoriaId.ToString(),
                                BalotarioDescripcion = item.BalotarioDescripcion,
                                BalotarioEstado = item.BalotarioEstado.ToString(),
                            });
                        }
                    }
                    else
                    {
                        svcResponse = new svcBalotarioList()
                        {
                            Result = "Ok",
                            ListaBalotario = new List<svcBalotario>()
                        };
                    }

                }
                else
                {
                    svcResponse = new svcBalotarioList()
                    {
                        Result = "NoOk",
                        ListaBalotario = new List<svcBalotario>()
                    };
                }
            }
            catch (Exception ex)
            {
                svcResponse = new svcBalotarioList()
                {
                    Result = "NoOk",
                    ListaBalotario = new List<svcBalotario>(),
                    Message = ex.Message
                };

            }
            return svcResponse;
        }

        public svcBancoAnualList Obtener_ListaBancoAnual()
        {
            BaseEntity objBase = new BaseEntity();
            svcBancoAnualList svcResponse = new svcBancoAnualList()
            { 
              Result = "Ok",
            };
            
            try
            {
                List<BancoAnual> lstBancoAnual = BancoAnualBl.Instance.Obtener_ListaBancoAnual(ref objBase);
                if (objBase.Errors.Count == 0 && lstBancoAnual != null)
                {
                    if (lstBancoAnual.Count > 0)
                    {
                        foreach (BancoAnual item in lstBancoAnual)
                        {
                            svcResponse.ListaBancoAnual.Add(new svcBancoAnual
                            {
                                BancoAnualId = item.BancoAnualId.ToString(),
                                BancoAnualNombre = item.BancoAnualNombre,
                                BancoAnualDescripcion = item.BancoAnualDescripcion,
                                BancoAnualEstado = item.BancoAnualEstado.ToString(),
                            });
                        }
                    }
                    else 
                    {
                        svcResponse = new svcBancoAnualList()
                        {
                            Result = "Ok",
                            ListaBancoAnual = new List<svcBancoAnual>()
                        };
                    }
                  
                }
                else
                {
                    svcResponse = new svcBancoAnualList()
                    {
                        Result = "NoOk",
                        ListaBancoAnual = new List<svcBancoAnual>()
                    };
                }
            }
            catch (Exception ex)
            {
                svcResponse = new svcBancoAnualList()
                {
                    Result = "NoOk",
                    ListaBancoAnual = new List<svcBancoAnual>(),
                    Message = ex.Message
                };

            }
            return svcResponse;
        }

        public svcModalidadList Obtener_ListaModalidades(Int32 bancoAnualId)
        {
            BaseEntity objBase = new BaseEntity();
            svcModalidadList svcResponse = new svcModalidadList()
            {
                Result = "Ok",
            };

            try
            {
                List<Modalidad> lstModalidad = ModalidadBl.Instance.Obtener_ListaModalidad(ref objBase, bancoAnualId);
                if (objBase.Errors.Count == 0 && lstModalidad != null)
                {
                    if (lstModalidad.Count > 0)
                    {
                        foreach (Modalidad item in lstModalidad)
                        {
                            svcResponse.ListaModalidad.Add(new svcModalidad
                            {
                                ModalidadId = item.ModalidadId.ToString(),
                                BancoAnualId = item.BancoAnualId.ToString(),
                                ModalidadNombre = item.ModalidadNombre,
                                ModalidadDescripcion = item.ModalidadDescripcion,
                                ModalidadEstado = item.ModalidadEstado.ToString(),
                            });
                        }
                    }
                    else
                    {
                        svcResponse = new svcModalidadList()
                        {
                            Result = "Ok",
                            ListaModalidad = new List<svcModalidad>()
                        };
                    }

                }
                else
                {
                    svcResponse = new svcModalidadList()
                    {
                        Result = "NoOk",
                        ListaModalidad = new List<svcModalidad>()
                    };
                }
            }
            catch (Exception ex)
            {
                svcResponse = new svcModalidadList()
                {
                    Result = "NoOk",
                    ListaModalidad = new List<svcModalidad>(),
                    Message = ex.Message
                };

            }
            return svcResponse;
        }
        public svcCategoriaList Obtener_ListaCategoria(Int32 modalidadId)
        {
            BaseEntity objBase = new BaseEntity();
            svcCategoriaList svcResponse = new svcCategoriaList()
            {
                Result = "Ok",
            };

            try
            {
                List<Categoria> lstCategoria = CategoriaBl.Instance.Obtener_ListaCategoria(ref objBase, modalidadId);
                if (objBase.Errors.Count == 0 && lstCategoria != null)
                {
                    if (lstCategoria.Count > 0)
                    {
                        foreach (Categoria item in lstCategoria)
                        {
                            svcResponse.ListaCategoria.Add(new svcCategoria
                            {
                                CategoriaId = item.CategoriaId.ToString(),
                                CategoriaNombre = item.CategoriaNombre,
                                CategoriaDescripcion = item.CategoriaDescripcion,
                                ModalidadId = item.ModalidadId.ToString(),
                                ModalidadNombre = item.ModalidadNombre,
                                TotalPreguntas = item.TotalPreguntas.ToString(),
                            });
                        }
                    }
                    else
                    {
                        svcResponse = new svcCategoriaList()
                        {
                            Result = "Ok",
                            ListaCategoria = new List<svcCategoria>()
                        };
                    }

                }
                else
                {
                    svcResponse = new svcCategoriaList()
                    {
                        Result = "NoOk",
                        ListaCategoria = new List<svcCategoria>()
                    };
                }
            }
            catch (Exception ex)
            {
                svcResponse = new svcCategoriaList()
                {
                    Result = "NoOk",
                    ListaCategoria = new List<svcCategoria>(),
                    Message = ex.Message
                };

            }
            return svcResponse;
        }
    }
}

