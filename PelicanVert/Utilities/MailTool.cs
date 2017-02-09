using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net.Mail;

namespace QLyx.Utilities
{
    public class MailTool
    {

        public MailTool() { 
        }


        public static bool SendMailCc(string subject, string body,
       string[] attachment, List<string> lMailReceiver, List<string> lMailReceiverCc)
        {
            string sender = "par-lyxor-inv-sam-pri@lyxor.com";
            return SendMailCc(sender, subject, body, attachment, lMailReceiver, lMailReceiverCc);
        }



        public static bool SendMailCc(string sender, string subject, string body,
            string[] attachment, List<string> lMailReceiver, List<string> lMailReceiverCc)
        {
            bool resMail;

            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient client = new SmtpClient();
                client.Port = 25;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Host = "smtp-goss.int.world.socgen";
                mail.Subject = subject;
                mail.Body = body;

                mail.From = new MailAddress(sender);

                if (lMailReceiver.Count() != 0)
                    foreach (string to in lMailReceiver)
                        if (!string.IsNullOrEmpty(to))
                            mail.To.Add(to);

                if (lMailReceiverCc != null)
                    foreach (string cc in lMailReceiverCc)
                        if (!string.IsNullOrEmpty(cc))
                            mail.CC.Add(cc);

                if (attachment != null)
                    foreach (string att in attachment)
                        if (!string.IsNullOrEmpty(att))
                            mail.Attachments.Add(new Attachment(att));

                client.Send(mail);

                client.Dispose();

                mail.Dispose();
                resMail = true;
            }

            catch //(Exception ex)
            {
                resMail = false;
            }

            return resMail;
        }

    }




}
