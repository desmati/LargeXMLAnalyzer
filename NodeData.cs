namespace LargeXMLAnalyzer;

public class NodeData
{
    public string Name { get; set; } = null!;
    public string Path { get; set; } = null!;
    public string Text { get; set; } = null!;
    public List<long> LineNumbers { get; set; } = new List<long>();
    public List<KeyValuePair<string, string>> Attributes { get; set; } = new List<KeyValuePair<string, string>>();
}