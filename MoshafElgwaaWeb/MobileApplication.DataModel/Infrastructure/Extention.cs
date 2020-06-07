using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
using System.Globalization;
using Contract_QvLib.QVUtil;
using System.Reflection.Emit;
using System.Security.Cryptography;
using System.IO;
using System.Text;
using System.Web.Hosting;
using System.Drawing;

/// <summary>
/// Summary description for Extention
/// </summary>
/// 

namespace DataModel.Extention
{
    public static partial class contract_Extention
    {

        /// <summary>
        /// Convert list of sting to list of integers
        /// </summary>
        /// <param name="lstStr"></param>
        /// <returns></returns>
        public static IEnumerable<int> ConvertToInt(this IEnumerable<string> lstStr)
        {
            int temp = 0;
            foreach (string str in lstStr)
            {
                int.TryParse(str, out temp);
                yield return temp;
            }

        }



        public static DateTime? ToHijriDateTimeObject(this string date)
        {
            if (string.IsNullOrEmpty(date.Trim()))
                return null;
            string[] DateParts = date.Split('/');
            var umAlQura = new System.Globalization.UmAlQuraCalendar();
            return umAlQura.ToDateTime(int.Parse(DateParts[2]), int.Parse(DateParts[1]), int.Parse(DateParts[0]), 0, 0, 0, 0);
        }

        /// <summary>
        /// This Method Convert to  Gregoria nArabic Date
        /// <example>
        ///   25 سبتمبر  2001
        /// </example>
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string ToGregorianArabicDate(this DateTime dt)
        {
            string month = "";
            switch (dt.Month)
            {
                case 1:
                    month = "يناير";
                    break;
                case 2:
                    month = "فبراير";
                    break;
                case 3:
                    month = "مارس";
                    break;
                case 4:
                    month = "ابريل";
                    break;
                case 5:
                    month = "مايو";
                    break;
                case 6:
                    month = "يونيو";
                    break;
                case 7:
                    month = "يوليو";
                    break;
                case 8:
                    month = "اغسطس";
                    break;
                case 9:
                    month = "سبتمبر";
                    break;
                case 10:
                    month = "أكتوبر";
                    break;
                case 11:
                    month = "نوفمبر";
                    break;
                case 12:
                    month = "ديسمبر";
                    break;
            }
            return dt.Day + " " + month + " " + dt.Year;

        }

        public static string ToHijriArabicDate(this DateTime dt)
        {
            CultureInfo higri_format = new CultureInfo("ar-SA");
            higri_format.DateTimeFormat.Calendar = new UmAlQuraCalendar();
            return dt.ToString("ddd d MMM   yyyy", higri_format);
        }
        public static string ToHijriArabicDateDay(this DateTime dt)
        {
            CultureInfo higri_format = new CultureInfo("ar-SA");
            higri_format.DateTimeFormat.Calendar = new UmAlQuraCalendar();
            return dt.ToString("dd", higri_format);
        }
        public static string ToHijriArabicDateMonth(this DateTime dt)
        {
            CultureInfo higri_format = new CultureInfo("ar-SA");
            higri_format.DateTimeFormat.Calendar = new UmAlQuraCalendar();
            return dt.ToString("MMM", higri_format);
        }
        public static string ToHijriArabicDateYear(this DateTime dt)
        {
            CultureInfo higri_format = new CultureInfo("ar-SA");
            higri_format.DateTimeFormat.Calendar = new UmAlQuraCalendar();
            return dt.ToString("yyyy", higri_format);
        }

        public static string GetTime(this DateTime dt)
        {
            string am_pm = dt.ToString("tt");
            if (am_pm == "AM")
                am_pm = "صباحاً";
            else
                am_pm = "مساءاً";
            return dt.ToString("hh") + ":" + dt.ToString("mm") + ":" + dt.Second + " " + am_pm;
        }

