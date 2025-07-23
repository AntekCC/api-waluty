using Newtonsoft.Json;
public class Rate
{
    public string no { get; set; }
    public string effectiveDate { get; set; }
    public double mid { get; set; }
}
public class Root
{
    public string table { get; set; }
    public string currency { get; set; }
    public string code { get; set; }
    public List<Rate> rates { get; set; }
}
namespace MyApp
{
 class Program
    {
        static async Task Main(string[] args)
        {            
            while (true)
            {
                await currData();
            }
        }
        static async Task currData()
        {
            Console.WriteLine("jaka walute chcesz pobrac ?(np USD)");
            string input = Console.ReadLine().ToUpper();
            if (!string.IsNullOrEmpty(input) && input.Length == 3)
            {
                string url = $"https://api.nbp.pl/api/exchangerates/rates/a/{input}/?format=json";
                HttpClient client = new HttpClient();
                var response = await client.GetAsync(url);
                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine("brak waluty");
                }
                else
                {
                    string json = await response.Content.ReadAsStringAsync();
                    var obj = JsonConvert.DeserializeObject<Root>(json);
                    foreach (var rate in obj.rates)
                    {
                        Console.WriteLine($"{obj.code} - {obj.currency}: {rate.mid}");
                    }

                }

            }
        }
    }
}
