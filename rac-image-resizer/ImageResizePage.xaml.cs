namespace rac_image_resizer;

public partial class ImageResizePage : ContentPage
{
    public ImageSource SelectedImage { get; set; }

    public ImageResizePage(ImageSource imageSource)
	{
		InitializeComponent();
		SelectedImage = imageSource;

        selectedImage.Source = SelectedImage;
    }

    private async void SaveImage(object sender, EventArgs e)
    {
        Image image = new Image() { Source = SelectedImage};

        // TODO: save image with new quality and size

        await DisplayAlert("Bild speichern", $"save image", "OK");
    }
}