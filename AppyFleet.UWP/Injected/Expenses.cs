using System;
using System.Collections.Generic;
using mvvmframework;
using mvvmframework.Interfaces;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.ApplicationModel.Email;

namespace AppyFleet.UWP.Injected
{
    public class Expenses : IExpenses
    {
        public void ExportJourneysToEmail(List<JourneyModel> journeys)
        {
            Task.Run(async () =>
            {
                var file = await ApplicationData.Current.LocalFolder.CreateFileAsync("expenses.csv",
                    CreationCollisionOption.ReplaceExisting);

                var emailMessage = new EmailMessage
                {
                    Body = string.Empty,
                    Subject = string.Format("Expenses {0}", DateTime.Now.ToString("dd/MM/yyyy")),
                    SentTime = DateTime.Now
                };

                var stream = Windows.Storage.Streams.RandomAccessStreamReference.CreateFromFile(file);
                var attachment = new EmailAttachment("expenses.csv", stream);
                emailMessage.Attachments.Add(attachment);

                await EmailManager.ShowComposeNewEmailAsync(emailMessage);
            });
        }
    }
}
