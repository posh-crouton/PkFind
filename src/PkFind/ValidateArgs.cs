namespace Posh.PkFind;

internal static class ValidateArgs
{
    public static void ValidateProgramArgs(string[] args)
    {
        if (args.Length == 0 || string.IsNullOrWhiteSpace(args[0]))
        {
            Console.WriteLine("Missing required argument: File");
            Environment.Exit(1);
        }

        if (!File.Exists(args[0]))
        {
            Console.WriteLine($"{args[0]} is not a file.");
            Environment.Exit(1);
        }

    }
}
