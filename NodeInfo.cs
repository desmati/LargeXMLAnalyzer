namespace LargeXMLAnalyzer;

public class NodeInfo
{
    public string Name { get; set; } = null!;
    public string Path { get; set; } = null!;
    public string Text { get; set; } = null!;
    public bool IsLeaf { get; set; } = false;
    public bool IsEmpty { get; set; } = false;
    public bool HasText { get; set; } = false;
    public List<long> LineNumbers { get; set; } = new List<long>();
    public List<NodeData> Data { get; set; } = new List<NodeData>();
    public List<KeyValuePair<string, string>> Attributes { get; set; } = new List<KeyValuePair<string, string>>();
}