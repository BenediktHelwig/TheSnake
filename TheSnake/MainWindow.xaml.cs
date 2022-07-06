using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Shapes;
using System.Windows.Threading;
using TheSnake.Classes;

namespace TheSnake
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly DispatcherTimer _gameTickTimer = new();

        private readonly Snake _snake;
        //private readonly Food _food;
        private readonly SnakeControl _snakeControl;

        public MainWindow(Snake snake, 
                          //Food food, 
                          SnakeControl snakeControl)
        {
            _snake = snake;
            //_food = food;
            _snakeControl = snakeControl;

            InitializeComponent();

            _gameTickTimer.Tick += GameTickTimer_Tick;
            _gameTickTimer.IsEnabled = true;
        }

        protected override void OnInitialized(EventArgs e)
        {
            _snake.DrawSnake(Gamearea);
            base.OnInitialized(e);
        }

        private void GameTickTimer_Tick(object sender, EventArgs e)
        {
            _snake.MoveSnake(Gamearea);
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            _snake.DrawSnake(Gamearea);
            _gameTickTimer.Interval = TimeSpan.FromMilliseconds(400);
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Left:
                    _snakeControl.snakeDirection = SnakeControl.SnakeDirection.Left;
                    break;
                case Key.Up:
                    _snakeControl.snakeDirection = SnakeControl.SnakeDirection.Up;
                    break;
                case Key.Right:
                    _snakeControl.snakeDirection = SnakeControl.SnakeDirection.Right;
                    break;
                case Key.Down:
                    _snakeControl.snakeDirection = SnakeControl.SnakeDirection.Down;
                    break;
                case Key.Space:
                    break;
            }
        }
    }
}
