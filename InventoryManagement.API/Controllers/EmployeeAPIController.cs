using InventoryManagement.API.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Microsoft.ApplicationBlocks.Data;
using System.Configuration;

namespace InventoryManagement.API.Controllers
{
    public class EmployeeAPIController : ApiController
    {
        public void GetEmployee(long EmpId = 0)
        {


            SqlParameter param = new SqlParameter();
            //param.ParameterName = "@Id";
            //param.SqlDbType = SqlDbType.Int;
            //param.Value = Id;
            //ExecuteDataSet
            DataSet dd=SqlHelper.ExecuteDataset(ConfigurationManager.AppSettings["ConnectionString"], CommandType.StoredProcedure, "GetEmployee");
            //DataSet ds = SqlHelper.ExecuteDataset(, CommandType.StoredProcedure, "GetEmployee", param);

        }

    }
}
