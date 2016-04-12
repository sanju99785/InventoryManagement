using InventoryManagement.API.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using Microsoft.ApplicationBlocks.Data;
using System.Configuration;
using System.Net.Http;
using InventoryManagement.BAL;
using InventoryManagement.BAL.Common;
using System.Net;

namespace InventoryManagement.API.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class EmployeeAPIController : ApiController
    {
        public HttpResponseMessage GetEmployee(long EmpId = 0)
        {

            try
            {
                EmployeeBA objEmployeeBA = new EmployeeBA();
                DataSet ds = objEmployeeBA.GetEmployee(EmpId.CheckNull());
                if (CommonFunctions.ValidateDataset(ds))
                {
                    return Request.CreateResponse(HttpStatusCode.OK, ds);
                }
                return Request.CreateResponse(HttpStatusCode.NotFound, CommonMessages.NoDataFoundMessage);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new ErrorLogDetails() { Message = CommonMessages.DEFAULT_ERRORMESSAGE, StackTrace = ex.ToString() });
            }
        }

        /// <summary>
        /// Insert OR Update City Details
        /// </summary>
        /// <param name="objEmployeeBA"></param>
        /// <returns></returns>
        [System.Web.Http.HttpPost]
        public HttpResponseMessage InsertUpdateEmployee(EmployeeBA objEmployeeBA)
        {
            try
            {
                //EmployeeBA _objEmployeeBA = new EmployeeBA();
                //if (objEmployeeBA != null)
                //{
                //    _objCityMasterBA.CityId = objEmployeeBA.CityId.CheckNull();
                //    _objCityMasterBA.CityName = objEmployeeBA.CityName.CheckNull();
                //    _objCityMasterBA.StateId = objEmployeeBA.StateId.CheckNull();
                //    _objCityMasterBA.CountryId = objEmployeeBA.CountryId.CheckNull();
                //    _objCityMasterBA.FromPincode = objEmployeeBA.FromPincode.CheckNull();
                //    _objCityMasterBA.ToPinCode = objEmployeeBA.ToPinCode.CheckNull();
                //    _objCityMasterBA.IsActive = objEmployeeBA.IsActive.CheckNull();
                //    _objCityMasterBA.By = objEmployeeBA.By.CheckNull();

                //}

                int value = objEmployeeBA.InsertUpdateEmployee(objEmployeeBA);


                switch (value)
                {
                    case (Int32)StoreProcedureStatus.Success:
                        return Request.CreateResponse(HttpStatusCode.OK, CommonMessages.InsertSuccessMessage);
                        break;
                    case (Int32)StoreProcedureStatus.Duplicate:
                        return Request.CreateResponse(HttpStatusCode.Conflict, CommonMessages.Dublicate);
                        break;
                    case (Int32)StoreProcedureStatus.Error:
                        return Request.CreateResponse(HttpStatusCode.BadRequest, CommonMessages.DEFAULT_ERRORMESSAGE);
                    default:
                        return Request.CreateResponse(HttpStatusCode.OK, CommonMessages.InsertSuccessMessage);

                }

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new ErrorLogDetails() { Message = CommonMessages.DEFAULT_ERRORMESSAGE, StackTrace = ex.ToString() });
            }
        }
        /// <summary>
        /// Delete City Details
        /// </summary>
        /// <param name="CityId"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage DeleteEmployee(long EmpId)
        {
            try
            {
                if (EmpId.CheckNull() > 0)
                {
                    EmployeeBA objEmployeeBA = new EmployeeBA();
                    Int32 retValue = objEmployeeBA.DeleteEmployee(EmpId);
                    if (retValue == -3)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, 3);
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, 1);
                    }

                }
                return Request.CreateResponse(HttpStatusCode.BadRequest, CommonMessages.InvalidInputMessage);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new ErrorLogDetails() { Message = CommonMessages.DEFAULT_ERRORMESSAGE, StackTrace = ex.ToString() });
            }
        }
        /// <summary>
        /// check exist duplication of city
        /// </summary>
        /// <param name="objEmployeeBA"></param>
        /// <returns></returns>
        //[HttpPost]
        //public HttpResponseMessage CheckExistEmployee(EmployeeBA objEmployeeBA)
        //{

        //    try
        //    {
        //        //CityMasterBA _objCityMasterBA = new CityMasterBA();

        //        //_objCityMasterBA.CityId = objEmployeeBA.CityId;
        //        //_objCityMasterBA.CityName = objEmployeeBA.CityName.CheckNull();
        //        //_objCityMasterBA.FromPincode = objEmployeeBA.FromPincode.CheckNull();
        //        //_objCityMasterBA.ToPinCode = objEmployeeBA.ToPinCode.CheckNull();
        //        objEmployeeBA.ReturnCode = 999;


        //        int value = objEmployeeBA.InsertUpdateEmployee(objEmployeeBA);

        //        if (value == -2)
        //        {
        //            return Request.CreateResponse(HttpStatusCode.OK, 2);
        //        }
        //        else if (value == -3)
        //        {
        //            return Request.CreateResponse(HttpStatusCode.OK, 3);
        //        }
        //        else if (value == -4)
        //        {
        //            return Request.CreateResponse(HttpStatusCode.OK, 4);
        //        }
        //        else
        //        {
        //            return Request.CreateResponse(HttpStatusCode.OK, 1);
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        return Request.CreateResponse(HttpStatusCode.InternalServerError, new ErrorLogDetails() { Message = CommonMessages.DEFAULT_ERRORMESSAGE, StackTrace = ex.ToString() });
        //    }
        //}

    }
}
