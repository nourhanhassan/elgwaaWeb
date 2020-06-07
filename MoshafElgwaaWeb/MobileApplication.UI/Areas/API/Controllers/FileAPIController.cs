using MobileApplication.DataService;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MobileApplication.UI.Areas.API.Controllers
{
    public class FileAPIController : Controller
    {
        //
        // GET: /API/FileAPI/
        [HttpPost]
        public ActionResult UploadFile(object context=null)
        {
            string path = "/DataUpload/Images/"; //ConfigurationManager.AppSettings["InspectionFilesPath"];
            if ( HttpContext.Request.Files.Count > 0)
            {      
                HttpPostedFileBase file = HttpContext.Request.Files[0];
                string strExtension = System.IO.Path.GetExtension(file.FileName).Length >0 ?System.IO.Path.GetExtension(file.FileName).ToLower().Remove(0, 1):"flv";
                string MimeType = file.ContentType;
                string desExt;
                if (MimeType.Contains("audio")) { desExt = "mp3"; }
                else if (MimeType.Contains("video")) { desExt = "mp4"; }
                else desExt = strExtension;

                string strFileOriginalName = file.FileName;
                string orExtStrFileNewName = Guid.NewGuid().ToString() + "." + strExtension; 

                string strFileNewName = Guid.NewGuid().ToString()+"."+desExt;
                string orExtStrSaveLocation = HttpContext.Server.MapPath(path) + orExtStrFileNewName;

                string strSaveLocation = HttpContext.Server.MapPath(path) + strFileNewName;
                string hostname = HttpContext.Request.Url.Scheme + System.Uri.SchemeDelimiter + HttpContext.Request.Url.Host + (HttpContext.Request.Url.IsDefaultPort ? "" : ":" + HttpContext.Request.Url.Port);
                string fullpath = null;
                
               var converter = new NReco.VideoConverter.FFMpegConverter();
        //       var movtest = HttpContext.Server.MapPath(path) + "sample_iTunes.mov";
               //FileStream fileStream = new FileStream(movtest, FileMode.Open, FileAccess.Read);
               //fileStream.Seek(0, SeekOrigin.Begin);
               ////using (StreamReader sr = new StreamReader(movtest))
               ////{
               // var options =  new NReco.VideoConverter.ConvertSettings();
               // options.VideoCodec = "h264";
               // //options.AudioCodec = "libvo_aacen";  
               // var des = HttpContext.Server.MapPath(path) + "sample_iTunes_converted.mp4";
               // var tprocess = converter.ConvertLiveMedia(fileStream, "mov", des, "mp4",options);
               //             //"ffmpeg -i {fileStream}.mov -vcodec h264 -acodec aac -strict -2 des";

               //    tprocess.Start();
               //    tprocess.Wait();


                   //converter.ConvertMedia(movtest, HttpContext.Server.MapPath(path) + "sample_iTunes_converted2.mp4", "mp4");
             //  }
          
                
                    if((desExt =="mp3" && strExtension !=desExt) )
                    {
                        HttpContext.Request.Files[0].SaveAs(orExtStrSaveLocation);
                        converter.ConvertMedia(orExtStrSaveLocation, strSaveLocation, "mp3");
                        System.IO.File.Delete(orExtStrFileNewName);
                        /*
                         * //This method fails when converting from input stream
                        var process = converter.ConvertLiveMedia(HttpContext.Request.Files[0].InputStream, strExtension, strSaveLocation, desExt, new NReco.VideoConverter.ConvertSettings());
                        process.Start();
                        process.Wait();
                         * */
                    }

                        else if(desExt == "mp4" && strExtension != desExt)
                    {

                        HttpContext.Request.Files[0].SaveAs(orExtStrSaveLocation);
                        converter.ConvertMedia(orExtStrSaveLocation,strSaveLocation, "mp4");
                        System.IO.File.Delete(orExtStrFileNewName);
                    }
                    else 
                    {
                            //resize ,watermark and thumb image
                    
                       ImageService.ProcessImage(HttpContext.Request.Files[0].InputStream, strSaveLocation);
                     

                    }
                
               
                HttpContext.Response.ContentType = "text/html";
                //HttpContext.Response.Write("[{status:'done', filename:'" + strFileNewName + "'}]");
                fullpath = hostname + path + strFileNewName;
                return Json(fullpath, JsonRequestBehavior.AllowGet);
            }
            else
                return Json((false), JsonRequestBehavior.AllowGet);   
        }


        public int ResizeAllFiles()
        {
            var placeFiles = Directory.GetFiles(Server.MapPath("~/DataUpload/Place/Image"));
            var placeFilesResized = Server.MapPath("~/DataUpload/Place/Image_resized");
            int convertedCount = 0;
            foreach (var placeFile in placeFiles)
            {
                Bitmap image = null;
                using (var fileStream = System.IO.File.Open(placeFile,FileMode.Open))
                {
                    try
                    {
                        string savePath = placeFilesResized + "\\" + placeFile.Substring(placeFile.LastIndexOf("\\") + 1);
                        var resizedimage = ImageService.ResizeImage(fileStream);
                      //  image = ImageService.AddWaterMarkAndSaveImage(resizedimage,savePath);
                        ImageService.ThumbAndSaveImage(resizedimage, savePath);
                        convertedCount++;
                    }
                    catch(Exception ex){

                    }
                    
                    fileStream.Close();
                    fileStream.Dispose();
                }
                
            }
            return convertedCount;

        }

        public int CreateThumbs()
        {
            var placeFiles = Directory.GetFiles(Server.MapPath("~/DataUpload/Comments"));
            int convertedCount = 0;
            foreach (var placeFile in placeFiles)
            {
                if (placeFile.ToLower().Contains("thumb") && placeFiles.Any(a=>a.Contains(placeFile) && a.ToLower().Contains("thumb")))
                {
                    continue;
                }
                Image image = null;
                using (var fileStream = System.IO.File.Open(placeFile, FileMode.Open))
                {
                    try
                    {
                       
                         image = Bitmap.FromStream(fileStream);
                        ImageService.ThumbAndSaveImage(image, placeFile);
                        convertedCount++;
                    }
                    catch (Exception ex)
                    {

                    }

                    fileStream.Close();
                    fileStream.Dispose();
                }

            }
            return convertedCount;
        }

    }
}
