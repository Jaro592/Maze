

using Model;

namespace View
{
    public class MazeView
    {
        public void DisplayMaze(Maze maze)
        {
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.BackgroundColor = ConsoleColor.White;
            var array = maze.MazeArray; // get the 1-D jagged maze grid

            Console.WriteLine("\n");

            for (int rowIdx = 0; rowIdx < array.Length; rowIdx++)
            {
                var row = array[rowIdx];
                for (int colIdx = 0; colIdx < row.Length; colIdx++)
                {
                    switch (row[colIdx])
                    {
                        case -1: Console.Write("🟦"); break;  // wall
                        case 1: Console.Write("🏠"); break;  // start
                        case 2: Console.Write("🍦"); break;  // end
                        case 0: Console.Write("  "); break;  // unvisited passage
                        case 10: Console.Write("🏅"); break;  // solved path cell
                        case 4: Console.Write("⚽️"); break;  // visited cell
                        default: break;
                    }
                }
                Console.WriteLine("🟦"); // right border of this row
            }

            for (int colIdx = 0; colIdx <= array[0].Length; colIdx++)
                Console.Write("🟦"); // bottom border
            Console.WriteLine("\n");
        }

        public void DisplayMaze(Maze maze, int[] currPos)
        {
            var array = maze.MazeArray;
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.BackgroundColor = ConsoleColor.White;
            Console.Clear();
            var rand = new Random();

            for (int rowIdx = 0; rowIdx < array.Length; rowIdx++)
            {
                var row = array[rowIdx];
                for (int colIdx = 0; colIdx < row.Length; colIdx++)
                {
                    switch (row[colIdx])
                    {
                        case -1:
                            Console.Write("🟦"); // wall
                            break;
                        case 1:
                            if (currPos[0] == rowIdx && currPos[1] == colIdx)
                                Console.Write("⚽️"); // player is at start
                            else
                                Console.Write("🏠"); // start tile
                            break;
                        case 2:
                            Console.Write("🍦"); // end tile
                            break;
                        case 0:
                            if (currPos[0] == rowIdx && currPos[1] == colIdx)
                                Console.Write("⚽️"); // player's current position
                            else
                                Console.Write("  "); // unvisited passage
                            break;
                        case 10:
                            Console.Write("🏅"); // solved path cell
                            break;
                        case 4:
                            if (currPos[0] == rowIdx && currPos[1] == colIdx)
                                Console.Write("⚽️"); // player on a previously visited cell
                            else
                            {
                                if (rand.NextDouble() < 0.3)
                                    Console.Write("🦖"); // random decoration for visited cells
                                else if (rand.NextDouble() >= 0.3 && rand.NextDouble() < 0.6)
                                    Console.Write("🦕");
                                else
                                    Console.Write("🐈");
                            }
                            break;
                        default:
                            break;
                    }
                }
                Console.WriteLine("🟦"); // right border
            }

            for (int colIdx = 0; colIdx <= array[0].Length; colIdx++)
                Console.Write("🟦"); // bottom border
            Console.WriteLine();
        }

