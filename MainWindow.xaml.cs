using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Tetris
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ImageSource[] tetrominoTileImages = new ImageSource[]
        {
            new BitmapImage(new Uri("Assets/TileEmpty.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileCyan.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileBlue.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileOrange.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileYellow.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileGreen.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TilePurple.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileRed.png", UriKind.Relative)),
        };

        private readonly ImageSource[] tetrominoBlockImages = new ImageSource[]
        {
            new BitmapImage(new Uri("Assets/Block-Empty.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-I.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-J.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-L.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-O.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-S.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-T.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-Z.png", UriKind.Relative)),
        };

        private readonly ImageSource[] pentominoBlockImages = new ImageSource[]
        {
            new BitmapImage(new Uri("Assets/EmptyBlock.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/IBlockPent.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/FBlockPent.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/LBlockPent.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TBlockPent.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/NBlockPent.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/PBlockPent.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/UBlockPent.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/VBlockPent.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/WBlockPent.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/XBlockPent.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/YBlockPent.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/ZBlockPent.png", UriKind.Relative))
        };
        private readonly ImageSource[] pentominoTileImages = new ImageSource[]
        {
            new BitmapImage(new Uri("Assets/TileEmptyPent.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/ITile.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/FTile.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/LTile.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TTile.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/NTile.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/PTile.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/UTile.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/VTile.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/WTile.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/XTile.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/YTile.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/ZTile.png", UriKind.Relative))
        };

        private Image[,] imageControls;
        private BlockQueue blockQueue;

        private readonly int minDelay = 75;
        private readonly int decreasedDelayPerLine = 25;
        private readonly int decreasedDelayPerScore = 75;
        private int currentDelay;
        private int lastScoreChange = 0;

        public bool isPentominoMode = false;
        private GameState gameState;

        public MainWindow()
        {
            InitializeComponent();
            gameState = new GameState(isPentominoMode);
            imageControls = SetupGameCanvas(gameState.GameGrid);
            blockQueue = new BlockQueue(isPentominoMode);
            currentDelay = 1000;
        }

        private Image[,] SetupGameCanvas(GameGrid grid)
        {
            int cellSize = 25;
            Image[,] imageControls = new Image[grid.Rows, grid.Columns];

            for (int r = 0; r < grid.Rows; r++)
            {
                for (int c = 0; c < grid.Columns; c++)
                {
                    Image imageControl = new Image()
                    {
                        Width = cellSize,
                        Height = cellSize
                    };

                    if (isPentominoMode)
                    {
                        Canvas.SetTop(imageControl, (r - 2) * cellSize + 10);
                        Canvas.SetLeft(imageControl, c * cellSize);
                        GameCanvasPentomino.Children.Add(imageControl);
                        imageControl.Source = pentominoTileImages[grid[r, c]];
                    }
                    else
                    {
                        Canvas.SetTop(imageControl, (r - 2) * cellSize + 10);
                        Canvas.SetLeft(imageControl, c * cellSize);
                        GameCanvas.Children.Add(imageControl);
                        imageControl.Source = tetrominoTileImages[grid[r, c]];
                    }

                    imageControls[r, c] = imageControl;
                }
            }

            return imageControls;
        }

        private void StartNewGame()
        {
            GameCanvas.Children.Clear();
            GameCanvasPentomino.Children.Clear();

            gameState = new GameState(isPentominoMode);
            blockQueue = new BlockQueue(isPentominoMode);
            imageControls = SetupGameCanvas(gameState.GameGrid);
            currentDelay = 1000;
            lastScoreChange = 0;

            if (isPentominoMode)
            {
                ScoreTextPentomnino.Text = $"Рахунок: {gameState.Score}";
            }
            else
            {
                ScoreText.Text = $"Рахунок: {gameState.Score}";
            }

            _ = GameLoop();

        }

        private void DrawGrid(GameGrid grid)
        {
            for (int r = 0; r < grid.Rows; r++)
            {
                for (int c = 0; c < grid.Columns; c++)
                {
                    int id = grid[r, c];
                    imageControls[r, c].Opacity = 1;

                    if (isPentominoMode)
                    {
                        imageControls[r, c].Source = pentominoTileImages[id];
                    }
                    else
                    {
                        imageControls[r, c].Source = tetrominoTileImages[id];
                    }
                }
            }
        }
        private void DrawNextBlock(BlockQueue blockQueue)
        {
            Block next = blockQueue.NextBlock;

            if (isPentominoMode)
            {
                NextImagePentomino.Source = pentominoBlockImages[next.Id];
            }
            else
            {
                NextImage.Source = tetrominoBlockImages[next.Id];
            }
        }

        private void DrawBlock(Block block)
        {
            foreach (Position p in block.TilePositions())
            {
                imageControls[p.Row, p.Column].Opacity = 1;
                imageControls[p.Row, p.Column].Source = isPentominoMode ? pentominoTileImages[block.Id] : tetrominoTileImages[block.Id];
            }

        }
        private void Tetromino_Click(object sender, RoutedEventArgs e)
        {
            Menu.Visibility = Visibility.Hidden;
            Tetromino.Visibility = Visibility.Visible;
            Pentomino.Visibility = Visibility.Hidden;
            isPentominoMode = false;

            if (!isPentominoMode || isPentominoMode)
            {
                StartNewGame();
            }
        }

        private void Pentomino_Click(object sender, RoutedEventArgs e)
        {
            Menu.Visibility = Visibility.Hidden;
            Tetromino.Visibility = Visibility.Hidden;
            Pentomino.Visibility = Visibility.Visible;
            isPentominoMode = true;

            if (isPentominoMode || isPentominoMode)
            {
                StartNewGame();
            }
        }

        private void DrawHeldBlock(Block heldBlock)
        {
            if (heldBlock == null)
            {
                if (isPentominoMode)
                {
                    HoldImagePentomino.Source = pentominoBlockImages[0];
                }
                else
                {
                    HoldImage.Source = tetrominoBlockImages[0];
                }
            }
            else
            {
                if (isPentominoMode)
                {
                    HoldImagePentomino.Source = pentominoBlockImages[heldBlock.Id];
                }
                else
                {
                    HoldImage.Source = tetrominoBlockImages[heldBlock.Id];
                }
            }
        }

        public void DrawGhostBlock(Block block)
        {
            int dropDistance = gameState.BlockDropDistance();

            if (!isPentominoMode)
            {
                foreach (Position p in block.TilePositions())
                {
                    imageControls[p.Row + dropDistance, p.Column].Opacity = 0.25;
                    imageControls[p.Row + dropDistance, p.Column].Source = tetrominoTileImages[block.Id];
                }
            }
            else
            {
                foreach (Position p in block.TilePositions())
                {
                    imageControls[p.Row + dropDistance, p.Column].Opacity = 0.25;
                    imageControls[p.Row + dropDistance, p.Column].Source = pentominoTileImages[block.Id];
                }
            }
        }
        private void Draw(GameState gameState)
        {
            DrawGrid(gameState.GameGrid);
            DrawGhostBlock(gameState.CurrentBlock);
            DrawBlock(gameState.CurrentBlock);
            DrawNextBlock(gameState.BlockQueue);
            DrawHeldBlock(gameState.HeldBlock);

            if (isPentominoMode)
            {
                ScoreTextPentomnino.Text = $"Рахунок: {gameState.Score}";
            }
            else
            {
                ScoreText.Text = $"Рахунок: {gameState.Score}";
            }
        }

        private async Task GameLoop()
        {
            Draw(gameState);
            while (!gameState.GameOver)
            {
                int delay = Math.Max(minDelay, currentDelay - (gameState.LinesCleared * decreasedDelayPerLine));
                await Task.Delay(delay);
                gameState.MoveBlockDown();
                Draw(gameState);
            }

            await Task.Delay(1000);
            GameOverMenu.Visibility = Visibility.Visible;
            FinalScoreText.Text = $"Рахунок: {gameState.Score}";

        }
        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (gameState.GameOver)
            {
                return;
            }
            switch (e.Key)
            {
                case Key.Left:
                    gameState.MoveBlockLeft();
                    break;
                case Key.Right:
                    gameState.MoveBlockRight();
                    break;
                case Key.Down:
                    gameState.MoveBlockDown();
                    break;
                case Key.Up:
                    gameState.RotateBlockCW();
                    break;
                case Key.Z:
                    gameState.RotateBlockCCW();
                    break;
                case Key.C:
                    gameState.HoldBlock();
                    break;
                case Key.Space:
                    gameState.DropBlock();
                    break;
                default:
                    break;
            }
            Draw(gameState);
        }
        private async void GameCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            await GameLoop();
        }

        private void PlayAgain_Click(object sender, RoutedEventArgs e)
        {
            GameOverMenu.Visibility = Visibility.Hidden;

            Tetromino.Visibility = Visibility.Hidden;
            Pentomino.Visibility = Visibility.Hidden;

            Menu.Visibility = Visibility.Visible;

            GameCanvas.Children.Clear();
            GameCanvasPentomino.Children.Clear();

            gameState = new GameState(isPentominoMode);
            blockQueue = new BlockQueue(isPentominoMode);
            imageControls = SetupGameCanvas(gameState.GameGrid);
            currentDelay = 1000;
            lastScoreChange = 0;

            Draw(gameState);
        }
    }
}
