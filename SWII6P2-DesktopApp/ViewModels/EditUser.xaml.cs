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
            await DisplayAlert("Informações Vazias", "Verifique se todos os campos foram preenchidos corretamente.", "Ok");
        }

        string url = "http://localhost:5133/api/User";
        // Convertendo o objeto User para JSON
        string json = JsonSerializer.Serialize(user);

        // Criando o conteúdo para o body da requisição (application/json)
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        using (HttpClient client = new HttpClient())
        {
            try
            {
                HttpResponseMessage response = await client.PutAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    await DisplayAlert("Sucesso", "O usuário foi editado com sucesso", "Ok");
                    await Navigation.PopAsync();
                }
                else
                {
                    await DisplayAlert("Erro", "Ocorreu um erro ao atualizar o usuário. Por favor tente novamente.", "Ok");
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
        var answer = await DisplayAlert("Você deseja cancelar a operação?", "Atenção! Ao cancelar a edição do usuário você perde todas as alterações realizadas, tem certeza que deseja cancelar a operação?", "Cancelar", "Não");
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
                    await DisplayAlert("Sucesso", "O usuário foi excluído com sucesso.", "Ok");
                }
                else
                {
                    await DisplayAlert("Erro", "Houve um erro ao excluir o usuário", "Voltar");
                }
            }catch(Exception ex)
            {
                await DisplayAlert("Erro Inesperado", $"Detalhes: {ex.Message}", "Retornar");
            }
        }
        await Navigation.PopAsync();
    }
}