using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathFinding.src
{
    public enum SearchType
    {
        Bfs
    }

    public class Search
    {
        public bool Searching { get; set; }
        public bool AutoAdvance { get; set; }
        public Dictionary<Tile, bool> Seen { get; set; }
        public Dictionary<Tile, Tile> CameFrom { get; set; }
        public Tile Start { get; set; }
        public Tile Current { get; set; }
        public Tile Goal { get; set; }
        public List<Tile> Path { get; set; }
        public int Steps { get; set; }

        private Queue<Tile> _queue;


        public Search(Tile start, Tile goal)
        {
            Start = start;
            Goal = goal;

            Searching = false;
            AutoAdvance = false;
            Seen = new Dictionary<Tile, bool>();
            CameFrom = new Dictionary<Tile, Tile>();
            Path = new List<Tile>();
            Steps = 0;

            _queue = new Queue<Tile>();
        }

        public void Update()
        {
            Steps += 1;
            Current = _queue.Dequeue();
            Seen[Current] = true;

            if (Current == Goal)
            {
                Searching = false;
                AutoAdvance = false;
                Console.WriteLine($"Found goal in {Steps} steps");
                return;
            }

            foreach (Tile tile in Current.Neighbours)
            {
                if (!Seen.ContainsKey(tile) && !_queue.Contains(tile))
                    _queue.Enqueue(tile);
            }

            if (_queue.Count == 0)
                Searching = false;

        }

        public void StartSearch(SearchType type)
        {
            Searching = true;
            Steps = 0;
            Seen = new Dictionary<Tile, bool>();
            CameFrom = new Dictionary<Tile, Tile>();
            _queue = new Queue<Tile>();

            if (type == SearchType.Bfs)
            {
                _queue.Enqueue(Start);
            }

        }
    }
}
