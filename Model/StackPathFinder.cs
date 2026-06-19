
namespace Model
{
    public class StackPathFinder : IPathFinder
    {
        PathFinderType _algType = PathFinderType.Stack;
        public PathFinderType algType { get => _algType; set { } }

        public int ExplorationSteps { get; set; } = 0;
        public int PathLength { get; set; } = 0;

        public void FindPath(Maze maze, int[] pos, Queue<int[]> visitedPositions)  // (1,1) => (1,2)rigth, (2,1)down
        {
            //stack()(1,2)  (2,1) () 
            int row = pos[0];
            int col = pos[1];
            Stack<int[]> stack = new Stack<int[]>();

            bool[,] visited = new bool[maze.MazeArray.Length, maze.MazeArray[0].Length];



            visited[row, col] = true;
            stack.Push(pos);


            while (stack.Count != 0)
            {
                var currentPos = stack.Pop();
                visitedPositions.Enqueue(currentPos);
                ExplorationSteps++;
                var currRow = currentPos[0];
                var currCol = currentPos[1];



                if (currRow == maze.End[0] && currCol == maze.End[1])
                {
                    PathLength = visitedPositions.Count;
                    return;
                }
                foreach (var move in maze.moves)
                {
                    var newRow = move[0] + currRow;
                    var newCol = move[1] + currCol;
                    if (maze.IsValidMove(newRow, newCol) && !visited[newRow, newCol])
                    {
                        visited[newRow, newCol] = true;

                        int[] newPos = [newRow, newCol];
                        stack.Push(newPos);
                    }
                }




            }

        }
    }
}



