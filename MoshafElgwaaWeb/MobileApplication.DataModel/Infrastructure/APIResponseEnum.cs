using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataModel.APIResponseEnums
{
    public enum APIResponseCodeEnum
    {

        /********General Codes ************/

        OK=200,
        ServerError=500,

        /*********Login Codes 1000**************/

        LoginSuccessful=1000,
        LoginWrongCredentials= 1001,
        LoginUserNotActive = 1002,
        
        LoginNeedsVerificationCode = 1003,
       
        /**********Change Status 2000 ************/
        AssignedSuccessfully=2000,
        ShipmentCancelled=2001,
        AssignedToAnotherDriver=2002,
        NoDriversFound=2003

        /***********Logout 3000************/
    }
}