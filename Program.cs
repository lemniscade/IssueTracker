using IssueTracker.Business.UI;

namespace IssueTracker
{
    public class Program
    {
        static void Main(string[] args)
        {
            ConsoleUI consoleUI = new ConsoleUI();
            consoleUI.Execute();
            Console.WriteLine("Hello, World!");
        }
    }
}
