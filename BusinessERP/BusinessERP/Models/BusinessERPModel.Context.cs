//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BusinessERP.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class BusinessERPDbContext : DbContext
    {
        public BusinessERPDbContext()
            : base("name=BusinessERPDbContext")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<CompanyInvoice> CompanyInvoices { get; set; }
        public virtual DbSet<CompanyLineItem> CompanyLineItems { get; set; }
        public virtual DbSet<CompanyProductCategory> CompanyProductCategories { get; set; }
        public virtual DbSet<CompanyProduct> CompanyProducts { get; set; }
        public virtual DbSet<CustomerInvoice> CustomerInvoices { get; set; }
        public virtual DbSet<CustomerLineItem> CustomerLineItems { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<CustomerText> CustomerTexts { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<EmployeeText> EmployeeTexts { get; set; }
        public virtual DbSet<JobCategory> JobCategories { get; set; }
        public virtual DbSet<Notice> Notices { get; set; }
        public virtual DbSet<RawMaterial> RawMaterials { get; set; }
        public virtual DbSet<RawMaterialUsesLog> RawMaterialUsesLogs { get; set; }
        public virtual DbSet<RegistrationRequestLog> RegistrationRequestLogs { get; set; }
        public virtual DbSet<RegistrationRequest> RegistrationRequests { get; set; }
        public virtual DbSet<RequestToSupport> RequestToSupports { get; set; }
        public virtual DbSet<SupplierLog> SupplierLogs { get; set; }
        public virtual DbSet<SupplierSchedule> SupplierSchedules { get; set; }
        public virtual DbSet<SupportLog> SupportLogs { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<VendorProduct> VendorProducts { get; set; }
        public virtual DbSet<Vendor> Vendors { get; set; }
    }
}
