using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookStore.ViewModel
{
    public class Cart
    {
        //orderdetails
        public int ProductID { get; set; }
        public Nullable<decimal> Price { get; set; }
        public Nullable<int> Quantity { get; set; }
        public Nullable<decimal> TotalPrice { get; set; }
        public Nullable<System.DateTime> OrderDate { get; set; }
        public Nullable<int> CustomersID { get; set; }
        public Nullable<int> SupplierID { get; set; }
        //suppliers       
        public string UserName { get; set; }
        //books
        public string Name { get; set; }
        public string Picture1 { get; set; }


    }
}