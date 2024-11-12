using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using Spectre.Console;

/*
 * Back Tracking
 * 
 *~Jakob Smogawetz (Arwezom)
 * 
 * HTL Hallein
 * 
 */

class BackTracking
{
    static int selectedOption = 1;
    static char[,] spielfeld;
    static Random rand = new Random();



    //Draw Interface (Main Menu)
    static void DrawUi()
    {
        //Width & Height of Console
        System.Console.WindowWidth = 122;
        System.Console.WindowHeight = 42;

        //Center Anything
        int numSpaces = (Console.WindowWidth - (Console.WindowWidth / 2)) / 2;
        Console.Write(new string(' ', numSpaces));

        //Select
        string[] options =
        {
                "Spielfeld",
                "Start",
                "Exit",
            };

        //Clear & Write Title
        Console.Clear();
        var title = new Table();
        title.Border = TableBorder.Heavy;
        title.Centered();
        title.AddColumn(new TableColumn("[magenta3] ______            _    _                  _    _             \r\n| ___ \\          | |  | |                | |  (_)            \r\n| |_/ / __ _  ___| | _| |_ _ __ __ _  ___| | ___ _ __   __ _ \r\n| ___ \\/ _` |/ __| |/ / __| '__/ _` |/ __| |/ / | '_ \\ / _` |\r\n| |_/ / (_| | (__|   <| |_| | | (_| | (__|   <| | | | | (_| |\r\n\\____/ \\__,_|\\___|_|\\_\\\\__|_|  \\__,_|\\___|_|\\_\\_|_| |_|\\__, |\r\n                                                        __/ |\r\n                                                       |___/ [/]").Centered());
        AnsiConsole.Write(title);

        //Write Options
        Console.WriteLine("                                                   ");
        Console.WriteLine("{0}{1} {2}                  ", "                    ", selectedOption == 1 ? "[\x1B[95mx\x1B[97m]" : "[ ]", $"{options[0]}");
        Console.WriteLine("{0}{1} {2}                  ", "                    ", selectedOption == 2 ? "[\x1B[95mx\x1B[97m]" : "[ ]", $"{options[1]}");
        Console.WriteLine("{0}{1} {2}                  ", "                    ", selectedOption == 3 ? "[\x1b[95mx\x1B[97m]" : "[ ]", $"{options[2]}");
        Console.WriteLine("                                                   ");
        Console.WriteLine("                                                   ");
        Console.WriteLine("                                                   ");
        Console.WriteLine("                                       (Use The Arrow Keys And ENTER)");
    }

    //Select Option From Interface (Main Menu)
    static void RedirectToOption()
    {
        //Redirect From Menu to other Void
        switch (selectedOption)
        {
            case 1:
                Test1();
                break;

            case 2:
                Start();
                break;

            case 3:
                Exit();
                break;

            default:
                return;
        }
    }

    //Main Programm (Interface)
    static void Main()
    {

    LabelMethodEntry:
        Console.Title = "Back Tracking";
        Console.OutputEncoding = Encoding.UTF8;
        Console.CursorVisible = false;
        Console.Clear();

    LabelDrawUi:

        //Set Cursor Position
        Console.SetCursorPosition(0, 3);

        //DrawUi
        DrawUi();

    LabelReadKey:

        //Read Key
        ConsoleKey pressedKey = Console.ReadKey(true).Key;

        //Check which Key pressed
        switch (pressedKey)
        {
            case ConsoleKey.Escape:
                Environment.Exit(0);
                break;

            case ConsoleKey.DownArrow:
                selectedOption = (selectedOption + 1 <= 3) ? selectedOption + 1 : selectedOption;
                goto LabelDrawUi;

            case ConsoleKey.UpArrow:
                selectedOption = (selectedOption - 1 >= 1) ? selectedOption - 1 : selectedOption;
                goto LabelDrawUi;

            case ConsoleKey.Enter:
                RedirectToOption();
                break;

            default:
                goto LabelReadKey;
        }

        goto LabelMethodEntry;
    }


