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
    
    public partial class UserCode
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public string Code { get; set; }
        public System.DateTime CreatedDate { get; set; }
    
        public virtual User User { get; set; }
    }
}