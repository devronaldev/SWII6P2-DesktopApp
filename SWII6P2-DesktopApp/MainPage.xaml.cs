using SWII6P2_DesktopApp.Models;
using SWII6P2_DesktopApp.ViewModels;
using SWII6P2_DesktopApp.Views;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SWII6P2_DesktopApp
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            LoadUsers();
        }

        private async void FatalErrorAlert(string title, string message)
        {
            await DisplayAlert(title, message, "Fechar aplicação.");
            OnAppearing();
        }

        private async void LoadUsers()
        {
            string url = "http://localhost:5133/api/User";

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        var options = new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        };
                        string jsonResponse = await response.Content.ReadAsStringAsync();
                        var resultado = JsonSerializer.Deserialize<List<User>>(jsonResponse, options);
                        if (resultado == null)
                        {
                            FatalErrorAlert("Erro ao converter dados", "Ouve um erro ao desserializar os dados");
                        }
                        TaskListView.ItemsSource = resultado;
                        Console.WriteLine("Dados recebidos e convertidos com sucesso.");
                    }
                    else
                    {
                        Console.WriteLine($"Código HTTP não esperado: {response.StatusCode} - {response.Content}");
                        FatalErrorAlert("Erro na requisição.", "Um erro inesperado ocorreu durante a execução do carregamento da tela inicial. " +
                            $"Detalhes: {response.StatusCode} - {response.Content}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    FatalErrorAlert("Erro inesperado", "Um erro inesperado ocorreu. " +
                        $"Detalhes: {ex.Message}");
                }
            }
        }

        private async void OnUserSelected(object sender, ItemTappedEventArgs eventArgs)
        {
            await Navigation.PushAsync(new EditUser
            {
                BindingContext = eventArgs.Item
            });
        }

        private async void OnCreateClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CreateUser());
        }

        private async void OnCreditsClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Credits());
        }
    }
}
