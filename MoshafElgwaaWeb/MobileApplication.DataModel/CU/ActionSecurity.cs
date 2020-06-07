using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using QVEnterprise;
using OperationalPlanning_Context ;

namespace Service.Contracts.Models.Security
{
    public class ActionSecurity
    {
        public Dictionary<QVEnterprise.ActionType, bool> Permission;

        public ActionSecurity(string strController)
        {
            GetPermission(strController);
        }

        /// <summary>
        /// check the user permission over all actions 
        /// of passed controller.
        /// </summary>
        /// <param name="strController"></param>
        private void GetPermission(string controller, string action = "")
        {
            string page = controller + (string.IsNullOrEmpty(action) ? "/" + action : "");
            EDLQ_AppEntities edm = new EDLQ_AppEntities();
            Permission = new Dictionary<QVEnterprise.ActionType, bool>();
            Permission = new QVEnterprise.ActionSecurity(page).Permission;
        }
    }
}