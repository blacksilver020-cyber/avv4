using CadastroEventos.Models;

namespace CadastroEventos.Pages;

public partial class CadastroEventoPage : ContentPage
{
    private readonly Evento _evento;

    public CadastroEventoPage()
    {
        InitializeComponent();

        _evento = new Evento
        {
            DataInicio = DateTime.Today,
            DataTermino = DateTime.Today.AddDays(1)
        };

        // BindingContext associa diretamente os controles da tela à model Evento.
        BindingContext = _evento;
    }

    private async void OnCadastrarClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(_evento.NomeEvento))
        {
            await DisplayAlert("Atenção", "Informe o nome do evento.", "OK");
            return;
        }

        if (_evento.DataTermino.Date < _evento.DataInicio.Date)
        {
            await DisplayAlert("Atenção", "A data de término não pode ser anterior à data de início.", "OK");
            return;
        }

        if (_evento.NumeroParticipantes <= 0)
        {
            await DisplayAlert("Atenção", "Informe um número de participantes maior que zero.", "OK");
            return;
        }

        if (string.IsNullOrWhiteSpace(_evento.LocalEvento))
        {
            await DisplayAlert("Atenção", "Informe o local do evento.", "OK");
            return;
        }

        if (!_evento.CustoPorParticipanteTextoValido)
        {
            await DisplayAlert("Atenção", "Informe um custo válido por participante. Exemplo: 85,00.", "OK");
            return;
        }

        await Navigation.PushAsync(new ResumoEventoPage(_evento));
    }
}
