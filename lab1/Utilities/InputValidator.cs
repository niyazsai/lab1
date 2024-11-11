namespace lab1;

public static class InputValidator
{
    public static int GetValidatedInt(string prompt, int min, int max)
    {
        int result;
        do
        {
            Console.Write(prompt);
            var input = Console.ReadLine();
            if (int.TryParse(input, out result) && result >= min && result <= max)
            {
                break;
            }
            Console.WriteLine("Некорректный ввод. Пожалуйста, попробуйте снова.");
        } while (true);

        return result;
    }

    public static string GetNonEmptyString(string prompt)
    {
        string input;
        do
        {
            Console.Write(prompt);
            input = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(input))
            {
                break;
            }
            Console.WriteLine("Строка не должна быть пустой. Пожалуйста, попробуйте снова.");
        } while (true);

        return input;
    }
}
