using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.BAL.Common
{
    public class ErrorLogDetails
    {
        public string Message { get; set; }
        public string StackTrace { get; set; }
    }
    public class CommonMessages
    {
        #region Default Msg

        public const string DEFAULT_ERRORMESSAGE = "An internal error has occured.";
        public const string InsertSuccessMessage = "Record(s) Saved Successfully";
        public const string UpdateSuccessMessage = "Record(s) Updated Successfully";
        public const string DeleteSuccessMessage = "Record(s) Deleted Successfully";
        public const string NoDataFoundMessage = "No Data Found";
        public const string InvalidInputMessage = "Invalid Input Data";
        public const string Dublicate = "Data Already Exist";
        public const string ChildRecordExist = "Child table record found";
        #endregion
    }
}
