using System.Drawing;
using System.Globalization;
using MusicFinder;
using MusicFinder.MeloBit;
using MusicFinder.Tools;
using Newtonsoft.Json.Linq;

Console.ForegroundColor = ConsoleColor.White;
var httpApi = new HttpApi();
//await httpApi.Download("https://ups.music-fa.com/tagdl/8e401/Bahman%20Saadat%20-%20Boghz%20(320).mp3", "D:\\Downloads\\MusicFinder\\","music.mp3");
bool correctSearchOptionSelected = false;

SearchType searchTypeSelected = SearchType.MostRelated;

while (true)
{

    Console.ForegroundColor = ConsoleColor.DarkCyan;
    Console.WriteLine("Please Enter Music or Artist that You want ! \n");

    Console.ForegroundColor = ConsoleColor.White;

    var searchKeyinput = Console.ReadLine();

    Console.WriteLine("\n");

    Console.ForegroundColor = ConsoleColor.DarkCyan;
    Console.WriteLine("\n Enter Sort Type Of Musics : \n");
    Console.ResetColor();

    var options = Enum.GetValues(typeof(SearchType));

    while (correctSearchOptionSelected == false)
    {
        int enumCounter = 1;
        bool parseValid;
        foreach (var option in options)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(enumCounter.ToString());
            Console.ResetColor();
            Console.WriteLine("." + option.ToString() + "\n");
            enumCounter++;
        }
        Console.ForegroundColor = ConsoleColor.White;

        var selectedOption = Console.ReadLine();

        Console.WriteLine("\n");
        var canParseInt = int.TryParse(selectedOption, out int enumNumber);

        bool canParseEnum = false;

        if (canParseInt)
        {
            canParseEnum = Enum.TryParse((enumNumber - 1).ToString(), true, out SearchType searchType) && Enum.IsDefined(typeof(SearchType), searchType);

            parseValid = canParseEnum && canParseInt;

            if (parseValid)
            {
                searchTypeSelected = searchType;
                correctSearchOptionSelected = true;
            }
            else
            {
                correctSearchOptionSelected = false;
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine("Oh ! '" + selectedOption + "' is Not Correct Option ! Please Enter an Valid number Of options \n");
                Console.ResetColor();
            }
        }
        else
        {
            correctSearchOptionSelected = false;
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("Oh ! '" + selectedOption + "' is Not Correct Option ! Please Enter an Valid number Of options \n");
            Console.ResetColor();
        }

    }
    Console.Write("please wait....");
    await httpApi.GetProductsAsync("https://haji-api.ir/musi" + $"c/?q=search&t={searchKeyinput}", searchTypeSelected);
    correctSearchOptionSelected = false;
    bool wantToDownload = true;
    while (wantToDownload)
    {
        Console.WriteLine("if you want to download any song from this list , enter number of song to continue ,  Otherwise enter '-c' to new search");

        string? choose = Console.ReadLine();
        if (choose != "-c")
        {
            await httpApi.DownloadById(choose);
            wantToDownload = true;
        }
        else
        {
            wantToDownload = false;
        }
    }
}