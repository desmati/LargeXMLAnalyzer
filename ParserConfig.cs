namespace LargeXMLAnalyzer;

public class ParserConfig
{
    public long LinesCount { get; set; }

    public string SourceFile { get; set; } = null!;
    public string OutputPath { get; set; } = null!;

    public bool FullDataSample { get; set; } = false;
    public bool GenerateSampleCsv { get; set; } = false;
    public string Delimiter { get; set; } = ",";

    public List<string> Nodes { get; set; } = [];
}
