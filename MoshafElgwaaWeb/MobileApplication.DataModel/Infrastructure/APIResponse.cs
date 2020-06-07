using DataModel.APIResponseEnums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataModel.APIResponses
{
    public class APIResponse
    {
        public APIResponseCodeEnum code { get; set; }
        public object data { get; set; }

    }
}