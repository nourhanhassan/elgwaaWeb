using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileApplication.DataService
{
    public class ImageService
    {
        private readonly AppSettingService _appSettingService;

        public ImageService()
        {
            _appSettingService = new AppSettingService();
        }

        public static void ProcessImage(Stream fileStream,string savePath)
        {

            var resizedImage = ImageService.ResizeImage(fileStream);
            resizedImage.Save(savePath);
          //  var resultImage = ImageService.AddWaterMarkAndSaveImage(resizedImage, savePath);
            ImageService.ThumbAndSaveImage(resizedImage, savePath);
        }

        public static Bitmap ResizeImage(Stream fileStream, int maxWidth, int maxHeight)
        {
           
            //convert the stream to image and get it's dimensions
            Image imageToBeResized = Image.FromStream(fileStream);
            
            float ImageWidth = imageToBeResized.Width;
            float ImageHeight = imageToBeResized.Height;
            float ratio = ImageWidth / ImageHeight;

            //if the height or width are bigger than the max resize the image
            if (ImageHeight > maxHeight || ImageWidth > maxHeight)
            {
                //compare the difference to know which dimension need to be resized by the biggest value
                int HeightDifference = (int)(ImageHeight - maxHeight);
                int WidthDifference = (int)(ImageWidth - maxWidth);

                //get the new diminsions
                if (HeightDifference > WidthDifference)
                {
                    //will change image height to be same as max height, then resize the width with same ratio
                    ImageHeight = maxHeight;
                    ImageWidth = ratio * ImageHeight;
                }
                else
                {
                    //will change image width to be same as max width, then resize the height with same ratio
                    ImageWidth = maxWidth;
                    ImageHeight = ImageWidth / ratio;
                }

                //resize the image by it's new dimensions
                var destRect = new Rectangle(0, 0, (int)ImageWidth, (int)ImageHeight);
                var destImage = new Bitmap((int)ImageWidth, (int)ImageHeight);

                destImage.SetResolution(imageToBeResized.HorizontalResolution, imageToBeResized.VerticalResolution);

                using (var graphics = Graphics.FromImage(destImage))
                {
                    graphics.CompositingMode = CompositingMode.SourceCopy;
                    graphics.CompositingQuality = CompositingQuality.AssumeLinear;
                    graphics.InterpolationMode = InterpolationMode.Low;
                    graphics.SmoothingMode = SmoothingMode.None;
                    graphics.PixelOffsetMode = PixelOffsetMode.None;
                  
                    using (var wrapMode = new ImageAttributes())
                    {
                        wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                        graphics.DrawImage(imageToBeResized, destRect, 0, 0, imageToBeResized.Width, imageToBeResized.Height, GraphicsUnit.Pixel, wrapMode);
                    }
                }

                return destImage;
            }
            else //if the image diminsions doesn't exeed the max values=>return the original image
            { return (Bitmap)imageToBeResized; }
        }
        public static Bitmap ResizeImage(Stream fileStream)//, int maxWidth = 1024, int maxHeight = 1024)
        {
            //get max height and width from configurations
            int maxWidth, maxHeight;
            new AppSettingService().GetMaximumUploadHeightAndWidth(out maxHeight, out maxWidth);

            return ResizeImage(fileStream, maxWidth, maxHeight);
        }

        public static Bitmap ThumbAndSaveImage(Image image,string filePath)
        {
            int maxWidth, maxHeight;
            new AppSettingService().GetMaximumThumbHeightAndWidth(out maxHeight, out maxWidth);

            string thumbPath = filePath.Substring(0,filePath.LastIndexOf(".")) +"_thumb"+ filePath.Substring(filePath.LastIndexOf("."));
            Bitmap resultBitmap = null;
            using (MemoryStream memoryStream = new MemoryStream())
            {
                image.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                resultBitmap =  ResizeImage(memoryStream, maxWidth, maxHeight);

                ImageCodecInfo jpgEncoder = GetEncoder(ImageFormat.Jpeg);
                System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Quality;

                EncoderParameters myEncoderParameters = new EncoderParameters(1);

                EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, 50L);
                myEncoderParameters.Param[0] = myEncoderParameter;


                resultBitmap.Save(thumbPath,jpgEncoder,myEncoderParameters);
            }

            return resultBitmap;
            
        }

        public static Bitmap MergeTwoImages(Image firstImage, Image secondImage)
        {
            if (firstImage == null)
            {
                throw new ArgumentNullException("firstImage not found");
            }

            if (secondImage == null)
            {
                throw new ArgumentNullException("secondImage not found");
            }

            int outputImageWidth = firstImage.Width;// > secondImage.Width ? firstImage.Width : secondImage.Width;

            int outputImageHeight =firstImage.Height; //+ secondImage.Height + 1;

            float maxAllowedWaterMarkRatio = 0.4f; //max allowed ratio before resizing the image

            Bitmap outputImage = new Bitmap(outputImageWidth, outputImageHeight, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            using (Graphics graphics = Graphics.FromImage(outputImage))
            {

                graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Low;

                graphics.DrawImage(firstImage, new Rectangle(new Point(), new Size(outputImageWidth, outputImageHeight)),
                    new Rectangle(new Point(), firstImage.Size), GraphicsUnit.Pixel);
                ColorMatrix matrix = new ColorMatrix();

                //set the opacity  
                matrix.Matrix33 = 0.6f;

                //create image attributes  
                ImageAttributes attributes = new ImageAttributes();

                //set the color(opacity) of the image  
                attributes.SetColorMatrix(matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
                
                //scale the width to 20% of the total width if the watermark exceeds 20% in width ;

                float widthPercentage = (float)secondImage.Width / (float)outputImageWidth;
                float heightPercentage = (float)secondImage.Height / (float)outputImageHeight;

                int secondImageWidth = secondImage.Width;
                int secondImageHeight = secondImage.Height;

                if(widthPercentage>maxAllowedWaterMarkRatio && widthPercentage>heightPercentage){
                    secondImageWidth = (int)(maxAllowedWaterMarkRatio*outputImageWidth);
                    secondImageHeight = (int)(((float)secondImageWidth/(float)secondImage.Width)*secondImage.Height);
                }
                else if(heightPercentage>maxAllowedWaterMarkRatio && heightPercentage>widthPercentage){
                    secondImageHeight = (int)(maxAllowedWaterMarkRatio*outputImageHeight);
                    secondImageWidth = (int)(((float)secondImageHeight/(float)secondImage.Height)*secondImage.Width);
                }
                 //secondImageWidth = widthPercentage > maxWaterMarkRatio ? (int)(maxWaterMarkRatio * outputImageWidth) : secondImage.Width;
                //scale the height to 20% of the total height if the watermark exceeds 20% in height ;
                // secondImageHeight = heightPercentage > maxWaterMarkRatio ? (int)(maxWaterMarkRatio * outputImageHeight) : secondImage.Height;



                graphics.DrawImage(secondImage, new Rectangle(new Point((outputImageWidth/2)-(secondImageWidth/2), (outputImageHeight/2) -(secondImageHeight/2)), new Size(secondImageWidth,secondImageHeight)),
                    0,0,secondImage.Width,secondImage.Height, GraphicsUnit.Pixel,attributes);

                
            }
            
            return outputImage;
        }

        public static Bitmap AddWaterMarkToImage(Stream fileStream )
        {
            Image imageToBeWaterMarked = Image.FromStream(fileStream);
            return AddWaterMarkToImage(imageToBeWaterMarked);
        }

        public static Bitmap AddWaterMarkToImage(Image imageToBeWaterMarked)
        {
            var waterMarkImage = Image.FromFile(System.Web.HttpContext.Current.Server.MapPath("~/Areas/Site/Content/img/diar-watermark.png"));
            return MergeTwoImages(imageToBeWaterMarked, waterMarkImage);
        }

        public static Bitmap AddWaterMarkAndSaveImage(Stream fileStream, string filePath)
        {
            return AddWaterMarkAndSaveImage(Image.FromStream(fileStream),  filePath);

        }

        public static Bitmap AddWaterMarkAndSaveImage(Image imageToBeWaterMarked, string filePath)
        {
         
            var waterMarkImage = Image.FromFile(System.Web.HttpContext.Current.Server.MapPath("~/Areas/Site/Content/img/diar-watermark.png"));
            Bitmap resultImage= MergeTwoImages(imageToBeWaterMarked, waterMarkImage);

            ImageCodecInfo jpgEncoder = GetEncoder(ImageFormat.Jpeg);
            System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Quality;

            EncoderParameters myEncoderParameters = new EncoderParameters(1);

            EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, 50L);
            myEncoderParameters.Param[0] = myEncoderParameter;

            resultImage.Save(filePath, jpgEncoder, myEncoderParameters);
           

            return resultImage;

        }

        private static ImageCodecInfo GetEncoder(ImageFormat format)
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
    }
}
