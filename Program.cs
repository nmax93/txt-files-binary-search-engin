namespace Search_Engine
{
    class Program
    {
        static void Main(string[] args)
        {
            SearchEngine search = SearchEngine.instance();
            search.start();
        }
    }
}