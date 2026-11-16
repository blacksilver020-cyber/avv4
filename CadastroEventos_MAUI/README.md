# Cadastro de Eventos - .NET MAUI

Projeto acadêmico de cadastro de eventos desenvolvido em .NET MAUI (Android), com uma tela de formulário e uma página de resumo.

## Requisitos implementados

- Cadastro do nome do evento, datas de início e término, participantes, local e custo por participante.
- `BindingContext` da página de cadastro associado diretamente à model `Evento`.
- Model `Evento` com `INotifyPropertyChanged` para atualizar os cálculos enquanto os campos são editados.
- Cálculo da duração com `DateTime` e `TimeSpan` (`DataTermino.Date - DataInicio.Date`).
- Cálculo do custo total (`NumeroParticipantes * CustoPorParticipante`).
- Navegação para a página `ResumoEventoPage`, com todos os dados formatados e o custo total.
- Validação de campos obrigatórios e da ordem das datas.

## Estrutura

- `CadastroEventos/Models/Evento.cs`: model e cálculos.
- `CadastroEventos/Pages/CadastroEventoPage.xaml`: formulário.
- `CadastroEventos/Pages/CadastroEventoPage.xaml.cs`: BindingContext, validação e navegação.
- `CadastroEventos/Pages/ResumoEventoPage.xaml`: resumo formatado.
- `CadastroEventos/Pages/ResumoEventoPage.xaml.cs`: recebe a mesma instância da model Evento.
- `CadastroEventos/CadastroEventos.csproj`: projeto .NET MAUI para Android.

## Abrir e executar

O projeto requer .NET 10 SDK com workload MAUI/Android instalado.

Pelo terminal, na pasta que contém `CadastroEventos.sln`:

```bash
dotnet workload install maui-android
dotnet restore CadastroEventos.sln
dotnet build CadastroEventos.sln -t:Run -f net10.0-android
```

Também pode abrir `CadastroEventos.sln` no Visual Studio com a carga de trabalho .NET MAUI instalada.

## Exemplo para conferir os cálculos

- Nome: Feira de Tecnologia 2026
- Início: 10/12/2026
- Término: 12/12/2026
- Participantes: 150
- Local: Centro de Convenções - São Paulo
- Custo por participante: R$ 85,00
- Duração: 2 dias (diferença entre as datas)
- Custo total: R$ 12.750,00

## PDF de documentação

O arquivo `Documentacao/Previa_Visual_Cadastro_Eventos.pdf` apresenta prévias ilustrativas das duas telas. Elas representam o layout projetado; não são capturas de uma execução do aplicativo. Para cumprir literalmente a parte de capturas de funcionamento, é necessário executar o projeto em um dispositivo/emulador Android e substituir as prévias pelas capturas reais.
