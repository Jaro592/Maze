
namespace Model
{
    public class RecursivePathFinder : IPathFinder
    {
        PathFinderType _algType = PathFinderType.Recursive;
        public PathFinderType algType { get => _algType; set {} }

        public void FindPath(Maze maze, int[] pos, Queue<int[]> visitedPositions) // akif
        {
            var path = new List<int[]>();
            var visited = new HashSet<string>(); // to keep track of visited positions, using a hashset for O(1) lookup
            if (DFS(maze, pos, path, visited))
            {
                foreach (var step in path)
                {
                    visitedPositions.Enqueue(step); // enqueue the steps in the path to the visitedPositions queue
                }
            }
        }

        private bool DFS(Maze maze, int[] pos, List<int[]> path, HashSet<string> visited)
        {
            if (!maze.IsValidMove(pos[0], pos[1])) return false; // if the move is invalid, return false
            string key = $"{pos[0]},{pos[1]}"; // create a unique key for the current position
            if (visited.Contains(key)) return false; // if we've already visited this position, return false
            visited.Add(key); // mark the current position as visited
            path.Add(pos); // add the current position to the path
            if (pos[0] == maze.End[0] && pos[1] == maze.End[1]) return true; // if we've reached the end, return true
            foreach (var move in maze.moves)
            {
                if(DFS(maze, new int[] { pos[0] + move[0], pos[1] + move[1] }, path, visited))
                {
                    return true; // if the recursive call returns true, propagate that back up the call stack
                }
            }
            
            path.RemoveAt(path.Count - 1); // remove the current position from the path
            return false; // if no valid path is found, return false
        }
    }
}
