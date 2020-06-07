using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenericEngine.ServiceContract.Infrastructure;
using System.Linq.Expressions;


namespace MobileApplication.DataModel
{
   public class RoleProgramModel : GenericModel
    {
       public RoleProgramModel()          
       {         
            SetPublicSettings("صلاحيات المجموعات", "صلاحية المجموعة");          
       }
        public int ID { get; set; }
        public int IdRole { get; set; }
        public int IdProgram { get; set; }
    }
}
