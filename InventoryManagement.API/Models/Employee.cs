using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InventoryManagement.API.Models
{
    public class Employee
    {
        public long EmpId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }
        public string Description { get; set; }
    }
}