using SWII6P2_DesktopApp.Models;
using System.Text;
using System.Text.Json;

namespace SWII6P2_DesktopApp.ViewModels;

public partial class EditUser : ContentPage
{
	public EditUser()
	{
		InitializeComponent();
	}

    protected async void OnEditClicked(object sender, EventArgs e)
    {
        User user = (User)BindingContext;
        user.Status = SwitchStatus.IsToggled;
        user.Name = EntryName.Text;
        user.Password = EntryPassword.Text;

        if(!SWII6P2Verifications.Verifications.IsUserFull(user.Name, user.Password))
        {
            await DisplayAlert("Informa��es Vazias", "Verifique se todos os campos foram preenchidos corretamente.", "Ok");
        }

        string url = "http://localhost:5133/api/User";
        // Convertendo o objeto User para JSON
        string json = JsonSerializer.Serialize(user);

        // Criando o conte�do para o body da requisi��o (application/json)
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        using (HttpClient client = new HttpClient())
        {
            try
            {
                HttpResponseMessage response = await client.PutAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    await DisplayAlert("Sucesso", "O usu�rio foi editado com sucesso", "Ok");
                    await Navigation.PopAsync();
                }
                else
                {
                    await DisplayAlert("Erro", "Ocorreu um erro ao atualizar o usu�rio. Por favor tente novamente.", "Ok");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro Inesperado", $"Detalhes {ex.Message}", "Ok");
            }
        }
    }

    protected async void OnCancelClicked(object sender, EventArgs e)
    {
        var answer = await DisplayAlert("Voc� deseja cancelar a opera��o?", "Aten��o! Ao cancelar a edi��o do usu�rio voc� perde todas as altera��es realizadas, tem certeza que deseja cancelar a opera��o?", "Cancelar", "N�o");
        if (answer)
        {
            await Navigation.PopAsync();
        }
    }

    protected async void OnDeleteClicked(object sender, EventArgs e)
    {
        var user = BindingContext as User;
        using(HttpClient client = new HttpClient())
        {
            string url = $"http://localhost:5133/api/User?userId={user.Id}";

            try
            {
                HttpResponseMessage response = await client.DeleteAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    await DisplayAlert("Sucesso", "O usu�rio foi exclu�do com sucesso.", "Ok");
                }
                else
                {
                    await DisplayAlert("Erro", "Houve um erro ao excluir o usu�rio", "Voltar");
                }
            }catch(Exception ex)
            {
                await DisplayAlert("Erro Inesperado", $"Detalhes: {ex.Message}", "Retornar");
            }
        }
        await Navigation.PopAsync();
    }
}