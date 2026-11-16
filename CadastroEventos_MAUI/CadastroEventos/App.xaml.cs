namespace CadastroEventos;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        MainPage = new NavigationPage(new Pages.CadastroEventoPage())
        {
            BarBackgroundColor = Color.FromArgb("#17324D"),
            BarTextColor = Colors.White
        };
    }
}
