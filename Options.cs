namespace LargeXMLAnalyzer;

public class Options
{
    public string SourceFile { get; set; } = null!;
    public string OutputPath { get; set; } = null!;

    public bool FullDataSample { get; set; } = false;
    public bool ShowVersion { get; set; } = false;
    public bool DisplayHelp { get; set; } = false;

    public bool GenerateSampleCsv { get; set; } = false;
    public int SampleCsvRows { get; set; } = 100;
    public string Delimiter { get; set; } = ",";

    public List<string> Nodes { get; set; } = [];

    public bool IsValid { get; set; } = true;
    public string? ErrorMessage { get; set; }
}
