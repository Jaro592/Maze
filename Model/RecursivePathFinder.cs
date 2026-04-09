
namespace Model
{
    public class RecursivePathFinder : IPathFinder
    {
        PathFinderType _algType = PathFinderType.Recursive;
        public PathFinderType algType { get => _algType; set {} }

        public void FindPath(Maze maze, int[] pos, Queue<int[]> visitedPositions) // akif
        {
            bool[,] visisted = new bool[maze.MazeArray.Length, maze.MazeArray[0].Length]; // new 2d arrey with the same dimensions as the maze to keep track of which cell has been visisted
            List<int[]> path = new List<int[]>();
            Solve(maze, pos[0], pos[1], visisted, path); // enters the recursion starting from the first coordinate

            foreach (var posloop in path) visitedPositions.Enqueue(posloop);
        }

        private bool Solve(Maze maze, int row, int col, bool[,] visited, List<int[]> path) // akif
        {
            if (!maze.IsValidMove(row, col) || visited[row, col]) return false;  // if on a invalid location (out of bounds/already visited)
            visited[row, col] = true; // keeping track of visisted
            path.Add(new int[] {row, col});            

            if (maze.MazeArray[row][col] == 2) // 2 : goal, if current coords are equal to the goal just return true
                return true;

            foreach (var move in maze.moves) // call solve for every possible move
            {
                int newRow = row + move[0];
                int newCol = col + move[1];

                if (Solve(maze, newRow, newCol, visited, path)) // if any of the called solve's return true return true
                    return true;
            }

            path.RemoveAt(path.Count - 1);
            return false;
        }
    }
}
