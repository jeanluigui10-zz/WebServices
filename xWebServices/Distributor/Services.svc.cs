using System;
using System.Collections.Generic;
using System.Data;
using System.ServiceModel.Activation;
using xAPI.BL;
using xAPI.Entity.Base;

namespace xWebServices.Distributor
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

    }
}

