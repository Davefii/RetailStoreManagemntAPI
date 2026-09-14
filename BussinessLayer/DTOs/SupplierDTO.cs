using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLayer.DTOs
{
    public class SupplierDTO
    {
        public int ID { get; set; }
        public string Supplier_Name { get; set; }
        public string Contact_Person { get; set; }
        public string Phone_Number { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public bool Status { get; set; }
    }
}