        public void DisplayMaze(Maze maze, int[] currPos, string[] symbolsArr, Queue<int[]> visitedPositions, PathFinderType algType = PathFinderType.Manual)
        {
            var array = maze.MazeArray;
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.BackgroundColor = ConsoleColor.White;
            Console.Clear();

            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine($"\n\n{String.Concat(Enumerable.Repeat("🟨", maze.MazeMDArray.GetLength(1) / 2 - algType.ToString().Length / 3))}{"  " + algType + "  "}{String.Concat(Enumerable.Repeat("🟨", maze.MazeMDArray.GetLength(1) / 2 - algType.ToString().Length / 3))}"); // centered algorithm name banner
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.BackgroundColor = ConsoleColor.White;
            System.Console.WriteLine();

            var visitedList = visitedPositions.ToList(); // convert queue to list for indexed access
            int visitedCount = visitedList.Count;

            var positionIndexMap = new Dictionary<string, int>(visitedCount); // maps "row,col" → index in trail
            for (int i = 0; i < visitedCount; i++)
                positionIndexMap[$"{visitedList[i][0]},{visitedList[i][1]}"] = i; // 0 = oldest, n-1 = newest

            for (int rowIdx = 0; rowIdx < array.Length; rowIdx++)
            {
                var row = array[rowIdx];
                for (int colIdx = 0; colIdx < row.Length; colIdx++)
                {
                    switch (row[colIdx])
                    {
                        case -1:
                            Console.Write("🟦"); // wall
                            break;
                        case 1:
                            Console.Write("🏠"); // start tile
                            break;
                        case 2:
                            if (currPos[0] == rowIdx && currPos[1] == colIdx)
                                Console.Write("🏅"); // reached the end
                            else
                                Console.Write("🍦"); // end tile
                            break;
                        case 0:
                            if (currPos[0] == rowIdx && currPos[1] == colIdx)
                            {
                                Console.Write("⚽️"); // current head of the path
                            }
                            else if (positionIndexMap.TryGetValue($"{rowIdx},{colIdx}", out int posIdx))
                            {
                                int symIdx = (visitedCount - 1 - posIdx) % symbolsArr.Length; // newest cell → symbolsArr[0], cycles backward
                                Console.Write(symbolsArr[symIdx]); // snake-trail symbol for this cell
                            }
                            else
                                Console.Write("  "); // unvisited passage
                            break;
                        case 10:
                            Console.Write("🏅"); // solved path cell
                            break;
                        case 4:
                            if (currPos[0] == rowIdx && currPos[1] == colIdx)
                            {
                                Console.Write("⚽️"); // current head on a marked cell
                            }
                            else if (positionIndexMap.TryGetValue($"{rowIdx},{colIdx}", out int posIdx4))
                            {
                                int symIdx = (visitedCount - 1 - posIdx4) % symbolsArr.Length; // same cycling formula for marked cells
                                Console.Write(symbolsArr[symIdx]);
                            }
                            else
                            {
                                Console.Write("🏃"); // marked cell not part of the current trail
                            }
                            break;
                        default:
                            break;
                    }
                }
                Console.WriteLine("🟦"); // right border
            }

            for (int colIdx = 0; colIdx <= array[0].Length; colIdx++)
                Console.Write("🟦"); // bottom border

            if (algType == PathFinderType.Manual)
            {
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine("\n\n👉 👉 👉        Press S to go start again.    👈 👈 👈");
                Console.WriteLine("👉 👉 👉 Press M or ⬅️  to go back to the Menu. 👈 👈 👈\n");
            }

            if (!maze.IsValidMove(currPos[0], currPos[1], true))
                PrintWrongMove(currPos); // flash an error if the player stepped into a wall

            if (currPos[0] == maze.End[0] && currPos[1] == maze.End[1])
            {
                visitedPositions = new Queue<int[]>(); // reset trail on completion
                Console.WriteLine("\n");
                Console.WriteLine("👍 DONE!!! AMAZING!!! 👍");
                Thread.Sleep(300);
                return;
            }
        }

