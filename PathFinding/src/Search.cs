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
        public Dictionary<Tile, bool> Seen { get; set; }
        public Tile Start { get; set; }
        public Tile Current { get; set; }
        public Tile Goal { get; set; }
        public int Steps { get; set; }

        private Queue<Tile> _queue;


        public Search(Tile start, Tile goal)
        {
            Start = start;
            Goal = goal;

            Searching = false;
            Seen = new Dictionary<Tile, bool>();
            Steps = 0;

            _queue = new Queue<Tile>();
        }

        public void Update()
        {
            Steps += 1;
            Current = _queue.Dequeue();

            if (Current == Goal)
            {
                Searching = false;
                Console.WriteLine($"Found goal in {Steps} steps");
                return;
            }

            if (Seen.ContainsKey(Current))
            {
                return;
            }

            Seen[Current] = true;


            foreach (Tile tile in Current.Neighbours)
            {
                _queue.Enqueue(tile);
            }

        }

        public void StartSearch(SearchType type)
        {
            Searching = true;
            Steps = 0;
            Seen = new Dictionary<Tile, bool>();
            _queue = new Queue<Tile>();

            if (type == SearchType.Bfs)
            {
                _queue.Enqueue(Start);
            }

        }
    }
}