        public static string ToDateTimePeriod(this DateTime dt)
        {
            DateTime now = DateTime.Now;
            if (now.Date == dt.Date && now.Hour == dt.Hour && (now.Minute - dt.Minute) < 1)
            {
                return "منذ " + (now.Second - dt.Second) + " ثانية ";
            }
            else if (now.Date == dt.Date && (now.Hour - dt.Hour) < 1)
            {
                int min_count = now.Minute - dt.Minute;
                string strMinAR = "دقائق";
                if (min_count == 1 || (min_count > 10))
                    strMinAR = "دقيقة";
                if (min_count == 2)
                    return "منذ دقيقتين";
                else
                    return "منذ " + min_count.ToString() + " " + strMinAR;

            }
            else
            {
                string am_pm = dt.ToString("tt");
                if (am_pm == "AM")
                    am_pm = "صباحاً";
                else
                    am_pm = "مساءاً";
                return dt.ToString("hh") + ":" + dt.ToString("mm") + ":" + dt.Second + " " + am_pm;
            }
        }
        public static T Clone<T>(this T value)
        {

            Type obj = value.GetType();
            var v = value;
            if (obj.BaseType != null && obj.Namespace == "System.Data.Entity.DynamicProxies")
            {
                obj = value.GetType().BaseType;
                v = (T)value;
            }

            object ret = Activator.CreateInstance(obj);

            PropertyInfo[] objProperties = obj.GetProperties();
            PropertyInfo[] retProperties = ret.GetType().GetProperties();

            for (int i = 0; i < objProperties.Length; i++)
            {
                retProperties[i].SetValue(ret, objProperties[i].GetValue(v));
            }

            var result = (T)ret;

            return result;
        }

        //public static T Clonel<T>(this T source)
        //{
        //    Delegate myExec = null;

        //    var dymMethod = new DynamicMethod("Hamada", typeof(T), new Type[] { typeof(T) }, true);
        //    var cInfo = source.GetType().GetConstructor(new Type[] { });

        //    var generator = dymMethod.GetILGenerator();
        //    generator.DeclareLocal(typeof(T));
        //    generator.Emit(OpCodes.Newobj, cInfo);
        //    generator.Emit(OpCodes.Stloc_0);

        //    foreach (var field in source.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
        //    {
        //        // Load the new object on the eval stack... (currently 1 item on eval stack)
        //        generator.Emit(OpCodes.Ldloc_0);
        //        // Load initial object (parameter)          (currently 2 items on eval stack)
        //        generator.Emit(OpCodes.Ldarg_0);
        //        // Replace value by field value             (still currently 2 items on eval stack)
        //        generator.Emit(OpCodes.Ldfld, field);
        //        // Store the value of the top on the eval stack into the object underneath that value on the value stack.
        //        //  (0 items on eval stack)
        //        generator.Emit(OpCodes.Stfld, field);
        //    }

        //    // Load new constructed obj on eval stack -> 1 item on stack
        //    generator.Emit(OpCodes.Ldloc_0);
        //    // Return constructed object.   --> 0 items on stack
        //    generator.Emit(OpCodes.Ret);

