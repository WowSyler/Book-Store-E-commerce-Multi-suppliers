using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookStore.ViewModel
{
    public class Sales
    {
        //cus
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Country { get; set; }
        public string City { get; set; }      
        public string Address { get; set; }
        //OrderProduscts
        public Nullable<int> OrderID { get; set; }
        public Nullable<int> ProductID { get; set; }
        public Nullable<decimal> Price { get; set; }
        public Nullable<int> Quantity { get; set; }
        public Nullable<decimal> TotalPrice { get; set; }
        public Nullable<System.DateTime> OrderDate { get; set; }
        public Nullable<int> SupplierID { get; set; }
        //books
        public string Name { get; set; }
        public string Picture1 { get; set; }
    }
}