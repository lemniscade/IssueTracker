using IssueManager.IssueManager.Application.Interceptor;
using Spectre.Console;
using static IssueManager.IssueManager.Presentation.Commands.UserCommand;

namespace IssueManager
{
    class Program
    {
        static void Main(string[] args)
        {
            ConsoleApp calistir = new();
            calistir.Baslat();
        }
    }

    class ConsoleApp
    {
        public void Baslat()
        {
            AnsiConsole.Write(new FigletText("Issue Tracker").Centered().Color(Color.Yellow));
            AnsiConsole.MarkupLine("[grey]v1.0.0 - Spectre.Console örnek uygulama[/]");
            string Username;
            String Password;

            while (true)
            {
                var secim = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("\n[green]Bir işlem seçin:[/]")
                        .PageSize(5)
                        .AddChoices(new[] {
                        "1. Register",
                        "2. Login",
                        "3. Add Project",
                        "4. Update Project",
                        "5. Add Issue",
                        "6. Edit Issue"
                        }));

                switch (secim)
                {
                    case "1. Register":
                        Username=Console.ReadLine();
                        Password = Console.ReadLine();
                        UserOperations.Register(Username, Password);
                        break;
                    case "2. Spinner ile Yükleniyor Efekti":
                        SpinnerEfekti();
                        break;
                    case "3. İlerleme Çubuğu Göster":
                        ProgressBar();
                        break;
                    case "4. Yazı Yazdır (Renkli)":
                        RenkliYazi();
                        break;
                    case "5. Çıkış":
                        AnsiConsole.MarkupLine("[bold red]Çıkılıyor...[/]");
                        return;
                }
            }
        }

        void KullaniciTablosu()
        {
            var table = new Table();
            table.AddColumn("ID");
            table.AddColumn("Ad");
            table.AddColumn("Şehir");

            table.AddRow("1", "Ayşe", "İzmir");
            table.AddRow("2", "Mehmet", "Ankara");
            table.AddRow("3", "Can", "Bursa");

            AnsiConsole.Write(table);
        }

        void SpinnerEfekti()
        {
            AnsiConsole.Status()
                .Start("Veriler yükleniyor...", ctx =>
                {
                    Thread.Sleep(2000);
                    ctx.Status("Yükleme tamamlandı");
                    Thread.Sleep(1000);
                });
        }

        void ProgressBar()
        {
            AnsiConsole.Progress()
                .Start(ctx =>
                {
                    var task = ctx.AddTask("[green]İşleniyor...[/]");

                    while (!task.IsFinished)
                    {
                        task.Increment(5);
                        Thread.Sleep(300);
                    }
                });
        }

        void RenkliYazi()
        {
            var yazi = AnsiConsole.Ask<string>("Hangi [yellow]yazıyı[/] yazmak istiyorsun?");
            var renk = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Bir [green]renk[/] seç:")
                    .AddChoices("red", "green", "blue", "yellow", "purple", "cyan"));

            AnsiConsole.MarkupLine($"[{renk}]{yazi}[/]");
        }
    }

}