        public void DisplayMaze(Maze maze, string[] symbolsArr, int timeInterval, Queue<int[]> visitedPositions)
        {
            var array = maze.MazeMDArray; // 2-D array used by this animated overload

            var toBeShownPositions = new Queue<int[]>(visitedPositions); // positions waiting to be animated
            var shownPositions = new Queue<int[]>();                      // positions already revealed

            while (toBeShownPositions.Count > 0) // one iteration = one animation frame
            {
                Console.ForegroundColor = ConsoleColor.DarkBlue;
                Console.BackgroundColor = ConsoleColor.White;
                Console.Clear(); // wipe previous frame

                var currPos = toBeShownPositions.Dequeue(); // reveal the next cell this frame
                shownPositions.Enqueue(currPos);             // add it to the visible snake trail

                var shownList = shownPositions.ToList();
                int shownCount = shownList.Count; // current length of the snake trail

                var posMap = new Dictionary<string, int>(shownCount); // "row,col" → trail index
                for (int i = 0; i < shownCount; i++)
                    posMap[$"{shownList[i][0]},{shownList[i][1]}"] = i; // 0 = start, shownCount-1 = newest

                for (int rowIdx = 0; rowIdx < array.GetLength(0); rowIdx++)
                {
                    for (int colIdx = 0; colIdx < array.GetLength(1); colIdx++)
                    {
                        switch (array[rowIdx, colIdx])
                        {
                            case -1:
                                Console.Write("🟦"); // wall
                                break;
                            case 1:
                                if (currPos[0] == rowIdx && currPos[1] == colIdx)
                                    Console.Write("⚽️"); // player at start
                                else
                                    Console.Write("🏠"); // start tile
                                break;
                            case 2:
                                if (currPos[0] == rowIdx && currPos[1] == colIdx)
                                    Console.Write("🏅"); // reached the end
                                else
                                    Console.Write("🍦"); // end tile
                                break;
                            case 0:
                                if (currPos[0] == rowIdx && currPos[1] == colIdx)
                                {
                                    Console.Write("⚽️"); // snake head
                                }
                                else if (posMap.TryGetValue($"{rowIdx},{colIdx}", out int posIdx))
                                {
                                    int symIdx = (shownCount - 1 - posIdx) % symbolsArr.Length; // cycle symbols from head toward tail
                                    Console.Write(symbolsArr[symIdx]); // snake body symbol
                                }
                                else
                                    Console.Write("  "); // unvisited cell
                                break;
                            case 10:
                                Console.Write("🏅"); // solved path cell
                                break;
                            case 4:
                                if (currPos[0] == rowIdx && currPos[1] == colIdx)
                                {
                                    Console.Write("⚽️"); // snake head on a marked cell
                                }
                                else if (posMap.TryGetValue($"{rowIdx},{colIdx}", out int posIdx4))
                                {
                                    int symIdx = (shownCount - 1 - posIdx4) % symbolsArr.Length; // same cycling formula
                                    Console.Write(symbolsArr[symIdx]);
                                }
                                else
                                {
                                    Console.Write("🏃"); // marked but not in the trail
                                }
                                break;
                            default:
                                break;
                        }
                    }
                    Console.WriteLine("🟦"); // right border
                }

                for (int colIdx = 0; colIdx <= array.GetLength(1); colIdx++)
                    Console.Write("🟦"); // bottom border
                Console.WriteLine();

                Thread.Sleep(timeInterval); // pause so the frame is visible
            }
        }

