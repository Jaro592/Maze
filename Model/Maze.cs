
using System.Data;

namespace Model
{
    public enum MazeGeneratorType
    {
        DFS,
        BinaryTree,

    }
    public class Maze
    {
        public int[][] MazeArray { get; private set; }
        public int[,] MazeMDArray { get; private set; }
        public int[] Begin { get; private set; }
        public int[] End { get; private set; }

        public readonly int[][] moves = {
            new int[] {  1,  0 },  //down
            new int[] { -1,  0 },  //up
            new int[] {  0, -1 },  //left
            new int[] {  0,  1 },  //right
            };

        public Maze() => GenerateMaze();
        public Maze(bool automatic = true) { if (automatic) GenerateMaze(); else GenerateFromText(MazeGrids.mazeText); }
        public Maze(int rows, int cols) { if (rows <= 0 && cols <= 0) GenerateFromText(MazeGrids.mazeText); else GenerateMaze(rows, cols); }

        ///
        public Maze(int rows, int cols, MazeGeneratorType generatorType)
        {
            GenerateMaze(rows, cols, generatorType);
        }
        ///
        public Maze(string lines) => GenerateFromText(lines);

        void GenerateFromText(string lines)
        {
            MazeArray = ToMazeArray(lines);
            MazeMDArray = ToMazeMDArray(lines);
        }

        void GenerateMaze(int rows = 20, int cols = 40, MazeGeneratorType generatorType = MazeGeneratorType.BinaryTree)
        {
            if (rows < 4 || cols < 4)
            {
                rows = 20;
                cols = 49;
            }

            if (rows % 2 != 0) rows++;
            if (cols % 2 != 0) cols++;

            MazeMDArray = new int[rows, cols];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    MazeMDArray[i, j] = -1;
                }
            }

            Begin = new int[] { 1, 1 };

            if (generatorType == MazeGeneratorType.DFS)
            {
                End = new int[] { rows - 2, cols - 2 };
                Carve(1, 1, rows, cols);
            }
            else
            {
                End = new int[] { rows - 3, cols - 3 };
                CarveBinaryTree(rows, cols);
            }

            MazeMDArray[Begin[0], Begin[1]] = 1;
            MazeMDArray[End[0], End[1]] = 2;

