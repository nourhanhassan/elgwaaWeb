<%@ WebHandler Language="C#" Class="ImageUpload" %>

using System;
using System.Web;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using System.Drawing;
using System.Drawing.Imaging;
using nQuant;
public class ImageUpload : IHttpHandler {

    public void ProcessRequest(HttpContext context)
    {
      
        string folder= context.Request.Form["folder"];
        string path =  folder;
        string isResolutionImage = context.Request.Form["isResolutionImage"];
        string pixelsWidth = context.Request.Form["pixelsWidth"];
        string pixelsHeight = context.Request.Form["pixelsHeight"];
        string ConcatPrefix = context.Request.Form["ConcatPrefix"];
        if (pixelsHeight =="")
        {
            pixelsHeight = "0";
        }
        if (pixelsWidth == "")
        {
            pixelsWidth = "0";
        }
        
        string[] validStringArray = System.Configuration.ConfigurationManager.AppSettings["MimeTypes"].ToString().Split(',');
        

        Dictionary<string, byte[]> FileHeader = new Dictionary<string, byte[]>();
        FileHeader = Extentions.ValidHeaders(FileHeader);
        byte[] header;
        for (int i = 0; i < context.Request.Files.Count; i++)
        {
            if (context.Request.Files[i].ContentLength > 0 && context.Request.Files[i].ContentLength <= int.Parse(System.Configuration.ConfigurationManager.AppSettings["maxfileSize"].ToString()))
            {
                if (Extentions.CheckMimeType(context.Request.Files[i].ContentType))
                {
                    string fileExt;
                    fileExt = context.Request.Files[i].FileName.Substring(context.Request.Files[i].FileName.LastIndexOf('.') + 1).ToUpper();
                    byte[] tmp = FileHeader[fileExt];
                    header = new byte[tmp.Length];
                    // GET HEADER INFORMATION OF UPLOADED FILE
                    context.Request.Files[i].InputStream.Read(header, 0, header.Length);

                    if (Extentions.CompareArray(tmp, header))
                    {
                        //Valid
                        string strFileName = Guid.NewGuid().ToString() +
                                     Path.GetExtension(context.Request.Files[i].FileName);
                        string strSaveLocation = context.Server.MapPath(path) + strFileName;
                        context.Request.Files[i].SaveAs(strSaveLocation);
                        int flag = 0;
                        for (int h = 0; h < validStringArray.Count(); h++)
                        { 
                            int x = validStringArray[h].IndexOf("image/");
                            if (x>=0&&fileExt.ToLower().Contains(validStringArray[h].Substring(6).ToLower()))
                            {
                                flag = 1;
                            }
                            
                        }
                        if (flag==1)
                        {
                            //Sqweeze Your Image
                            //apply quantization algoritm on png and JPG compression algorithm on other image extensions<follow comments inside function declaration for more info>
                            SYI(strSaveLocation, strFileName);
                        }
                        if (isResolutionImage!="false")
                        {
                            //Resolute Your Image
                            //apply change image resolution to the submitted pixelsWidth and pixelsHeight,
                            //and save new image instance with the same provided name plus provided  ConcatPrefix in the same place strSaveLocation
                            RYI(strSaveLocation, strFileName, int.Parse(pixelsWidth), int.Parse(pixelsHeight), ConcatPrefix, fileExt);
                        }
                        context.Response.ContentType = "text/html";
                        context.Response.Write("[{status:'done', filename:'" + strFileName + "'}]");
                    }
                    else
                    {
                        //InValid
                        context.Response.Write("[{status:'fail'}]");
                    }
                }
            }
        }
    }
    private void RYI(string imgPath, string fileName, int pixelsWidth, int pixelsHeight, string ConcatPrefix, string fileExt)
    {
        
        Bitmap original = new Bitmap(imgPath);
        float scale;
        Bitmap resizedImage = new Bitmap(imgPath);
        //scale to original image width using 500 as default value
        if (pixelsHeight==0 && pixelsWidth==0)
        {
            scale = 500 / (float)original.Width;
            resizedImage = new Bitmap(original, (int)(original.Width * scale), (int)(original.Height * scale));
        }
        //scale to original Image width using desired output image width
        else if (pixelsHeight==0)
        {
            scale = (float)pixelsWidth / (float)original.Width;
            resizedImage = new Bitmap(original, (int)(original.Width * scale), (int)(original.Height * scale));
        }
        //scale to original Image height using desired output image height            
        else if (pixelsWidth==0)
        {
            scale = (float)pixelsHeight / (float)original.Height;
            resizedImage = new Bitmap(original, (int)(original.Width * scale), (int)(original.Height * scale));
        }
        //setting same path with renaming image to oldname+ConcatPrefix
       string newPath = imgPath.Substring(0, imgPath.LastIndexOf("\\")) + "\\" + fileName.Substring(0, fileName.IndexOf(".")) + ConcatPrefix +"."+ fileExt;
       resizedImage.Save(newPath);
    }
    private void SYI(string imgPath,string fileName)
    {
        File.SetAttributes(imgPath,FileAttributes.Normal);
        Bitmap inputImage = new Bitmap(imgPath);
        if (imgPath.ToLower().Contains(".png"))
        {
            //you can use here fixed bitdepth leaving it as it is 
            //Or adding the parameter FormatPixel.(choose desired format but pay attention to formats that doesn't apply to alpha channel(Transparency))
            Bitmap tempBMP = new Bitmap(inputImage.Width, inputImage.Height);
            //setting the graphics tools to redraw image on pallete
            Graphics g = Graphics.FromImage(tempBMP);
            g.FillEllipse(new SolidBrush(System.Drawing.Color.Red), 0, 0, tempBMP.Width, tempBMP.Width);
            g.DrawLine(new System.Drawing.Pen(System.Drawing.Color.Black), 0, 0, tempBMP.Width, tempBMP.Width);
            g.DrawLine(new System.Drawing.Pen(System.Drawing.Color.Black), tempBMP.Width, 0, 0, tempBMP.Width);
            g.Dispose();
            // Prepare image transparancy by copying it from original image,at current it is set to the 
            // color of the pixel in top left corner(0,0)
            ImageAttributes attr = new ImageAttributes();
            attr.SetColorKey(tempBMP.GetPixel(0, 0), tempBMP.GetPixel(0, 0));
            // Draw the image to your output using the transparancy key attributes
            Bitmap outputImage = new Bitmap(inputImage.Width, inputImage.Height);
            g = Graphics.FromImage(outputImage);
            Rectangle destRect = new Rectangle(0, 0, tempBMP.Width, tempBMP.Height);
            g.DrawImage(inputImage, destRect, 0, 0, tempBMP.Width, tempBMP.Height, GraphicsUnit.Pixel, attr);
            inputImage.Dispose();
            tempBMP.Dispose();
            g.Dispose();
            //apply Quantization Algorithm converting image bit depth to 8 bit
            //seting savingPath
            string filePrntPath = imgPath.Substring(0,imgPath.IndexOf(fileName));
            string newFullPath = filePrntPath + "1.png";
            var quantizer = new WuQuantizer();
            using (var bitmap = new Bitmap(imgPath))
            {
                using (var quantized = quantizer.QuantizeImage(outputImage))
                {
                    outputImage.Dispose();
                    quantized.Save(newFullPath);
                }
            }
            outputImage.Dispose();
            File.Delete(imgPath);
            File.Move(newFullPath, imgPath);
        }
        else
        {
            //setting the graphics tools to redraw image on pallete
            Bitmap blank = new Bitmap(inputImage.Width, inputImage.Height);
            Graphics g1 = Graphics.FromImage(blank);
            g1.Clear(System.Drawing.Color.Transparent);
            g1.DrawImage(inputImage, 0, 0, inputImage.Width, inputImage.Height);
            Bitmap tempImage = new Bitmap(blank);
            blank.Dispose();
            inputImage.Dispose();
            //Using JPEG Compression Encoder to 50% of the quality attribute(Vowala =D =D)
            using (Bitmap bmp1 = new Bitmap(tempImage))
            {
                //here we selected JPeg image format as it is classified as lossy image format (lowest byte size)
                ImageCodecInfo jpgEncoder = GetEncoder(ImageFormat.Jpeg);
                System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Quality;
                EncoderParameters myEncoderParameters = new EncoderParameters(1);
                //You can change the quality percentage here,Pay attention lowering this value under 40%(Its your Funeral =D =D)
                EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, 50L);
                myEncoderParameters.Param[0] = myEncoderParameter;
                bmp1.Save(imgPath, jpgEncoder, myEncoderParameters);
                inputImage = new Bitmap(tempImage);
                tempImage.Dispose();
            }
        }
    }
    //Getting the codecs for the selected image format
    public static ImageCodecInfo GetEncoder(ImageFormat format)
    {
        ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
        foreach (ImageCodecInfo codec in codecs)
        {
            if (codec.FormatID == format.Guid)
            {
                return codec;
            }
        }
        return null;
    }
    public bool IsReusable {
        get {
            return false;
        }
    }
}
