using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Repository;
using Service;
using System.ComponentModel;
using System.Net.Mail;
using System.Threading;
using System.Web;
using System.IO;
using MobileApplication.Context;


namespace MobileApplication.DataService
{
    public class MailEngineService : BaseService
    {
        private readonly Repository<MailEngine> _sendMailRepository;
   

        public MailEngineService()
        {
            _sendMailRepository = new Repository<MailEngine>(_unitOfWork);
 
    }

        public void Initialize()
        {
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += new DoWorkEventHandler(worker_DoWork);
            worker.RunWorkerAsync();
        }


        void worker_DoWork(object sender, DoWorkEventArgs e)
        {

            //while (true)
            //{
            //    //var setting = new SettingService().Setting;
            //    var allNotSentMails = _sendMailRepository.GetList().Where(i => i.IsSent == false).ToList();
            //    // Code to send email here
            //    foreach (var item in allNotSentMails)
            //    {
            //        if (SendMail(setting.ServerName, setting.UserName, setting.Password,
            //            setting.PortNo, setting.SSL, item.Subject, item.ToEmail,
            //            item.Body, setting.FromEmail, item.CcEmail, item.BccEmail))
            //        {
            //            item.IsSent = true;
            //            _sendMailRepository.Update(item);
            //            _unitOfWork.Submit();
            //        }
            //    }
            //    Thread.Sleep(20000);
            //}
        }

        public int InsertNewMail(string strSubject, string strBody, string strToEmail, string strCcEmail = "", string strBccEmail = "")
        {
            MailEngine smObj = new MailEngine();
            smObj.Subject = strSubject;
            smObj.Body = strBody;
            smObj.ToEmail = strToEmail;
            if (!string.IsNullOrWhiteSpace(strCcEmail))
            {
                smObj.CcEmail = strCcEmail;
            }
            if (!string.IsNullOrWhiteSpace(strBccEmail))
            {
                smObj.BccEmail = strBccEmail;
            }
            smObj.IsSent = false;
            _sendMailRepository.Save(smObj);
            return _unitOfWork.Submit();
        }

        public string LoadMailTemplate(string path)
        {
            string strPath = HttpContext.Current.Server.MapPath(path);
            StreamReader strLetter = new StreamReader(strPath);
            string strFinalLetter = string.Empty;

            while (strLetter.Peek() != -1)
            {
                strFinalLetter = strLetter.ReadToEnd();
            }
            return strFinalLetter;
        }

        public bool SendMail(string ServerName, string UserName, string Password,
            int PortNo, bool SSL, string subject, string to, string body, string from, string cc, string bcc)
        {

            bool success = true;
            System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();
            SmtpClient smtpClient = new SmtpClient();
            try
            {

                MailAddress fromAddress = new MailAddress(from.Trim());
                message.From = fromAddress;
                message.To.Add(to);

                message.Subject = subject;
                message.IsBodyHtml = true;
                if (!string.IsNullOrWhiteSpace(cc))
                {
                    message.CC.Add(cc);
                }
                if (!string.IsNullOrWhiteSpace(bcc))
                {
                    message.Bcc.Add(bcc);
                }
                message.Body = body;
                smtpClient.Host = ServerName;
                smtpClient.Port = PortNo;
                smtpClient.UseDefaultCredentials = false;

                smtpClient.Credentials = new System.Net.NetworkCredential(UserName.Trim().ToString(), Password.Trim().ToString());
                smtpClient.EnableSsl = SSL;
                smtpClient.Send(message);

            }
            catch (Exception ex)
            {
                success = false;
            }

            return success;
        }

    }

    public static class MailService
    {
        private readonly static MailEngineService _MailEngineService = new MailEngineService();

        public static bool ChangePasswordMail(string Host, string userEmail, string LoginName, string newPassword)
        {

            string mailMessage = _MailEngineService.LoadMailTemplate("/MailTemplate/ChangePasswordMail.html")
                            .Replace("{Host}", Host)
                            .Replace("{Name}", LoginName)
                            .Replace("{Email}", userEmail)
                             .Replace("{Password}", newPassword);
            var mailResult = _MailEngineService.InsertNewMail(DataServiceArabicResource.ChangePassword, mailMessage, userEmail);
            return mailResult > 0;
        }

     

        public static bool ForgotPasswordMail(string Host, string link, string userEmail)
        {
            string mailMessage = _MailEngineService.LoadMailTemplate("/MailTemplate/ForgotpasswordMail.html")
                            .Replace("{Host}", Host)
                            .Replace("{link}", link);
            var mailResult = _MailEngineService.InsertNewMail(DataServiceArabicResource.ForgetPassword, mailMessage, userEmail);
            return mailResult > 0;
        }

        public static object ChangePasswordMailByAdmin(string Host, string Email, string newPassword)
        {
            string mailMessage = _MailEngineService.LoadMailTemplate("/MailTemplate/ChangePasswordMailByAdmin.html")
                            .Replace("{Host}", Host)
                            .Replace("{Password}", newPassword);
            var mailResult = _MailEngineService.InsertNewMail(DataServiceArabicResource.NewPassword, mailMessage, Email);
            return mailResult > 0;
        }
       

    }
}

