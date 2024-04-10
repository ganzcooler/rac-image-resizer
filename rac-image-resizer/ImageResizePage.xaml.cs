using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using Image = System.Drawing.Image;
using ImageFormat = System.Drawing.Imaging.ImageFormat;

namespace rac_image_resizer;

public partial class ImageResizePage : ContentPage
{
    public Stream SourceStream { get; set; }
    public Image OriginalImage { get; set; }

    public ImageResizePage(Stream stream)
	{
		InitializeComponent();

        selectedImage.Source = ImageSource.FromStream(() => stream);

        SourceStream = stream;
        OriginalImage = new Bitmap(Bitmap.FromStream(stream));
    }

    private async void SaveImage(object sender, EventArgs e)
    {
        long quality = 10; // 0 - 100
        string outputPath = "rac3.jpg";

        try
        {
            // set quality parameter
            ImageCodecInfo jpgEncoder = GetEncoder(ImageFormat.Jpeg);
            Encoder encoder = Encoder.Quality;
            EncoderParameters encoderParameters = new EncoderParameters(1);
            EncoderParameter encoderParameter = new EncoderParameter(encoder, 10L);
            encoderParameters.Param[0] = encoderParameter;

            var newImage = new Bitmap(OriginalImage);
            newImage = ResizeImage(newImage, 500, 300);

            newImage.Save(outputPath, jpgEncoder, encoderParameters);
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

    /// <summary>
    /// Resize the image to the specified width and height.
    /// </summary>
    /// <param name="image">The image to resize.</param>
    /// <param name="width">The width to resize to.</param>
    /// <param name="height">The height to resize to.</param>
    /// <returns>The resized image.</returns>
    private static Bitmap ResizeImage(Image image, int width, int height)
    {
        var destRect = new Rectangle(0, 0, width, height);
        var destImage = new Bitmap(width, height);

        destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

        using (var graphics = Graphics.FromImage(destImage))
        {
            graphics.CompositingMode = CompositingMode.SourceCopy;
            graphics.CompositingQuality = CompositingQuality.HighQuality;
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

            using (var wrapMode = new ImageAttributes())
            {
                wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
            }
        }

        return destImage;
    }

    private ImageCodecInfo GetEncoder(ImageFormat format)
    {
        ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
        foreach (ImageCodecInfo codec in codecs)
        {
            if (codec.FormatID == format.Guid)
            {
                return codec;
            }
        }
        return null;
    }
}