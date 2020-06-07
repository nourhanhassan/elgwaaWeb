using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileApplication.DataModel.QvDataAnnotation
{
    public static class ValidationExpressions
    {
        public const string ValidPrice = @"^(\-)?\d{1,10}(\.\d+)?$";
        public const string ValidUnitPrice = @"^(\-)?\d{1,6}(\.\d+)?$";
        public const string phone = @"^\(?[\d]{3}\)?[\s-]?[\d]{3}[\s-]?[\d]{4}$";
        public const string revMailBoxNo = @"^[0-9]{5}";
        public const string revNationalIdNo = @"^[0-9]{10}";
        public const string revNationalIdNo2 = @"^[a-zA-Z0-9]{0,20}$";

        public const string revPostalCode = @"^[0-9]{5}";
        public const string revLoginName = @"^[a-zA-Z0-9]{0,20}$";
        public const string revString6 = @"^[a-zA-Z0-9 \u0600-\u06ff,./\s_-]{0,6}$";

        public const string revStringOnly50 = @"^[a-zA-Z\u0600-\u06ff,./\s_-]{0,50}$";
        public const string revStringOnly150 = @"^[a-zA-Z\u0600-\u06ff,./\s_-]{0,150}$";

        public const string revString15 = @"^[a-zA-Z0-9 \u0600-\u06ff,./\s_-]{0,15}$";
        public const string revString20 = @"^[a-zA-Z0-9 \u0600-\u06ff,./\s_-]{0,20}$";
        public const string revString50 = @"^[a-zA-Z0-9 \u0600-\u06ff,./\s_-]{0,50}$";
        public const string revString100 = @"^[a-zA-Z0-9 \u0600-\u06ff,.\s_-]{0,100}$";
        public const string revString150 = @"^[a-zA-Z0-9 \u0600-\u06ff,./\s_-]{0,150}$";
        public const string revString200 = @"^[a-zA-Z0-9 \u0600-\u06ff,./\s_-]{0,200}$";
        public const string revString250 = @"^[a-zA-Z0-9 \u0600-\u06ff,./\s_-]{0,250}$";
        public const string revString300 = @"^[a-zA-Z0-9 \u0600-\u06ff,./\s_-]{0,300}$";
        public const string revString350 = @"^[a-zA-Z0-9 \u0600-\u06ff,./\s_-]{0,350}$";
        public const string revString400 = @"^[a-zA-Z0-9 \u0600-\u06ff,./\s_-]{0,400}$";
        public const string revString450 = @"^[a-zA-Z0-9 \u0600-\u06ff,./\s_-]{0,450}$";
        public const string revString500 = @"^[a-zA-Z0-9 \u0600-\u06ff,./\s_-]{0,500}$";
        public const string revString750 = @"^[a-zA-Z0-9 \u0600-\u06ff,./\s_-]{0,750}$";

        public const string revSafeString15 = @"^[a-zA-Z0-9 ؟?\u0600-\u06ff._^%$#!~@,&*()-]{0,15}$";
        public const string revSafeString20 = @"^[a-zA-Z0-9 ؟?\u0600-\u06ff._^%$#!~@,&*()-]{0,20}$";
        public const string revSafeString30 = @"^[a-zA-Z0-9 ؟?\u0600-\u06ff._^%$#!~@,&*()-]{0,30}$";
        public const string revSafeString50 = @"^[a-zA-Z0-9 ؟?\u0600-\u06ff._^%$#!~@,&*()-]{0,50}$";
        public const string revSafeString100 = @"^[a-zA-Z0-9 ؟?\u0600-\u06ff._^%$#!~@,&*()-]{0,100}$";
        public const string revSafeString150 = @"^[a-zA-Z0-9 ؟?\u0600-\u06ff._^%$#!~@,&*()-]{0,150}$";
        public const string revSafeString200 = @"^[a-zA-Z0-9 ؟?\u0600-\u06ff._^%$#!~@,&*()-]{0,200}$";
        public const string revSafeString250 = @"^[a-zA-Z0-9 ؟?\u0600-\u06ff._^%$#!~@,&*()-]{0,250}$";
        public const string revSafeString300 = @"^[a-zA-Z0-9 ؟?\u0600-\u06ff._^%$#!~@,&*()-]{0,300}$";
        public const string revSafeString350 = @"^[a-zA-Z0-9 ؟?\u0600-\u06ff._^%$#!~@,&*()-]{0,350}$";
        public const string revSafeString400 = @"^[a-zA-Z0-9 ؟?\u0600-\u06ff._^%$#!~@,&*()-]{0,400}$";
        public const string revSafeString450 = @"^[a-zA-Z0-9 ؟?\u0600-\u06ff._^%$#!~@,&*()-]{0,450}$";
        public const string revSafeString500 = @"^[a-zA-Z0-9 \n?\u0600-\u06ff._^%$!~@,&*()-]{0,500}$";
        public const string revSafeStringMax = @"^[a-zA-Z0-9 \n?\u0600-\u06ff._^%$!~@,&*()-]{0,}$";
        public const string ValidSafeString = @"^[a-zA-Z0-9 ؟?\u0600-\u06ff._^%$#!~@,&*()-]*$";
        public const string revPassword = @"^(?=.*\d).{4,8}$";
        public const string revNumber50 = @"^[0-9]{1,50}$";
        public const string revNumb = @"^[1-9][0-9]{0,}$";
        public const string revNumber5 = @"^[0-9]{1,5}$";
        public const string revNumber4 = @"^[0-9]{1,4}$";
        public const string revNumberOnly = @"^[1-9]+[0-9]*$";
        public const string revDecemailNumber = @"\b[0-9]{1,18}\b";
        public const string revMail = @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";
        public const string revMailChar50 = @"^([\w\.\-]{1,20})@([\w\-]{1,20})((\.(\w){3,5})+)$";
        public const string revMail50 = @"[\w\.\-]\@[\w]{1,50}(\.[\w]{2,10}?\.[\w]{2,2}|\.[\w]{2,10})";

        public const string revMail2 = @"/\w+([-+.']\w+)*&#64;\w+([-.]\w+)*\.\w+([-.]\w+)*";
        public const string revMail1 = @"/^[_a-z0-9]+(\.[_a-z0-9]+)*&#64;[a-z0-9-]+(\.[a-z0-9-]+)*(\.[a-z]{2,4})$/";
        public const string revMailfull = @"\[_a-z0-9]+(\.[_a-z0-9]+)*@[a-z0-9-]+(\.[a-z0-9-]+)*(\.[a-z]{2,4})$/";
      //  public const string revMail530 = @"\[\w\.\-]{1,255}\@[\w]{1,255}(\.[\w]{2,10}?\.[\w]{2,2}|\.[\w]{2,10})*";
        public const string revMail530 =@"[\w\.\-]{1,255}\@[\w]{1,255}(\.[\w]{2,10}?\.[\w]{2,2}|\.[\w]{2,10})";
       
        //example http://www.mysite.com 
        public const string revUrl = @"(http|https)://([\w-]+\.)+[\w-]+(/[\w- ./?%&amp;=]*)?";

        //example https://www.google.com.eg/maps/@31.2527269,29.9749954,16z
        public const string revUrlFull = @"^(http:\/\/www.google.com.eg\/maps\/@|https:\/\/www.google.com.eg\/maps\/@)[-+]?[0-9][0-9]\.[0-9]+[,][-+]?[0-9][0-9]\.[0-9]+[,]([0-9]|[0-9]\.([0-9][0-9]|[0-9])|[0-9][0-9]|[0-9][0-9]\.([0-9][0-9]|[0-9]))(z)$";

        public const string revStringmax = @"^[a-zA-Z0-9\u0600-\u06ff,./\s_-]{0,}$";
        public const string ImageExtensions = @"gif,png,jpeg,jpg";
        public const string FileExtensions = @"doc,docx,pdf,xls,gif,png,jpeg,jpg";
        public const string MaxImageSize = @"1024000";
        public const string MinHeightForEmployee = "240";
        public const string MinWidthForEmployee = "320";
        public const string ArVision = "31";
        public const string ArMession = "2";
        public const string Numbers = @"^\d{1,15}$";
        public const string MonthNumbers = @"^\d{3,60}$";
        public const string MobNumbersG = @"^[0-9\u0660-\u0669]{10,15}$";
        public const string MobNumbers = @"^\d{6,10}$";
        public const string revPhone = @"^[0-9]{7,15}$";
        public const string revPhone9 = @"^[0-9]{1,9}$";
        public const string ValidRegularPhone = @"[\+]{0,1}(\d{10,13}|[\(][\+]{0,1}\d{2,}[\13)]*\d{5,13}|\d{2,6}[\-]{1}\d{2,13}[\-]*\d{3,13})";
        public const string revNumber9 = @"^[0-9]{1,9}$";
        public const string revNumber10 = @"^[0-9]{1,10}$";
        public const string revNumber15 = @"^[0-9]{1,15}$";
        public const string numberonly = @"^\d{1,9}$";
        public const string orderNumberOnly = @"^\d{1,3}$";
        public const string revCivilNum = @"^\d{10,10}$";
        //public const string revCivilNum = @"^\d[۹-۰0-9]{10,10}+$";
        public const string BankAcoountNo = @"^[a-zA-Z]{2,2}[0-9]{22,22}$";
        public const string revNumber7 = @"^[0-9]{1,7}$";
        public const string revNumber30 = @"^[0-9]{1,30}$";
        public const string revNumberOnly5 = @"^[0-9]{1,5}$";
        public const string revNumberOnly50 = @"^[0-9]{1,50}$";
        public const string revNumberOnly150 = @"^[0-9]{1,150}$";
        public const string revNumber2 = @"^[0-9]{1,2}$";
        public const string revMobile = @"^[0-9]{10,13}$";
        public const string number = @"^[0-9]{1,}$";
        public const string revNumberOnlyExpectedZero = @"^[0-9][0-9]{0,}$";
        public const string revNumberOnlyExpectedZeroMinus = @"^[1-9][0-9]{0,}$";
        public const string revNumberOnlyExpectedZeroMinusWithLength = @"^[1-9][0-9]{0,9}$";
        public const string charactersOnly200 = @"^[a-zA-Z\u0600-\u06ff,./\s_-]{0,200}$";
        public const string lettersandnumbersonly200 = @"^[a-zA-Z0-9\u0600-\u06ff ]{0,200}$";
        public const string lettersandnymbersonly = @"^[a-zA-Z0-9]+$";
        public const string lettersnumbersspecialchar = @"^[a-zA-Z0-9\u0600-\u06FF .,\-:\s_]{0,350}$";
        public const string registrationNum = @"^\d{0,11}$";

        public const string TwitterValid = @"(?:(?:http|https):\/\/)?(?:www.)?twitter.com\/(?:(?:\w)*#!\/)?(?:pages\/)?(?:[?\w\-]*\/)?(?:profile.php\?id=(?=\d.*))?([\w\-]*)?";
        public const string FacebookValid = @"http[s]?://(www|[a-zA-Z]{2}-[a-zA-Z]{2})\.facebook\.com/(pages/[a-zA-Z0-9\.-]+/[0-9]+|[a-zA-Z0-9\u0600-\u06ff\.-]+)[/]?$";
        //public const string FacebookValid = @"http[s]?://(www|[a-zA-Z]{2}-[a-zA-Z]{2})\.facebook\.com/(pages/[a-zA-Z0-9\.-]+/[0-9]+|[a-zA-Z0-9\u0600-\u06ff\.-]+)[/]?$";
        public const string ValidUrlYoutube = @"(?:(?:http|https):\/\/)?(?:www.)?youtube.com\/(?:(?:\w)*#!\/)?(?:pages\/)?(?:[?\w\-]*\/)?(?:profile.php\?id=(?=\d.*))?([\w\-]*)?";
        public const string ValidUrlGooglePlus = @"(?:(?:http|https):\/\/)?(?:www.)?plus.google.com\/(?:(?:\w)*#!\/)?(?:pages\/)?(?:[?\w\-\]*+\/)?(?:profile.php\?id=(?=\d.*))?([\w\-]*)?";

        public const string ValidUrlInstagram = @"http[s]?://(www|[a-zA-Z]{2}-[a-zA-Z]{2})\.instagram\.com/(pages/[a-zA-Z0-9\.-]+/[0-9]+|[a-zA-Z0-9\.-_]+)[/]?$";

        public const string ValidUrlYoutubeVideo = @"^(https?\:\/\/)?(www\.)?(youtube\.com|youtu\.?be)\/.+$";
        public const string revLatitudeLongitiude = @"^-?([1-8]?[0-9]\.{1}\d{1,6}$|90\.{1}0{1,6}$)";
        public const string revPhoneSaudiArabia = @"^(\d{5}\d{2}\d{8})*$";
        public const string revPortNo6 = @"^[0-9]{1,6}$";
        public const string nameRange6to150 = @"^[a-zA-Z0-9 \u0600-\u06ff,./\s_-]{6,150}$"; // @"^.{6,150}$"
        public const string revPhone2 = @"^[0-9]{9,15}$";
        public const string revStringNoNum150 = @"^[a-zA-Z \u0600-\u06ff,./\s_-]{0,150}$";
        public const string revStringNoNum100 = @"^[a-zA-Z \u0600-\u06ff,./\s_-]{0,100}$";
        public const string ValidSafeString150 = @"^[a-zA-Z0-9 ؟?\u0600-\u06ff._^%$#!~@,&*()-]{0,150}$";
        public const string ValidDecimalNum = @"^(100|\d{1,7})(\.\d{1,2})?$";
        public const string ValidDecimal15Num = @"^(100|\d{1,15})(\.\d{1,2})?$";
        public const string ValidDecimalNum3 = @"^(100|\d{1,7})(\.\d{1,3})?$";
        public const string revSafeString1000 = @"^[a-zA-Z0-9 \n?\u0600-\u06ff._^%$!~@,&*()-]{0,1000}$";

        public const string ValidDecimalNumWithoutZero = @"^(?!0)(100|\d{1,7})(\.\d{1,2})?$";
        

    }
}
