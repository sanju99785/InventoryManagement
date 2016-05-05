using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InventoryManagement.BAL.Common;

namespace InventoryManagement.BAL
{
    public class EmployeeBA
    {
        #region Property
        public long EmpId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }
        public string Description { get; set; }
        public Int32 ReturnCode { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Get Batch Process Scheduling Details
        /// </summary>
        /// <param name="EmpId">The emp identifier.</param>
        /// <returns></returns>
        public DataSet GetEmployee(long EmpId = 0, String searchValue = "")
        {
            try
            {
                GenericDALAccess objGenericDALAccess = new GenericDALAccess();
                List<SqlParameter> liSqlParameter = new List<SqlParameter>();
                liSqlParameter.Add(new SqlParameter() { ParameterName = "@EmpId", Value = EmpId.CheckNull() });
                return objGenericDALAccess.GetData("GetEmployee", liSqlParameter);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Insert OR Update Batch Process Scheduling Details
        /// </summary>
        /// <param name="objEmployeeBA">The object employee ba.</param>
        /// <returns></returns>
        public Int32 InsertUpdateEmployee(EmployeeBA objEmployeeBA)
        {
            try
            {
                GenericDALAccess objGenericDALAccess = new GenericDALAccess();
                List<SqlParameter> liSqlParameter = new List<SqlParameter>();
                if (objEmployeeBA != null)
                {
                    liSqlParameter.Add(new SqlParameter() { ParameterName = "@EmpId", Value = objEmployeeBA.EmpId.CheckNull() });
                    liSqlParameter.Add(new SqlParameter() { ParameterName = "@Name", Value = objEmployeeBA.Name.CheckNull() });
                    liSqlParameter.Add(new SqlParameter() { ParameterName = "@Address", Value = objEmployeeBA.Address.CheckNull() });
                    liSqlParameter.Add(new SqlParameter() { ParameterName = "@Phone", Value = objEmployeeBA.Phone.CheckNull() });
                    liSqlParameter.Add(new SqlParameter() { ParameterName = "@Mobile", Value = objEmployeeBA.Mobile.CheckNull() });
                    liSqlParameter.Add(new SqlParameter() { ParameterName = "@Description", Value = objEmployeeBA.Description.CheckNull() });
                    liSqlParameter.Add(new SqlParameter() { ParameterName = "@ReturnCode", Value = objEmployeeBA.ReturnCode, Direction = ParameterDirection.InputOutput });
                }

                return objGenericDALAccess.InsertUpdateData("InsertUpdateEmployee", liSqlParameter);
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Delete Batch Process Scheduling Details
        /// </summary>
        /// <param name="EmpId">The emp identifier.</param>
        /// <returns></returns>
        public Int32 DeleteEmployee(Int64 EmpId)
        {
            try
            {
                GenericDALAccess objGenericDALAccess = new GenericDALAccess();
                List<SqlParameter> liSqlParameter = new List<SqlParameter>();
                liSqlParameter.Add(new SqlParameter() { ParameterName = "@EmpId", Value = EmpId.CheckNull() });
                liSqlParameter.Add(new SqlParameter() { ParameterName = "@ReturnCode", Value = 0, Direction = ParameterDirection.InputOutput });

                return objGenericDALAccess.DeleteData("InsertUpdateEmployee", liSqlParameter);
            }
            catch (Exception)
            {

                throw;
            }
        }

        #endregion
    }
}

