using GenericEngine.Service.SharedService;
using MobileApplication.Context;
using Repository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileApplication.DataService
{
    public class BaseService
    {
        private  IUnitOfWork instance;
        private  ILogService LogService;

        public BaseService() { }

        protected  IUnitOfWork _unitOfWork
        {
            get
            {
                if (instance == null)
                {
                    instance = new UnitOfWork(GetContext());
                }
                
                return instance;
            }
            set { instance = value; }
        }

        public DbContext GetContext() 
        {
            var context = new QVMobileApplicationEntities();
          //  context.Configuration.ProxyCreationEnabled = false;
            return context;
        }
        

        public void RefreshContext()
        {
            this.instance.Dispose();
            this.instance.SetDB(GetContext());
            

        }

        public ILogService _LogService
        {
            get
            {
                //if (LogService == null)
                {
                    LogService = new CU_LogService();
                }
                return LogService;
            }
        }
    }
}
