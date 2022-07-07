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

        private SnakeDirection _originalSnakeDirection;

        public SnakeDirection OriginalSnakeDirection
        { 
            get { return _originalSnakeDirection; }
            set { _originalSnakeDirection = value; }
        }

        private SnakeDirection _snakeDirec;

        public SnakeDirection SnakeDirec
        {
            get { return _snakeDirec; }
            set { _snakeDirec = value; }
        }

    }
}
