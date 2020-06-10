using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathFinding.src
{
    public enum TileType
    {
        Open,
        Blocked,
        Start,
        Goal
    }

    public class Tile
    {
        static Color OPEN_COLOR = Color.Black;
        static Color BLOCKED_COLOR = Color.Gray;
        static Color START_COLOR = Color.White;
        static Color GOAL_COLOR = Color.Green;

        public TileType Type { get; set; }
        public List<Tile> Neighbours { get; set; }

        public Tile(TileType type)
        {
            Type = type;

            Neighbours = new List<Tile>();
        }

        public Color GetColor(Search search)
        {
            Color color = OPEN_COLOR;
            if (Type == TileType.Open)
            {
                if (search.Current == this)
                    color = Color.Red;
                else if (search.Seen.ContainsKey(this))
                    color = Color.IndianRed;
                else
                    color = OPEN_COLOR;
            }
            else if (Type == TileType.Blocked)
                color = BLOCKED_COLOR;
            else if (Type == TileType.Start)
            {
                if (search.Current == this)
                    color = Color.IndianRed;
                else
                    color = START_COLOR;
            }

            else if (Type == TileType.Goal)
                color = GOAL_COLOR;

            return color;
        }

    }
}
