using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace CadastroEventos.Models;

/// <summary>
/// Model do evento. A página de cadastro define esta instância como BindingContext.
/// Os campos em texto mantêm a digitação do Entry; as propriedades numéricas
/// representam os valores convertidos usados nos cálculos.
/// </summary>
public class Evento : INotifyPropertyChanged
{
    private static readonly CultureInfo CulturaBrasil = CultureInfo.GetCultureInfo("pt-BR");

    private string _nomeEvento = string.Empty;
    private DateTime _dataInicio = DateTime.Today;
    private DateTime _dataTermino = DateTime.Today.AddDays(1);
    private int _numeroParticipantes;
    private string _numeroParticipantesTexto = string.Empty;
    private string _localEvento = string.Empty;
    private decimal _custoPorParticipante;
    private string _custoPorParticipanteTexto = string.Empty;

    public event PropertyChangedEventHandler? PropertyChanged;

    public string NomeEvento
    {
        get => _nomeEvento;
        set
        {
            if (_nomeEvento == value) return;
            _nomeEvento = value;
            OnPropertyChanged();
        }
    }

    public DateTime DataInicio
    {
        get => _dataInicio;
        set
        {
            if (_dataInicio.Date == value.Date) return;
            _dataInicio = value.Date;
            OnPropertyChanged();
            AtualizarPropriedadesCalculadas();
        }
    }

    public DateTime DataTermino
    {
        get => _dataTermino;
        set
        {
            if (_dataTermino.Date == value.Date) return;
            _dataTermino = value.Date;
            OnPropertyChanged();
            AtualizarPropriedadesCalculadas();
        }
    }

    /// <summary>Quantidade numérica de participantes usada no cálculo do custo total.</summary>
    public int NumeroParticipantes
    {
        get => _numeroParticipantes;
        set
        {
            if (_numeroParticipantes == value) return;
            _numeroParticipantes = Math.Max(0, value);
            _numeroParticipantesTexto = _numeroParticipantes.ToString(CultureInfo.InvariantCulture);
            OnPropertyChanged();
            OnPropertyChanged(nameof(NumeroParticipantesTexto));
            AtualizarPropriedadesCalculadas();
        }
    }

    /// <summary>Propriedade textual associada ao Entry numérico do formulário.</summary>
    public string NumeroParticipantesTexto
    {
        get => _numeroParticipantesTexto;
        set
        {
            if (_numeroParticipantesTexto == value) return;
            _numeroParticipantesTexto = value;
            _numeroParticipantes = int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var quantidade)
                ? Math.Max(0, quantidade)
                : 0;
            OnPropertyChanged();
            OnPropertyChanged(nameof(NumeroParticipantes));
            AtualizarPropriedadesCalculadas();
        }
    }

    public string LocalEvento
    {
        get => _localEvento;
        set
        {
            if (_localEvento == value) return;
            _localEvento = value;
            OnPropertyChanged();
        }
    }

    /// <summary>Valor numérico por participante, usado no cálculo financeiro.</summary>
    public decimal CustoPorParticipante
    {
        get => _custoPorParticipante;
        set
        {
            if (_custoPorParticipante == value) return;
            _custoPorParticipante = Math.Max(0m, value);
            _custoPorParticipanteTexto = _custoPorParticipante.ToString("0.00", CulturaBrasil);
            OnPropertyChanged();
            OnPropertyChanged(nameof(CustoPorParticipanteTexto));
            AtualizarPropriedadesCalculadas();
        }
    }

    /// <summary>Propriedade textual associada ao Entry de moeda.</summary>
    public string CustoPorParticipanteTexto
    {
        get => _custoPorParticipanteTexto;
        set
        {
            if (_custoPorParticipanteTexto == value) return;
            _custoPorParticipanteTexto = value;
            _custoPorParticipante = ConverterMoeda(value);
            OnPropertyChanged();
            OnPropertyChanged(nameof(CustoPorParticipante));
            AtualizarPropriedadesCalculadas();
        }
    }

    /// <summary>
    /// Diferença entre as datas usando DateTime e TimeSpan.
    /// A duração abaixo conta os dias corridos do intervalo, sem incluir um dia extra.
    /// </summary>
    public TimeSpan Duracao => DataTermino.Date - DataInicio.Date;

    public int DuracaoEmDias => Math.Max(0, Duracao.Days);

    public string DuracaoFormatada
    {
        get
        {
            var dias = DuracaoEmDias;
            return dias == 1 ? "1 dia" : $"{dias} dias";
        }
    }

    public bool CustoPorParticipanteTextoValido =>
        !string.IsNullOrWhiteSpace(CustoPorParticipanteTexto) &&
        TryConverterMoeda(CustoPorParticipanteTexto, out var valor) && valor >= 0m;

    public decimal CustoTotal => NumeroParticipantes * CustoPorParticipante;

    public string DataInicioFormatada => DataInicio.ToString("dd/MM/yyyy", CulturaBrasil);
    public string DataTerminoFormatada => DataTermino.ToString("dd/MM/yyyy", CulturaBrasil);
    public string NumeroParticipantesFormatado => NumeroParticipantes.ToString("N0", CulturaBrasil);
    public string CustoPorParticipanteFormatado => CustoPorParticipante.ToString("C2", CulturaBrasil);
    public string CustoTotalFormatado => CustoTotal.ToString("C2", CulturaBrasil);

    private static decimal ConverterMoeda(string texto)
        => TryConverterMoeda(texto, out var valor) ? Math.Max(0m, valor) : 0m;

    private static bool TryConverterMoeda(string texto, out decimal valor)
    {
        valor = 0m;
        if (string.IsNullOrWhiteSpace(texto)) return false;

        // Aceita tanto 125,50 quanto 125.50, sem exigir que o usuário altere a região do aparelho.
        var normalizado = texto.Trim()
            .Replace("R$", string.Empty, StringComparison.OrdinalIgnoreCase)
            .Replace(" ", string.Empty);

        if (normalizado.Contains(',') && normalizado.Contains('.'))
        {
            // O último separador é considerado o separador decimal.
            var ultimaVirgula = normalizado.LastIndexOf(',');
            var ultimoPonto = normalizado.LastIndexOf('.');
            if (ultimaVirgula > ultimoPonto)
                normalizado = normalizado.Replace(".", string.Empty).Replace(',', '.');
            else
                normalizado = normalizado.Replace(",", string.Empty);
        }
        else if (normalizado.Contains(','))
        {
            normalizado = normalizado.Replace(',', '.');
        }

        return decimal.TryParse(normalizado, NumberStyles.Number, CultureInfo.InvariantCulture, out valor);
    }

    private void AtualizarPropriedadesCalculadas()
    {
        OnPropertyChanged(nameof(Duracao));
        OnPropertyChanged(nameof(DuracaoEmDias));
        OnPropertyChanged(nameof(DuracaoFormatada));
        OnPropertyChanged(nameof(DataInicioFormatada));
        OnPropertyChanged(nameof(DataTerminoFormatada));
        OnPropertyChanged(nameof(NumeroParticipantesFormatado));
        OnPropertyChanged(nameof(CustoPorParticipanteFormatado));
        OnPropertyChanged(nameof(CustoPorParticipanteTextoValido));
        OnPropertyChanged(nameof(CustoTotal));
        OnPropertyChanged(nameof(CustoTotalFormatado));
    }

    private void OnPropertyChanged([CallerMemberName] string? nomePropriedade = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nomePropriedade));
}
