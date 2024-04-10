namespace rac_image_resizer
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void ChooseImage(object sender, EventArgs e)
        {
            try
            {
                var options = new PickOptions
                {
                    FileTypes = FilePickerFileType.Images,
                    PickerTitle = "Select a JPG file"
                };

                var result = await FilePicker.PickAsync(options);
                if (result != null)
                {
                    //var stream = await result.OpenReadAsync();

                    await Navigation.PushAsync(new ImageResizePage(result));
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
            }
        }
    }

}
