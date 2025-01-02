using SWII6P2_DesktopApp.Models;
using System.Text;
using System.Text.Json;
using System.Net.Http;

namespace SWII6P2_DesktopApp.ViewModels;

public partial class CreateUser : ContentPage
{
	public CreateUser()
	{
		InitializeComponent();
	}

	protected async void OnCreateClicked(object sender, EventArgs e)
	{
		if (!SWII6P2Verifications.Verifications.IsUserFull(EntryName.Text, EntryPassword.Text))
		{
			await DisplayAlert("Informa��o necess�ria", "Verifique qual dos campos est� vazio e tente novamente.", "Ok");
			return;
		}
		User user = new User
		{
			Id = 0,
			Name = EntryName.Text,
			Password = EntryPassword.Text,
			Status = SwitchStatus.IsToggled
		};

		using (var client = new HttpClient())
		{
			string url = "http://localhost:5133/api/User";

            // Convertendo o objeto User para JSON
            string json = JsonSerializer.Serialize(user);

            // Criando o conte�do para o body da requisi��o (application/json)
            var content = new StringContent(json, Encoding.UTF8, "application/json");

			try
			{
				HttpResponseMessage response = await client.PostAsync(url, content);

				if (response.IsSuccessStatusCode)
				{
					await DisplayAlert("Sucesso", "O usu�rio foi criado com sucesso.", "Ok");
					await Navigation.PopAsync();
				}
				else
				{
					await DisplayAlert("Erro", "Houve um erro ao criar o usu�rio. Por favor, tente novamente.", "Ok");
					return;
				}
			}
			catch (Exception ex)
			{
				await DisplayAlert("Erro Inesperado", $"Houve um erro inesperado ao criar o usu�rio. Detalhes: {ex.Message}", "Ok");
			}
		}
	}

	protected async void OnCancelClicked(object sender, EventArgs e)
	{
		var answer = await DisplayAlert("Voc� deseja cancelar a opera��o?", "Aten��o! Ao cancelar a cria��o do usu�rio voc� perde todos os dados preenchidos, tem certeza que deseja cancelar a opera��o?", "Cancelar", "N�o");
		if (answer)
		{
			await Navigation.PopAsync();
		}
	}
}