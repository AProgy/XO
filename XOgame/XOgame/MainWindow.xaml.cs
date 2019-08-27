using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace XOgame
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            o.Visibility = Visibility.Hidden;
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
                Win(turn, xPos, oPos);
        }

        private async void Win(byte turn, List<byte> xPos, List<byte> oPos)
        {
            await Task.Run(() =>
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
                    else if (MessageBox.Show($"Do you want to start new game?", "X win!", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No) Application.Current.Shutdown();

                    if (oCount != 3) oCount = 0;
                    else if (MessageBox.Show($"Do you want to start new game?", "O win!", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                    {
                        Process.Start(Application.ResourceAssembly.Location);
                        Application.Current.Shutdown();
                    }
                }
            });
        }
    }
}