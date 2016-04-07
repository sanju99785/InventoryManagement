using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using InventoryManagement.BAL.Common;

namespace DAL
{
    public class GenericDALAccess
    {
        #region Methods
        /// <summary>
        ///   Get Account Head Master Details
        /// </summary>
        /// <param name="_storeProcedureName"></param>
        /// <param name="_liSqlParameter"></param>
        /// <returns></returns>
        public DataSet GetData(String _storeProcedureName, List<SqlParameter> _liSqlParameter)
        {
            try
            {
                return SqlHelper.ExecuteDataset(CommonFunctions.ConnectionString, CommandType.StoredProcedure, _storeProcedureName, _liSqlParameter.ToArray());

            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Insert OR Update Account Head Details
        /// </summary>
        /// <param name="_storeProcedureName"></param>
        /// <param name="_liSqlParameter"></param>
        /// <returns></returns>
        public Int32 InsertUpdateData(String _storeProcedureName, List<SqlParameter> _liSqlParameter)
        {
            try
            {
                SqlHelper.ExecuteDataset(CommonFunctions.ConnectionString, CommandType.StoredProcedure, _storeProcedureName, _liSqlParameter.ToArray());
                int _value = _liSqlParameter.Count - 1;
                return Convert.ToInt32(_liSqlParameter[_value].Value);
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Delete Account Head Details
        /// </summary>
        /// <param name="_storeProcedureName"></param>
        /// <param name="_liSqlParameter"></param>
        /// <returns></returns>
        public Int32 DeleteData(String _storeProcedureName, List<SqlParameter> _liSqlParameter)
        {
            try
            {
                SqlHelper.ExecuteDataset(CommonFunctions.ConnectionString, CommandType.StoredProcedure, _storeProcedureName, _liSqlParameter.ToArray());
                int _value = _liSqlParameter.Count - 1;
                return Convert.ToInt32(_liSqlParameter[_value].Value);
            }
            catch (Exception)
            {

                throw;
            }
        }

        #endregion
    }
}