        public void DisplayMaze(Maze maze, string[] symbolsArr, int timeInterval, Queue<int[]> visitedPositions, PathFinderType algType = PathFinderType.Manual)
        {
            var array = maze.MazeMDArray;

            var toBeShownPositions = new Queue<int[]>(visitedPositions); // positions to animate
            var shownPositions = new Queue<int[]>();                      // positions already drawn
            var finalPathPositions = new Queue<string>();
            bool showingRed = false;
            bool showingResult = false;

            while (toBeShownPositions.Count > 0) // one iteration = one animation frame
            {
                var currPos = toBeShownPositions.Dequeue(); // cell being revealed this frame

                if (currPos[0] == -998 && currPos[1] == -998)
                {
                    showingRed = true;
                    continue;
                }
                if (currPos[0] == -999 && currPos[1] == -999)
                {
                    showingResult = true;
                    continue;
                }
                if (showingResult)
                    finalPathPositions.Enqueue($"{currPos[0]},{currPos[1]}");

                Console.ForegroundColor = ConsoleColor.DarkBlue;
                Console.BackgroundColor = ConsoleColor.White;
                Console.Clear(); // wipe previous frame

                shownPositions.Enqueue(currPos);             // add to the growing snake trail

                var shownList = shownPositions.ToList();
                int shownCount = shownList.Count; // snake trail length for this frame

                var posMap = new Dictionary<string, int>(shownCount); // "row,col" → trail index
                for (int i = 0; i < shownCount; i++)
                    posMap[$"{shownList[i][0]},{shownList[i][1]}"] = i;

                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine($"\n\n{String.Concat(Enumerable.Repeat("🟨", maze.MazeMDArray.GetLength(1) / 2 - algType.ToString().Length / 3))}{"  " + algType + "  "}{String.Concat(Enumerable.Repeat("🟨", maze.MazeMDArray.GetLength(1) / 2 - algType.ToString().Length / 3))}"); // algorithm name banner
                Console.ForegroundColor = ConsoleColor.DarkBlue;
                Console.BackgroundColor = ConsoleColor.White;
                Console.WriteLine();

                for (int rowIdx = 0; rowIdx < array.GetLength(0); rowIdx++)
                {
                    for (int colIdx = 0; colIdx < array.GetLength(1); colIdx++)
                    {
                        switch (array[rowIdx, colIdx])
                        {
                            case -1:
                                Console.Write("🟦"); // wall
                                break;
                            case 1:
                                if (currPos[0] == rowIdx && currPos[1] == colIdx)
                                    Console.Write("⚽️"); // player at start
                                else
                                    Console.Write("🏠"); // start tile
                                break;
                            case 2:
                                if (currPos[0] == rowIdx && currPos[1] == colIdx)
                                    Console.Write("🏅"); // reached the end
                                else
                                    Console.Write("🍦"); // end tile
                                break;
                            case 0:
                                if (currPos[0] == rowIdx && currPos[1] == colIdx)
                                {
                                    Console.Write(showingResult ? "🟩" : "⚽️"); // snake head
                                }
                                else if (finalPathPositions.Contains($"{rowIdx},{colIdx}"))
                                {
                                    Console.Write("🟩"); // groen = finale pad
                                }
                                else if (posMap.TryGetValue($"{rowIdx},{colIdx}", out int posIdx))
                                {
                                    if (showingRed || showingResult)
                                        Console.Write("🟥"); // rood = bezochte exploratie nodes
                                    else
                                    {
                                        int symIdx = (shownCount - 1 - posIdx) % symbolsArr.Length;
                                        Console.Write(symbolsArr[symIdx]);
                                    }
                                }
                                else
                                    Console.Write("  "); // unvisited passage
                                break;
                            case 10:
                                Console.Write("🏅"); // solved path cell
                                break;
                            case 4:
                                if (currPos[0] == rowIdx && currPos[1] == colIdx)
                                {
                                    Console.Write("⚽️"); // snake head on a marked cell
                                }
                                else if (finalPathPositions.Contains($"{rowIdx},{colIdx}"))
                                {
                                    Console.Write("🟩"); // groen = finale pad op marked cell
                                }
                                else if (posMap.TryGetValue($"{rowIdx},{colIdx}", out int posIdx4))
                                {
                                    if (showingRed || showingResult)
                                        Console.Write("🟥"); // rood = bezochte marked cell
                                    else
                                    {
                                        int symIdx = (shownCount - 1 - posIdx4) % symbolsArr.Length;
                                        Console.Write(symbolsArr[symIdx]); // emoji tijdens exploratie
                                    }
                                }
                                else
                                {
                                    Console.Write("🏃"); // marked cell outside the trail
                                }
                                break;
                            default:
                                break;
                        }
                    }
                    Console.WriteLine("🟦"); // right border
                }

                for (int colIdx = 0; colIdx <= array.GetLength(1); colIdx++)
                    Console.Write("🟦"); // bottom border
                Console.WriteLine();

                Thread.Sleep(timeInterval); // delay between frames
            }
        }

        public string[] generateSymbols(int spaces)
        {
            string[] palette = { "🦖", "🐈", "🦕", "🦊", "🐢", "🦜" }; // available trail emojis

            var symbols = new string[Math.Max(8, 2 * spaces)]; // at least 8 symbols, scales with maze width

            for (int i = 0; i < symbols.Length; i++)
                symbols[i] = palette[i % palette.Length]; // cycle through the palette to fill the array

            return symbols;
        }

        public void DisplaySuccess(bool success, string msg, int timeInterval)
        {
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.WriteLine(success ? msg + "🎉 Path found! 🎊" : msg + "🔎  No path found. 🔎 "); // print result
            if (!success)
            {
                Console.WriteLine("press any key to continue...");
                Console.ReadLine(); // hold failure message so the user can read it
            }
        }

        private void PrintWrongMove(int[] tmppos)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.BackgroundColor = ConsoleColor.DarkMagenta;
            Console.WriteLine($"Wrong Direction -> {tmppos[0]}, {tmppos[1]}"); // show invalid position
            Thread.Sleep(100);
            Console.BackgroundColor = ConsoleColor.White;
        }
    }
}
