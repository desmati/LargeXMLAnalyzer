# HugeXMLInspector

**A tool for efficiently parsing massive XML files to extract specified nodes, elements, attributes, and sample data.**

## Table of Contents

- [Introduction](#introduction)
- [Features](#features)
- [Installation](#installation)
- [Usage](#usage)
  - [Command-Line Options](#command-line-options)
  - [Examples](#examples)
- [To-Do](#to-do)
- [Contributing](#contributing)
- [License](#license)

## Introduction

HugeXMLInspector is a console application designed to efficiently parse extremely large XML files (50 GB and above). It allows users to extract specified nodes, along with their elements, attributes, and sample data, without loading the entire XML file into memory.

## Features

- Efficient parsing of massive XML files.
- Extraction of specified nodes and their associated data.
- Supports full data sampling or limited samples for performance.
- Customizable output options, including specifying output paths.
- **Upcoming Feature:** CSV generation of extracted data (see [To-Do](#to-do)).

## Installation

1. **Prerequisites:**

   - [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) or later installed on your system.

2. **Clone the Repository:**

   ```bash
   git clone https://github.com/desmati/LargeXMLAnalyzer.git
   ```

3. **Build the Application:**

   Navigate to the project directory and build the application:

   ```bash
   cd HugeXMLInspector
   dotnet build
   ```

## Usage

Run the application using the `dotnet run` command followed by the desired options:

```bash
dotnet run -- [options]
```

### Command-Line Options

- `--input`, `-i <file>`  
  Specifies the input XML file to process.

- `--output`, `-o <path>`  
  Specifies the output directory or file path for the results.

- `--full`, `-f`  
  Enables full data sampling instead of a limited sample.

- `--csv`, `-c`  
  Generates a sample CSV file of the extracted data. **(Coming Soon)**

- `--delimiter`, `-d <value>`  
  Sets the delimiter for CSV output. Options are:
  - `"tab"`   : Use a tab character as the delimiter.
  - `"space"` : Use a space character as the delimiter.
  - `"none"`  : No delimiter.
  - Any other character (default is comma `,`).

- `--nodes`, `-n <nodes>`  
  Specifies the list of node names to extract. List the node names separated by spaces.

- `--version`, `-v`  
  Displays the application's version information.

- `--help`, `-h`  
  Displays the help menu.

### Examples

1. **Extract specific nodes from an XML file:**

   ```bash
   dotnet run -- --input largefile.xml --nodes book author title
   ```

2. **Extract nodes with full data sampling and specify output path:**

   ```bash
   dotnet run -- -i largefile.xml -o results/ -f -n book author
   ```

3. **Generate a CSV file with tab as the delimiter:**

   ```bash
   dotnet run -- -i largefile.xml -c -d tab -n book author
   ```

4. **Use a custom delimiter and extract nodes:**

   ```bash
   dotnet run -- -i largefile.xml -c -d "|" -n book author
   ```

5. **Display version information:**

   ```bash
   dotnet run -- --version
   ```

6. **Display the help menu:**

   ```bash
   dotnet run -- --help
   ```

## To-Do

- [ ] **Implement `--csv` Option:** Enable the generation of CSV files containing the extracted data.

## Contributing

Contributions are welcome! Please follow these steps:

1. Fork the repository.
2. Create a new branch for your feature or bugfix.
3. Commit your changes with clear messages.
4. Open a pull request describing your changes.

## License

This project is licensed under the [Apache License v2](LICENSE).









