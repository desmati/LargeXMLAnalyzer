namespace LargeXMLAnalyzer;

public class Helpers
{
    public static long LineCount(string filePath)
    {
        long lineCount = 0;
        int bufferSize = 1024 * 1024 * 1024; // 1GB buffer

        using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize))
        using (var reader = new StreamReader(fs))
        {
            while (reader.ReadLine() != null)
            {
                lineCount++;
            }
        }

        return lineCount;
    }
}
