using DGFSharp;
using System;
using System.Collections.Generic;
using System.IO;

/// <summary>
/// Remove DGF passwords namespace
/// </summary>
namespace RemoveDGFPasswords
{
    /// <summary>
    /// Program class
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// Input flags
        /// </summary>
        private static readonly IReadOnlyDictionary<string, InputFlag> inputFlags = new Dictionary<string, InputFlag>
        {
            { "-help", new InputFlag("Help", "This option shows help topics", EInputFlag.ShowHelp) },
            { "-verbose", new InputFlag("Verbose", "This option enables verbose printing.", EInputFlag.Verbose) },
            { "-output", new InputFlag("Output path", "This option sets the output path for the previously specified file.", EInputFlag.SpecifyOutputPath) },
            { "-output-directory", new InputFlag("Output directory", "This options defines a directory where output is generated.", EInputFlag.SpecifyOutputDirectoryPath) }
        };

        /// <summary>
        /// Input flag aliases
        /// </summary>
        private static readonly IReadOnlyDictionary<string, string> inputFlagAliases = new Dictionary<string, string>
        {
            { "h", "-help" },
            { "v", "-verbose" },
            { "o", "-output" },
            { "od", "-output-directory" }
        };

        /// <summary>
        /// Print input flag error
        /// </summary>
        /// <param name="inputFlag">Input state</param>
        private static void PrintInputFlagError(EInputFlag inputFlag)
        {
            switch (inputFlag)
            {
                case EInputFlag.SpecifyOutputPath:
                    Console.Error.WriteLine("Empty output flag");
                    break;
                case EInputFlag.SpecifyOutputDirectoryPath:
                    Console.Error.WriteLine("Empty output directory flag");
                    break;
            }
        }

        /// <summary>
        /// Print input flags
        /// </summary>
        private static void PrintInputFlags()
        {
            Dictionary<string, List<string>> input_flags = new Dictionary<string, List<string>>();
            foreach (string key in inputFlags.Keys)
            {
                input_flags.Add(key, new List<string> { key });
            }
            foreach (KeyValuePair<string, string> alias in inputFlagAliases)
            {
                input_flags[alias.Value].Add(alias.Key);
            }
            Console.WriteLine();
            Console.WriteLine("Available options:");
            Console.WriteLine();
            foreach (KeyValuePair<string, List<string>> input_flag in input_flags)
            {
                bool first = true;
                foreach (string flag in input_flag.Value)
                {
                    if (first)
                    {
                        first = false;
                    }
                    else
                    {
                        Console.Write(", ");
                    }
                    Console.Write("-");
                    Console.Write(flag);
                }
                Console.WriteLine();
                Console.Write("\t");
                Console.WriteLine(inputFlags[input_flag.Key].Description);
                Console.WriteLine();
                input_flag.Value.Clear();
            }
            input_flags.Clear();
        }

