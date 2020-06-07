using MobileApplication.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileApplication.DataService
{
    public class ReportService
    {
        private readonly QVMobileApplicationEntities _Entities;

        public ReportService()
        {
            _Entities = new QVMobileApplicationEntities();
        }
        
        //public object Users()
        //{
        //    return _Entities.usp_DUC_REP_Users();
        //}
    }
}
