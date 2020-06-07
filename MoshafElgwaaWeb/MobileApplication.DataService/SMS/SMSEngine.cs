using MobileApplication.Context;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Configuration;

namespace QvSMS
{
    public class SMSEngine
    {
        private static SMSEngine _Current;
        public static SMSEngine Current
        {
            get
            {
                if (_Current == null) _Current = new SMSEngine();
                return _Current;
            }
        }

        public void Initialize()
        {
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += new DoWorkEventHandler(worker_DoWork);
            worker.RunWorkerAsync();
        }

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            var _EntitiesContext = new QVMobileApplicationEntities();

            while (true)
            {
                var allNotSentMessages = _EntitiesContext.SMS_Message.Where(model => model.IsSent == false).ToList();
                var SMSConfig = _EntitiesContext.SMS_Config.FirstOrDefault(s=>s.IsDefault==true);
                var SMSParam = _EntitiesContext.SMS_ConfigParam.Where(p=>p.SMSConfigId==SMSConfig.ID).ToList();
                // Code to send SMS here
                foreach (var item in allNotSentMessages)
                {
                    int smsResponseNumber;
                    if (SendMessage(item, SMSConfig,SMSParam, out smsResponseNumber))
                    {
                        item.IsSent = true;
                        item.SendDate = DateTime.Now;
                    }

                    item.ResponseNumber = smsResponseNumber;
                  //  item.ResponseNumber = 500;
                    _EntitiesContext.SaveChanges();
                }
                //Thread.Sleep(20000);
                Thread.Sleep((int)SMSConfig.IntervalTimeToSend);
            }
        }

        public int InsertNewMessage(string PhoneNumber, string TxtMessage)
        {
            var _EntitiesContext = new QVMobileApplicationEntities();

            SMS_Message MessageObj = new SMS_Message();
            MessageObj.PhoneNumber = PhoneNumber;
            MessageObj.TextMessage = TxtMessage;
            MessageObj.IsSent = false;
            MessageObj.CreationDate = DateTime.Now;
            _EntitiesContext.SMS_Message.Add(MessageObj);
            var ret= _EntitiesContext.SaveChanges();
            if(ret>0)
            {
                return MessageObj.ID;
            }
            else { return 0; }
        }
        public bool SendMessage(SMS_Message Message, SMS_Config SMSConfig,List<SMS_ConfigParam> SMSParams, out int smsResponseNumber)
        {
            bool success = false;
            CompilationSection compilationSection = (CompilationSection)System.Configuration.ConfigurationManager.GetSection(@"system.web/compilation");

            try
            {
                if (!compilationSection.Debug )
                {
                    HttpWebRequest req = (HttpWebRequest)WebRequest.Create(SMSConfig.Url);
                    req.Method = "POST";
                    req.ContentType = "application/x-www-form-urlencoded";
                    string postData = string.Empty;
                    List<string> postDataParams = new List<string>();
                       // "mobile=" + SMSConfig.UserName + "&password=" + SMSConfig.Password + "&numbers=" + Message.PhoneNumber + "&sender=" + SMSConfig.SenderName + "&msg=" + ConvertToUnicode(Message.TextMessage) + "&applicationType=59";
                    foreach (var param in SMSParams.Where(p=>p.IsStatic==true))
                    {
                        postDataParams.Add(param.Key + "=" + param.Value);
                    }
                    postData = string.Join("&", postDataParams);
                    postData += "&" + SMSConfig.MessageParamName + "=" +ConvertToUnicode( Message.TextMessage);
                    postData += "&" + SMSConfig.NumberParamName + "=" + Message.PhoneNumber;

                    req.ContentLength = postData.Length;
                    StreamWriter stOut = new
                    StreamWriter(req.GetRequestStream(), System.Text.Encoding.ASCII);
                    stOut.Write(postData);
                    stOut.Close();
                    // Do the request to get the response
                    StreamReader stIn = new StreamReader(req.GetResponse().GetResponseStream());
                    //var ou = stIn.ReadToEnd();
                    //var t = ou;
                    smsResponseNumber = Int32.Parse(stIn.ReadToEnd());
                    stIn.Close();

                    if (smsResponseNumber == SMSConfig.SuccessResponseNumber)
                    {
                        success = true;
                    }
                }
                else
                {
                    smsResponseNumber = 0;
                }
            }
            catch (Exception)
            {
                smsResponseNumber = 0;
                //_SMSMessageRepository.Save(Message);
                //_unitOfWork.Submit();
            }
            return success;
        }

        private string ConvertToUnicode(string val)
        {
            string msg2 = string.Empty;

            for (int i = 0; i < val.Length; i++)
            {
                msg2 += convertToUnicode(System.Convert.ToChar(val.Substring(i, 1)));
            }

            return msg2;
        }
        private string convertToUnicode(char ch)
        {
            System.Text.UnicodeEncoding class1 = new System.Text.UnicodeEncoding();
            byte[] msg = class1.GetBytes(System.Convert.ToString(ch));

            return fourDigits(msg[1] + msg[0].ToString("X"));
        }
        private string fourDigits(string val)
        {
            string result = string.Empty;

            switch (val.Length)
            {
                case 1: result = "000" + val; break;
                case 2: result = "00" + val; break;
                case 3: result = "0" + val; break;
                case 4: result = val; break;
            }

            return result;
        }

        public List<SMS_Message> GetSMSMessageList()
        {
            var _EntitiesContext = new QVMobileApplicationEntities();
            return _EntitiesContext.SMS_Message.ToList();
        }

        public List<SMSMessageModel> GetSMSMessageModelList()
        {
            return GetSMSMessageList().Select(a => new SMSMessageModel()
            {
                ID=a.ID,
                CreationDate= a.CreationDate,
                IsSent= a.IsSent,
                PhoneNumber= a.PhoneNumber,
                SendDate = a.SendDate,
                TextMessage = a.TextMessage
            }).ToList();

        }
    }

    public partial class CheckValidationModel
    {
        public bool Success { get; set; }
        public string Fail { get; set; }

        public CheckValidationModel CheckValidation(string PhoneNumber, string TextMessage)
        {
            CheckValidationModel model = new CheckValidationModel();
            bool PhoneValidation = ((TextMessage.Length > 12) ? false : true);
            //Regex.Match(PhoneNumber, @"^(\+[0-9]{12})$").Success;
            bool TextValidation = ((TextMessage.Length > 70) ? false : true);
            if (PhoneValidation && TextValidation)
            {
                model.Success = true;
            }
            else
            {
                model.Fail = "";
                if (!PhoneValidation)
                {
                    model.Fail += "رقم الموبايل يجب ان يكون 12 رقم ";
                }
                if (!TextValidation)
                {
                    model.Fail += "يجب ان لا يزيد طول الرسالة عن 70 حرف";
                }
            }
            return model;
        }

    }

    public class SMSMessageModel
    {

        public int ID { get; set; }
        public string PhoneNumber { get; set; }

        public string TextMessage { get; set; }
        public bool? IsSent { get; set; }

        public DateTime? CreationDate { get; set; }

        public DateTime? SendDate { get; set; }
    }
}