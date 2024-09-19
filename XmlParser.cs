using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace LargeXMLAnalyzer;

public class XmlParser
{
    static XmlReaderSettings XmlSettings = new XmlReaderSettings
    {
        IgnoreWhitespace = true,
        DtdProcessing = DtdProcessing.Ignore
    };

    public static void Parse(ParserConfig config)
    {
        foreach (var node in config.Nodes)
        {
            Console.WriteLine($"Current Node: {node}");
            GetInfo(node, config);
        }
    }

    public static void GetInfo(string node, ParserConfig config)
    {
        var start = DateTime.Now;

        var consolePosition = Console.GetCursorPosition();
        var paths = new List<NodeInfo>();

        using (var reader = XmlReader.Create(config.SourceFile , XmlSettings))
        {
            var pathStack = new Stack<string>();
            IXmlLineInfo lineInfo = reader as IXmlLineInfo;

            var logInterval = new Random().Next(3, 10);

            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    pathStack.Push(reader.LocalName);

                    if (reader.LocalName.Equals(node, StringComparison.InvariantCultureIgnoreCase))
                    {
                        var path = GetPath(pathStack);
                        var name = reader.LocalName;
                        var exists = paths.Any(x => x.Name == name && x.Path == path);
                        var isEmpty = reader.IsEmptyElement;

                        var info = exists
                             ? paths.First(x => x.Name == name && x.Path == path)
                             : new NodeInfo()
                             {
                                 Name = reader.LocalName,
                                 LineNumbers = new List<long>(),
                                 Attributes = new List<KeyValuePair<string, string>>(),
                                 Path = path,
                                 IsLeaf = false,
                                 IsEmpty = isEmpty,
                                 HasText = false,
                                 Text = "",
                                 Data = new List<NodeData>()
                             };

                        if (lineInfo != null && lineInfo.HasLineInfo())
                        {
                            info.LineNumbers.Add(lineInfo.LineNumber);
                        }

                        if (reader.HasAttributes)
                        {
                            for (int i = 0; i < reader.AttributeCount; i++)
                            {
                                reader.MoveToAttribute(i);
                                info.Attributes.Add(new KeyValuePair<string, string>(reader.LocalName, reader.Value));
                            }

                            reader.MoveToElement(); // Move back to the element node
                        }

                        var elapsed = DateTime.Now - start;
                        if (elapsed.Seconds % logInterval == 0)
                        {
                            logInterval = new Random().Next(3, 10);

                            var progress = Math.Floor((lineInfo?.LineNumber ?? 0d) * 100 / config.LinesCount );

                            var remained = TimeSpan.FromSeconds(elapsed.TotalSeconds * 100 / (progress + 1));

                            Console.SetCursorPosition(consolePosition.Left, consolePosition.Top);
                            Console.WriteLine($"Progress: {progress}%");
                            Console.WriteLine($"Current line: {lineInfo?.LineNumber}");

                            // Get the current process
                            Process currentProcess = Process.GetCurrentProcess();

                            // Get the physical memory usage (Working Set)
                            long workingSet = currentProcess.WorkingSet64;

                            // Get the private memory usage
                            long privateMemory = currentProcess.PrivateMemorySize64;

                            // Get the virtual memory usage
                            long virtualMemory = currentProcess.VirtualMemorySize64;

                            // Convert bytes to megabytes
                            double workingSetMB = workingSet / (1024.0 * 1024.0);
                            double privateMemoryMB = privateMemory / (1024.0 * 1024.0);
                            double virtualMemoryMB = virtualMemory / (1024.0 * 1024.0);
                            Console.WriteLine($"Memory Usage: {workingSetMB:F2} MB PWS, {privateMemoryMB:F2} MB PMU, {virtualMemoryMB:F2} MB VMU");

                            Console.WriteLine($"Elapsed: {elapsed.ToString().Substring(0, 8)}                                 ");
                            Console.WriteLine($"Remained: {remained.ToString().Substring(0, 8)}                                 ");
                            Console.WriteLine();
                        }

                        if (isEmpty)
                        {
                            // Empty element, process accordingly
                            info.IsLeaf = true;
                            if (!exists)
                            {
                                paths.Add(info);
                                Console.SetCursorPosition(consolePosition.Left, consolePosition.Top);
                                Console.WriteLine($"Progress: {(Math.Floor((lineInfo?.LineNumber ?? 0d) * 100 / config.LinesCount))}%");
                                Console.WriteLine($"Current line: {lineInfo?.LineNumber}");
                            }
                            pathStack.Pop(); // Pop since we pushed earlier
                            continue;
                        }
                        else
                        {
                            // Not empty, process its content
                            info.IsLeaf = false;
                            ProcessSubtree(reader, pathStack, info, node);
                            if (!exists)
                            {
                                paths.Add(info);
                                Console.SetCursorPosition(consolePosition.Left, consolePosition.Top);
                                Console.WriteLine($"Progress: {(Math.Floor((lineInfo?.LineNumber ?? 0d) * 100 / config.LinesCount))}%");
                                Console.WriteLine($"Current line: {lineInfo?.LineNumber}");
                            }
                            pathStack.Pop(); // Pop after processing
                        }
                    }
                    else if (reader.IsEmptyElement)
                    {
                        // Pop the element from the stack if it's empty and not the node name
                        pathStack.Pop();
                    }
                }
                else if (reader.NodeType == XmlNodeType.EndElement)
                {
                    // Pop the element from the stack
                    if (pathStack.Count > 0)
                    {
                        pathStack.Pop();
                    }
                }
            }
        }

        Console.SetCursorPosition(consolePosition.Left, consolePosition.Top);
        Console.WriteLine($"Progress: 100%                                                ");
        Console.WriteLine($"Current line: {config.LinesCount}                                    ");
        Console.WriteLine($"Memory Usage: -                                               ");
        Console.WriteLine($"Elapsed: -                                                    ");
        Console.WriteLine($"Remained: -                                                   ");
        Console.WriteLine();

        PrintInfo(paths, Path.GetFileName(config.SourceFile), config.OutputPath, node, config.FullDataSample);
    }

    public static void ProcessSubtree(XmlReader reader, Stack<string> pathStack, NodeInfo info, string node)
    {
        string elementName = reader.LocalName;
        bool isEmpty = reader.IsEmptyElement;
        var path = GetRelativePath(pathStack, node);
        var exists = info.Data.Any(x => x.Name == elementName && x.Path == path);

        // Push the current element onto the stack
        // Since we are processing child elements, we need to manage their names
        // Note: We already pushed the node's name in Main, so we don't need to push it again here
        // But for other elements, we need to push their names onto the stack
        // Since we called reader.Read() in Main to move to the child element, we need to adjust here

        var data = exists
            ? info.Data.First(x => x.Name == elementName && x.Path == path)
            : new NodeData()
            {
                Attributes = new List<KeyValuePair<string, string>>(),
                LineNumbers = new List<long>(),
                Name = elementName,
                Path = path,
                Text = ""
            };

        IXmlLineInfo lineInfo = reader as IXmlLineInfo;
        if (lineInfo != null && lineInfo.HasLineInfo())
        {
            data.LineNumbers.Add(lineInfo.LineNumber);
        }

        if (reader.HasAttributes)
        {
            for (int i = 0; i < reader.AttributeCount; i++)
            {
                reader.MoveToAttribute(i);
                data.Attributes.Add(new KeyValuePair<string, string>(reader.LocalName, reader.Value));
            }
            reader.MoveToElement();
        }

        if (isEmpty)
        {
            // Empty element, process as leaf node
            if (!exists)
            {
                info.Data.Add(data);
            }
            pathStack.Pop(); // Pop since we pushed earlier in Main
            return; // Since it's empty, we can return
        }
        else
        {
            // Not empty, process content
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Text || reader.NodeType == XmlNodeType.CDATA)
                {
                    if (!data.Text.Contains(reader.Value + ","))
                    {
                        data.Text += reader.Value + ",";
                    }
                }
                else if (reader.NodeType == XmlNodeType.Element)
                {
                    pathStack.Push(reader.LocalName); // Push child element onto the stack
                                                      // Nested element, process recursively
                    ProcessSubtree(reader, pathStack, info, node);
                    // After processing, the child element would have been popped
                }
                else if (reader.NodeType == XmlNodeType.EndElement)
                {
                    if (reader.LocalName.Equals(elementName, StringComparison.InvariantCultureIgnoreCase))
                    {
                        // End of current element
                        break;
                    }
                    else
                    {
                        // Pop the child element from the stack
                        if (pathStack.Count > 0)
                        {
                            pathStack.Pop();
                        }
                    }
                }
            }

            if (!exists)
            {
                info.Data.Add(data);
            }

            pathStack.Pop(); // Pop the current element
        }
    }

    public static string GetPath(Stack<string> pathStack)
    {
        // Construct the full path from the stack
        var array = pathStack.ToArray();
        Array.Reverse(array);
        return "/" + string.Join("/", array);
    }

    public static string GetRelativePath(Stack<string> pathStack, string node)
    {
        // Get the path relative to the node name
        var array = pathStack.ToArray();
        Array.Reverse(array);
        int index = Array.IndexOf(array, node);

        if (index >= 0 && index < array.Length - 1)
        {
            var relativeArray = new string[array.Length - index - 1];
            Array.Copy(array, index + 1, relativeArray, 0, relativeArray.Length);
            return string.Join("/", relativeArray);
        }
        else
        {
            return ""; // the node is not in the stack or it's the last element
        }
    }

    public static void PrintInfo(List<NodeInfo> paths, string origin, string outputPath, string node, bool fullSamples)
    {
        paths = paths.Where(x => !string.IsNullOrEmpty(x.Name.Trim())).OrderBy(x => x.Path).ToList();
        paths.ForEach(x =>
        {
            x.Attributes = x.Attributes.Where(h => !string.IsNullOrEmpty(h.Key.Trim())).OrderBy(y => y.Key).ToList();
            x.Data = x.Data.Where(dd => !string.IsNullOrEmpty(dd.Name.Trim())).OrderBy(z => z.Path).ToList();
            x.Data.ForEach(d =>
            {
                d.Attributes = d.Attributes.Where(aa => !string.IsNullOrEmpty(aa.Key.Trim())).OrderBy(a => a.Key).ToList();
            });
        });

        foreach (var info in paths)
        {
            var fileName = info.Path.Replace("/", "__") + ".txt";
            var directory = Path.Combine(outputPath, Path.GetFileNameWithoutExtension(origin), node);
            Directory.CreateDirectory(directory);
            var path = Path.Combine(directory, fileName);

            File.WriteAllText(path, $"Element Name: {info.Name}");
            File.AppendAllText(path, Environment.NewLine);

            File.AppendAllText(path, $"Path: {info.Path}");
            File.AppendAllText(path, Environment.NewLine);

            File.AppendAllText(path, $"Total Occurances: {info.LineNumbers.Count}");
            File.AppendAllText(path, Environment.NewLine);

            File.AppendAllText(path, $"Sample occurances' line number: {string.Join(", ", info.LineNumbers.Take(10))}");
            File.AppendAllText(path, Environment.NewLine);

            if (info.Attributes.Count > 0)
            {
                File.AppendAllText(path, "Attributes count:" + info.Attributes.Count);
                File.AppendAllText(path, Environment.NewLine);

                File.AppendAllText(path, "Attributes:");
                File.AppendAllText(path, Environment.NewLine);

                foreach (var attr in info.Attributes)
                {
                    File.AppendAllText(path, $"\t{attr.Key} = {attr.Value}");
                    File.AppendAllText(path, Environment.NewLine);

                }
            }

            if (info.Data.Count > 0)
            {
                File.AppendAllText(path, "Nested Elements Count:" + info.Data.Count);
                File.AppendAllText(path, Environment.NewLine);

                File.AppendAllText(path, "Nested Elements' Samples:");
                File.AppendAllText(path, Environment.NewLine);

                foreach (var data in info.Data)
                {
                    var sample = fullSamples
                        ? data.Text
                        : GetSample(data.Text, 10);

                    File.AppendAllText(path, $"\t{data.Path} = {sample}");
                    File.AppendAllText(path, Environment.NewLine);

                    if (data.Attributes.Count > 0)
                    {
                        File.AppendAllText(path, "Nested Elements' Attributes Count:" + data.Attributes.Count);
                        File.AppendAllText(path, Environment.NewLine);

                        File.AppendAllText(path, "Nested Elements' Attributes Sample:");
                        File.AppendAllText(path, Environment.NewLine);

                        foreach (var attr in data.Attributes)
                        {
                            File.AppendAllText(path, $"\t\t{attr.Key} = {attr.Value}");
                            File.AppendAllText(path, Environment.NewLine);
                        }
                    }
                }
            }
        }
    }

    public static string GetSample(string str, int nth)
    {
        if (string.IsNullOrEmpty(str) || str.Length <= 50)
        {
            return str;
        }

        var ch = ',';

        var startIndex = str.IndexOf(ch) + 1;

        var idx = str.IndexOf(ch, startIndex);

        while (idx >= 0 && --nth > 0)
        {
            if (startIndex + idx + 1 >= str.Length)
            {
                if (str.Length <= 100)
                {
                    return str;
                }

                return str.Substring(0, 100);
            }
            idx = str.IndexOf(ch, startIndex + idx + 1);
        }

        return str.Substring(0, idx);
    }
}
