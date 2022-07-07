using System;
using System.Windows;
using System.Windows.Media;

namespace TheSnake.Classes
{
    public class Food
    {
        private UIElement _snakeFood;

        public UIElement SnakeFood
        {
            get { return _snakeFood; }
            set { _snakeFood = value; }
        }

        private SolidColorBrush _foodBrush;

        public SolidColorBrush FoodBrush
        {
            get { return _foodBrush = Brushes.Red; }
        }

    }
}
