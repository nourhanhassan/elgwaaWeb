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
    
    public partial class GroupTarget
    {
        public int ID { get; set; }
        public int GroupID { get; set; }
        public int TargetTypeID { get; set; }
        public int PeriodTypeID { get; set; }
        public Nullable<int> JuzID { get; set; }
        public Nullable<int> SurahID { get; set; }
        public int AyaatCount { get; set; }
        public long NotificationTime { get; set; }
        public Nullable<long> NotificationID { get; set; }
        public Nullable<int> StartPage { get; set; }
        public Nullable<int> EndPage { get; set; }
        public Nullable<int> PagesCount { get; set; }
    
        public virtual Group Group { get; set; }
        public virtual PeriodType PeriodType { get; set; }
        public virtual TargetType TargetType { get; set; }
    }
}