# LargeXMLAnalyzer

A tool for efficiently parsing massive XML files to extract specified nodes, elements, attributes, and sample data.

Usage:
  HugeXMLInspector [options]

Description:
  A tool for efficiently parsing massive XML files to extract specified nodes, elements, attributes, and sample data.

Options:
  --input, -i <file>        Specifies the input XML file to process.
  --output, -o <path>       Specifies the output directory or file path for the results.
  --full, -f                Enables full data sampling instead of a limited sample.
  --csv, -c                 Generates a sample CSV file of the extracted data.
  --delimiter, -d <value>   Sets the delimiter for CSV output. Options are:
                              - "tab"   : Use a tab character as the delimiter.
                              - "space" : Use a space character as the delimiter.
                              - "none"  : No delimiter.
                              - any other character (default is comma ',').
  --nodes, -n <nodes>       Specifies the list of node names to extract. List the node names separated by spaces.
  --version, -v             Displays the application's version information.
  --help, -h                Displays this help menu.

Examples:

  1. **Extract specific nodes from an XML file:**

     ```
     HugeXMLInspector --input largefile.xml --nodes book author title
     ```

  2. **Extract nodes with full data sampling and specify output path:**

     ```
     HugeXMLInspector -i largefile.xml -o results/ -f -n book author
     ```

  3. **Generate a CSV file with tab as the delimiter:**

     ```
     HugeXMLInspector -i largefile.xml -c -d tab -n book author
     ```

  4. **Use a custom delimiter and extract nodes:**

     ```
     HugeXMLInspector -i largefile.xml -c -d "|" -n book author
     ```

  5. **Display version information:**

     ```
     HugeXMLInspector --version
     ```

  6. **Display this help menu:**

     ```
     HugeXMLInspector --help
     ```

Notes:

- **Input File (`--input`, `-i`):** The path to the large XML file you want to process. This option is required unless specified otherwise.
- **Output Path (`--output`, `-o`):** The directory or file path where the results will be saved. If not specified, the output will be saved in the current directory.
- **Full Data Sampling (`--full`, `-f`):** By default, the tool may limit the amount of data sampled for performance reasons. Use this option to process and extract all available data.
- **Generate CSV (`--csv`, `-c`):** If specified, the tool will generate a CSV file containing the extracted data.
- **Delimiter (`--delimiter`, `-d`):** Use this option to specify a delimiter for the CSV output. If not set, the default delimiter is a comma `,`.
- **Nodes (`--nodes`, `-n`):** List the XML node names you wish to extract. Provide them as a space-separated list after the `--nodes` or `-n` option. This option should be placed at the end of the command to ensure all nodes are captured.
- **Help (`--help`, `-h`):** Displays information about command-line options and usage examples.

**Example Explanation:**

- **Example 1:** Extracts the nodes `book`, `author`, and `title` from `largefile.xml` using default settings.
- **Example 2:** Extracts nodes with full data sampling and saves the results to the `results/` directory.
- **Example 3:** Generates a CSV file with a tab delimiter, extracting the specified nodes.
- **Example 4:** Uses a custom delimiter `|` (pipe character) for the CSV output.
- **Example 5 & 6:** Display version information and help menu, respectively.

Feel free to combine these options to suit your needs. If you encounter any issues or need further assistance, please open an issue.
