using mvvmframework.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Chat;
using Windows.ApplicationModel.Email;
using Windows.Storage;

namespace AppyFleet.UWP.Injected
{
    public class EmailSms : IEmailSms
    {
        public string GenerateEmailHashingString(string key, string emailAddress)
        {
            var sha = SHA256.Create();
            var hashBytes = sha.ComputeHash(Encoding.UTF8.GetBytes($"{emailAddress}:{key}"));

            var builder = new StringBuilder();

            foreach (var b in hashBytes)
            {
                builder.AppendFormat("{0:x2}", b);
            }

            var hash = builder.ToString();

            return hash;
        }

        public bool SendEmailFile(string to, string subject, string body)
        {
            Task.Run(async () =>
            {
                var file = await ApplicationData.Current.LocalFolder.CreateFileAsync("Log.txt",
                    CreationCollisionOption.ReplaceExisting);

                var emailMessage = new EmailMessage
                {
                    Body = string.Empty,
                    Subject = subject,
                    SentTime = DateTime.Now
                };
                var emailRecipient = new EmailRecipient(to);
                emailMessage.To.Add(emailRecipient);
                var stream = Windows.Storage.Streams.RandomAccessStreamReference.CreateFromFile(file);
                var attachment = new EmailAttachment(body, stream);
                emailMessage.Attachments.Add(attachment);

                await EmailManager.ShowComposeNewEmailAsync(emailMessage);
            });

            return true;
        }

        public bool SendEmail(string to, string subject, string body)
        {
            Task.Run(async () =>
            {
                var file = await ApplicationData.Current.LocalFolder.CreateFileAsync("Log.txt",
                    CreationCollisionOption.ReplaceExisting);

                var emailMessage = new EmailMessage
                {
                    Body = string.Empty,
                    Subject = subject,
                    SentTime = DateTime.Now
                };
                var emailRecipient = new EmailRecipient(to);
                emailMessage.To.Add(emailRecipient);

                await EmailManager.ShowComposeNewEmailAsync(emailMessage);
            });

            return true;
        }

        public void SendSOSSMS(string message, string mobileNumber)
        {
            Task.Run(async () =>
            {
                var chatMessage = new ChatMessage
                {
                    Body = message,
                };
                chatMessage.Recipients.Add(mobileNumber);
                
                await ChatMessageManager.ShowComposeSmsMessageAsync(chatMessage);
            });
        }
    }
}
