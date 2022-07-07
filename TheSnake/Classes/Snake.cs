using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace TheSnake.Classes
{
    public class Snake
    {
        public readonly int snakeSquareSize = 20;
        public readonly int snakeStartLength = 3;
        public readonly int snakeStartSpeed = 400;
        public readonly int snakeSpeedThreshold = 100;
        
        public int snakeLength;

        public readonly SolidColorBrush snakeBodyBrush = Brushes.Blue;

        public List<SnakePart> snakeParts = new ();
    }
}
