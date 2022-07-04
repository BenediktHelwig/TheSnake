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

        public void MoveSnake(UIElementCollection children)
        {
        }
    }
}
