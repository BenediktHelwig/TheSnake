using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using TheSnake.Interfaces;

namespace TheSnake.Classes
{
    public class Snake : ISnake
    {
        private int _snakeSquareSize;

        public int SnakeSquareSize
        {
            get { return _snakeSquareSize; }
            set { _snakeSquareSize = value; }
        }

        private int _snakeStartLength;

        public int SnakeStartLength
        {
            get { return _snakeStartLength; }
            set { _snakeStartLength = value; }
        }

        private int _snakeStartSpeed;

        public int SnakeStartSpeed
        {
            get { return _snakeStartSpeed; }
            set { _snakeStartSpeed = value; }
        }

        private int _snakeSpeedThreshold;

        public int SnakeSpeedThreshold
        {
            get { return _snakeSpeedThreshold; }
            set { _snakeSpeedThreshold = value; }
        }

        private int _snakeLength;

        public int SnakeLength
        {
            get { return _snakeLength; }
            set { _snakeLength = value; }
        }

        private SolidColorBrush _snakeBodyBrush;

        public SolidColorBrush SnakeBodyBrush
        {
            get { return _snakeBodyBrush; }
            set { _snakeBodyBrush = value; }
        }

        private List<SnakePart> _SnakeParts;

        public List<SnakePart> SnakeParts
        {
            get { return _SnakeParts; }
            set { _SnakeParts = value; }
        }

        public Snake()
        {
            SnakeParts = new();
        }

        public Canvas DrawSnake(Canvas Gamearea)
        {
            foreach (SnakePart snakePart in SnakeParts)
            {
                if (snakePart.UiElement == null)
                {
                    snakePart.UiElement = new Rectangle()
                    {
                        Width = SnakeSquareSize,
                        Height = SnakeSquareSize,
                        Fill = SnakeBodyBrush
                    };
                    Gamearea.Children.Add(snakePart.UiElement);
                    Canvas.SetTop(snakePart.UiElement, snakePart.Position.Y);
                    Canvas.SetLeft(snakePart.UiElement, snakePart.Position.X);
                }
            }
            return Gamearea;
        }

        public void MoveSnake(Canvas Gamearea, SnakeControl snakeControl)
        {
            while (SnakeParts.Count >= SnakeLength)
            {
                Gamearea.Children.Remove(SnakeParts[0].UiElement);
                SnakeParts.RemoveAt(0);
            }

            foreach (SnakePart snakePart in SnakeParts)
            {
                (snakePart.UiElement as Rectangle).Fill = SnakeBodyBrush;
            }

            SnakePart snakeHead = SnakeParts[SnakeParts.Count - 1];
            double nextX = snakeHead.Position.X;
            double nextY = snakeHead.Position.Y;
            switch (snakeControl.SnakeDirec)
            {
                case SnakeControl.SnakeDirection.Left:
                    nextX -= SnakeSquareSize;
                    break;
                case SnakeControl.SnakeDirection.Up:
                    nextY -= SnakeSquareSize;
                    break;
                case SnakeControl.SnakeDirection.Right:
                    nextX += SnakeSquareSize;
                    break;
                case SnakeControl.SnakeDirection.Down:
                    nextY += SnakeSquareSize;
                    break;
                default:
                    break;
            }

            SnakeParts.Add(new SnakePart()
            {
                Position = new Point(nextX, nextY)
            });

            DrawSnake(Gamearea);
        }

        public void EatFood()
        {
            SnakeLength++;
        }
    }
}