    //Set Layers 3-20
    static void Test1()
    {
        //Clear & Write Title
        Console.Clear();
        var title = new Table();
        title.Border = TableBorder.Heavy;
        title.Centered();
        title.AddColumn(new TableColumn("[magenta3]______            _    _                  _    _             \r\n| ___ \\          | |  | |                | |  (_)            \r\n| |_/ / __ _  ___| | _| |_ _ __ __ _  ___| | ___ _ __   __ _ \r\n| ___ \\/ _` |/ __| |/ / __| '__/ _` |/ __| |/ / | '_ \\ / _` |\r\n| |_/ / (_| | (__|   <| |_| | | (_| | (__|   <| | | | | (_| |\r\n\\____/ \\__,_|\\___|_|\\_\\\\__|_|  \\__,_|\\___|_|\\_\\_|_| |_|\\__, |\r\n                                                        __/ |\r\n                                                       |___/ [/]").Centered());
        AnsiConsole.Write(title);
        Console.WriteLine();

        Console.Write("                             Enter grid size[\u001b[95m<:\u001b[97m] ");
        int gridSize = int.Parse(Console.ReadLine());

        Console.Write("                             Enter Number of Obstacles[\u001b[95m<:\u001b[97m] ");
        int obstacleCount = int.Parse(Console.ReadLine());

        spielfeld = new char[gridSize, gridSize];
        for (int i = 0; i < gridSize; i++)
            for (int j = 0; j < gridSize; j++)
                spielfeld[i, j] = ' ';

        spielfeld[0, 0] = 'C';             // Starting position
        spielfeld[gridSize - 1, gridSize - 1] = 'O'; // Goal position

        // Place obstacles
        PlaceObstacles(obstacleCount, gridSize);

    }
    public static void PlaceObstacles(int obstacleCount, int gridSize)
    {
        int placedObstacles = 0;
        while (placedObstacles < obstacleCount)
        {
            int x = rand.Next(gridSize);
            int y = rand.Next(gridSize);

            // Make sure not to place an obstacle at start (0,0) or goal
            if ((x == 0 && y == 0) || (x == gridSize - 1 && y == gridSize - 1) || spielfeld[x, y] == 'X')
                continue;

            spielfeld[x, y] = 'X';
            placedObstacles++;
        }
    }


    //Step
    // Step (modified to keep track of visited path)
    static bool Step(int x, int y, int xAlt, int yAlt)
    {
        // Check if out of bounds or hitting an obstacle
        if (x < 0 || x >= spielfeld.GetLength(0) || y < 0 || y >= spielfeld.GetLength(1) || spielfeld[x, y] == 'X' || spielfeld[x, y] == 'V')
        {
            return false;
        }

        // Check if the target is reached
        if (spielfeld[x, y] == 'O')
        {
            spielfeld[x, y] = 'C'; // Mark goal as reached
            ShowSpielfeld();
            return true;
        }

        if (spielfeld[x, y] == '#')
        {
            return false;
        }

        // Mark current cell as visited (add to path)
        spielfeld[x, y] = 'V'; // Changed from 'V' to
        ShowSpielfeld();

        // Try moving right
        if (Step(x, y + 1, x, y))
        {
            return true;
        }

        // Try moving down
        if (Step(x + 1, y, x, y))
        {
            return true;
        }

        // Try moving up
        if (Step(x - 1, y, x, y))
        {
            return true;
        }

        // Try moving left
        if (Step(x, y - 1, x, y))
        {
            return true;
        }

        // If no path is found, backtrack by unmarking this cell and removing from path
        spielfeld[x, y] = '#'; // Reset cell to empty
        ShowSpielfeld();
        return false;
    }


