using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GenericEngine.ServiceContract.Infrastructure;
using System.Linq.Expressions;

namespace MobileApplication.DataModel
{
    public class EmployeeRoleProgramModel :GenericModel
    {
        public int ID { get; set; }
        public int IdEmployee { get; set; }
        public int IdRoleProgram { get; set; }

        public virtual EmployeeModel CU_Employee { get; set; }
        public virtual RoleProgramModel CU_Role_Program { get; set; }

    }
}
