using SkiaSharp;

namespace rac_image_resizer;

public partial class ImageResizePage : ContentPage
{
    public FileResult InputFileResult{ get; set; }

    public ImageResizePage(FileResult fileResult)
	{
		InitializeComponent();

        InputFileResult = fileResult;

        // input image to XAML
        selectedImage.Source = ImageSource.FromFile(fileResult.FullPath);
    }

    private async void BtnSaveImage(object sender, EventArgs e)
    {
        int quality = 20; // 0 - 100 (100 is best)
        string inputPath = InputFileResult.FullPath;
        string outputPath = InputFileResult.FullPath + "_compressed.jpg";
        int targetDim = 500;

        try
        {
            await CheckSetPermissions();
            var newImage = SetRes(inputPath, targetDim);
            SaveImage(newImage, outputPath, quality);
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Error: {ex.ToString}:{ex.Message}", "OK");
        }
        finally
        {
            await Navigation.PopAsync();
        }
    }

    private async Task CheckSetPermissions()
    {
        if ((await Permissions.RequestAsync<Permissions.StorageRead>() != PermissionStatus.Granted) &&
            (await Permissions.RequestAsync<Permissions.StorageWrite>() != PermissionStatus.Granted))
        {
            await DisplayAlert("Berechtigung erteilen", "Um Bilder speichern zu können müssen entsprechende Lese-/Schreibberechtigungen erteilt sein.", "OK");
        }
    }

    private static void SaveImage(SKBitmap image, string outputFilePath, int jpegQuality)
    {
        using (image)
        {
            // Create output stream
            using (var outputStream = File.Create(outputFilePath))
            {
                // Save the resized image as JPEG with specified quality
                image.Encode(outputStream, SKEncodedImageFormat.Jpeg, jpegQuality);
            }
        }
    }

    private static SKBitmap SetRes(string inputFilePath, int targetDimension)
    {
        using (var inputStream = File.OpenRead(inputFilePath))
        {
            using (var originalBitmap = SKBitmap.Decode(inputStream))
            {
                // Calculate scaling factor
                float scaleWidth = (float)targetDimension / originalBitmap.Width;
                float scaleHeight = (float)targetDimension / originalBitmap.Height;
                float scale = Math.Min(scaleWidth, scaleHeight);

                // Calculate new dimensions
                int newWidth = (int)(originalBitmap.Width * scale);
                int newHeight = (int)(originalBitmap.Height * scale);

                return originalBitmap.Resize(new SKImageInfo(newWidth, newHeight), SKFilterQuality.Medium);
            }
        }
    }
}