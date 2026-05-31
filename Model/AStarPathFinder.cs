
namespace Model
{
    public class AStarPathFinder : IPathFinder
    {
        PathFinderType _algType = PathFinderType.Astar;
        public PathFinderType algType { get => _algType; set {} }
        private string Key(int[] pos) => $"{pos[0]},{pos[1]}";

        public void FindPath(Maze maze, int[] pos, Queue<int[]> visitedPositions)
        {
            int[] start = pos;
            int[] end = maze.End;

            var openset = new List<int[]>();
            var cameFrom = new Dictionary<string, int[]>();
            var gScore = new Dictionary<string, int>();
            var fScore = new Dictionary<string, int>();

            var visited = new List<int[]>();
            openset.Add(start);
            gScore[Key(start)] = 0;
            fScore[Key(start)] = Heuristic(start, end);

            while (openset.Count > 0)
            {
                int[] current = openset
                    .OrderBy(n => fScore.GetValueOrDefault(Key(n), int.MaxValue)).First();
                if (current.SequenceEqual(end))
                {
                    // Eerst alle verkende posities
                    foreach (var v in visited)
                        visitedPositions.Enqueue(v);

                    // Dan het kortste pad
                    var path = new Stack<int[]>();
                    while (cameFrom.ContainsKey(Key(current)))
                    {
                        path.Push(current);
                        current = cameFrom[Key(current)];
                    }
                    path.Push(start);
                    while (path.Count > 0)
                        visitedPositions.Enqueue(path.Pop());

                    return;
                }

                openset.Remove(current);
                visited.Add(current);
                foreach (var move in maze.moves)
                {
                    int newRow = current[0] + move[0];
                    int newCol = current[1] + move[1];

                    if (!maze.IsValidMove(newRow, newCol)) continue;

                    int[] neighbor = new int[] { newRow, newCol };

                    int tentativeG = gScore[Key(current)] + 1;
                    if (!gScore.ContainsKey(Key(neighbor)) || tentativeG < gScore[Key(neighbor)])
                    {
                        cameFrom[Key(neighbor)] = current;
                        gScore[Key(neighbor)] = tentativeG;
                        fScore[Key(neighbor)] = tentativeG + Heuristic(neighbor, end);
                        if (!openset.Any(n => n.SequenceEqual(neighbor)))
                        {
                            openset.Add(neighbor);
                        }
                    }
                }
            }


        }

    private int Heuristic(int[] pos, int[] end) =>
        Math.Abs(pos[0] - end[0]) + Math.Abs(pos[1] - end[1]);

    }
}

            