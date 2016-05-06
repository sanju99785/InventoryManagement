using InventoryManagement.BAL;
using InventoryManagement.ViewModels;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using InventoryManagement.BAL.Common;

namespace InventoryManagement.Controllers
{
    public class EmployeeController : Controller
    {
        //
        // GET: /Employee/
        private RestClient _client;
        private readonly string _url = ConfigurationManager.AppSettings["WebApiPath"];
        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            try
            {
                return View();
            }
            catch (Exception)
            {

                throw;
            }

        }

        public ActionResult test()
        {
            return View();
        }

        public JsonResult InsertUdateEmployee(long EmpId = 0)
        {
            try
            {
                EmployeeViewModel objEmployeeViewModel = new EmployeeViewModel();
                if (EmpId > 0)
                {
                    var request = new RestRequest("EmployeeAPI/GetEmployee", Method.GET);
                    request.Parameters.Clear();
                    _client = new RestClient(_url);
                    _client.FollowRedirects = false;
                    request.AddParameter("EmpId", EmpId);

                    var response = _client.Execute(request);

                    JavaScriptSerializer j = new JavaScriptSerializer();
                    var JSONObj = j.Deserialize<Dictionary<string, List<Dictionary<String, Object>>>>(response.Content);


                    foreach (var item in JSONObj)
                    {
                        var EmpBA = item.Value;
                        foreach (var obj in EmpBA)
                        {
                            objEmployeeViewModel.EmpId = obj["EmpId"] == null ? 0 : Convert.ToInt64(obj["EmpId"]);
                            objEmployeeViewModel.Name = obj["Name"] == null ? "" : Convert.ToString(obj["Name"]);
                            objEmployeeViewModel.Address = obj["Address"] == null ? "" : Convert.ToString(obj["Address"]);
                            objEmployeeViewModel.Phone = obj["Phone"] == null ? "" : Convert.ToString(obj["Phone"]);
                            objEmployeeViewModel.Mobile = obj["Mobile"] == null ? "" : Convert.ToString(obj["Mobile"]);
                            objEmployeeViewModel.Description = obj["Description"] == null ? "" : Convert.ToString(obj["Description"]);
                        }
                    }
                }

                return Json(objEmployeeViewModel, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public JsonResult SaveEmployee(EmployeeViewModel _objEmployeeViewModel)
        {
            try
            {
                String values = string.Empty;
                if (_objEmployeeViewModel != null)
                {
                    

                    EmployeeBA objEmployeeBA = new EmployeeBA();
                    objEmployeeBA.EmpId = _objEmployeeViewModel.EmpId.CheckNull();
                    objEmployeeBA.Name = _objEmployeeViewModel.Name.CheckNull();
                    objEmployeeBA.Address = _objEmployeeViewModel.Address.CheckNull();
                    objEmployeeBA.Phone = _objEmployeeViewModel.Phone.CheckNull();
                    objEmployeeBA.Mobile = _objEmployeeViewModel.Mobile.CheckNull();
                    objEmployeeBA.Description = _objEmployeeViewModel.Description.CheckNull();
                    //objCityMasterBA.ReturnCode = 999;
                    var request = new RestRequest("EmployeeAPI/InsertUpdateEmployee", Method.POST) { RequestFormat = DataFormat.Json };
                    _client = new RestClient(_url);
                    request.AddBody(objEmployeeBA);
                    var response = _client.Execute(request);

                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var serializer = new JavaScriptSerializer(); //using System.Web.Script.Serialization;
                        values = serializer.Deserialize<String>(response.Content);
                        TempData["SuccessMessage"] = values;
                    }
                    else
                    {
                        var serializer = new JavaScriptSerializer(); //using System.Web.Script.Serialization;
                        values = serializer.Deserialize<String>(response.Content);
                        TempData["WarningMessage"] = values;
                    }
                }

                return Json(values, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
