using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Crp.Tools.Api.Send
{
    public static class EmailHelper
    {


        /// <summary>
        /// QQ发送纯文本邮件
        /// </summary>
        /// <returns></returns>
        public static void SendTextEmail(string myAccount, string myPass, string toAccount,string title,string subject, string content )
        {
            SendQQEmail(myAccount, myPass, toAccount, title , content);
        }

        /// <summary>
        /// QQ发送html邮件
        /// </summary>
        /// <returns></returns>
        public static void SendHtmlEmail(string myAccount, string myPass,string toAccount,string title ,string subject , string filePath)
        {
            SendQQEmail(myAccount, myPass, toAccount, title ,"",filePath);
        }

        /// <summary>
        /// 发送QQ邮件  (支持纯文本或html邮件)
        /// </summary>
        /// <param name="myAccount">邮箱用户名</param>
        /// <param name="myPass">邮箱密码</param>
        /// <param name="toAccount">目标用户名</param>
        /// <param name="content">文本邮件（文本和html邮件 两者取其一)</param>
        /// <param name="filePath">html邮件路径</param>
        private static void SendQQEmail(string myAccount, string myPass, string toAccount, string title, string subject, string content = "", string filePath = "") {
            string emailContent = string.Empty;

            if (content.Length > 0)
                emailContent = content;
            if (filePath.Length > 0) { 
                emailContent = filePath;
                using (var reader = new StreamReader(filePath))
                {
                    emailContent = reader.ReadToEnd();
                }
            }
            MailMessage msg = new MailMessage();
            msg.To.Add(toAccount);//收件人
            msg.From = new MailAddress(myAccount, title);
            msg.Subject = "";
            //标题格式为UTF8  
            msg.SubjectEncoding = Encoding.UTF8;
            if (content.Length > 0)
                msg.IsBodyHtml = false;
            else
                msg.IsBodyHtml = true;
            msg.Body = emailContent;
            msg.BodyEncoding = Encoding.UTF8;
            SmtpClient client = new SmtpClient();
            //SMTP服务器地址 
            client.Host = "smtp.qq.com";  //企业账号用smtp.exmail.qq.com  
            //SMTP端口，QQ邮箱填写587  
            client.Port = 587;

            //SMTP服务器地址 
            //client.Host = "smtp.ym.163.com";
            //client.Port = 25;
            //启用SSL加密  
            client.EnableSsl = true;
            client.Credentials = new NetworkCredential(myAccount, myPass); //这个密码要注意：如果是一般账号，要用授权码；企业账号用登录密码  
            //发送邮件  
            client.Send(msg);
        }
    }
}




