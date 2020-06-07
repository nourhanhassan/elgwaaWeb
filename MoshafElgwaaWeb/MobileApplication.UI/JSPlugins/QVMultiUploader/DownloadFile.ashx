<%@ WebHandler Language="C#" Class="ImageUpload" %>

using System;
using System.Web;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Web.Script.Serialization;

public class ImageUpload : IHttpHandler {

    public void ProcessRequest(HttpContext context)
    {

        try
        {
            string filepath = context.Request.QueryString["filepath"];
            string originalFileName = context.Request.QueryString["originalFileName"];



            FileInfo file = new FileInfo(context.Server.MapPath(filepath));

            if (file.Exists)
            {
                context.Response.Clear();
                context.Response.ClearHeaders();
                context.Response.ClearContent();
                context.Response.AddHeader("Content-Disposition", "attachment; filename=" + originalFileName);
                context.Response.AddHeader("Content-Length", file.Length.ToString());
               // context.Response.ContentType = "text/plain";
                context.Response.Flush();
                context.Response.TransmitFile(file.FullName);
                context.Response.End();
            }
            else
            {
                context.Response.Write("File not found");
            }
        }
        catch (Exception ex)
        {
            context.Response.Write("File not found");
        }


    }
    public bool IsReusable {
        get {
            return false;
        }
    }
}