        /// <summary>
        /// Main entry point
        /// </summary>
        /// <param name="args">Command line arguments</param>
        private static void Main(string[] args)
        {
            Dictionary<string, string> files = new Dictionary<string, string>();
            string output_directory = string.Empty;
            bool show_help = false;
            bool verbose = false;
            EInputFlag current_input_flag = EInputFlag.Nothing;
            string last_path = null;
            string help_topic = null;
            foreach (string arg in args)
            {
                string trimmed_arg = arg.Trim().ToLower();
                if (trimmed_arg.Length > 0)
                {
                    if (trimmed_arg[0] == '-')
                    {
                        string key = ((trimmed_arg.Length > 1) ? trimmed_arg.Substring(1) : string.Empty);
                        InputFlag? input_flag = null;
                        if (inputFlags.ContainsKey(key))
                        {
                            input_flag = inputFlags[key];
                        }
                        else if (inputFlagAliases.ContainsKey(key))
                        {
                            input_flag = inputFlags[inputFlagAliases[key]];
                        }
                        if (input_flag == null)
                        {
                            Console.Error.Write("Invalid input flag \"");
                            Console.Error.Write(arg);
                            Console.Error.WriteLine("\"");
                        }
                        else
                        {
                            PrintInputFlagError(current_input_flag);
                            current_input_flag = input_flag.Value.Flag;
                            switch (current_input_flag)
                            {
                                case EInputFlag.ShowHelp:
                                    show_help = true;
                                    break;
                                case EInputFlag.Verbose:
                                    verbose = true;
                                    current_input_flag = EInputFlag.Nothing;
                                    break;
                            }
                        }
                    }
                    else
                    {
                        switch (current_input_flag)
                        {
                            case EInputFlag.Nothing:
                                if (!(files.ContainsKey(arg)))
                                {
                                    files.Add(arg, arg);
                                }
                                last_path = arg;
                                break;
                            case EInputFlag.ShowHelp:
                                help_topic = trimmed_arg;
                                break;
                            case EInputFlag.SpecifyOutputPath:
                                if (last_path == null)
                                {
                                    Console.Error.WriteLine("No input path is defined");
                                }
                                else
                                {
                                    files[last_path] = arg;
                                }
                                current_input_flag = EInputFlag.Nothing;
                                break;
                            case EInputFlag.SpecifyOutputDirectoryPath:
                                output_directory = arg;
                                current_input_flag = EInputFlag.Nothing;
                                break;
                        }
                        current_input_flag = EInputFlag.Nothing;
                    }
                }
            }
            PrintInputFlagError(current_input_flag);
            if (show_help || (files.Count <= 0))
            {
                if (help_topic == null)
                {
                    PrintInputFlags();
                }
                else
                {
                    InputFlag? input_flag = null;
                    if (inputFlags.ContainsKey(help_topic))
                    {
                        input_flag = inputFlags[help_topic];
                    }
                    else if (inputFlagAliases.ContainsKey(help_topic))
                    {
                        input_flag = inputFlags[inputFlagAliases[help_topic]];
                    }
                    if (input_flag == null)
                    {
                        Console.Error.Write("Invalid help topic \"");
                        Console.Error.Write(help_topic);
                        Console.Error.WriteLine("\".");
                        PrintInputFlags();
                    }
                    else
                    {
                        Console.WriteLine();
                        Console.Write("Help topic: ");
                        Console.WriteLine(input_flag.Value.Description);
                        Console.WriteLine();
                        string[] full_description = input_flag.Value.FullDescription.Split(Environment.NewLine);
                        if (full_description != null)
                        {
                            foreach (string full_description_line in full_description)
                            {
                                if (full_description_line != null)
                                {
                                    Console.Write("\t");
                                    Console.WriteLine(full_description_line);
                                }
                            }
                        }
                        Console.WriteLine();
                    }
                }
            }
            else
            {
                foreach (KeyValuePair<string, string> file in files)
                {
                    try
                    {
                        string input_file_path = Path.GetFullPath(file.Key);
                        if (File.Exists(input_file_path))
                        {
                            if (verbose)
                            {
                                Console.Write("Opening file \"");
                                Console.Write(input_file_path);
                                Console.WriteLine("\"...");
                            }
                            DGF dgf = DGF.Open(input_file_path);
                            dgf.EncryptedEditPassword = null;
                            dgf.EncryptedPlayPassword = null;
                            dgf.EncryptedApplyPlayPasswordUntilGardenNumber = null;
                            if (dgf == null)
                            {
                                Console.Error.Write("Could not open file \"");
                                Console.Error.Write(input_file_path);
                                Console.Error.WriteLine("\".");
                            }
                            else
                            {
                                string output_path = Path.GetFullPath(Path.Combine(output_directory, file.Value));
                                string directory_path = Path.GetFullPath(Path.GetDirectoryName(output_path));
                                if (!(Directory.Exists(directory_path)))
                                {
                                    Directory.CreateDirectory(directory_path);
                                    if (verbose)
                                    {
                                        Console.Write("Created directory \"");
                                        Console.Write(directory_path);
                                        Console.WriteLine("\".");
                                    }
                                }
                                if (dgf.Save(output_path))
                                {
                                    if (verbose)
                                    {
                                        Console.Write("Created file \"");
                                        Console.Write(output_path);
                                        Console.WriteLine("\".");
                                    }
                                }
                                else
                                {
                                    Console.Error.Write("Failed to write output to \"");
                                    Console.Error.Write(output_path);
                                    Console.Error.WriteLine("\".");
                                }
                            }
                        }
                        else
                        {
                            Console.Error.Write("File \"");
                            Console.Error.Write(input_file_path);
                            Console.Error.WriteLine("\" does not exist.");
                        }
                    }
                    catch (Exception e)
                    {
                        Console.Error.WriteLine(e);
                    }
                }
            }
        }
    }
}