            MazeArray = ToJaggedArray(MazeMDArray, rows, cols);
        }

        private int[][] ToJaggedArray(int[,] mdArray, int rows, int cols)
        {
            int[][] jagged = new int[rows][];
            for (int i = 0; i < rows; i++)
            {
                jagged[i] = new int[cols];
                for (int j = 0; j < cols; j++)
                {
                    jagged[i][j] = mdArray[i, j];
                }
            }
            return jagged;
        }

        private void Carve(int startX, int startY, int rows, int cols)
        {
            MazeMDArray[startX, startY] = 0;
            var rng = new Random();
            var dirs = moves.OrderBy(_ => rng.Next()).ToArray();

            foreach (var dir in dirs)
            {
                int neighborRow = startX + dir[0] * 2;
                int neighborCol = startY + dir[1] * 2;
                int wallRow = startX + dir[0];
                int wallCol = startY + dir[1];

                if (neighborRow >= 0 && neighborCol >= 0 && neighborRow < rows && neighborCol < cols && MazeMDArray[neighborRow, neighborCol] == -1)
                {
                    MazeMDArray[wallRow, wallCol] = 0;
                    Carve(neighborRow, neighborCol, rows, cols);
                }
            }

        }
        private void CarveBinaryTree(int rows, int cols)
        {
            var rng = new Random();

            for (int row = 1; row < rows; row += 2)
            {
                for (int col = 1; col < cols; col += 2)
                {
                    MazeMDArray[row, col] = 0;

                    if (row == 1 && col == 1)
                        continue;

                    if (row == 1)
                        MazeMDArray[row, col - 1] = 0;

                    else if (col == 1)
                        MazeMDArray[row - 1, col] = 0;

                    else
                    {
                        if (rng.Next(2) == 0)
                            MazeMDArray[row - 1, col] = 0;
                        else
                            MazeMDArray[row, col - 1] = 0;
                    }
                }
            }
        }

        int[][] ToMazeArray(string maze)
        {
            // substrings from the maze string
            var arrayLines = maze.Split(new char[] { '.', '\n', '\r' },
                StringSplitOptions.RemoveEmptyEntries);

            int[][] outArray = new int[arrayLines.Length][];

            for (var rowIdx = 0; rowIdx < arrayLines.Length; rowIdx++)
            {
                var line = arrayLines[rowIdx];

                // row array:
                var row = new int[line.Length];
                for (int colIdx = 0; colIdx < line.Length; colIdx++)
                {
                    //from chars to integers
                    switch (line[colIdx])
                    {
                        case 'x':
                            row[colIdx] = -1;  //walls
                            break;
                        case '1':
                            row[colIdx] = 1;   //begin
                            Begin = [rowIdx, colIdx];
                            break;
                        case '2':
                            row[colIdx] = 2;   //end 
                            End = [rowIdx, colIdx];
                            break;
                        default:
                            row[colIdx] = 0;   //not visited
                            break;
                    }
                }
                // row in the output jagged array.
                outArray[rowIdx] = row;
            }

            return outArray;

        }

        int[,] ToMazeMDArray(string maze)
        {
            // substrings from the maze string
            var arrayLines = maze.Split(new char[] { '.', '\n', '\r' },
                StringSplitOptions.RemoveEmptyEntries);

            var lineLength = 0;
            if (arrayLines != null && arrayLines.Length > 0)
                lineLength = arrayLines[0].Length;
            else
                throw new Exception($"Maze incorrect");

            for (var rowIdx = 0; arrayLines != null && rowIdx < arrayLines.Length; rowIdx++)
            {
                var line = arrayLines[rowIdx];
                if (arrayLines[rowIdx] == null || line.Length != lineLength)
                    throw new Exception($"Not same line length for rows in maze:\n at row 0: {lineLength}, at row {rowIdx}: {line.Length}");
            }

            int[,] outArray = new int[arrayLines.Length, lineLength];

            for (var rowIdx = 0; rowIdx < arrayLines.Length; rowIdx++)
            {
                var line = arrayLines[rowIdx];

                for (int colIdx = 0; colIdx < line.Length; colIdx++)
                {
                    //from chars to integers
                    switch (line[colIdx])
                    {
                        case 'x':
                            outArray[rowIdx, colIdx] = -1;  //walls
                            break;
                        case '1':
                            outArray[rowIdx, colIdx] = 1;   //begin
                            Begin = [rowIdx, colIdx];
                            break;
                        case '2':
                            outArray[rowIdx, colIdx] = 2;   //end 
                            End = [rowIdx, colIdx];
                            break;
                        default:
                            outArray[rowIdx, colIdx] = 0;   //not visited
                            break;
                    }
                }
            }
            return outArray;
        }

        static int CountNotVisited(int[][] maze)
        {
            int cnt = 0;
            if (maze != null && maze.Length > 0)
            {
                for (int rowIdx = 0; rowIdx < maze.Length; rowIdx++)
                {
                    for (int colIdx = 0; maze[rowIdx] != null && colIdx < maze[rowIdx].Length; colIdx++)
                    {
                        cnt = maze[rowIdx][colIdx] == 0 ? cnt + 1 : cnt;
                    }
                }
            }
            return cnt;
        }

        public int CountNotVisited() => CountNotVisited(MazeArray);

        static bool IsValidPos(int[][] array, int newRow, int newColumn)
        {
            // ... Ensure position is within the array bounds.
            /*
            if (newRow < 0) return false;
            if (newColumn < 0) return false;
            if (newRow >= array.Length) return false;
            if (newColumn >= array[newRow].Length) return false;
            return true;
            */
            return !(newRow < 0)
                    && !(newColumn < 0)
                    && !(newRow >= array.Length)
                    && !(newColumn >= array[newRow].Length);
        }

        // Make sure the position is within the maze array bounds.
        // no walls
        public bool IsValidMove(int newRow, int newColumn) =>
            IsValidPos(MazeArray, newRow, newColumn) &&
            !(MazeArray[newRow][newColumn] == -1); //no walls 

        //Marking strategy
        public bool IsValidMove(int newRow, int newColumn, bool notVisited = true)
        {
            // Make sure the position is within the maze array bounds.
            // no walls, not yet visited ? (flag notVisited: false)
            return notVisited ?
                    IsValidPos(MazeArray, newRow, newColumn) &&
                    !(MazeArray[newRow][newColumn] == -1)  //no walls, but already visited -> ok
                    :
                    IsValidPos(MazeArray, newRow, newColumn) &&
                    !(MazeArray[newRow][newColumn] == -1 || MazeArray[newRow][newColumn] == 4); //no walls, not yet visited 
        }

    }

    public static class MazeGrids
    {
        public static string mazeText = @"
xxxxxx1xxxxxxxxxxxxxxxxxxxxxxx.
 x   x   x                    .
xx2x xxx   x xxxxxxxx    x xx .
x  x xxxxxxx xxxxxxxxxxxxx xxx.
 x x xx      x                .
x  x xx xxxxx  x xxxx xxxxx  x.
xx    x xxx   xx xxx  xxx   xx.
xxx   xxx   x xxxx   xx   x xx.
xx     xx   x xxxx   xx   x xx.
xxxx    xxxxx xx xxxx xxxxx xx.
xx            xx            xx.";
    }
}







