using Newtonsoft.Json;
using System.IO;
public class Rate
{
    public string currency { get; set; }
    public string code { get; set; }
    public double mid { get; set; }
}

public class Root
{
    public string table { get; set; }
    public string no { get; set; }
    public string effectiveDate { get; set; }
    public List<Rate> rates { get; set; }
}
namespace MyApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "lines.txt");

            string[] menu = new string[2] { "(1)lista walut", "(2)pojedyncza waluta" };
            await currData(path);
            while (true)
            {
                foreach (string item in menu) { 
                Console.WriteLine(item);
                }
                int choice = int.Parse(Console.ReadLine());
                switch (choice)
                {
                    case 1:
                        currlist(path);
                        break;
                        case 2:
                        singleCurr(path);
                        break;
                }
            }
        }
        static async Task currData(string path)
        {
            if (File.Exists(path) && new FileInfo(path).Length > 0)
            {
                return;
            }

            string url = $"https://api.nbp.pl/api/exchangerates/tables/a/?format=json";
            HttpClient client = new HttpClient();
            var response = await client.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine("Brak walut");
            }
            else
            {
                string json = await response.Content.ReadAsStringAsync();
                var obj = JsonConvert.DeserializeObject<List<Root>>(json);
                string line = " ";
                if (File.Exists(path))
                {
                    if (new FileInfo(path).Length == 0)
                    {
                        foreach (var root in obj)
                        {
                            foreach (var item in root.rates)
                            {
                                line = $"{item.code} {item.currency} {item.mid}\n";
                                File.AppendAllText(path, line);
                                line = " ";
                            }

                        }
                    }
                    else
                    {
                        Console.WriteLine("Plik już istnieje i ma dane.");
                    }

                }
                else
                {
                    Console.WriteLine("Plik nie istnieje.Tworzenie nowego");
                    foreach (var root in obj)
                    {
                        foreach (var item in root.rates)
                        {
                            line = $"{item.code}{item.currency}{item.mid}\n";
                            File.WriteAllText(path, line);
                            line = " ";
                        }

                    }
                }

            }
            
        }
        static void singleCurr(string path)
        {
            Console.WriteLine("podaj kod waluty zeby wyswietlic kurs");
            string input = Console.ReadLine().ToUpper();
            if (File.Exists(path))
            {
                var lines = File.ReadAllLines(path);
                {
                    foreach (var line in lines)
                    {
                        if (line.Contains(input))
                        {
                            Console.WriteLine(line);
                        }
                    }
                }
            }

        }
        static void currlist(string path)
        {
            if (File.Exists(path))
            {
                var lines = File.ReadAllLines(path);
                {
                    foreach (var line in lines)
                    {
                        Console.WriteLine(line);
                    }
                }
            }

        }
    }
}   
    
