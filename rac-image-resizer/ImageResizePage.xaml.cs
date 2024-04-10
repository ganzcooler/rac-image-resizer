using Microsoft.Maui.Graphics.Platform;
using IImage = Microsoft.Maui.Graphics.IImage;

namespace rac_image_resizer;

public partial class ImageResizePage : ContentPage
{
    public Stream SourceStream { get; set; }
    public IImage OriginalImage { get; set; }

    public ImageResizePage(Stream stream)
	{
		InitializeComponent();

        SourceStream = stream;
        OriginalImage = PlatformImage.FromStream(stream);

        selectedImage.Source = ImageSource.FromStream(() => stream);
    }

    private async void SaveImage(object sender, EventArgs e)
    {
        float quality = 0.2f; // 0 - 100
        string outputPath = "rac3.jpg";

        try
        {
            // resize image
            IImage newImage = OriginalImage.Resize(200f, 200f, ResizeMode.Fit);

            using (MemoryStream memoryStream = new MemoryStream())
            {
                // save image with reduced quality
                await newImage.SaveAsync(memoryStream, format: ImageFormat.Jpeg, quality: quality);

                string filePath = outputPath;
                using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                {
                    memoryStream.Seek(0, SeekOrigin.Begin); // Setze den Stream zurück auf den Anfang
                    memoryStream.CopyTo(fileStream); // Kopiere den Inhalt des MemoryStreams in den FileStream
                }
            }

        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Error: {ex.ToString}:{ex.Message}", "OK");
        }

        await Navigation.PopAsync();
    }
}