using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using TheSnake.Classes;
using TheSnake.Interfaces;

namespace TheSnake
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DispatcherTimer _gameTickTimer = new();

        private int score = 0;

        private readonly Random random = new();

        private ISnake _snake;
        private SnakePart _snakePart;
        private SnakeControl _snakeControl;
        private IFood _food;

        public MainWindow(ISnake snake,
                          SnakePart snakePart,
                          SnakeControl snakeControl,
                          IFood food)
        {
            _snake = snake;
            _snakePart = snakePart;
            _snakeControl = snakeControl;
            _food = food;
            
            _food.FoodBrush = Brushes.Red;

            _snake.SnakeSquareSize = 20;
            _snake.SnakeStartLength = 3;
            _snake.SnakeStartSpeed = 400;
            _snake.SnakeSpeedThreshold = 100;
            _snake.SnakeBodyBrush = Brushes.Blue; ;

            InitializeComponent();

            _gameTickTimer.Tick += GameTickTimer_Tick;
        }

        protected override void OnInitialized(EventArgs e)
        {
            _snake.DrawSnake(Gamearea);
            base.OnInitialized(e);
        }

        private void GameTickTimer_Tick(object sender, EventArgs e)
        {
            _snake.MoveSnake(Gamearea, _snakeControl);
            DoCollisionCheck();
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            StartNewGame();
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            _snakeControl.OriginalSnakeDirection = _snakeControl.SnakeDirec;
            switch (e.Key)
            {
                case Key.Left:
                    if (_snakeControl.SnakeDirec != SnakeControl.SnakeDirection.Right)
                        _snakeControl.SnakeDirec = SnakeControl.SnakeDirection.Left;
                    break;
                case Key.Up:
                    if (_snakeControl.SnakeDirec != SnakeControl.SnakeDirection.Down)
                        _snakeControl.SnakeDirec = SnakeControl.SnakeDirection.Up;
                    break;
                case Key.Right:
                    if (_snakeControl.SnakeDirec != SnakeControl.SnakeDirection.Left)
                        _snakeControl.SnakeDirec = SnakeControl.SnakeDirection.Right;
                    break;
                case Key.Down:
                    if (_snakeControl.SnakeDirec != SnakeControl.SnakeDirection.Up)
                        _snakeControl.SnakeDirec = SnakeControl.SnakeDirection.Down;
                    break;
                case Key.Space:
                    StartNewGame();
                    break;
            }
            if (_snakeControl.SnakeDirec != _snakeControl.OriginalSnakeDirection)
                _snake.MoveSnake(Gamearea, _snakeControl);
        }

        private void DrawSnakeFood()
        {
            Point foodPosition = GetNextFoodPosition();
            _food.SnakeFood = new Ellipse()
            {
                Width = _snake.SnakeSquareSize,
                Height = _snake.SnakeSquareSize,
                Fill = _food.FoodBrush
            };
            Gamearea.Children.Add(_food.SnakeFood);
            Canvas.SetTop(_food.SnakeFood, foodPosition.Y);
            Canvas.SetLeft(_food.SnakeFood, foodPosition.X);
        }

        private Point GetNextFoodPosition()
        {
            int maxX = (int)(Gamearea.ActualWidth / _snake.SnakeSquareSize);
            int maxY = (int)(Gamearea.ActualHeight / _snake.SnakeSquareSize);
            int foodX = random.Next(0, maxX) * _snake.SnakeSquareSize;
            int foodY = random.Next(0, maxY) * _snake.SnakeSquareSize;

            foreach (SnakePart snakePart in _snake.SnakeParts)
            {
                if ((snakePart.Position.X == foodX)
                    &&
                    (snakePart.Position.Y == foodY))
                    return GetNextFoodPosition();
            }

            return new Point(foodX, foodY);
        }

        private void DoCollisionCheck()
        {
            SnakePart snakeHead = _snake.SnakeParts[_snake.SnakeParts.Count - 1];
            //var x = snakeHead.Position.X;
            //var y = snakeHead.Position.Y;
            //var left = Canvas.GetLeft(_food.SnakeFood);
            //var top = Canvas.GetLeft(_food.SnakeFood);
            if ((snakeHead.Position.X == Canvas.GetLeft(_food.SnakeFood))
                &&
                (snakeHead.Position.Y == Canvas.GetTop(_food.SnakeFood)))
            {
                _snake.EatFood();
                score+=10;
                GameSpeed();
                Gamearea.Children.Remove(_food.SnakeFood);
                DrawSnakeFood();
                UpdateGameStatus();
                return;
            }

            if ((snakeHead.Position.Y < 0) || (snakeHead.Position.Y >= Gamearea.ActualHeight) ||
                (snakeHead.Position.X < 0) || (snakeHead.Position.X >= Gamearea.ActualWidth))
            {
                EndGame();
            }

            foreach (SnakePart snakeBodyPart in _snake.SnakeParts.Take(_snake.SnakeParts.Count - 1))
            {
                if ((snakeHead.Position.X == snakeBodyPart.Position.X) && (snakeHead.Position.Y == snakeBodyPart.Position.Y))
                    EndGame();
            }
        }

        private void UpdateGameStatus()
        {
            this.Title = "Snake - Score: " + score + " - Game speed: " + _gameTickTimer.Interval.TotalMilliseconds;
        }

        private void StartNewGame()
        {
            foreach (SnakePart snakePart in _snake.SnakeParts)
            {
                if (snakePart.UiElement != null)
                    Gamearea.Children.Remove(snakePart.UiElement);
            }
            _snake.SnakeParts.Clear();
            if (_food.SnakeFood != null)
                Gamearea.Children.Remove(_food.SnakeFood);

            // Reset stuff
            score = 0;
            _snake.SnakeLength = _snake.SnakeStartLength;
            _snake.SnakeParts.Add(new SnakePart()
            {
                Position = new Point(_snake.SnakeSquareSize * 5,
                                    _snake.SnakeSquareSize * 5)
            });
            _gameTickTimer.Interval = TimeSpan.FromMilliseconds(_snake.SnakeStartSpeed);

            _snake.DrawSnake(Gamearea);
            DrawSnakeFood();

            UpdateGameStatus();

            _gameTickTimer.IsEnabled = true;
        }

        private void GameSpeed()
        {
            if (score % 100 == 0)
            {
                int timerInterval = Math.Max(_snake.SnakeSpeedThreshold, (int)_gameTickTimer.Interval.TotalMilliseconds - (20));
                _gameTickTimer.Interval = TimeSpan.FromMilliseconds(timerInterval); 
            }
        }

        private void EndGame()
        {
            _gameTickTimer.IsEnabled = false;
            MessageBox.Show("Leider verloren. Versuchs nochmal!\n\nZum neustart einfach die Leertaste drücken...", "Snake");
        }
    }
}
