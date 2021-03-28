using System;
using CommandLine;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace Converter
{
    class Program
    {
        public class Options
        {
            [Option('s', "sheet", Required = true, HelpText = "Путь до таблицы")]
            public String Sheet { get; set; }
            [Option('o', "output", Required = true, HelpText = "Путь до выходного файла")]
            public String Output { get; set; }
            [Option('i', "input", Required = true, HelpText = "Путь до входного файла")]
            public String Input { get; set; }
            [Option('m', "mode", Required = true, HelpText = "Режим работы: h=hex->text, t=text->hex")]
            public String Mode { get; set; }
        }

        static void Main(string[] args)
        {
            string sheet_path = default;
            string input_file = default;
            string output_file = default;
            string mode = default;
            var parseresults = Parser.Default.ParseArguments<Options>(args).WithParsed<Options>(o =>
            {
                sheet_path = o.Sheet;
                input_file = o.Input;
                output_file = o.Output;
                mode = o.Mode;
            });
            if (parseresults.Tag == ParserResultType.NotParsed)
            {
                Console.WriteLine("Invalid arguments");
                Environment.Exit(0);
            }
            if (!File.Exists(sheet_path))
                Console.WriteLine("Sheet file do not exist");
            if (!File.Exists(input_file))
                Console.WriteLine("Input file do not exist");
            List<string> current_sheet = null;
            List<string> second_sheet = null;
            List<string> sheet_hex = new List<string>();
            List<string> sheet_text = new List<string>();
            StreamReader sheet_file = new StreamReader(sheet_path);
            while (!sheet_file.EndOfStream)
            {
                Match match = Regex.Match(sheet_file.ReadLine(), "([0-9A-F]*)=(.*)");
                sheet_hex.Add(match.Groups[1].Value);
                sheet_text.Add(match.Groups[2].Value);
            }
            sheet_file.Dispose();
            int max = default;
            string text = new StreamReader(input_file).ReadToEnd();
            MatchCollection match1 = Regex.Matches(text, "{([0-9A-F]{2})}*");
            if (match1.Count > 0)
                foreach (Match m in match1)
                {
                    string add = "";
                    sheet_text.Add(m.Groups[0].Value);
                    for(int i=1;i<m.Groups.Count;i++)
                    {
                        add += m.Groups[i].Value;
                    }
                    sheet_hex.Add(add);
                }
            switch (mode)
            {
                case "t":
                    max = Max(sheet_text);
                    current_sheet = sheet_text;
                    second_sheet = sheet_hex;
                    break;
                case "h":
                    max = Max(sheet_hex);
                    current_sheet = sheet_hex;
                    second_sheet = sheet_text;
                    break;
            }
            string text3 = "";
            for (int i = 0; i < text.Length;)
            {
                string text2 = Init(text, i, max);
                while (!current_sheet.Exists(e => e.Equals(text2)))
                {
                    text2 = Decrement(text2);
                }
                text3 += second_sheet[current_sheet.IndexOf(text2)];


                i += text2.Length;
            }
            File.WriteAllText(output_file, text3);
        }
        static int Max(List<string> sheet)
        {
            int max = default;
            foreach (string str in sheet)
            {
                if (str.Length > max)
                    max = str.Length;
            }
            return max;
        }
        static string Decrement(string str)
        {
            string str2 = default;
            for (int i = 0; i < str.Length - 1; i++)
            {
                str2 += str[i];
            }
            return str2;
        }
        static string Init(string text, int i, int max)
        {
            string str = default;
            for (int j = i; j < max + i; j++)
            {
                try
                {
                    str += text[j];
                }
                catch
                {
                    return str;
                }
            }
            return str;
        }
    }
}
