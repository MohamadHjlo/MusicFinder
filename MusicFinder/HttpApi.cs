using MusicFinder.MeloBit;
using MusicFinder.Tools;
using Newtonsoft.Json;
using System.IO;
using System.Net.Http.Headers;

namespace MusicFinder
{
    public class HttpApi
    {
        static HttpClient _client = new HttpClient();

        List<Result> _products = new List<Result>();

        public async Task<List<Product?>> GetProductsAsync(string path, SearchType searchType)
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
                    foreach (var song in songs)
                    {
                        try
                        {
                            c++;
                            song.Song.Id = c.ToString();
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.Write($" {c} : ");
                            Console.ResetColor();
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                            Console.Write(song.Song.Title + "  ");
                            Console.ResetColor();
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.Write(string.Join(" & ", song.Song.Artists.Select(a => a.FullName).ToList()));
                            Console.ResetColor();
                            Console.ForegroundColor = ConsoleColor.DarkGreen;
                            Console.Write(" ... downloads : ");
                            Console.ForegroundColor = ConsoleColor.DarkCyan;
                            Console.WriteLine(int.Parse(song.Song.DownloadCount).ToString("##,###") + "\n");
                            Console.ResetColor();
                        }

                        catch (Exception e)
                        {
                            Console.WriteLine("Something was Wrong in Api Data , please try again");

                        }
                    }

                    _products = songs;
                }


            }
            return products;
        }

        public async Task<bool> DownloadById(string id)
        {
            if (!_products.Any())
            {
                Console.WriteLine("No music available in list , if you want to exit enter -c otherwise enter another Id for download");
                Console.ResetColor();

                return false;
            }
            else
            {
                var song = _products.FirstOrDefault(p => p?.Song?.Id == id);
                if (song == null)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("No Music was found with such Id , if you want to exit enter -c otherwise enter another Id for download");
                    Console.ResetColor();

                    return false;
                }
                else
                {
                    if (song.Song.Audio == null)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("this music has not audio link to download , if you want to exit enter -c otherwise enter another Id for download");
                        Console.ResetColor();

                        return false;
                    }

                    bool qualitySelection = true;
                    while (qualitySelection == true)
                    {
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine("Choose quality to Download : \n");
                        Console.ResetColor();
                        string? lowFileName = song.Song.Audio.Medium.Url.Split('/').LastOrDefault();
                        string? highFileName2 = song.Song.Audio.High.Url.Split('/').LastOrDefault();
                        Console.Write("1.Medium quality :  ");
                        Console.ForegroundColor = ConsoleColor.DarkCyan;
                        Console.WriteLine(lowFileName);
                        Console.ResetColor();
                        Console.Write("2.High quality :  ");
                        Console.ForegroundColor = ConsoleColor.DarkCyan;
                        Console.WriteLine(highFileName2);
                        Console.ResetColor();
                        string? selectedQuality = Console.ReadLine();
                        if (selectedQuality.Trim() == "1")
                        {
                            qualitySelection = false;
                            await Download(song.Song.Audio.Medium.Url, "D:\\Downloads\\MusicFinder\\", lowFileName);
                        }
                        else if (selectedQuality.Trim() == "2")
                        {
                            qualitySelection = false;
                            await Download(song.Song.Audio.High.Url, "D:\\Downloads\\MusicFinder\\", highFileName2);
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("not found such option , if you want to id selection menu enter -m otherwise select correct quality number");
                            string? wantToExit = Console.ReadLine();
                            if (wantToExit == "-m")
                            {
                                qualitySelection = false;
                            }
                            else
                            {
                                qualitySelection = true;
                            }
                        }
                    }
                    return true;
                }

            }

        }

        public async Task Download(string downloadUrl, string fileSaveLocation, string fileName)
        {
            List<string> urls = new List<string>()
            {
                downloadUrl
            };
            if (!Directory.Exists(fileSaveLocation))
            {
                Directory.CreateDirectory(fileSaveLocation);
            }

            var progress = new ProgressBar();
            var fullPath = fileSaveLocation + fileName;
            await progress.StartDownloading(urls, fullPath);

        }
    }
}
