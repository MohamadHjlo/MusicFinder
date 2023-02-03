using System.Drawing;
using System.Globalization;
using MusicFinder;
using MusicFinder.MeloBit;
using Newtonsoft.Json.Linq;


var httpApi = new HttpApi();

bool correctSearchOptionSelected = false;

SearchType searchTypeSelected = SearchType.MostRelated;

while (true)
{

    Console.ForegroundColor = ConsoleColor.DarkCyan;
    Console.WriteLine("Please Enter Music or Artist that You want ! \n");
    Console.ResetColor();

    var searchKeyinput = Console.ReadLine();

    Console.WriteLine("\n");

    Console.ForegroundColor = ConsoleColor.DarkCyan;
    Console.WriteLine("Enter Sort Type Of Musics : \n");
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

        var selectedOption = Console.ReadLine();

        Console.WriteLine("\n");
        var canParseInt = int.TryParse(selectedOption, out int enumNumber);

        bool canParseEnum = false;

        if (canParseInt)
        {
            canParseEnum = Enum.TryParse((enumNumber-1).ToString(), true, out SearchType searchType) && Enum.IsDefined(typeof(SearchType), searchType);

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


}