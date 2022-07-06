using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace TheSnake.Classes
{
    public class Snake
    {
        const int BodySquareSize = 20;

        public int StartPoint = 290;

        private readonly SnakeControl _snakeControl;

        public Snake(SnakeControl snakeControl)
        {
            _snakeControl = snakeControl;
        }

        public Canvas DrawSnake(Canvas gameArea)
        {
            if (gameArea.Children.Count == 0)
            {
                Rectangle rectangle = new()
                {
                    Width = BodySquareSize,
                    Height = BodySquareSize,
                    Fill = Brushes.Blue,
                };
                gameArea.Children.Add(rectangle);
                Canvas.SetTop(rectangle, StartPoint);
                Canvas.SetLeft(rectangle, StartPoint);
                return gameArea;
            }
            else
            {
                //Snake move automatic
                return gameArea;
            }
        }

        public void MoveSnake(Canvas gameArea)
        {
            foreach (Rectangle element in gameArea.Children)
            {
                switch (_snakeControl.snakeDirection)
                {
                    case SnakeControl.SnakeDirection.Left:
                        Canvas.SetLeft(element, Canvas.GetLeft(element) - BodySquareSize);
                        break;
                    case SnakeControl.SnakeDirection.Up:
                        Canvas.SetTop(element, Canvas.GetTop(element) - BodySquareSize);
                        break;
                    case SnakeControl.SnakeDirection.Right:
                        Canvas.SetLeft(element, Canvas.GetLeft(element) + BodySquareSize);
                        break;
                    case SnakeControl.SnakeDirection.Down:
                        Canvas.SetTop(element, Canvas.GetTop(element) + BodySquareSize);
                        break;
                }
            }
        }
    }
}
