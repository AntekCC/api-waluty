using Newtonsoft.Json;
using System.Runtime.CompilerServices;
using System.Text;
string url = "https://api.nbp.pl/api/exchangerates/tables/a/?format=jsonl";
HttpClient client = new HttpClient();
var response = await client.GetAsync(url);
if (response.IsSuccessStatusCode)
{
    string json = await response.Content.ReadAsStringAsync();
    var obj = JsonConvert.DeserializeObject<List<Root>>(json);
    StringBuilder stringBuilder = new StringBuilder();
    Dictionary<string, string> map = new Dictionary<string, string>();

    Console.WriteLine("Kody walut");
    foreach (var root in obj)
    {
        foreach (var item in root.rates)
        {
            map.Add(item.code, ($"{item.currency} - {item.mid}"));
            Console.WriteLine($"{item.code} - {item.currency}");
        }

    }
    while (true)
    {
        Console.WriteLine("podaj kod waluty zeby wyswietlic kurs");
        try
        {
            string input = Console.ReadLine().ToUpper();
            if (map.ContainsKey(input))
            {
                string result = map[input];
                Console.WriteLine($"Kurs dla {input}: {result}");

            }
            else
            {
                Console.WriteLine("nie znaleziono kodu");
            }
        }
        catch
        {

        }
    }
}
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