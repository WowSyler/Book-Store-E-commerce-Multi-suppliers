//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BookStore.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Comments
    {
        public int commentID { get; set; }
        public string name { get; set; }
        public string text { get; set; }
        public Nullable<int> ProductsID { get; set; }
        public Nullable<System.DateTime> Commentdate { get; set; }
        public Nullable<int> customersID { get; set; }
        public string customersname { get; set; }
        public Nullable<bool> active { get; set; }
    
        public virtual Customers Customers { get; set; }
        public virtual Products Products { get; set; }
    }
}
