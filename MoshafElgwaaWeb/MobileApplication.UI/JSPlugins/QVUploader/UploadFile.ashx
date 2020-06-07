<%@ WebHandler Language="C#" Class="ImageUpload" %>

using System;
using System.Web;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Web.Script.Serialization;

public class ImageUpload : IHttpHandler
{
    private const string ServerSafeFileExtentions = "png,jpg,jpeg,gif,bmp,svg,pdf,doc,docx,xls,xlsx,ppt,pptx,mp3,mp4";

    public void ProcessRequest(HttpContext context)
    {
        string path = context.Request.Form["folder"];

        //----------------check if directory doesn't exit
        if (!Directory.Exists(context.Server.MapPath(path)))
        {
            //if it doesn't, create it
            Directory.CreateDirectory(context.Server.MapPath(path));
        }

        if (context.Request.Files.Count > 0)
        {
            HttpPostedFile file = context.Request.Files[0];
            string strExtension = System.IO.Path.GetExtension(file.FileName).ToLower();

            string strFileName = Guid.NewGuid().ToString() + strExtension;
            string strSaveLocation = context.Server.MapPath(path) + strFileName;
            try
            {
                // check allowed extensions to prevent attacks using malware
                if (!ServerSafeFileExtentions.Split(',').Contains(strExtension.Replace(".", ""))) throw new Exception("File extension is rejected from server, " + strExtension);

                // save file
                // context.Request.Files[0].SaveAs(strSaveLocation);
                // QV.Service.ImageService.AddWaterMarkAndSaveImage(context.Request.Files[0].InputStream, strSaveLocation);
                MobileApplication.DataService.ImageService.ProcessImage(context.Request.Files[0].InputStream, strSaveLocation);


                context.Response.Write("[{status:'done', filename:'" + strFileName + "'}]");
            }
            catch (Exception ex)
            {
                context.Response.Write("[{status:'fail',str:'" + strSaveLocation + " Exception:" + ex.Message + "'}]");
            }
            context.Response.ContentType = "text/html";

        }
    }

    //public void ProcessRequest(HttpContext context)
    //{
    //    string path = context.Request.Form["folder"];

    //    //----------------check if directory doesn't exit
    //    if (!Directory.Exists(context.Server.MapPath(path)))
    //    {
    //        //if it doesn't, create it
    //        Directory.CreateDirectory(context.Server.MapPath(path));
    //    }

    //    if (context.Request.Files.Count > 0)
    //    {
    //        HttpPostedFile file = context.Request.Files[0];
    //        string strExtension = System.IO.Path.GetExtension(file.FileName).ToLower();

    //        string strFileName = Guid.NewGuid().ToString() + strExtension;
    //        string strSaveLocation = context.Server.MapPath(path) + strFileName;
    //        try
    //        {
    //            context.Request.Files[0].SaveAs(strSaveLocation);
    //            context.Response.Write("[{status:'done', filename:'" + strFileName + "'}]");
    //        }
    //        catch (Exception ex)
    //        {
    //            context.Response.Write("[{status:'fail',str:'" + strSaveLocation + " Exception:" + ex.Message + "'}]");
    //        }
    //        context.Response.ContentType = "text/html";

    //    }
    //}

    
    
    public bool IsReusable
    {
        get
        {
            return false;
        }
    }
}
