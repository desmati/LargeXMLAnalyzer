namespace LargeXMLAnalyzer;

public class ArgumentHandler
{
    public static Options HandleArguments(string[] args)
    {
        var result = new Options();

        for (int i = 0; i < args.Length; i++)
        {
            var arg = args[i]?.ToLower()?.Trim()
                ?.TrimStart('-', '/', '\\');

            switch (arg)
            {
                case "input":
                case "i":
                    if (i + 1 < args.Length)
                    {
                        result.SourceFile = args[++i];
                    }
                    else
                    {
                        result.ErrorMessage="Error: Missing value for --input";
                        result.IsValid=false;
                        return result;
                    }
                    break;

                case "output":
                case "o":
                    if (i + 1 < args.Length)
                    {
                        result.OutputPath = args[++i];
                    }
                    else
                    {
                        result.ErrorMessage="Error: Missing value for --output";
                        result.IsValid=false;
                        return result;
                    }
                    break;

                case "full":
                case "f":
                    result.FullDataSample = true;
                    break;

                case "csv":
                case "c":
                    result.GenerateSampleCsv = true;
                    if (i + 1 < args.Length)
                    {
                        if (int.TryParse(args[++i], out var csvRows))
                        {
                            result.SampleCsvRows = csvRows;
                        }
                    }
                    break;

                case "version":
                case "v":
                    result.ShowVersion = true;
                    break;

                case "help":
                case "h":
                    result.DisplayHelp = true;
                    break;

                case "nodes":
                case "n":
                    i++;
                    while (i < args.Length && !args[i].StartsWith("-"))
                    {
                        result.Nodes.Add(args[i]);
                        i++;
                    }
                    i--; // Adjust for extra increment at the end
                    break;

                case "delimiter":
                case "d":
                    if (i + 1 < args.Length)
                    {
                        var argValue = args[++i]?.ToLower()?.Trim();
                        result.Delimiter = argValue switch
                        {
                            "tab" => "\t",
                            "space" => " ",
                            _ => argValue ?? ","
                        };
                    }
                    else
                    {
                        result.ErrorMessage="Error: Missing value for --delimiter";
                        result.IsValid=false;
                        return result;
                    }
                    break;

                default:
                    result.ErrorMessage=$"Warning: Unknown argument '{args[i]}'";
                    result.IsValid=false;
                    return result;
            }
        }

        return result;
    }
}
