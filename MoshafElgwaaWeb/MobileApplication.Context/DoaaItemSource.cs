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
    
    public partial class DoaaItemSource
    {
        public int ID { get; set; }
        public int DoaaID { get; set; }
        public int SourceNumber { get; set; }
        public string ItemSource { get; set; }
        public string ItemSynonyms { get; set; }
        public string Type { get; set; }
        public string Reference { get; set; }
    
        public virtual Doaa Doaa { get; set; }
    }
}
