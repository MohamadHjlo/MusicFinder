using MusicFinder.MeloBit;
using Newtonsoft.Json;

namespace MusicFinder
{
    public class HttpApi
    {
        static HttpClient _client = new HttpClient();

        public async Task<List<Product?>> GetProductsAsync(string path,SearchType searchType)
        {

            List<Product?> products = new List<Product?>();
            
            HttpResponseMessage response = await _client.GetAsync(path);

            if (response.IsSuccessStatusCode)
            {
                var jsonStr = await response.Content.ReadAsStringAsync();

                var melobit = JsonConvert.DeserializeObject<Root>(jsonStr);

                Console.Write("\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b");

                if (melobit == null)
                {
                    Console.WriteLine("Not found such result");
                }
                else
                {
                    List<Result> songs = new List<Result>();
                    if (searchType == SearchType.MostDownloaded)
                    {
                        songs = melobit.Results.Where(r => r.Type == "song")
                            .OrderByDescending(r => r.Song.DownloadCount != null ? int.Parse(r.Song.DownloadCount) : 0)
                            .ToList();
                    }
                    else
                    {
                        songs = melobit.Results.Where(r => r.Type == "song")
                            .ToList();
                    }
                    int c = 0;
                    foreach (var r in songs)
                    {
                        try
                        {
                            c++;
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.Write($" {c} : ");
                            Console.ResetColor();
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                            Console.Write(r.Song.Title + "  ");
                            Console.ResetColor();
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.Write(string.Join(" & ", r.Song.Artists.Select(a => a.FullName).ToList()));
                            Console.ResetColor();
                            Console.ForegroundColor = ConsoleColor.DarkGreen;
                            Console.Write(" ... downloads : ");
                            Console.ForegroundColor = ConsoleColor.DarkCyan;
                            Console.WriteLine(int.Parse(r.Song.DownloadCount).ToString("##,###") + "\n");
                            Console.ResetColor();
                        }

                        catch (Exception e)
                        {
                            Console.WriteLine("Something was Wrong in Api Data please try again");

                        }
                    }
                }

            }
            return products;
        }

    }
}