        //    myExec = dymMethod.CreateDelegate(typeof(Func<T, T>));
        //    return ((Func<T, T>)myExec)(source);
        //}
        public static void Shuffle<T>(this IList<T> list)
        {
            Random rng = new Random();
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public static string DisplayAsHtml(this string Text)
        {
            string result = Text;
            string picPath = "/App_Themes/Main/editor/";
            string titleClass = "side-title";
            string ayaclass = "editorAya";
            string hadethckass = "hadeth";
            string symbol1 = picPath + "1.png";
            string symbol2 = picPath + "2.png";
            string symbol3 = picPath + "3.png";
            string symbol4 = picPath + "6.png";
            string symbol5 = picPath + "4.png";
            string symbol6 = picPath + "5.png";
            string symbol7 = picPath + "parse-num1.png";
            result = result.Replace("\r\n", "<br/>");
            result = result.Replace("@", "");
            result = result.Replace("&", "");
            result = result.Replace("$", "<img  src='" + symbol1 + "'/>"); // صلى الله عليه وسلم
            result = result.Replace("&", "<img  src='" + symbol2 + "'/>"); // عليه السلام
            result = result.Replace("^", "<img  src='" + symbol3 + "'/>"); // رضى الله عنه
            result = result.Replace("|", "<img  src='" + symbol4 + "'/>"); // رضى الله عنهما
            result = result.Replace("Φ", "<img  src='" + symbol5 + "'/>"); // رضى الله عنهم
            result = result.Replace("~", "<img  src='" + symbol6 + "'/>"); // رضى الله عنها
            result = result.Replace("%", "<img  src='" + symbol7 + "'/>"); // فاصلة لأيه

            result = result.Replace("[/HR]", "<br /><hr /><br />"); // Break
            result = result.Replace("!", "<span class='" + titleClass + "'>").Replace("*", "</span>"); // SupTitel
            result = result.Replace("{", "<img  src='/App_Themes/Arabic/images/editor/start-icon.png'/><span class='" + ayaclass + "'>").Replace("}", "</span><img  src='/App_Themes/Arabic/images/editor/end-icon.png'/>"); // Quran
            result = result.Replace("((", "<img  src='/App_Themes/Arabic/images/editor/start-icon.png'/><span class='" + hadethckass + "'>").Replace("))", "</span><img  src='/App_Themes/Arabic/images/editor/end-icon.png'/>"); // 7adeth

            try
            {
                while (result.Contains("[Head="))
                {
                    int index = result.IndexOf("[Head=");
                    string value = result.Substring(index + 6, 3);
                    string text = result.Substring(index + 10, result.IndexOf("[/Head]") - index - 10);
                    result = result.Replace("[Head=" + value + "]" + text + "[/Head]", "<a class='" + titleClass + "' name='Head_" + value + "'>" + text + "</a><br>");
                }

                while (result.Contains("[Link="))
                {
                    int index = result.IndexOf("[Link=");
                    string value = result.Substring(index + 6, 3);
                    string text = result.Substring(index + 10, result.IndexOf("[/Link]") - index - 10);
                    result = result.Replace("[Link=" + value + "]" + text + "[/Link]", "<a class='editorLink' name='Link_" + value + "'>" + text + "</a>");
                }

                while (result.Contains('['))
                {
                    int index = result.IndexOf('[');
                    if (result.Substring(index - 1, 1) != "@")
                        break;
                    string s = result.Substring(index + 1, (result.IndexOf(']') - index - 1));
                    string replace = s;
                    if (s.Contains('@'))
                    {
                        replace = "<a href=\"mailto:" + s + "\">" + s + "</a>";
                    }
                    else if (s.Contains('$'))
                    {
                        string[] linkData = s.Split('$');
                        string linkUrl = linkData[1].ToLower().Replace("http://", "");
                        replace = "<a target=\"_blank\" href=\"http://" + linkUrl + "\">" + linkData[0] + "</a>";
                    }
                    else
                    {
                        string linkText = s.Replace("http://", "");
                        replace = "<a href=\"http://" + linkText + "\">" + linkText + "</a>";
                    }
                    result = result.Replace("[" + s + "]", replace);
                }
            }
            catch
            {
                result = result.ToString().Replace("[", "").Replace("]", "").Replace("$", "");
            }
            return result;
        }

        public static string SubString(this string value, int Index)
        {
            string result = string.Empty;
            if (!string.IsNullOrEmpty(value))
            {
                if (value.Length > Index)
                    result = value.Substring(0, Index) + "..";
                else
                    result = value;
            }
            return result;
        }


        public static string ReplaceString(this string value)
        {
            string result = string.Empty;
            if (!string.IsNullOrEmpty(value))
                result = value.Replace('"', '\'').Replace("\r\n", "<br/>");

            return result;
        }


        public static object ToJson(this object value)
        {
            if (value != null)
            {
                object newobject = Activator.CreateInstance(value.GetType());
                foreach (PropertyInfo prop in value.GetType().GetProperties())
                {
                    string propName = prop.PropertyType.Name.ToLower();
                    if (propName.Contains("entityset") || propName.Contains("entityref"))
                        continue;

                    if (propName == "string")
                        prop.SetValue(newobject, prop.GetValue(value, null).ToString().ReplaceString(), null);
                    else
                        prop.SetValue(newobject, prop.GetValue(value, null), null);
                }
                return newobject;
            }
            else
                return new object();
        }


        /// <summary>
        /// take hijri string as  and convert it to hijri date
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static DateTime? ToHijriDate(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return null;

            string[] DateParts = value.Split('/'); // date format dd/MM/yyyy
            return new DateTime(int.Parse(DateParts[2]), int.Parse(DateParts[1]), int.Parse(DateParts[0]), new System.Globalization.UmAlQuraCalendar());
        }



        /// <summary> 
        /// To Convert Gerg. Date To Hijri Date in Format dd/M
        /// M/yyyy .
        /// </summary>
        /// <param name="dt"></param>
        /// <returns>Return String</returns>
        public static string ToHijriDate(this DateTime dt)
        {
            CultureInfo higri_format = new CultureInfo("ar-SA");
            higri_format.DateTimeFormat.Calendar = new UmAlQuraCalendar();
            return dt.ToString("dd/MM/yyyy", higri_format);
        }


        /// <summary>
        /// take Greg string as  and convert it to Greg date
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static DateTime? ToGregDate(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return null;

            string[] DateParts = value.Split('/'); // date format dd/MM/yyyy
            return new DateTime(int.Parse(DateParts[2]), int.Parse(DateParts[1]), int.Parse(DateParts[0]), new System.Globalization.GregorianCalendar());
        }

        public static string HijriToGregDate(this string value)
        {
            var umQra = new UmAlQuraCalendar();
            CultureInfo arCul = new CultureInfo("ar-SA");
            CultureInfo enCul = new CultureInfo("en-US");
            // string[] allFormats = { "yyyy/MM/dd", "yyyy/M/d", "dd/MM/yyyy", "d/M/yyyy", "dd/M/yyyy", "d/MM/yyyy", "yyyy-MM-dd", "yyyy-M-d", "dd-MM-yyyy", "d-M-yyyy", "dd-M-yyyy", "d-MM-yyyy", "yyyy MM dd", "yyyy M d", "dd MM yyyy", "d M yyyy", "dd M yyyy", "d MM yyyy" };
            arCul.DateTimeFormat.Calendar = umQra;

            if (string.IsNullOrEmpty(value))
                return null;

            DateTime tempDate = DateTime.ParseExact(value, "d/M/yyyy", arCul.DateTimeFormat, DateTimeStyles.AllowWhiteSpaces);

            return tempDate.ToString("dd/MM/yyyy", enCul.DateTimeFormat);
        }

        public static DateTime? HijriToGregDateObj(this string value)
        {
            var umQra = new UmAlQuraCalendar();
            CultureInfo arCul = new CultureInfo("ar-SA");
            CultureInfo enCul = new CultureInfo("en-US");
            arCul.DateTimeFormat.Calendar = umQra;

            if (string.IsNullOrEmpty(value))
                return null;

            DateTime tempDate = DateTime.MinValue;
            DateTime.TryParseExact(value, "yyyy-MM-dd", arCul.DateTimeFormat, DateTimeStyles.AllowWhiteSpaces, out tempDate);

            return tempDate == DateTime.MinValue ? null : (DateTime?)tempDate;
        }

        public static string HijriToGreg(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return null;

            return value.ToHijriDate().Value.GetGregDate();

            //var umQra = new UmAlQuraCalendar();
            //CultureInfo arCul = new CultureInfo("ar-SA");
            //CultureInfo enCul = new CultureInfo("en-US");
            //// string[] allFormats = { "yyyy/MM/dd", "yyyy/M/d", "dd/MM/yyyy", "d/M/yyyy", "dd/M/yyyy", "d/MM/yyyy", "yyyy-MM-dd", "yyyy-M-d", "dd-MM-yyyy", "d-M-yyyy", "dd-M-yyyy", "d-MM-yyyy", "yyyy MM dd", "yyyy M d", "dd MM yyyy", "d M yyyy", "dd M yyyy", "d MM yyyy" };
            //arCul.DateTimeFormat.Calendar = umQra;

            //if (string.IsNullOrEmpty(value))
            //    return null;

            //DateTime tempDate = DateTime.ParseExact(value, "d/M/yyyy", arCul.DateTimeFormat, DateTimeStyles.AllowWhiteSpaces);

            //return tempDate.ToString("dd/MM/yyyy", enCul.DateTimeFormat);
        }

        /// <summary>
        /// take greg date as string and return hijri date as string
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GregToHijri(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return null;

            return value.ToGregDate().Value.GetHijriDate();

        }

        public static DateTime? GregToHijriDate(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return null;

            DateTime dt = value.ToGregDate().Value.GetHijriDate().ToHijriDate().Value;

            return dt;
            //return Convert.ToDateTime(Date.GregToHijri(value));
        }



        //public static DateTime? GregToHijriDate(string value)
        //{
        //    if (string.IsNullOrEmpty(value))
        //        return null;

        //    string[] dateparts = value.Split('/'); // dateformat d/m/yyyy
        //    if (int.Parse(dateparts[0]) < 10)
        //        dateparts[0] = "0" + dateparts[0];

        //    if (int.Parse(dateparts[1]) < 10)
        //        dateparts[1] = "0" + dateparts[1];
        //    string newvalue = dateparts[0] + "/" + dateparts[1] + "/" + dateparts[2];
        //    string daet = Date.GregToHijri(newvalue);
        //    return Convert.ToDateTime(Date.GregToHijri(newvalue));
        //}


        public static string GetHijriDate(this DateTime value)
        {
            CultureInfo higri_format = new CultureInfo("ar-SA");
            higri_format.DateTimeFormat.Calendar = new UmAlQuraCalendar();
            return value.ToString("dd/MM/yyyy", higri_format);
        }

        public static string GetHijriDate(this DateTime value, string format)
        {
            CultureInfo higri_format = new CultureInfo("ar-SA");
            higri_format.DateTimeFormat.Calendar = new UmAlQuraCalendar();
            return value.ToString(format, higri_format);
        }

        public static string GetGregDate(this DateTime value)
        {
            CultureInfo higri_format = new CultureInfo("en-US");
            higri_format.DateTimeFormat.Calendar = new GregorianCalendar();
            return value.ToString("dd/MM/yyyy", higri_format);
        }

        public static DateTime GetStartingWeekDay(this DateTime value)
        {
            return value.AddDays(1 - value.DayOfWeek.GetDayIndex());
        }

        public static DateTime GetNextWeekStartingDay(this DateTime value)
        {
            return value.AddDays(8 - value.DayOfWeek.GetDayIndex());
        }

        public static DateTime GetPrivousWeekStartingDay(this DateTime value)
        {
            return value.AddDays((6 + value.DayOfWeek.GetDayIndex()) * -1);
        }
        public static int GetDayIndex(this DayOfWeek day)
        {
            int iweekIndex = (int)day + 2;
            iweekIndex = iweekIndex == 8 ? 1 : iweekIndex;
            return iweekIndex;
        }

        public static DateTime GetFirstDayOfMonth(this DateTime value)
        {
            return value.AddDays(-(value.Day - 1));
        }

        public static DateTime GetLastDayOfMonth(this DateTime value)
        {
            DateTime dtDate = value.AddMonths(1);
            return dtDate.AddDays(-(dtDate.Day));
        }

        /// <summary>
        /// To Parse String From Hijri Date To Greg. Date In d/m/yyyy Format .
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime ToGregExact(this String dt)
        {
            return DateTime.ParseExact(dt.HijriToGregDate(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// To Parse String Greg Date To Greg. Date In d/m/yyyy Format .
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime ToGregExactformate(this String dt)
        {
            return DateTime.ParseExact(dt, "d/M/yyyy", CultureInfo.InvariantCulture);
        }


        //For Encrypt QueryString and RouteData
        public static string Encrypt(string plainText)
        {
            string key = "jdsg432387#";
            byte[] EncryptKey = { };
            byte[] IV = { 55, 34, 87, 64, 87, 195, 54, 21 };
            EncryptKey = System.Text.Encoding.UTF8.GetBytes(key.Substring(0, 8));
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] inputByte = Encoding.UTF8.GetBytes(plainText);
            MemoryStream mStream = new MemoryStream();
            CryptoStream cStream = new CryptoStream(mStream, des.CreateEncryptor(EncryptKey, IV), CryptoStreamMode.Write);
            cStream.Write(inputByte, 0, inputByte.Length);
            cStream.FlushFinalBlock();
            return Convert.ToBase64String(mStream.ToArray()).Replace('/', '_').Replace('+', '-');
        }
        //For Decrypt QueryString and RouteData
        public static string Decrypt(string encryptedText)
        {
            try
            {
                string key = "jdsg432387#";
                byte[] DecryptKey = { };
                byte[] IV = { 55, 34, 87, 64, 87, 195, 54, 21 };
                byte[] inputByte = new byte[encryptedText.Length];
                encryptedText = encryptedText.Replace('_', '/').Replace('-', '+');
                DecryptKey = System.Text.Encoding.UTF8.GetBytes(key.Substring(0, 8));
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                inputByte = Convert.FromBase64String(encryptedText);
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(DecryptKey, IV), CryptoStreamMode.Write);
                cs.Write(inputByte, 0, inputByte.Length);
                cs.FlushFinalBlock();
                System.Text.Encoding encoding = System.Text.Encoding.UTF8;
                return encoding.GetString(ms.ToArray());
            }
            catch (Exception ex)
            {
                return "";
            }

        }
        public static string testDecrypt(string strdecrypt)
        {
            if (!string.IsNullOrWhiteSpace(strdecrypt))
            {

                string skey = "33439650";
                byte[] inputByteArray = new byte[strdecrypt.Length + 1];
                try
                {
                     byte[] key = { };
                     byte[] IV = { 0X12, 0X34, 0X56, 0X78, 0X90, 0XAB, 0XCD, 0XEF };
                    key = System.Text.Encoding.UTF8.GetBytes(skey.Substring(0, 8));
                    DESCryptoServiceProvider des = new DESCryptoServiceProvider();


                    inputByteArray = Convert.FromBase64String(strdecrypt);
                    MemoryStream ms = new MemoryStream();
                    CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(key, IV), CryptoStreamMode.Write);
                    cs.Write(inputByteArray, 0, inputByteArray.Length);
                    cs.FlushFinalBlock();
                    System.Text.Encoding encoding = System.Text.Encoding.UTF8;
                    return encoding.GetString(ms.ToArray());
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }
            else
            {
                return "";
            }
        }
        public static byte[] ReduceSize(string strFullPath, int maxWidth, int maxHeight)
        {
            Image source = System.Drawing.Image.FromFile(strFullPath);//Image.FromStream(stream);
            double widthRatio = ((double)maxWidth) / source.Width;
            double heightRatio = ((double)maxHeight) / source.Height;
            double ratio = (widthRatio < heightRatio) ? widthRatio : heightRatio;
            Image thumbnail = source.GetThumbnailImage((int)(source.Width * ratio), (int)(source.Height * ratio), null, IntPtr.Zero);
            using (var memory = new MemoryStream())
            {
                thumbnail.Save(memory, source.RawFormat);
                return memory.ToArray();
            }
        }
        public static string ConvertImageToBase64(string strPath)
        {
            string base64String = string.Empty;
            if (!string.IsNullOrEmpty(strPath))
            {
                //remove comment
                string strFullPath = HttpContext.Current.Server.MapPath(Contract_QvLib.QVUtil.IO.GetVirtualPath(strPath));
                bool IsFound = Contract_QvLib.QVUtil.IO.IsFileExist(strPath);

                if (IsFound)
                {
                    using (System.Drawing.Image image = System.Drawing.Image.FromFile(strFullPath))
                    {
                        using (MemoryStream m = new MemoryStream())
                        {
                            int maxWidth = 400;
                            int maxHeight = 300;
                            try
                            {
                                maxWidth = int.Parse(Contract_QvLib.QVUtil.AppSetting.GetAppSetting("maxWidthBase64"));
                                maxHeight = int.Parse(Contract_QvLib.QVUtil.AppSetting.GetAppSetting("maxHeightBase64"));
                            }
                            catch (Exception ex)
                            {
                                //the keys were not present in the web.config :D
                            }
                            byte[] imageBytes = ReduceSize(strFullPath, maxWidth, maxHeight);//250, 180); 
                            string strExt = System.IO.Path.GetExtension(strPath).ToLower().Remove(0, 1);
                            // Convert byte[] to Base64 String
                            base64String = "data:image/" + strExt + ";base64," + Convert.ToBase64String(imageBytes);

                        }
                    }


                }
            }
            return base64String;
        }
    }
}