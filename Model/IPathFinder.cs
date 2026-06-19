
namespace Model
{
    public enum PathFinderType { Recursive, Stack, Astar, Dijkstra, Manual, BFS };



    public interface IPathFinder
    {
        public int ExplorationSteps { get; set; }
        public int PathLength { get; set; }
        PathFinderType algType { get; set; }
        void FindPath(Maze maze, int[] pos, Queue<int[]> visitedPositions);
    }

}