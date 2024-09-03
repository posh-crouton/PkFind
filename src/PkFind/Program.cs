namespace Posh.PkFind;

class Program
{
    public static void Main(string[] args)
    {
        ValidateArgs.ValidateProgramArgs(args);

        bool isVerbose = args.Contains("-v") || args.Contains("--verbose");

        string jsonString = File.ReadAllText(args[0]);
        var objects = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(jsonString);

        if (objects is null || objects.Count == 0)
        {
            Console.WriteLine("JSON file contains no objects");
            Environment.Exit(1);
        }

        var allKeys = objects.SelectMany(o => o.Keys).Distinct();
        Dictionary<string, List<string>> keyValues = new();
        foreach (var key in allKeys) keyValues.Add(key, []);

        foreach (var o in objects)
        {
            foreach (var kvp in o)
            {
                keyValues[kvp.Key].Add(kvp.Value);
            }
        }

        List<string> eligibleKeys = [];
        foreach (var kvp in keyValues)
        {
            var nUnique = kvp.Value.Distinct().Count();
            var percentUnique = nUnique / objects.Count;
            if (percentUnique == 1) eligibleKeys.Add(kvp.Key);
            if (isVerbose) Console.WriteLine($"{kvp.Key} - {nUnique} unique ({percentUnique}/1)");
        }

        if (eligibleKeys.Any())
        {
            Console.WriteLine(string.Join('\n', eligibleKeys));
            Environment.Exit(0);
        }
        else
        {
            Console.WriteLine("No single column is eligible to be the primary key.");
            Environment.Exit(1);
        }
    }
}
