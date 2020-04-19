using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace Search_Engine
{
    class SearchEngine
    {
        private HashSet<string> stopList;
        private Dictionary<string, Dictionary<string, string>> dictionary; // Dictionary < words, Dictionary< books, lines> > 
        private List<Book> books;
        private static SearchEngine Instance;

        public static SearchEngine instance()
        {
            if (Instance == null) Instance = new SearchEngine();
            return Instance;
        }

        private SearchEngine()
        {
            addStopList();
            books = new List<Book>();
            dictionary = new Dictionary<string, Dictionary<string, string>>();
            init_with_3_books();
        }

        public void start()
        {
            while(true)
                Login();
        }

        private void addStopList()
        {
            string text = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\Documents\Stop List.txt"));
            string[] words = text.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            stopList = new HashSet<string>(words);
        }

        private void init_with_3_books()
        {
            string[] path =
            {
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory,@"..\..\Documents\The Ant And The Grasshopper.txt"),
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory,@"..\..\Documents\The Hare And The Tortoise.txt"),
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory,@"..\..\Documents\The Three Little Pigs.txt")
            };
            for (int i = 0; i < 3; i++)
                addBook(path[i], false);
        }

        private void Login()
        {
            Console.Write("> Enter username: ");
            string username = Console.ReadLine();
            Console.Write("> Enter password: ");
            string password = Console.ReadLine();
            if (username.Equals("admin") && password.Equals("1234"))
                AdminMenu();
            else
                ClientMenu();
        }

        private void AdminMenu()
        {
            while (true)
            {
                Console.WriteLine(
                    ">\t--------------------------------------------------\n" +
                    " \t[1] Add new document" +
                    " \t[2] Disable document\n" +
                    " \t[3] Enable document" +
                    " \t[4] Search document\n" +
                    " \t[5] Print document" +
                    " \t[6] Print index table\n" +
                    " \t[7] Help\t" +
                    " \t[8] Logout\n" +
                    " \t[9] Exit\n" +
                    " \t--------------------------------------------------");
                Console.Write("> ");
                char input = Console.ReadKey().KeyChar;
                Console.WriteLine();
                switch (input)
                {
                    case '1':
                        Console.Write("> File path: ");
                        string path = Console.ReadLine();
                        addBook(path);
                        break;

                    case '2':
                        DisableOrEnableDoc(false);
                        break;

                    case '3': 
                        Console.WriteLine("> Choose the document you'd like to enable:");
                        DisableOrEnableDoc(true);
                        break;

                    case '4':
                        while (true)
                        {
                            Console.WriteLine("> Enter the @ key to go back to menu");
                            Console.Write("> Search: ");
                            string query = Console.ReadLine();
                            if (query.Equals("@")) break;
                            Search(query);
                        }
                        break;

                    case '5':
                        Console.WriteLine("> Choose the document you'd like to print:");
                        PrintBook();
                        break;

                    case '6':
                        printIndexTable();
                        break;

                    case '7':
                        Help();
                        break;

                    case '8':
                        return;

                    case '9':
                        Environment.Exit(0);
                        break;

                    default:
                        Console.WriteLine("> illegal choise\n");
                        break;
                }
            }
        }

        private void ClientMenu()
        {
            while (true)
            {
                Console.WriteLine(
                 ">\t--------------------------------------------------\n" +
                 " \t[1] Search document" +
                 " \t[2] Print document\n" +
                 " \t[3] Print index table" +
                 " \t[4] Help\n" +
                 " \t[5] Logout\t" +
                 " \t[6] Exit\n" +
                 " \t--------------------------------------------------\n");
                Console.Write("> ");
                char input = Console.ReadKey().KeyChar;
                Console.WriteLine();
                switch (input)
                {
                    case '1':
                        while (true)
                        {
                            Console.WriteLine("> Enter the @ key to go back to menu");
                            Console.Write("> Search: ");
                            string query = Console.ReadLine();
                            if (query.Equals("@")) break;
                            Search(query);
                        }
                        break;

                    case '2':
                        Console.WriteLine("> Choose the document you'd like to print:");
                        PrintBook();
                        break;

                    case '3':
                        printIndexTable();
                        break;

                    case '4':
                        Help();
                        break;

                    case '5':
                        return;

                    case '6':
                        Environment.Exit(0);
                        break;

                    default:
                        Console.WriteLine("> illegal choise\n");
                        break;
                }
            }
        }

        private void addBook(string path, bool backup = true)
        {
            if (File.Exists(path))
            {
                Book book = new Book(path);
                books.Add(book);
                indexBook(book);
                if (backup == true)
                {
                    File.Copy(path, Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\Documents\" + book.name + ".txt"));
                    Console.WriteLine("> Book has been added successfully\n");
                }

            }
            else
                Console.WriteLine("> Path does not exist\n");
        }

        private void DisableOrEnableDoc(bool state)
        {
            int i = 1, index;

            Console.WriteLine("> Choose the document you'd like to disable or 0 to go back to menu:");
            foreach (Book book in books)
            {
                Console.WriteLine($"\t{i} - {book.name}");
                i++;
            }
            while (true)
            {
                Console.Write("> ");
                string choise = Console.ReadLine();
                if (int.TryParse(choise, out index))
                {
                    if (index == 0)
                        break;
                    else if (index <= books.Count && index > 0)
                    {
                        books[index - 1].isEnabled = state;
                        Console.WriteLine("> Document state has been changed\n");
                        break;
                    }
                    else
                        Console.WriteLine("> Number is illegal");
                }
                else
                    Console.WriteLine("> Failed to change document state");
            }
        }

        private void PrintBook()
        {
            List<Book> availible_books = new List<Book>();
            int i = 1, index;

            foreach (Book book in books)
            {
                if (book.isEnabled)
                {
                    availible_books.Add(book);
                    Console.WriteLine($"\t{i} - {book.name}");
                    i++;
                }
            }
            while (true)
            {
                Console.Write("> ");
                string choise = Console.ReadLine();
                if (int.TryParse(choise, out index))
                {
                    if (index <= availible_books.Count && index > 0)
                    {
                        availible_books[index - 1].show_content();
                        Console.WriteLine();
                        break;
                    }
                    else
                        Console.WriteLine("> Number is illegal\n");
                }
                else
                    Console.WriteLine("> Failed to show the content\n");
            }
            Console.Write("\n> Press any key to continue ");
            Console.ReadKey();
            Console.WriteLine();
        }

        private void indexBook(Book book)
        {
            int i = 1; // line counter
            char[] splitChars = { ' ', ',', '.', ';', ':', '!', '?', '-', '(', ')', '"' };
            foreach (string line in book.lines)
            {
                string[] words = line.Split(splitChars, StringSplitOptions.RemoveEmptyEntries);
                foreach(string word in words) // for each of the words in the line
                {
                    if (!stopList.Any(item => item.Contains(word.ToUpper())))
                    {
                        if (!dictionary.ContainsKey(word)) // if its an unexisting word 
                        {
                            dictionary.Add(word, new Dictionary<string, string> { { book.name, $"{i}," } });
                        }
                        else // if existing
                        {
                            Dictionary<string, string> dict = dictionary[word];
                            if (!dict.ContainsKey(book.name)) // if its a new book
                            {
                                dict.Add(book.name, $"{i},");
                            }
                            else // if its an existing book
                            {
                                string updatedLines = $"{dict[book.name]}{i},";
                                dict[book.name] = updatedLines;
                            }

                            dictionary[word] = dict;
                        }
                    }
                }
                i++;
            }
        }

        private void printIndexTable()
        {
            foreach(KeyValuePair<string, Dictionary<string, string>> index in dictionary)
            {
                Console.WriteLine($"\t[{index.Key}]");
                Dictionary<string, string> dict = index.Value;

                foreach(KeyValuePair<string, string> kvp in dict)
                {
                    Console.WriteLine($"\t   {kvp.Key}\t\t{kvp.Value}");
                }
            }
            Console.WriteLine();
        }

        private void Help()
        {
            Console.WriteLine(
                "\t<User guide>\n" +
                "\t-Login: type \"admin\" as username and \"1234\" as password to login as admin.\n" + 
                "\t any other input logs the user in as a simple client\n" +
                "\t-Search: the search supports only one level of brackets of type \"()\".\n" +
                "\t the structure of the query should be \"word OP word OP ...\"\n" +
                "\t available operators are AND, OR, NOT.\n" +
                "\t there is no priority to any of the operators, only brackets do." +
                "\t there is also a built in stop list. this list contains words that should be\n" +
                "\t ignored in the search and index process.\n" +
                "\t to exit the search and go back to menu, enter \"@\".\n" +
                "\t-Document printing: a list of all available documents will be displayed. enter\n" +
                "\t the number of the document you would like to see its content.\n" +
                "\t-Index printing: print the index table to see all the words scanned so far,\n" +
                "\t including in what documents and what lines.\n" +
                "\t-Add document: (admin feature) add document to the system. the document will\n" +
                "\t be backed up and the index table will be updated.\n" +
                "\t notice that the word separation during the indexing process is by built in\n" +
                "\t split characters. in case the new document contains additional characters\n" +
                "\t they'll have to be added to the splitChars in the code" +
                "\t-Disable document: (admin feature) the document won't be displayed in the\n" +
                "\t search results and its content won't be available to watch.\n" +
                "\t-Enable document: (admin feature) enable the document back.\n");
            Console.Write("> Press any key to continue ");
            Console.ReadKey();
            Console.WriteLine();
        }

        private void Search(string _query)
        {
            List<string> result_list = new List<string>();
            List<string> brackets_result_list = null;
            List<string> temp_list;

            string query = _query.ToUpper();

            if (isQueryLegal(query))
            {
                if (areThereBrackets(ref query))
                {
                    brackets_result_list = queryInBrackets(ref query);
                }
                query = query.Trim(); // in case the whole query was in brackets and only a $ sign left
                if (!query.Equals("$"))  // $ is a sign put instead the brackets part in the brackets func
                {
                    string[] words = query.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    result_list = searchWord(words[0]);

                    for (int i = 2; i < words.Length; i += 2)
                    {
                        switch (words[i - 1])
                        {
                            case "AND":
                                if (!words[i].Equals("$"))
                                {
                                    temp_list = searchWord(words[i]);
                                    AND(result_list, temp_list);
                                }
                                else
                                    AND(result_list, brackets_result_list);
                                break;

                            case "OR":
                                if (!words[i].Equals("$"))
                                {
                                    temp_list = searchWord(words[i]);
                                    OR(result_list, temp_list);
                                }
                                else
                                    OR(result_list, brackets_result_list);
                                break;

                            case "NOT":
                                if (!words[i].Equals("$"))
                                {
                                    temp_list = searchWord(words[i]);
                                    NOT(result_list, temp_list);
                                }
                                else
                                    NOT(result_list, brackets_result_list);
                                break;

                            default:
                                break;
                        }
                    }
                }
                else
                    result_list = brackets_result_list;

                if (result_list.Count > 0)
                {
                    printResultList(result_list, _query.ToUpper());
                }
                else
                    Console.WriteLine("> 0 results\n");
            }
        }

        private void printResultList(List<string> result_list, string query)
        {
            List<Book> display_list = new List<Book>();
            int i = 1, index;
            foreach (string book in result_list)
            {
                foreach (Book b in books)
                {
                    if (b.name.Equals(book))
                        if (b.isEnabled)
                        {
                            Console.WriteLine($"\t{i} - {book}");
                            display_list.Add(b);
                            i++;
                        }
                }
            }
            Console.WriteLine($"> {display_list.Count} results");

            while (true) {
                Console.Write("> Choose document to display its content or 0 to go back to search: ");
                string choise = Console.ReadLine();
                if (int.TryParse(choise, out index))
                {
                    if (index == 0)
                    {
                        Console.WriteLine();
                        return;
                    }
                    else if (index <= display_list.Count && index > 0)
                    {
                        display_list[index - 1].show_content_with_emphasize(query);
                        break;
                    }
                    else
                        Console.WriteLine("> Number is illegal");
                }
                else
                    Console.WriteLine("> Failed to show the content");
            }
            Console.Write("> Press any key to continue ");
            Console.ReadKey();
            Console.WriteLine();
        }

        private bool areThereBrackets(ref string query)
        {
            char c;

            for (int i = 0; i < query.Length; i++)
            {
                c = query[i];
                if (c == '(') return true;
            }

            return false;
        }

        private List<string> queryInBrackets(ref string query)
        {
            char c;
            int open_bracket_index = 0, close_bracket_index = 0;
            List<string> result_list = new List<string>();

            for (int i = 0; i < query.Length; i++)
            {
                c = query[i];
                if (c == '(') open_bracket_index = i;
                if (c == ')') close_bracket_index = i;
            }

            string sub_query = query.Substring(open_bracket_index + 1, (close_bracket_index - open_bracket_index) - 1); // taking out the sub query inside brackets
            query = query.Remove(open_bracket_index, (close_bracket_index - open_bracket_index) + 1); // removing it from the original query
            query = query.Insert(open_bracket_index, " $ "); // and putting $ sign instead this part for later operations with the list of results of the brackets

            if (isQueryLegal(sub_query))
            {
                string[] words = sub_query.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                List<string> temp_list;

                result_list = searchWord(words[0]);

                for (int i = 2; i < words.Length; i += 2)
                {
                    switch (words[i - 1])
                    {
                        case "AND":
                            temp_list = searchWord(words[i]);
                            AND(result_list, temp_list);
                            break;

                        case "OR":
                            temp_list = searchWord(words[i]);
                            OR(result_list, temp_list);
                            break;

                        case "NOT":
                            temp_list = searchWord(words[i]);
                            NOT(result_list, temp_list);
                            break;

                        default:
                            break;
                    }
                }
            }
            return result_list;
        }

        private bool isQueryLegal(string query)
        {
            if (query.Length == 0)
            {
                return false;
            }
            // brackets and signs validation
            bool open_bracket = false, close_bracket = false;

            foreach(char c in query) 
            {
                if((c >= 'A' && c <= 'Z') || c == '(' || c == ')' || c == '\'' || c == ' ' || (c >= '0' && c <= '9'))
                {
                    if (c == '(' || c == ')')
                    {
                        if (c == '(' && open_bracket == false)
                            open_bracket = true;
                        else if (c == ')' && open_bracket == true && close_bracket == false)
                            close_bracket = true;
                        else if ((c == '(' && open_bracket == true) || (c == '(' && close_bracket == true) || (c == ')' && open_bracket == false) || (c == ')' && close_bracket == true))
                        {
                            Console.WriteLine("> Query invalid - brackets error");
                            return false;
                        }
                    }
                }
                else
                {
                    Console.WriteLine("> Query invalid - only letters, numbers and ' ( ) signes are allowed");
                    return false;
                }
            }
            if(open_bracket == true && close_bracket == false)
            {
                Console.WriteLine("> Query invalid - missing bracket");
                return false;
            }

            // operators and words validation
            string[] words = query.Split(new char[] { ' ', '(', ')' }, StringSplitOptions.RemoveEmptyEntries);
            if (words.Length == 0)
            {
                Console.WriteLine("> Query invalid");
                return false;
            }
            if ((words[0].Equals("AND")) || (words[0].Equals("OR")) || (words[0].Equals("NOT")) || 
                (words[words.Length - 1].Equals("AND")) || (words[words.Length - 1].Equals("OR")) || (words[words.Length - 1].Equals("NOT")))
            {
                Console.WriteLine("> Query invalid - query should not start or end with operator");
                return false;
            }
            for (int i = 0; i < words.Length; i++)
            {
                if (((i % 2 == 0) && ((words[i].Equals("AND")) || (words[i].Equals("OR")) || (words[i].Equals("NOT")))) || // even cells should be words
                    ((i % 2 != 0) && (!words[i].Equals("AND")) && (!words[i].Equals("OR")) && (!words[i].Equals("NOT"))))  // odd cells should be operators
                {
                    Console.WriteLine("> Query invalid - query structure: word OP word OP ...");
                    return false;
                }
            }

            return true;
        }

        private List<string> searchWord(string word)
        {
            List<string> result_list = new List<string>();
            Dictionary<string, string> dict = null;

            if (dictionary.ContainsKey(word))
            {
                dict = dictionary[word];
                foreach (KeyValuePair<string, string> kvp in dict)
                    result_list.Add(kvp.Key);
            }
            return result_list;
        }

        private void OR(List<string> list1, List<string> list2)
        {
            foreach (string book in list2)
                if (!list1.Contains(book)) list1.Add(book);
        }

        private void AND(List<string> list1, List<string> list2)
        {
            List<string> result_list = new List<string>();
            foreach (string book in list2)
                if (list1.Contains(book)) result_list.Add(book);

            list1.Clear();
            foreach(string book in result_list)
                list1.Add(book);
        }

        private void NOT(List<string> list1, List<string> list2)
        {
            foreach (string book in list2)
                list1.Remove(book);
        }

    }
}