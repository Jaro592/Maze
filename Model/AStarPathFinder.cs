
namespace Model
{
    public class AStarPathFinder : IPathFinder
    {
        PathFinderType _algType = PathFinderType.Astar;
        public PathFinderType algType { get => _algType; set {} }
        private string Key(int[] pos) => $"{pos[0]},{pos[1]}";

        public void FindPath(Maze maze, int[] pos, Queue<int[]> visitedPositions)
        {
            
        }
}
}

            