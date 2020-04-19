using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace Search_Engine
{
    class Book
    {
        public bool isEnabled;
        public string name;
        public string[] lines;

        public Book(string path)
        {
            isEnabled = true;
            string filename = Path.GetFileName(path);
            name = filename.Remove(filename.Length - 4);
            lines = File.ReadAllLines(path);
        }

        public void show_content()
        {
                foreach (string line in lines)
                    Console.WriteLine($"\t{line}");
        }

        public void show_content_with_emphasize(string query)
        {
            string[] query_words = query.Split(new char[] { ' ', '(', ')' }, StringSplitOptions.RemoveEmptyEntries);
            char[] splitChars = { ' ', ',', '.', ';', ':', '"', '!', '?', '-', '(', ')' };
            List<string> words_to_emphasize = new List<string>();

            words_to_emphasize.Add(query_words[0]);

            for (int i = 2; i < query_words.Length; i += 2)
            {
                if (!query_words[i - 1].Equals("NOT"))
                    words_to_emphasize.Add(query_words[i]);
            }
            Console.WriteLine();
            foreach (string line in lines) // the spaces between the words are cut so we need to know what sign comes after the word (' ' or '"' etc...)
            {
                Console.Write('\t');
                string[] words = line.Split(splitChars, StringSplitOptions.RemoveEmptyEntries);
                int location_in_line = -1;

                foreach (string word in words)
                {
                    if (words_to_emphasize.Contains(word))
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write(word);
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else
                        Console.Write(word);

                    location_in_line += word.Length;
                    location_in_line++;

                    while (location_in_line < line.Length && !Char.IsLetter(line[location_in_line])) // next char check
                    {
                        Console.Write(line[location_in_line]);
                        location_in_line++;
                    }
                    location_in_line--;
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }
}