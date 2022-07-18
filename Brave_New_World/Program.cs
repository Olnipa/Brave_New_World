namespace Brave_New_World
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.CursorVisible = false;

            bool isAlive = true;
            int cycleWhile = 0;
            int updateMapCycle = 0;
            int gameSpeed = -1;
            int characterPositionX = 2;
            int characterPositionY = 2;
            char whole = '#';
            char levelUp = '1';
            char enemyA = 'A';
            char characterView = '0';
            string levelToWin = "9";
            char[,] map = GenerateMap(whole, enemyA, levelUp);

            Console.SetCursorPosition(0, 0);
            Console.WriteLine("Help Mr. Zero grow to the Nine.\n" +
                "You need to eat \"1\" for growing. Use Up and Down Arrow keys for contol.\nPress any key to start...");
            Console.ReadKey(true);
            Console.WriteLine("\nChoose difficulty level:\n1 - Child\n2 - Advanced\n3 - Pro-Gamer\n4 - Impossible");

            while (gameSpeed == -1)
            {
                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.D1:
                        gameSpeed = 100;
                        break;
                    case ConsoleKey.D2:
                        gameSpeed = 60;
                        break;
                    case ConsoleKey.D3:
                        gameSpeed = 45;
                        break;
                    case ConsoleKey.D4:
                        gameSpeed = 4;
                        break;
                }
            }

            Console.Clear();
            Console.SetCursorPosition(0, 0);

            DrawMap(map);
            DrawCharacter(characterView, characterPositionX, characterPositionY);

            Console.ReadKey();

            while (isAlive && characterView != Convert.ToChar(levelToWin))
            {
                Console.SetCursorPosition(0, 0);

                DrawMap(map);
                DrawCharacter(characterView, characterPositionX, characterPositionY);
                ReadMovementKey(ref characterPositionX, characterPositionY, ref map, whole, enemyA, levelUp, ref characterView, ref isAlive, ref gameSpeed);
                MoveArray(ref map, characterPositionX, characterPositionY, enemyA, levelUp, whole, ref characterView, ref isAlive, ref updateMapCycle, cycleWhile);
                CheckPosition(ref characterPositionX, characterPositionY, ref map, whole, enemyA, levelUp, ref characterView, ref isAlive, ref gameSpeed);

                cycleWhile++;
                System.Threading.Thread.Sleep(gameSpeed);
            }

            if (isAlive == false)
            {
                Console.SetCursorPosition(0, 7);
                Console.WriteLine("############### Game Over ###############");
            }
            else if (characterView == Convert.ToChar(levelToWin))
            {
                Console.SetCursorPosition(0, 7);
                Console.WriteLine("############### Congratulations! You Win! ###############\n");
            }

            Console.ReadKey(true);
            Console.WriteLine($"Score:\nMax. level: {characterView}\nDistance travalled: {updateMapCycle}");
            Console.SetCursorPosition(0, 12);
            Console.WriteLine("If you want to play again, press any key. Or press \"0\" to exit.");

            switch (Console.ReadKey(true).Key)
            {
                case ConsoleKey.D0:
                case ConsoleKey.NumPad0:
                    Console.WriteLine("Goodbye!");
                    break;
                default:
                    Console.Clear();

                    Main(args);

                    break;
            }
        }

        static void ReadMovementKey(ref int characterPositionX, int characterPositionY, ref char[,] map, char whole, char enemyA, char levelUp, ref char characterView, ref bool isAlive, ref int gameSpeed)
        {
            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                int movementWay;

                switch (key.Key)
                {
                    case ConsoleKey.UpArrow:
                        movementWay = -1;
                        MoveCharacter(ref characterPositionX, characterPositionY, ref map, movementWay, whole, enemyA, levelUp, ref characterView, ref isAlive, ref gameSpeed);
                        break;
                    case ConsoleKey.DownArrow:
                        movementWay = 1;
                        MoveCharacter(ref characterPositionX, characterPositionY, ref map, movementWay, whole, enemyA, levelUp, ref characterView, ref isAlive, ref gameSpeed);
                        break;
                }
            }
        }

        static void DrawCharacter(char characterView, int characterPositionX, int characterPositionY)
        {
            ConsoleColor backgroundColor = Console.BackgroundColor;
            Console.SetCursorPosition(characterPositionY, characterPositionX);
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.Write(characterView);
            Console.BackgroundColor = backgroundColor;
        }

        static void MoveArray(ref char[,] map, int characterPositionX, int characterPositionY, char enemyA, char levelUp, char whole, ref char characterView, ref bool isAlive, ref int updateMapCycle, int cycleWhile)
        {
            int cycleForMoveMap = 4;

            if (cycleWhile % cycleForMoveMap == 0)
            {
                char[,] tempMapArray = new char[map.GetLength(0), map.GetLength(1)];

                for (int i = 0; i < map.GetLength(0); i++)
                {
                    for (int j = 0; j < map.GetLength(1) - 1; j++)
                    {
                        if (j != map.GetLength(1) - 1)
                        {
                            tempMapArray[i, j] = map[i, j + 1];
                        }
                    }

                    if (i == 0 || i == map.GetLength(0) - 1)
                    {
                        tempMapArray[i, map.GetLength(1) - 1] = whole;
                    }
                    else if (updateMapCycle % 2 == 0)
                    {
                        tempMapArray[i, map.GetLength(1) - 1] = GenerateMapContent(i, map.GetLength(1) - 1, tempMapArray, enemyA, levelUp);
                    }
                    else
                    {
                        tempMapArray[i, map.GetLength(1) - 1] = ' ';
                    }
                }
                updateMapCycle++;
                map = tempMapArray;
            }
        }

        static void MoveCharacter(ref int characterPositionX, int characterPositionY, ref char[,] map, int movementWay, char whole, char enemyA, char levelUp, ref char characterView, ref bool isAlive, ref int gameSpeed)
        {
            if (map[characterPositionX + movementWay, characterPositionY] != whole)
            {
                Console.SetCursorPosition(characterPositionY, characterPositionX);
                Console.Write(' ');
                characterPositionX += movementWay;

                DrawCharacter(characterView, characterPositionX, characterPositionY);
                CheckPosition(ref characterPositionX, characterPositionY, ref map, whole, enemyA, levelUp, ref characterView, ref isAlive, ref gameSpeed);
            }
        }

        static void CheckPosition(ref int characterPositionX, int characterPositionY, ref char[,] map, char whole, char enemyA, char levelUp, ref char characterView, ref bool isAlive, ref int gameSpeed)
        {
            int speedUp = 5;

            if (map[characterPositionX, characterPositionY] == levelUp)
            {
                characterView = Convert.ToChar(Convert.ToInt32(characterView) + 1);
                map[characterPositionX, characterPositionY] = ' ';
                if (gameSpeed > speedUp)
                {
                    gameSpeed -= speedUp;
                }
            }
            else if (map[characterPositionX, characterPositionY] == enemyA)
            {
                isAlive = false;
            }
        }

        static void DrawMap(char[,] array)
        {
            for (int i = 0; i < array.GetLength(0); i++)
            {
                for (int j = 0; j < array.GetLength(1); j++)
                {
                    Console.Write(array[i, j]);
                }
                Console.WriteLine();
            }
        }

        static char[,] GenerateMap(char whole, char enemyA, char levelUp, int i = 0, int j = 0)
        {
            int mapLines = 5;
            int mapColumns = 90;
            char[,] map = new char[mapLines, mapColumns];

            int mapContentFrequency = 2;
            int mapContentStartPosition = 6;

            for (i = 0; i < mapLines; i++)
            {
                for (j = 0; j < mapColumns; j++)
                {
                    if (i == 0 || i == map.GetLength(0) - 1)
                    {
                        map[i, j] = whole;
                    }
                    else if (j % mapContentFrequency == 0 && j >= mapContentStartPosition)
                    {
                        map[i, j] = GenerateMapContent(i, j, map, enemyA, levelUp);
                    }
                    else
                    {
                        map[i, j] = ' ';
                    }
                }
            }

            return map;
        }

        static char GenerateMapContent(int currentMapLine, int currentMapColumn, char[,] map, char enemyA, char levelUp)
        {
            char randomMapObject = '0';
            int chanceToGenerateA;
            int chanceToGenerate1;

            switch (currentMapLine)
            {
                case 1:
                    chanceToGenerateA = 33;
                    chanceToGenerate1 = 5;
                    randomMapObject = GenerateRandomObject(chanceToGenerateA, chanceToGenerate1, enemyA, levelUp);
                    break;
                case 2:

                    switch (map[currentMapLine - 1, currentMapColumn])
                    {
                        case 'A':
                            chanceToGenerateA = 0;
                            chanceToGenerate1 = 5;
                            randomMapObject = GenerateRandomObject(chanceToGenerateA, chanceToGenerate1, enemyA, levelUp);
                            break;
                        case '1':
                            chanceToGenerateA = 33;
                            chanceToGenerate1 = 0;
                            randomMapObject = GenerateRandomObject(chanceToGenerateA, chanceToGenerate1, enemyA, levelUp);
                            break;
                        default:
                            chanceToGenerateA = 33;
                            chanceToGenerate1 = 5;
                            randomMapObject = GenerateRandomObject(chanceToGenerateA, chanceToGenerate1, enemyA, levelUp);
                            break;
                    }

                    break;
                case 3:

                    if ((map[currentMapLine - 1, currentMapColumn] == enemyA || map[currentMapLine - 1, currentMapColumn] == levelUp) 
                        && (map[currentMapLine - 2, currentMapColumn] == enemyA || map[currentMapLine - 2, currentMapColumn] == levelUp))
                    {
                        chanceToGenerateA = 0;
                        chanceToGenerate1 = 0;
                        randomMapObject = GenerateRandomObject(chanceToGenerateA, chanceToGenerate1, enemyA, levelUp);
                    }
                    else if (map[currentMapLine - 1, currentMapColumn] == enemyA || map[currentMapLine - 2, currentMapColumn] == enemyA)
                    {
                        chanceToGenerateA = 0;
                        chanceToGenerate1 = 5;
                        randomMapObject = GenerateRandomObject(chanceToGenerateA, chanceToGenerate1, enemyA, levelUp);
                    }
                    else if (map[currentMapLine - 1, currentMapColumn] == levelUp || map[currentMapLine - 2, currentMapColumn] == levelUp)
                    {
                        chanceToGenerateA = 33;
                        chanceToGenerate1 = 0;
                        randomMapObject = GenerateRandomObject(chanceToGenerateA, chanceToGenerate1, enemyA, levelUp);
                    }
                    else
                    {
                        chanceToGenerateA = 100;
                        chanceToGenerate1 = 0;
                        randomMapObject = GenerateRandomObject(chanceToGenerateA, chanceToGenerate1, enemyA, levelUp);
                    }

                    break;
            }

            return randomMapObject;
        }

        static char GenerateRandomObject(int chanceA, int chance1, char enemyA, char levelUp)
        {
            Random random = new Random();
            char randomMapObject;
            int randomMapObjectIndex = random.Next(1, 101);

            if (randomMapObjectIndex <= chance1)
            {
                randomMapObject = levelUp;
            }
            else if (randomMapObjectIndex > chance1 && randomMapObjectIndex <= chance1 + chanceA)
            {
                randomMapObject = enemyA;
            }
            else
            {
                randomMapObject = ' ';
            }

            return randomMapObject;
        }
    }
}