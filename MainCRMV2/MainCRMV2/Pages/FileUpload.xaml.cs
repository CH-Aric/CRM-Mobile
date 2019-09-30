using Plugin.FileUploader;
using Plugin.FileUploader.Abstractions;
using Plugin.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MainCRMV2.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FileUpload : ContentPage
    {
        string cusFilename = "";
        Queue<string> paths = new Queue<string>();
        string filePath = string.Empty;
        bool isBusy = false;
        int cusID;
        public FileUpload(int customerID)
        {
            InitializeComponent();
            cusID = customerID;
            setupUploader();
            /*string sql= "SELECT cusindex.Name,cusindex.IDKey,cusfields.Value,cusfields.Index,cusindex.Stage FROM cusindex INNER JOIN cusfields ON cusindex.IDKey=cusfields.CusID WHERE (cusfields.Index LIKE '%ress%') AND cusfields.CusID='"+customerID+"'";
            TaskCallback call = recordFileName;
            DatabaseFunctions.SendToPhp(false,sql,call);*/
        }/*
        public void recordFileName(string results)
        {
            Dictionary<string, List<string>> dictionary = FormatFunctions.createValuePairs(FormatFunctions.SplitToPairs(results));
            cusFilename = FormatFunctions.purgeSpace(dictionary["Value"][0] + " - " +dictionary["Name"][0]);
        }*/
        public void setupUploader()
        {
            takePhoto.Clicked += async (sender, args) =>
            {

                if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                {
                    await DisplayAlert("No Camera", ":( No camera available.", "OK");
                    return;
                }

                var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
                {
                    PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium,
                    Directory = "Sample",
                    Name = "test.jpg"
                });

                if (file == null)
                    return;
                filePath = file.Path;
                paths.Enqueue(filePath);
                image.Source = ImageSource.FromStream(() =>
                {
                    var stream = file.GetStream();

                    file.Dispose();
                    return stream;
                });
            };

            pickPhoto.Clicked += async (sender, args) =>
            {
                if (!CrossMedia.Current.IsPickPhotoSupported)
                {
                    await DisplayAlert("Photos Not Supported", ":( Permission not granted to photos.", "OK");
                    return;
                }
                var file = await CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
                {
                    PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium
                });


                if (file == null)
                    return;

                filePath = file.Path;
                paths.Enqueue(filePath);

                image.Source = ImageSource.FromStream(() =>
                {
                    var stream = file.GetStream();
                    //  file.Dispose();
                    return stream;
                });
            };
            //Taking videos needs some updates
            /*takeVideo.Clicked += async (sender, args) =>
            {
                if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakeVideoSupported)
                {
                    await DisplayAlert("No Camera", ":( No camera avaialble.", "OK");
                    return;
                }

                var file = await CrossMedia.Current.TakeVideoAsync(new Plugin.Media.Abstractions.StoreVideoOptions
                {
                    Name = "video.mp4",
                    Directory = "DefaultVideos",
                });

                if (file == null)
                    return;
                filePath = file.Path;
                paths.Enqueue(filePath);
                await DisplayAlert("Video Recorded", "Location: " + file.Path, "OK");

                file.Dispose();
            };*/

            pickVideo.Clicked += async (sender, args) =>
            {
                if (!CrossMedia.Current.IsPickVideoSupported)
                {
                    await DisplayAlert("Videos Not Supported", ":( Permission not granted to videos.", "OK");
                    return;
                }
                var file = await CrossMedia.Current.PickVideoAsync();

                if (file == null)
                    return;
                filePath = file.Path;
                paths.Enqueue(filePath);
                await DisplayAlert("Video Selected", "Location: " + file.Path, "OK");
                file.Dispose();
            };

            CrossFileUploader.Current.FileUploadCompleted += Current_FileUploadCompleted;
            CrossFileUploader.Current.FileUploadError += Current_FileUploadError;
            CrossFileUploader.Current.FileUploadProgress += Current_FileUploadProgress;
        }

        private void Current_FileUploadProgress(object sender, FileUploadProgress e)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                progress.Progress = e.Percentage / 100.0f;
            });
        }

        private void Current_FileUploadError(object sender, FileUploadResponse e)
        {
            isBusy = false;
            System.Diagnostics.Debug.WriteLine($"{e.StatusCode} - {e.Message}");
            Device.BeginInvokeOnMainThread(async () =>
            {
                await DisplayAlert("File Upload", "Upload Failed", "Ok");
                progress.IsVisible = false;
                progress.Progress = 0.0f;
            });
        }

        private void Current_FileUploadCompleted(object sender, FileUploadResponse e)
        {
            isBusy = false;
            System.Diagnostics.Debug.WriteLine($"{e.StatusCode} - {e.Message}");
            Device.BeginInvokeOnMainThread(async () =>
            {
                await DisplayAlert("File Upload", "Upload Completed", "Ok");
                progress.IsVisible = false;
                progress.Progress = 0.0f;
            });
        }

        void OnUpload(object sender, EventArgs args)
        {
            if (isBusy)
                return;
            isBusy = true;
            progress.IsVisible = true;
            string x = "http://coolheatcrm.duckdns.org/accessFile.php/?cus=" + cusID;
            CrossFileUploader.Current.UploadFileAsync(x, new FilePathItem("TestFile", filePath), new Dictionary<string, string>()
            {
            });
        }
    }
}