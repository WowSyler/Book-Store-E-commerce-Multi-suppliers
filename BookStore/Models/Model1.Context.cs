﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class bookStoreDBEntities : DbContext
    {
        public bookStoreDBEntities()
            : base("name=bookStoreDBEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Admin> Admin { get; set; }
        public virtual DbSet<Categories> Categories { get; set; }
        public virtual DbSet<Comments> Comments { get; set; }
        public virtual DbSet<Customers> Customers { get; set; }
        public virtual DbSet<Order> Order { get; set; }
        public virtual DbSet<OrderDetails> OrderDetails { get; set; }
        public virtual DbSet<OrderProducts> OrderProducts { get; set; }
        public virtual DbSet<Payment> Payment { get; set; }
        public virtual DbSet<Products> Products { get; set; }
        public virtual DbSet<Promo> Promo { get; set; }
        public virtual DbSet<ShippingDetails> ShippingDetails { get; set; }
        public virtual DbSet<Suppliers> Suppliers { get; set; }
    }
}
