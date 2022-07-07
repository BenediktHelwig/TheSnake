using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;
using TheSnake.Classes;

namespace TheSnake
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly System.Windows.Threading.DispatcherTimer _gameTickTimer = new();

        private int score = 0;

        private readonly Random random = new();

        private readonly Snake _snake;
        private readonly SnakePart _snakePart;
        private readonly SnakeControl _snakeControl;
        private readonly Food _food;

        public MainWindow(Snake snake,
                          SnakePart snakePart,
                          SnakeControl snakeControl,
                          Food food)
        {
            _snake = snake;
            _snakePart = snakePart;
            _snakeControl = snakeControl;
            _food = food;

            InitializeComponent();

            _gameTickTimer.Tick += GameTickTimer_Tick;
        }

        protected override void OnInitialized(EventArgs e)
        {
            DrawSnake();
            base.OnInitialized(e);
        }

        private void GameTickTimer_Tick(object sender, EventArgs e)
        {
            MoveSnake();
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
                    if(_snakeControl.SnakeDirec != SnakeControl.SnakeDirection.Right)
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
            MoveSnake();
        }

        private void DrawSnake()
        {
            foreach (SnakePart snakePart in _snake.snakeParts)
            {
                if (snakePart.UiElement == null)
                {
                    snakePart.UiElement = new Rectangle()
                    {
                        Width = _snake.snakeSquareSize,
                        Height = _snake.snakeSquareSize,
                        Fill = _snake.snakeBodyBrush
                    };
                    Gamearea.Children.Add(snakePart.UiElement);
                    Canvas.SetTop(snakePart.UiElement, snakePart.Position.Y);
                    Canvas.SetLeft(snakePart.UiElement, snakePart.Position.X);
                }
            }
        }

        private void MoveSnake()
        {
            while (_snake.snakeParts.Count >= _snake.snakeLength)
            {
                Gamearea.Children.Remove(_snake.snakeParts[0].UiElement);
                _snake.snakeParts.RemoveAt(0);
            }

            foreach (SnakePart snakePart in _snake.snakeParts)
            {
                (snakePart.UiElement as Rectangle).Fill = _snake.snakeBodyBrush;
            }

            SnakePart snakeHead = _snake.snakeParts[_snake.snakeParts.Count - 1];
            double nextX = snakeHead.Position.X;
            double nextY = snakeHead.Position.Y;
            switch (_snakeControl.SnakeDirec)
            {
                case SnakeControl.SnakeDirection.Left:
                    nextX -= _snake.snakeSquareSize;
                    break;
                case SnakeControl.SnakeDirection.Up:
                    nextY -= _snake.snakeSquareSize;
                    break;
                case SnakeControl.SnakeDirection.Right:
                    nextX += _snake.snakeSquareSize;
                    break;
                case SnakeControl.SnakeDirection.Down:
                    nextY += _snake.snakeSquareSize;
                    break;
                default:
                    break;
            }

            _snake.snakeParts.Add(new SnakePart()
            {
                Position = new Point(nextX, nextY)
            });

            DrawSnake();

            DoCollisionCheck();
        }

        private void DrawSnakeFood()
        {
            Point foodPosition = GetNextFoodPosition();
            _food.SnakeFood = new Ellipse()
            {
                Width = _snake.snakeSquareSize,
                Height = _snake.snakeSquareSize,
                Fill = _food.FoodBrush
            };
            Gamearea.Children.Add(_food.SnakeFood);
            Canvas.SetTop(_food.SnakeFood, foodPosition.Y);
            Canvas.SetLeft(_food.SnakeFood, foodPosition.X);
        }

        private Point GetNextFoodPosition()
        {
            int maxX = (int)(Gamearea.ActualWidth / _snake.snakeSquareSize);
            int maxY = (int)(Gamearea.ActualHeight / _snake.snakeSquareSize);
            int foodX = random.Next(0, maxX) * _snake.snakeSquareSize;
            int foodY = random.Next(0, maxY) * _snake.snakeSquareSize;

            foreach (SnakePart snakePart in _snake.snakeParts)
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
            SnakePart snakeHead = _snake.snakeParts[_snake.snakeParts.Count - 1];
            if ((snakeHead.Position.X == Canvas.GetLeft(_food.SnakeFood)) 
                && 
                (snakeHead.Position.Y == Canvas.GetLeft(_food.SnakeFood)))
            {
                EatFood();
                return;
            }

            if ((snakeHead.Position.Y < 0) || (snakeHead.Position.Y >= Gamearea.ActualHeight) ||
                (snakeHead.Position.X < 0) || (snakeHead.Position.X >= Gamearea.ActualWidth))
            {
                EndGame();
            }

            foreach (SnakePart snakeBodyPart in _snake.snakeParts.Take(_snake.snakeParts.Count - 1))
            {
                if ((snakeHead.Position.X == snakeBodyPart.Position.X) && (snakeHead.Position.Y == snakeBodyPart.Position.Y))
                EndGame();
            }
        }

        private void EatFood()
        {
            _snake.snakeLength++;
            score++;
            int timerInterval = Math.Max(_snake.snakeSpeedThreshold, (int)_gameTickTimer.Interval.TotalMilliseconds - (score * 2));
            _gameTickTimer.Interval = TimeSpan.FromMilliseconds(timerInterval);
            Gamearea.Children.Remove(_food.SnakeFood);
            DrawSnakeFood();
            UpdateGameStatus();
        }

        private void UpdateGameStatus()
        {
            this.Title = "Snake - Score: " + score + " - Game speed: " + _gameTickTimer.Interval.TotalMilliseconds;
        }

        private void StartNewGame()
        {
            foreach (SnakePart snakePart in _snake.snakeParts)
            {
                if (snakePart.UiElement != null)
                    Gamearea.Children.Remove(snakePart.UiElement);
            }
            _snake.snakeParts.Clear();
            if (_food.SnakeFood != null)
                Gamearea.Children.Remove(_food.SnakeFood);

            // Reset stuff
            score = 0;
            _snake.snakeLength = _snake.snakeStartLength;
            _snake.snakeParts.Add(new SnakePart()
            {
                Position = new Point(_snake.snakeSquareSize * 5,
                                    _snake.snakeSquareSize * 5)
            });
            _gameTickTimer.Interval = TimeSpan.FromMilliseconds(_snake.snakeStartSpeed);

            DrawSnake();
            DrawSnakeFood();

            UpdateGameStatus();

            _gameTickTimer.IsEnabled = true;
        }

        private void EndGame()
        {
            _gameTickTimer.IsEnabled = false;
            MessageBox.Show("Leider verloren. Versuchs nochmal!\n\nZum neustart einfach die Leertaste drücken...", "Snake");
        }
    }
}
