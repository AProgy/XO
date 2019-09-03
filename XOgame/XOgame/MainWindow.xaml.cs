using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace XOgame
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            o.Visibility = Visibility.Hidden;

            xWin.Text = xWins.ToString();
            oWin.Text = oWins.ToString();
        }

        private readonly Dictionary<string, byte> numbers = new Dictionary<string, byte>
        {
            ["one"] = 1,
            ["two"] = 2,
            ["three"] = 3,
            ["four"] = 4,
            ["five"] = 5,
            ["six"] = 6,
            ["seven"] = 7,
            ["eight"] = 8,
            ["nine"] = 9
        };

        private readonly byte[,] winPositions = new byte[,]
            {
                { 1,2,3 },
                { 4,5,6 },
                { 7,8,9 },
                { 1,4,7 },
                { 2,5,8 },
                { 3,6,9 },
                { 1,5,9 },
                { 3,5,7 }
            };

        private List<byte> xPos = new List<byte>(5);
        private List<byte> oPos = new List<byte>(4);

        private int xWins = 0;
        private int oWins = 0;

        private byte turn = 0;

        private void OnButtonClick(object sender, RoutedEventArgs e)
        {
            if (turn % 2 == 0)
            {
                xPos.Add(numbers[(sender as Button).Name]);
                (sender as Button).Content = "X";
                x.Visibility = Visibility.Hidden;
                o.Visibility = Visibility.Visible;
            }
            else
            {
                oPos.Add(numbers[(sender as Button).Name]);
                (sender as Button).Content = "O";
                x.Visibility = Visibility.Visible;
                o.Visibility = Visibility.Hidden;
            }
            (sender as Button).IsEnabled = false;

            turn++;

            if (turn >= 5)
                Win(turn);
        }

        private async void Win(byte turn)
        {
            string winFigure = await GetFigureCountAsync(turn);
            MessageBoxResult res;

            if (string.IsNullOrEmpty(winFigure) && turn == 9) res = MessageBox.Show($"Do you want to start new game?", "Draw!", MessageBoxButton.YesNo, MessageBoxImage.Question);

            else if (string.IsNullOrEmpty(winFigure) && turn != 9) return;

            else res = MessageBox.Show($"Do you want to start new game?", $"{winFigure} win!", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (res == MessageBoxResult.Yes) Restart(winFigure);
            else CloseApp();
        }

        private async Task<string> GetFigureCountAsync(byte turn)
        {
            return await Task.Run(() => FigureCount(turn));
        }

        private string FigureCount(byte turn)
        {
            byte xCount = 0;
            byte oCount = 0;

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (xPos.Contains(winPositions[i, j]))
                        xCount++;

                    if (turn != 5)
                        if (oPos.Contains(winPositions[i, j]))
                            oCount++;
                }

                if (xCount != 3) xCount = 0;
                else return "X";

                if (oCount != 3) oCount = 0;
                else return "O";
            }

            return null;
        }

        private void Restart(string winFigure)
        {
            ResetAsync();

            if (winFigure == "X")
            {
                xWins++;
                xWin.Text = xWins.ToString();
            }
            else if (winFigure == "O")
            {
                oWins++;
                oWin.Text = oWins.ToString();
            }
        }

        private async void ResetAsync() => await Task.Run(() => Reset());

        private async void Reset()
        {
            await Task.Run(() =>
            {
                one.Content = ""; one.IsEnabled = true;
                two.Content = ""; two.IsEnabled = true;
                three.Content = ""; three.IsEnabled = true;
                four.Content = ""; four.IsEnabled = true;
                five.Content = ""; five.IsEnabled = true;
                six.Content = ""; six.IsEnabled = true;
                seven.Content = ""; seven.IsEnabled = true;
                eight.Content = ""; eight.IsEnabled = true;
                nine.Content = ""; nine.IsEnabled = true;

                xPos.Clear();
                oPos.Clear();

                turn = 0;

                o.Visibility = Visibility.Hidden;
                x.Visibility = Visibility.Visible;
            });
        }

        private void CloseApp() => Application.Current.Shutdown();
    }
}