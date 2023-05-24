using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace isometricGame.Library.Models
{
    public class Entrance
    {
        public Rectangle EntranceRectangle { get; set; }
        public EntranceDirectionEnum Direction { get; set; }

        public Entrance(Rectangle entranceRectangle, EntranceDirectionEnum directionEnum)
        {
            EntranceRectangle = entranceRectangle;
            Direction = directionEnum;
        }
    }
}
