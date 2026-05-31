
namespace Model
{
    public class StackPathFinder : IPathFinder
    {
        PathFinderType _algType = PathFinderType.Stack;
        public PathFinderType algType { get => _algType; set { } }

        public void FindPath(Maze maze, int[] pos, Queue<int[]> visitedPositions)
        {
            //ToDo implement this method
            int row = pos[0];
            int col = pos[1];
            Stack<int[]> stack = new Stack<int[]>();

            bool[,] visited = new bool[maze.MazeArray.Length, maze.MazeArray[0].Length];

            Stack<int[]> path = new Stack<int[]>();

            visited[row, col] = true;
            stack.Push(pos);


            while (stack.Count != 0)
            {
                var currentPos = stack.Pop();
                var currRow = currentPos[0];
                var currCol = currentPos[1];

                path.Push(currentPos);


                if (currRow == maze.End[0] && currCol == maze.End[1])
                {
                    foreach (var p in path.Reverse())
                    {
                        visitedPositions.Enqueue(p);
                    }
                    return;
                }
                bool foundNextMove = false;
                foreach (var move in maze.moves)
                {
                    var newRow = move[0] + currRow;
                    var newCol = move[1] + currCol;
                    if (maze.IsValidMove(newRow, newCol) && !visited[newRow, newCol])
                    {
                        visited[newRow, newCol] = true;

                        int[] newPos = [newRow, newCol];
                        stack.Push(newPos);
                        foundNextMove = true;
                    }
                }
                //if (!foundNextMove)
                  //  path.Pop();



            }

        }
    }
}



