using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheSnake.Classes
{
    public class SnakeControl
    {
        public enum SnakeDirection { Left, Up, Right, Down};

        public SnakeDirection snakeDirection = SnakeDirection.Right;
    }
}
