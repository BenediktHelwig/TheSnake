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
        private Snake _snake;
        private Food _food;

        private DispatcherTimer gameTickTimer = new DispatcherTimer();

        public MainWindow(Snake snake, Food food)
        {
            _snake = snake;
            _food = food;

            InitializeComponent();

            gameTickTimer.Tick += GameTickTimer_Tick;
        }

        protected override void OnInitialized(EventArgs e)
        {
            _snake.DrawSnake(Gamearea);
            base.OnInitialized(e);
        }

        private void GameTickTimer_Tick(object sender, EventArgs e)
        {
            _snake.MoveSnake(Gamearea.Children);
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            _snake.DrawSnake(Gamearea);
            gameTickTimer.Interval = TimeSpan.FromMilliseconds(400);
        }
    }
}
