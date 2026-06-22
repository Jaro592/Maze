namespace Model
{
    public class BFSPathFinder : IPathFinder
    {
        PathFinderType _algType = PathFinderType.BFS;
        public PathFinderType algType { get => _algType; set { } }

        public int ExplorationSteps { get; set; } = 0;
        public int PathLength { get; set; } = 0;

        public void FindPath(Maze maze, int[] pos, Queue<int[]> visitedPositions)
        {
            int row = pos[0];
            int col = pos[1];

            Queue<int[]> queue = new Queue<int[]>();

            bool[,] visited = new bool[maze.MazeArray.Length, maze.MazeArray[0].Length];
            int rows = maze.MazeArray.Length;
            int cols = maze.MazeArray[0].Length;

            var prevRow = new int[rows, cols];
            var prevCol = new int[rows, cols];

            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    prevRow[r, c] = -1;
                    prevCol[r, c] = -1;
                }
            }

            visited[row, col] = true;
            queue.Enqueue(pos);

            while (queue.Count != 0)
            {
                var currentPos = queue.Dequeue();
                ExplorationSteps++;

                visitedPositions.Enqueue(currentPos);

                var currRow = currentPos[0];
                var currCol = currentPos[1];

                if (currRow == maze.End[0] && currCol == maze.End[1])
                {

                    break;
                }

                foreach (var move in maze.moves)
                {
                    int newRow = currRow + move[0];
                    int newCol = currCol + move[1];

                    // if (maze.IsValidMove(newRow, newCol) && !visited[newRow, newCol])
                    // {
                    //     visited[newRow, newCol] = true;

                    //     int[] newPos = { newRow, newCol };

                    //     queue.Enqueue(newPos);
                    // }
                    if (maze.IsValidMove(newRow, newCol) && !visited[newRow, newCol])
                    {
                        visited[newRow, newCol] = true;

                        prevRow[newRow, newCol] = currRow;
                        prevCol[newRow, newCol] = currCol;

                        int[] newPos = { newRow, newCol };

                        queue.Enqueue(newPos);
                    }
                }
            }
            visitedPositions.Enqueue(new int[] { -998, -998 });
            visitedPositions.Enqueue(new int[] { -999, -999 });

            var path = new Stack<int[]>();

            int[] current = maze.End;

            while (!(current[0] == pos[0] && current[1] == pos[1]))
            {
                path.Push(current);

                int r = current[0];
                int c = current[1];

                if (prevRow[r, c] == -1)
                    return;

                current = new int[]
                {
        prevRow[r, c],
        prevCol[r, c]
                };
            }
            path.Push(pos);
            PathLength = path.Count;
            while (path.Count > 0)
            {
                visitedPositions.Enqueue(path.Pop());
            }
        }

    }
}