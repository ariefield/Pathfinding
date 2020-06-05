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
        public TileType type;

        public Tile()
        {

        }

    }
}
