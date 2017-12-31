using System;
using System.Collections.Generic;
using System.IO;
using Android.Content;
using CsvHelper;
using mvvmframework;
using mvvmframework.Interfaces;

namespace NewAppyFleet.Droid.Injected
{
    public class Expenses : IExpenses
    {
        public void ExportJourneysToEmail(List<JourneyModel> journeys)
        {
            var filePath = Path.Combine(Path.Combine(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath, "AppyFleet", "expenses.csv"));
            if (!Directory.Exists(Path.Combine(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath, "AppyFleet")))
            {
                try
                {
                    Directory.CreateDirectory(Path.Combine(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath, "AppyFleet"));
                }
                catch
                {
                }
            }
            try
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }
            catch
            {
            }

            using (var file = File.Open(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                using (var textWriter = new StreamWriter(file))
                {
                    var csv = new CsvWriter(textWriter);

                    csv.WriteField("Start Time");
                    csv.WriteField("Start Location");
                    csv.WriteField("End Time");
                    csv.WriteField("End Location");
                    csv.WriteField("Vehicle");
                    csv.WriteField("Mileage");
                    csv.NextRecord();

                    foreach (var item in journeys)
                    {
                        csv.WriteField(item.StartDate);
                        csv.WriteField(item.StartLocation);
                        csv.WriteField(item.EndDate);
                        csv.WriteField(item.EndLocation);
                        csv.WriteField(item.Nickname);
                        csv.WriteField(item.Miles);
                        csv.NextRecord();
                    }
                }
            }
            var uri = Android.Net.Uri.FromFile(new Java.IO.File(filePath));

            //now send email
            var email = new Intent(Intent.ActionSend);
            email.PutExtra(Intent.ExtraSubject, string.Format("Expenses {0}", DateTime.Now.ToString("dd/MM/yyyy")));
            email.PutExtra(Intent.ExtraStream, uri);
            email.SetType("message/rfc822");

            MainActivity.Active.StartActivity(Intent.CreateChooser(email, "Email:"));
        }
    }
}
