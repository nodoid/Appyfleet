using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Android.Content;
using mvvmframework.Interfaces;

namespace NewAppyFleet.Droid.Injected
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
            var rv = false;

            try
            {
                var filePath = Path.Combine(Path.Combine(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath, "AppyFleet", "Log.txt"));

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

                File.WriteAllText(filePath, body);
                var uri = Android.Net.Uri.FromFile(new Java.IO.File(filePath));
                var emailIntent = new Intent(Intent.ActionSend);

                emailIntent.SetType("text/html");
                emailIntent.PutExtra(Intent.ExtraEmail, new String[] { to });
                emailIntent.PutExtra(Android.Content.Intent.ExtraSubject, subject);
                emailIntent.PutExtra(Intent.ExtraStream, uri);
                MainActivity.Active.StartActivity(Intent.CreateChooser(emailIntent, "Email:"));

                rv = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception thrown sending file via email {ex.Message}--{ex.InnerException?.Message}");
            }

            return rv;
        }

        public bool SendEmail(string to, string subject, string body)
        {
            var rv = false;
            try
            {
                var emailIntent = new Intent(Intent.ActionSend);

                emailIntent.SetType("text/html");
                emailIntent.PutExtra(Intent.ExtraEmail, new String[] { to });
                emailIntent.PutExtra(Intent.ExtraSubject, subject);
                emailIntent.PutExtra(Intent.ExtraText, body);
                MainActivity.Active.StartActivity(Intent.CreateChooser(emailIntent, "Email:"));

                rv = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception thrown sending email {ex.Message}--{ex.InnerException?.Message}");
            }

            return rv;
        }

        public void SendSOSSMS(string message, string mobileNumber)
        {
            var smsUri = Android.Net.Uri.Parse($"smsto:{mobileNumber}");

            var smsIntent = new Intent(Intent.ActionSendto, smsUri);
            smsIntent.PutExtra("sms_body", message);
            MainActivity.Active.StartActivity(smsIntent);
        }
    }
}
