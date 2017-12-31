using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Android.Content;
using mvvmframework.Interfaces;
using Xamarin.Forms;

namespace NewAppyFleet.Droid.Injected
{
    public class FileSystem: IFileSystem
    {
        static string FilePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        string path;

        public void DisplayFile(string filename = "")
        {
            if (!string.IsNullOrEmpty(filename))
                path = Path.Combine(FilePath, filename);
            try
            {
                var bytes = File.ReadAllBytes(path);
                var extPath = global::Android.OS.Environment.ExternalStorageDirectory.Path + "/tmp." + path.Split('/').Last();
                File.WriteAllBytes(extPath, bytes);
                var file = new Java.IO.File(extPath);

                var targetUri = Android.Net.Uri.FromFile(file);
                var intent = new Intent(Intent.ActionView);
                intent.AddFlags(ActivityFlags.ClearWhenTaskReset|ActivityFlags.NewTask);
                intent.AddFlags(ActivityFlags.GrantReadUriPermission);
                intent.AddFlags(ActivityFlags.GrantPrefixUriPermission);
                intent.SetDataAndType(targetUri, "application/pdf");
                MainActivity.Active.StartActivity(intent);
            }
            catch (Exception ex)
            {
                MessagingCenter.Send("display", "error", "activity");
            }
        }

        public async Task DownloadFile(string url, string filename)
        {
            path = Path.Combine(FilePath, filename);
            var webClient = new WebClient();
            webClient.DownloadDataCompleted += Completed;
            webClient.DownloadFileAsync(new Uri(url), path);
        }

        public bool FileExists(string filename)
        {
            path = Path.Combine(FilePath, filename);
            if (File.Exists(path))
            {
                DisplayFile(filename);
                return true;
            }
            else
                return false;
        }

        void Completed(object sender, AsyncCompletedEventArgs e)
        {
            DisplayFile();
        }
    }
}
