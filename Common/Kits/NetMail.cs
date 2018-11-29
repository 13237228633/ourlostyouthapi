using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Net.Mail;

namespace Common
{
    public class NetMail
    {
        public static bool MailSend(MailMessage message, ArrayList datapath, string subject, string body, out string outmessage)
        {

            outmessage = "正常";
            var client = new SmtpClient();
            // client.Host = "smtp.126.com";
            client.Host = "mail.9158.com";
            client.UseDefaultCredentials = false;
            // client.Credentials = new System.Net.NetworkCredential("tj9158@9158.com", "9158123ccstj");//邮件服务器账户
            client.Credentials = new System.Net.NetworkCredential("tj9158@9158.com", "SnI+v6hk");

            //  client.Host = "smtp.gmail.com"; 
            //  client.UseDefaultCredentials = false;
            //  client.Credentials = new System.Net.NetworkCredential("romeoqsam2@gmail.com", "romeoqsam2huangya1");//邮件服务器账户
             //client.Port = 465;

            //星号改成自己邮箱的密码
            client.DeliveryMethod = SmtpDeliveryMethod.Network;

            message.Subject = subject;
            message.Body = body;
            message.BodyEncoding = System.Text.Encoding.UTF8;
            message.IsBodyHtml = true;

            //添加附件
            // Attachment data = new Attachment(@"e:\a.txt", System.Net.Mime.MediaTypeNames.Application.Octet);

            System.Threading.Thread.Sleep(1000);
            if (datapath != null)
            {
                foreach (var datapa in datapath)
                {
                    Attachment data = new Attachment(datapa.ToString(), System.Net.Mime.MediaTypeNames.Application.Octet);
                    message.Attachments.Add(data);
                }
            }

            try
            {

                client.Send(message);
                message.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                Common.Web.HtmlHelperExtension.WriteTextLog(ex.ToString());
                outmessage = ex.ToString();
                message.Dispose();
                return false;
            }
        }

        private static string title = @"<!DOCTYPE html>
                                                <html lang='en'>
                                                <head>
	                                                <meta charset='UTF-8'>
	                                                <title>Document</title>
	                                                <style type='text/css'>
		                                                *{padding:0;margin:0;font-family:tahoma, Helvetica, STXihei, 'Microsoft YaHei', Arial, SimSun,sans-serif;}
		                                                .container{width:80%;margin:0 auto;font-size: 16px;}
		                                                h3{width:100%;text-align: center;padding:50px 0;font-size: 20px;}
		                                                h4{font-size: 18px;line-height: 1.625em;}
		                                                .content{text-indent: 2em;margin:20px 0;line-height: 1.625em;}
		                                                .footer{margin-top: 50px;}
		                                                .englishName{line-height: 2em;}
		                                                .line{width:200px;border-bottom: 1px solid #89caec;display: block;}
		                                                .company{line-height: 1.625em;}
		                                                .add{color:#0000ee;text-decoration: none;}
		                                                .add:hover{text-decoration: underline;}
	                                                </style>
                                                </head>
                                                <body>
	                                                <div class='container'>
		                                                <h3>市场部推广系统内容审核通知 </h3>
		                                                <p class='name'>尊敬的审核人员您好：</p><p class='content'>";

        //string boy = "系统有通知了，你有一封扣量变更申请审核内容，  提交了扣量变更申请，已经提交审核，请您审核。";


        private static string both = @"</p><p class='content'>请及时登录市场部推广系统登录审核，网址：<a class='add' href='http://tg.9158.com'>http://tg.9158.com</a> ,需要登录VPN。</p>
		                                <div class='footer'>
			                                <span class='line'></span>
			                                <p class='englishName'>Best regards</p>
			                                <h4>市场部推广系统自动邮件组</h4>
			                                <h4>研发人员:张晓辉</h4>
			                                <p class='company'>天鸽互动控股有限公司（香港主板上市公司代码：1980）</p>
			                                <a href='http://www.tiange.com' class='add'>www.tiange.com</a>
		                                </div>
	                                </div>
                                </body>
                                </html>";

        public static bool MailSend(MailMessage message, string body, out string outmessage)
        {

            outmessage = "正常";
            var client = new SmtpClient();
            client.Host = "mail.9158.com";
            client.UseDefaultCredentials = false;
            // client.Credentials = new System.Net.NetworkCredential("tj9158@9158.com", "9158123ccstj");//邮件服务器账户
            client.Credentials = new System.Net.NetworkCredential("tj9158@9158.com", "SnI+v6hk");

            //  client.Host = "smtp.gmail.com"; 
            //  client.UseDefaultCredentials = false;
            //  client.Credentials = new System.Net.NetworkCredential("romeoqsam2@gmail.com", "romeoqsam2huangya1");//邮件服务器账户
            ////  client.Port = 465;

            //星号改成自己邮箱的密码
            client.DeliveryMethod = SmtpDeliveryMethod.Network;

            message.Subject = "市场部推广系统内容审核通知";
            message.Body = title + body + both;
            message.BodyEncoding = System.Text.Encoding.UTF8;
            message.IsBodyHtml = true;

            //添加附件
            // Attachment data = new Attachment(@"e:\a.txt", System.Net.Mime.MediaTypeNames.Application.Octet);

            try
            {
                client.Send(message);
                message.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                outmessage = ex.ToString();
                message.Dispose();
                return false;
            }
        }
    }
}
