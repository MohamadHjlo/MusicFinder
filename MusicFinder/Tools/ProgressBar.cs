using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ConsoleProgressBar;

namespace MusicFinder.Tools
{
    public class ProgressBar
    {
        private const int blockCount = 10;
        private readonly TimeSpan animationInterval = TimeSpan.FromSeconds(1.0 / 8);
        private const string animation = @"|/-\";

        public async Task StartDownloading(List<string> urls, string path)
        {
           
            Console.ResetColor();
            await FileTransferProgressBars(urls, path);

        }
        private static async Task FileTransferProgressBars(List<string> urls, string path)
        {
            await DownloadFileWithProgressBar(urls, path);
            Console.ReadKey();
            Console.WriteLine("\n");
            Console.ResetColor();
        }

        private static void ProgressChanged(object sender, DownloadProgressChangedEventArgs e, Stopwatch sw, FileTransferProgressBar progress, int left, int top)
        {

            progress.BytesReceived = e.BytesReceived;
            var percent = (double)e.BytesReceived / e.TotalBytesToReceive;

            progress.Report(percent);
            if (percent==1)
            {
                Thread.Sleep(200);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\nSuccessfully downloaded !");
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("press any key to continue....");
            }

        }
        public static long GetFileSize(string url)
        {
            long result = -1;
            try
            {
                System.Net.WebRequest req = System.Net.WebRequest.Create(url);
                req.Method = "HEAD";
                using (System.Net.WebResponse resp = req.GetResponse())
                {
                    if (long.TryParse(resp.Headers.Get("Content-Length"), out long ContentLength))
                    {
                        result = ContentLength;
                    }
                }
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Somethings was wrong .. please check Your connection , download link may be filtered or not available More description : ");
                Console.WriteLine(e.Message + "\n");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("press any key to continue....");


            }
            return result;
        }

        public static void ClearCurrentConsoleLine(int currentLineCursor)
        {

            Console.SetCursorPosition(0, currentLineCursor);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, currentLineCursor);
        }
        private static async Task DownloadFileWithProgressBar(List<string> urls, string path)
        {
            var listTask = new List<Task>();
            await Task.Run(() =>
            {
                foreach (var item in urls)
                {
                    Stopwatch sw = new Stopwatch();
                    using (var webClient = new WebClient())
                    {
                        Uri URL = new Uri(item);
                        var totalBytes = GetFileSize(item);
                        if (totalBytes == -1) { return; }
                        Console.ForegroundColor = ConsoleColor.DarkCyan;
                        var textCaption = $"File {Path.GetFileName(item)} Downloading... ";
                        Console.ResetColor();
                        Console.Write(textCaption);
                        int left = Console.CursorLeft;
                        int top = Console.CursorTop;

                        var progress = new FileTransferProgressBar(totalBytes, TimeSpan.FromSeconds(5))
                        {
                            NumberOfBlocks = 20,
                            StartBracket = "|",
                            EndBracket = "|",
                            CompletedBlock = "|",
                            IncompleteBlock = "\u00a0",
                            AnimationSequence = animation,
                            ForegroundColor = ConsoleColor.DarkCyan,

                        };

                        webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler((sender, e) => ProgressChanged(sender, e, sw, progress, left, top));

                        sw.Start();
                        try
                        {
                            webClient.DownloadFileAsync(URL, path);

                        }
                        catch (WebException ex1)
                        {
                            Console.WriteLine(ex1.Message);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }

                    }

                }
            });


        }


    }
}

