using System.Windows;
using System.Windows.Media;

namespace TheSnake.Interfaces
{
    public interface IFood
    {
        public UIElement SnakeFood { get; set; }
        public SolidColorBrush FoodBrush { get; set; }
    }
}
