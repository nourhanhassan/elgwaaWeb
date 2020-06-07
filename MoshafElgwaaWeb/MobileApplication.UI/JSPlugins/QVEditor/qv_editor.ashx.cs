using System;
using System.Web;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Web.Script.Serialization;


public class ImageEditorUpload : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
        string path = "/DataUpload/Content/Image/";
        if (context.Request.Files.Count > 0)
        {
            HttpPostedFile file = context.Request.Files[0];
            string strExtension = System.IO.Path.GetExtension(context.Request.Files[0].FileName).ToLower();

            string strFileName = Guid.NewGuid().ToString() + strExtension;
            string strSaveLocation = context.Server.MapPath("~/" + path) + strFileName;
            context.Request.Files[0].SaveAs(strSaveLocation);
            context.Response.ContentType = "text/html";
            context.Response.Write(new JavaScriptSerializer().Serialize(new {filelink = path + strFileName}));
        }
        else
        {
            string map_path = HttpContext.Current.Server.MapPath(path);
            List<ImageMapFile> files =
                Directory.GetFiles(map_path)
                         .Where(
                             i =>
                             new string[] {".jpeg", ".jpg", ".gif", ".png"}.Contains(Path.GetExtension(i).ToLower()))
                         .Select(i => Path.GetFileName(i))
                         .Select(i => new ImageMapFile
                             {
                                 thumb = path + i,
                                 image = path + i
                             }).ToList();
            context.Response.Write(new JavaScriptSerializer().Serialize(files));
        }
    }

    public bool IsReusable
    {
        get { return false; }
    }

    protected class ImageMapFile
    {
        public string thumb { get; set; }
        public string image { get; set; }
    }

}
