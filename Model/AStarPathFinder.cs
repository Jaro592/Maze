namespace Model
{
    public class AStarPathFinder : IPathFinder
    {
        PathFinderType _algType = PathFinderType.Astar;
        public PathFinderType algType { get => _algType; set {} }

        private int Heuristic(int[] pos, int[] end) =>
            Math.Abs(pos[0] - end[0]) + Math.Abs(pos[1] - end[1]);

        public int ExplorationSteps { get; set; } = 0;
        public int PathLength { get; set; } = 0;

        public void FindPath(Maze maze, int[] pos, Queue<int[]> visitedPositions)
        {
            int[] start = pos;
            int[] end = maze.End;

            int rows = maze.MazeArray.Length;
            int cols = maze.MazeArray[0].Length;

            var gScore = new double[rows, cols];
            var prevRow = new int[rows, cols];
            var prevCol = new int[rows, cols];

            for (int r = 0; r < rows; r++)
                for (int c = 0; c < cols; c++)
                {
                    gScore[r, c] = double.PositiveInfinity;
                    prevRow[r, c] = -1;
                    prevCol[r, c] = -1;
                }

            gScore[start[0], start[1]] = 0;

            // Alleen ontdekte nodes zitten in de open set
            var openSet = new List<int[]> { start };

            while (openSet.Count > 0)
            {
                // Kies node met laagste f = g + h
                int[] current = openSet[0];
                foreach (var node in openSet)
                {
                    double fNode    = gScore[node[0], node[1]]    + Heuristic(node, end);
                    double fCurrent = gScore[current[0], current[1]] + Heuristic(current, end);

                    if (fNode < fCurrent ||
                    (fNode == fCurrent && Heuristic(node, end) < Heuristic(current, end)))
                        current = node;
                }

                openSet.Remove(current);
                visitedPositions.Enqueue(current);
                ExplorationSteps++;

                if (current[0] == end[0] && current[1] == end[1])
                    break;

                foreach (var move in maze.moves)
                {
                    int newRow = current[0] + move[0];
                    int newCol = current[1] + move[1];

                    if (!maze.IsValidMove(newRow, newCol)) continue;

                    double newG = gScore[current[0], current[1]] + 1;

                    if (newG < gScore[newRow, newCol])
                    {
                        gScore[newRow, newCol] = newG;
                        prevRow[newRow, newCol] = current[0];
                        prevCol[newRow, newCol] = current[1];

                        // Voeg toe aan open set als nog niet aanwezig
                        if (!openSet.Any(n => n[0] == newRow && n[1] == newCol))
                            openSet.Add(new int[] { newRow, newCol });
                    }
                }
            }

            // Sentinels + pad reconstructie (ongewijzigd)
            visitedPositions.Enqueue(new int[] { -998, -998 });
            visitedPositions.Enqueue(new int[] { -999, -999 });

            var path = new Stack<int[]>();
            int[] curr = end;

            while (curr != null && !(curr[0] == start[0] && curr[1] == start[1]))
            {
                int r = curr[0];
                int c = curr[1];
                if (prevRow[r, c] == -1 && prevCol[r, c] == -1)
                    return;
                path.Push(curr);
                curr = new int[] { prevRow[r, c], prevCol[r, c] };
            }

            path.Push(start);
            PathLength = path.Count;

            while (path.Count > 0)
                visitedPositions.Enqueue(path.Pop());
        }
    }
}