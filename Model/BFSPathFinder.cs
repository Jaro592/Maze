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
                    PathLength = 0; // Initialize path length
                    return;
                }

                foreach (var move in maze.moves)
                {
                    int newRow = currRow + move[0];
                    int newCol = currCol + move[1];

                    if (maze.IsValidMove(newRow, newCol) && !visited[newRow, newCol])
                    {
                        visited[newRow, newCol] = true;

                        int[] newPos = { newRow, newCol };

                        queue.Enqueue(newPos);
                    }
                }
            }
        }
    }
}