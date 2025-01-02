namespace SWII6P2_DesktopApp.Views;

public partial class Credits : ContentPage
{
	public Credits()
	{
		InitializeComponent();
	}

	protected async void OnBackClicked(object sender, EventArgs e)
	{
		await Navigation.PopAsync();
	}
}