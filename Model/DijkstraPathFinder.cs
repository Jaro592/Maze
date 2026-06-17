namespace Model
{
    public class DijkstraPathFinder : IPathFinder
    {
        PathFinderType _algType = PathFinderType.Dijkstra;
        public PathFinderType algType { get => _algType; set {} }

        public void FindPath(Maze maze, int[] pos, Queue<int[]> visitedPositions)
        {
            int[] start = pos;
            int[] end = maze.End;

            int rows = maze.MazeArray.Length;
            int cols = maze.MazeArray[0].Length;

            var unvisitedNodes = new List<int[]>();
            var distance = new double[rows, cols];
            var prevRow = new int[rows, cols];
            var prevCol = new int[rows, cols];

            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < maze.MazeArray[r].Length; c++)
                {
                    if (maze.IsValidMove(r, c))
                    {
                        distance[r, c] = double.PositiveInfinity;
                        prevRow[r, c] = -1;
                        prevCol[r, c] = -1;
                        unvisitedNodes.Add(new int[] { r, c });
                    }
                }
            }

            distance[start[0], start[1]] = 0;

            while (unvisitedNodes.Count > 0)
            {
                int[] closestNode = unvisitedNodes[0];
                foreach (var node in unvisitedNodes)
                {
                    if (distance[node[0], node[1]] < distance[closestNode[0], closestNode[1]])
                        closestNode = node;
                }

                if (distance[closestNode[0], closestNode[1]] == double.PositiveInfinity)
                    break;

                unvisitedNodes.Remove(closestNode);
                visitedPositions.Enqueue(closestNode);

                if (closestNode.SequenceEqual(end))
                    break;

                foreach (var move in maze.moves)
                {
                    int newRow = closestNode[0] + move[0];
                    int newCol = closestNode[1] + move[1];

                    if (!maze.IsValidMove(newRow, newCol)) continue;

                    double difRoute = distance[closestNode[0], closestNode[1]] + 1;

                    if (difRoute < distance[newRow, newCol])
                    {
                        distance[newRow, newCol] = difRoute;
                        prevRow[newRow, newCol] = closestNode[0];
                        prevCol[newRow, newCol] = closestNode[1];
                    }
                }
            }
            visitedPositions.Enqueue(new int[] { -998, -998 });
            visitedPositions.Enqueue(new int[] { -999, -999 });


            var path = new Stack<int[]>();
            int[] current = end;

            while (current != null && !current.SequenceEqual(start))
            {
                int r = current[0];
                int c = current[1];
                if (prevRow[r, c] == -1 && prevCol[r, c] == -1)
                    return;
                path.Push(current);
                current = new int[] { prevRow[r, c], prevCol[r, c] };
            }

            path.Push(start);

            while (path.Count > 0)
                visitedPositions.Enqueue(path.Pop());
        }
    }
}