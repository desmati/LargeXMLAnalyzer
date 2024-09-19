using System.Diagnostics;
using System.Reflection;

namespace LargeXMLAnalyzer;

public class OptionsHandler
{
    public static Options ValidateOptions(Options options)
    {
        if (options == null)
        {
            return new Options()
            {
                ErrorMessage="Invalid options provided.",
                IsValid = false
            };
        }

        options.IsValid = false;

        if (string.IsNullOrEmpty(options.SourceFile) ||
            !File.Exists(options.SourceFile))
        {
            options.ErrorMessage = "Source file not found.";
            return options;
        }

        if (!Directory.Exists(options.OutputPath))
        {
            try
            {
                Directory.CreateDirectory(options.OutputPath);
            }
            catch (Exception ex)
            {
                options.ErrorMessage = "Failed to create output path. "+ ex.Message;
                return options;
            }
        }

        if (options.Nodes.Count == 0)
        {
            options.ErrorMessage = "At least one XML element node should be provided.";
            return options;
        }

        options.IsValid = true;
        return options;
    }

    public static void TakeInitialAction(Options options)
    {

        if (!options.IsValid)
        {
            Console.WriteLine(options.ErrorMessage);
            Environment.Exit(1);
        }

        if (options.ShowVersion)
        {
            Console.WriteLine(Assembly.GetCallingAssembly().GetName().Version);
            Environment.Exit(0);
        }

        if (options.DisplayHelp)
        {
            DisplayHelp();
            Environment.Exit(0);
        }
    }

    public static void HandleOptions(Options options)
    {

        if (!options.IsValid)
        {
            Console.WriteLine(options.ErrorMessage);
            Environment.Exit(1);
        }

        Console.WriteLine("Inspecting XML file size...");
        var linesCount = Helpers.LineCount(options.SourceFile);
        Console.WriteLine($"XML total lines count: {linesCount}");



        var parserConfig = new ParserConfig()
        {
            Delimiter = options.Delimiter,
            FullDataSample = options.FullDataSample,
            GenerateSampleCsv = options.GenerateSampleCsv,
            LinesCount = linesCount,
            SourceFile = options.SourceFile,
            Nodes = options.Nodes,
            OutputPath = options.OutputPath
        };

        XmlParser.Parse(parserConfig);
    }

    static void DisplayHelp()
    {
        Console.WriteLine();
        Console.WriteLine("Usage:");
        Console.WriteLine("  HugeXMLInspector [options]");
        Console.WriteLine();
        Console.WriteLine("Description:");
        Console.WriteLine("  A tool for efficiently parsing massive XML files to extract specified nodes, elements, attributes, and sample data.");
        Console.WriteLine();
        Console.WriteLine("Options:");
        Console.WriteLine("  --input, -i <file>        Specifies the input XML file to process.");
        Console.WriteLine("  --output, -o <path>       Specifies the output directory or file path for the results.");
        Console.WriteLine("  --full, -f                Enables full data sampling instead of a limited sample.");
        Console.WriteLine("  --csv, -c                 Generates a sample CSV file of the extracted data.");
        Console.WriteLine("  --delimiter, -d <value>   Sets the delimiter for CSV output. Options are:");
        Console.WriteLine("                              - \"tab\"   : Use a tab character as the delimiter.");
        Console.WriteLine("                              - \"space\" : Use a space character as the delimiter.");
        Console.WriteLine("                              - \"none\"  : No delimiter.");
        Console.WriteLine("                              - any other character (default is comma ',').");
        Console.WriteLine("  --nodes, -n <nodes>       Specifies the list of node names to extract. List the node names separated by spaces.");
        Console.WriteLine("  --version, -v             Displays the application's version information.");
        Console.WriteLine("  --help, -h                Displays this help menu.");
        Console.WriteLine();
        Console.WriteLine("Examples:");
        Console.WriteLine();
        Console.WriteLine("  1. Extract specific nodes from an XML file:");
        Console.WriteLine();
        Console.WriteLine("     HugeXMLInspector --input largefile.xml --nodes book author title");
        Console.WriteLine();
        Console.WriteLine("  2. Extract nodes with full data sampling and specify output path:");
        Console.WriteLine();
        Console.WriteLine("     HugeXMLInspector -i largefile.xml -o results/ -f -n book author");
        Console.WriteLine();
        Console.WriteLine("  3. Generate a CSV file with tab as the delimiter:");
        Console.WriteLine();
        Console.WriteLine("     HugeXMLInspector -i largefile.xml -c -d tab -n book author");
        Console.WriteLine();
        Console.WriteLine("  4. Use a custom delimiter and extract nodes:");
        Console.WriteLine();
        Console.WriteLine("     HugeXMLInspector -i largefile.xml -c -d \"|\" -n book author");
        Console.WriteLine();
        Console.WriteLine("  5. Display version information:");
        Console.WriteLine();
        Console.WriteLine("     HugeXMLInspector --version");
        Console.WriteLine();
        Console.WriteLine("  6. Display this help menu:");
        Console.WriteLine();
        Console.WriteLine("     HugeXMLInspector --help");
        Console.WriteLine();
        Console.WriteLine("Notes:");
        Console.WriteLine();
        Console.WriteLine("  - Input File (`--input`, `-i`): The path to the large XML file you want to process. This option is required unless specified otherwise.");
        Console.WriteLine("  - Output Path (`--output`, `-o`): The directory or file path where the results will be saved. If not specified, the output will be saved in the current directory.");
        Console.WriteLine("  - Full Data Sampling (`--full`, `-f`): By default, the tool may limit the amount of data sampled for performance reasons. Use this option to process and extract all available data.");
        Console.WriteLine("  - Generate CSV (`--csv`, `-c`): If specified, the tool will generate a CSV file containing the extracted data.");
        Console.WriteLine("  - Delimiter (`--delimiter`, `-d`): Use this option to specify a delimiter for the CSV output. If not set, the default delimiter is a comma ','.");
        Console.WriteLine("  - Nodes (`--nodes`, `-n`): List the XML node names you wish to extract. Provide them as a space-separated list after the `--nodes` or `-n` option. This option should be placed at the end of the command to ensure all nodes are captured.");
        Console.WriteLine("  - Help (`--help`, `-h`): Displays information about command-line options and usage examples.");
        Console.WriteLine();
        Console.WriteLine("Example Explanation:");
        Console.WriteLine();
        Console.WriteLine("  - Example 1: Extracts the nodes `book`, `author`, and `title` from `largefile.xml` using default settings.");
        Console.WriteLine("  - Example 2: Extracts nodes with full data sampling and saves the results to the `results/` directory.");
        Console.WriteLine("  - Example 3: Generates a CSV file with a tab delimiter, extracting the specified nodes.");
        Console.WriteLine("  - Example 4: Uses a custom delimiter `|` (pipe character) for the CSV output.");
        Console.WriteLine("  - Example 5 & 6: Display version information and help menu, respectively.");
        Console.WriteLine();
        Console.WriteLine("Feel free to combine these options to suit your needs. If you encounter any issues or need further assistance, please refer to the documentation or contact support.");
        Console.WriteLine();
    }
}
