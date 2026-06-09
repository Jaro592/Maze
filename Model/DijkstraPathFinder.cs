namespace Model
{
    public class DijkstraPathFinder : IPathFinder
    {
        PathFinderType _algType = PathFinderType.Dijkstra;
        public PathFinderType algType { get => _algType; set {} }

        private string Key(int[] pos) => $"{pos[0]},{pos[1]}";

        public void FindPath(Maze maze, int[] pos, Queue<int[]> visitedPositions)
        {
            int[] start = pos;
            int[] end = maze.End;

            var unvisitedNodes = new List<int[]>();
            var distance = new Dictionary<string, double>();
            var prev = new Dictionary<string, int[]>();

            for (int r = 0; r < maze.MazeArray.Length; r++)
            {
                for (int c = 0; c < maze.MazeArray[r].Length; c++)
                {
                    if (maze.IsValidMove(r, c))
                    {
                        int[] node = new int[] { r, c };
                        string key = Key(node);
                        distance[key] = double.PositiveInfinity;
                        prev[key] = null;
                        unvisitedNodes.Add(node);
                    }
                }
            }

            distance[Key(start)] = 0;

            while (unvisitedNodes.Count > 0)
            {
                int[] closestNode = unvisitedNodes[0];
                foreach (var node in unvisitedNodes)
                {
                    if (distance[Key(node)] < distance[Key(closestNode)])
                        closestNode = node;
                }

                if (distance[Key(closestNode)] == double.PositiveInfinity)
                    break;

                unvisitedNodes.Remove(closestNode);

                if (closestNode.SequenceEqual(end))
                    break;

                foreach (var move in maze.moves)
                {
                    int newRow = closestNode[0] + move[0];
                    int newCol = closestNode[1] + move[1];

                    if (!maze.IsValidMove(newRow, newCol)) continue;

                    int[] buur = new int[] { newRow, newCol };
                    string buurKey = Key(buur);
                    double difRoute = distance[Key(closestNode)] + 1;

                    if (difRoute < distance[buurKey])
                    {
                        distance[buurKey] = difRoute;
                        prev[buurKey] = closestNode;
                    }
                }
            }

            var path = new Stack<int[]>();
            int[] current = end;

            while (current != null && !current.SequenceEqual(start))
            {
                string currentKey = Key(current);
                if (!prev.ContainsKey(currentKey) || prev[currentKey] == null)
                    return;
                path.Push(current);
                current = prev[currentKey];
            }

            path.Push(start);

            while (path.Count > 0)
                visitedPositions.Enqueue(path.Pop());
        }
    }
}