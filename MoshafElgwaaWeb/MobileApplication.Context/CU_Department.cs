//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MobileApplication.Context
{
    using System;
    using System.Collections.Generic;
    
    public partial class CU_Department
    {
        public CU_Department()
        {
            this.CU_Department1 = new HashSet<CU_Department>();
            this.CU_Employee = new HashSet<CU_Employee>();
            this.CU_Employee1 = new HashSet<CU_Employee>();
        }
    
        public int ID { get; set; }
        public string Name { get; set; }
        public string Descriptoin { get; set; }
        public int IdHeadDepartment { get; set; }
        public string MainTask { get; set; }
        public Nullable<int> IdParent { get; set; }
    
        public virtual ICollection<CU_Department> CU_Department1 { get; set; }
        public virtual CU_Department CU_Department2 { get; set; }
        public virtual CU_HeadDepartment CU_HeadDepartment { get; set; }
        public virtual ICollection<CU_Employee> CU_Employee { get; set; }
        public virtual ICollection<CU_Employee> CU_Employee1 { get; set; }
    }
}
