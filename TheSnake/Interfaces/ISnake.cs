using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using TheSnake.Classes;

namespace TheSnake.Interfaces
{
    public interface ISnake
    {
        public int SnakeSquareSize { get; set; }
        public int SnakeStartLength { get; set; }
        public int SnakeStartSpeed { get; set; }
        public int SnakeSpeedThreshold { get; set; }
        public int SnakeLength { get; set; }
        public SolidColorBrush SnakeBodyBrush { get; set; }
        public List<SnakePart> SnakeParts { get; set; }

        public Canvas DrawSnake(Canvas Gamearea);
        public void MoveSnake(Canvas Gamearea, SnakeControl snakeControl);
        public void EatFood();
    }
}