    //Start
    static void Start()
    {
        //Clear & Write Title
        Console.Clear();
        var title = new Table();
        title.Border = TableBorder.Heavy;
        title.Centered();
        title.AddColumn(new TableColumn("[magenta3]______            _    _                  _    _             \r\n| ___ \\          | |  | |                | |  (_)            \r\n| |_/ / __ _  ___| | _| |_ _ __ __ _  ___| | ___ _ __   __ _ \r\n| ___ \\/ _` |/ __| |/ / __| '__/ _` |/ __| |/ / | '_ \\ / _` |\r\n| |_/ / (_| | (__|   <| |_| | | (_| | (__|   <| | | | | (_| |\r\n\\____/ \\__,_|\\___|_|\\_\\\\__|_|  \\__,_|\\___|_|\\_\\_|_| |_|\\__, |\r\n                                                        __/ |\r\n                                                       |___/ [/]").Centered());
        AnsiConsole.Write(title);
        Console.WriteLine();


        // Startpunkt des Labyrinths
        int startX = 0;
        int startY = 0;

        // Rufe die rekursive Methode auf, um den Weg zu finden
        if (Step(startX, startY, -1, -1))
        {
            Console.WriteLine("Weg gefunden!");
        }
        else
        {
            Console.WriteLine("Kein Weg gefunden.");
        }
        Console.ReadKey();
    }



    static void ShowSpielfeld()
{
    // Clear the console
    Console.Clear();

    // Title with Spectre Console
    var title = new Table();
    title.Border = TableBorder.Heavy;
    title.Centered();
    title.AddColumn(new TableColumn("[magenta3]______            _    _                  _    _             \r\n| ___ \\          | |  | |                | |  (_)            \r\n| |_/ / __ _  ___| | _| |_ _ __ __ _  ___| | ___ _ __   __ _ \r\n| ___ \\/ _` |/ __| |/ / __| '__/ _` |/ __| |/ / | '_ \\ / _` |\r\n| |_/ / (_| | (__|   <| |_| | | (_| | (__|   <| | | | | (_| |\r\n\\____/ \\__,_|\\___|_|\\_\\\\__|_|  \\__,_|\\___|_|\\_\\_|_| |_|\\__, |\r\n                                                        __/ |\r\n                                                       |___/ [/]").Centered());
    AnsiConsole.Write(title);
    Console.WriteLine();

    int gridWidth = spielfeld.GetLength(1);
    int gridHeight = spielfeld.GetLength(0);

    // Calculate padding to center the grid based on console width
    int cellWidth = 2; // Each cell is 2 characters wide
    int totalGridWidth = gridWidth * cellWidth + 2; // 2 for the side borders
    int leftPadding = (Console.WindowWidth - totalGridWidth) / 2;

    // Draw the top border
    Console.Write(new string(' ', leftPadding)); // Centering padding
    Console.Write("┌");
    Console.Write(new string('─', gridWidth * cellWidth)); // Each cell width
    Console.WriteLine("┐");

    // Draw the grid with left and right borders
    for (int row = 0; row < gridHeight; row++)
    {
        Console.Write(new string(' ', leftPadding)); // Centering padding
        Console.Write("│"); // Left border
        for (int col = 0; col < gridWidth; col++)
        {
            // Check if the current cell contains "V" to color it
            if (spielfeld[row, col] == 'V')
            {
                // Print "V" in green
                AnsiConsole.Markup("[magenta3]V[/] ");
            }
            else if (spielfeld[row, col] == '#')
            {
                    // Print "V" in green
                    AnsiConsole.Markup("[yellow]#[/] ");
            }
            else
            {
                // Print other characters (e.g., X, 0) without color
                Console.Write(spielfeld[row, col] + " ");
            }
        }
        Console.WriteLine("│"); // Right border
    }

    // Draw the bottom border
    Console.Write(new string(' ', leftPadding)); // Centering padding
    Console.Write("└");
    Console.Write(new string('─', gridWidth * cellWidth)); // Each cell width
    Console.WriteLine("┘");

    Thread.Sleep(500); // Delay for animation
}



//Go Back to Main
    static void BackToMain()
    {
        Console.ReadKey();
    }

    //Exit Application
    static void Exit()
    {
        Environment.Exit(0);
    }
}
