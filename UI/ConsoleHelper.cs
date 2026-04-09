namespace TransportSystem.UI;

public static class ConsoleHelper
{
    public static void ShowSuccess(string message)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"\n✓ {message}");
        Console.ResetColor();
    }

    public static void ShowError(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"\n✗ {message}");
        Console.ResetColor();
    }

    public static void ShowTitle(string title)
    {
        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine($"--- {title} ---");
        Console.ResetColor();
    }

    public static string ReadRequiredString(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            var input = Console.ReadLine()?.Trim();
            if (!string.IsNullOrWhiteSpace(input))
                return input;
            ShowError("Este campo es obligatorio.");
        }
    }

    public static int ReadInt(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            if (int.TryParse(Console.ReadLine(), out var value))
                return value;
            ShowError("Ingresa un número entero válido.");
        }
    }

    public static double ReadDouble(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            if (double.TryParse(Console.ReadLine(), out var value) && value > 0)
                return value;
            ShowError("Ingresa un número válido mayor a cero.");
        }
    }

    public static void Pause()
    {
        Console.WriteLine("\nPresiona Enter para continuar...");
        Console.ReadLine();
    }
}