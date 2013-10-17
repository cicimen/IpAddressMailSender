using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Net.Sockets;
using System.IO;
using System.Net.Mail;
using System.Net;
using System.Web;
using System.Net.Mime;
using System.Collections;
using System.Text.RegularExpressions;
using System.Configuration;


namespace IpAddressMailSender
{
    class SMTPMail
    {
        public static bool SendMailThroughGmail(string IPAddress)
        {
            try
            {
                var fromAddress = new MailAddress(ConfigurationManager.AppSettings["smtpUser"], "Egemen");
                var toAddress = new MailAddress(ConfigurationManager.AppSettings["smtpUser"], "Egemen");
                string fromPassword = ConfigurationManager.AppSettings["smtpPassword"];
                string subject = IPAddress;
                string body = IPAddress;

                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
                };
                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body
                })
                {
                    smtp.Send(message);
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public static string SendMessage(string sendTo, string sendFrom, string sendSubject, string sendMessage)
        {
            try
            {
                // validate the email address
                bool bTest = ValidateEmailAddress(sendTo);

                // if the email address is bad, return message
                if (bTest == false)
                    return "Invalid recipient email address: " + sendTo;

                // create the email message
                MailMessage message = new MailMessage(
                   sendFrom,
                   sendTo,
                   sendSubject,
                   sendMessage);

                // create smtp client at mail server location
                SmtpClient client = new
                SmtpClient(ConfigurationManager.AppSettings["defaultSmtpAdress"]);

                // add credentials
                client.UseDefaultCredentials = true;

                // send message
                client.Send(message);

                return "Message sent to " + sendTo + " at " +
                DateTime.Now.ToString() + ".";
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }

        public static string SendMessageWithAttachment(string sendTo, string sendFrom, string sendSubject, string sendMessage, string CC, string BCC, ArrayList attachments, int siteID)
        {
            try
            {
                // validate email address
                bool bTest = ValidateEmailAddress(sendTo);

                if (bTest == false)
                    return "Invalid recipient email address: " + sendTo;


                // Create the basic message
                using (MailMessage message = new MailMessage(
                    sendFrom,
                    sendTo,
                    sendSubject,
                    sendMessage))
                {
                    if (!string.IsNullOrEmpty(CC))
                        message.CC.Add(CC);
                    if (!string.IsNullOrEmpty(BCC))
                        message.Bcc.Add(BCC);

                    // The attachments array should point to a file location     
                    // where
                    // the attachment resides - add the attachments to the
                    // message
                    Attachment attached;
                    foreach (string attach in attachments)
                    {
                        attached = new Attachment(attach, MediaTypeNames.Application.Octet);
                        message.Attachments.Add(attached);
                    }

                    // create smtp client at mail server location
                    SmtpClient client = new SmtpClient(ConfigurationManager.AppSettings["defaultSmtpAdress"]);

                    // Add credentials
                    string user = ConfigurationManager.AppSettings["smtpUser"];
                    string password = ConfigurationManager.AppSettings["smtpPassword"];
                    if (user == "" || password == "")
                    {
                        client.UseDefaultCredentials = true;
                    }
                    else
                    {
                        client.Host = ConfigurationManager.AppSettings["defaultSmtpAdress"];
                        client.Credentials = new System.Net.NetworkCredential(user, password);
                    }
                    // send message
                    client.Send(message);
                }



                return "Message sent to " + sendTo + " at " +
                DateTime.Now.ToString() + ".";
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }

        public static bool ValidateEmailAddress(string emailAddress)
        {
            try
            {
                string TextToValidate = emailAddress;
                Regex expression = new Regex(@"\w+@[a-zA-Z_]+?\.[a-zA-Z]{2,3}");

                // test email address with expression
                if (expression.IsMatch(TextToValidate))
                {
                    // is valid email address
                    return true;
                }
                else
                {
                    // is not valid email address
                    return false;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }


    }
}
