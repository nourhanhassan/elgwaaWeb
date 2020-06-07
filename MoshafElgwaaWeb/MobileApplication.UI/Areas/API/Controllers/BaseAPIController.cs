using MobileApplication.DataService;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MobileApplication.UI.Areas.API.Controllers
{
    public class BaseAPIController:Controller
    {

        protected override void OnException(ExceptionContext filterContext)
        {
            try
            {
                int? userID = null;
                string url = null;
                string data = null;

                try
                {
                 
                    url = HttpContext.Request.Url.OriginalString;

                    Stream req = Request.InputStream;
                    req.Seek(0, System.IO.SeekOrigin.Begin);
                    data = new StreamReader(req).ReadToEnd();



                }
                catch (Exception ex)
                {

                }
                new ExceptionHandlerService().LogException(filterContext.Exception, url, data);
                //Handle exception here

                //filterContext.ExceptionHandled = true; //set the exception as handled so that it won't trigger default error page
            }
            catch (Exception ex)
            {
                //an exception happened while handling exception !!!!
            }
            base.OnException(filterContext);
        }
       

    }
}