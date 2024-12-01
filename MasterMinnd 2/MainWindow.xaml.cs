using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MasterMinnd_2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<string> availibleColors = new List<string> { "Rood", "Geel", "Oranje", "Wit", "Groen", "Blauw" };
        private List<string> secretCode = new List<string>();
        private List<string> userCode = new List<string>(); // Toevoeging voor userCode

        public MainWindow()
        {
            InitializeComponent();
            GenerateRandomKleur();
            ComboBoxesKleurVuller();
        }

        // Random kleur generator + om in de titel weer te geven
        private void GenerateRandomKleur()
        {
            Random randomColorGenerator = new Random();
            secretCode.Clear();

            for (int i = 0; i < 4; i++)
            {
                int index = randomColorGenerator.Next(availibleColors.Count);
                secretCode.Add(availibleColors[index]);
            }

            this.Title = "Secret kleur combinatie is: " + string.Join(", ", secretCode);
        }

        // ComboBoxen vullen met beschikbare kleuren
        private void ComboBoxesKleurVuller()
        {
            var colors = new List<ColorItem>
            {
                new ColorItem { Name = "Rood", Color = Colors.Red },
                new ColorItem { Name = "Geel", Color = Colors.Yellow },
                new ColorItem { Name = "Oranje", Color = Colors.Orange },
                new ColorItem { Name = "Wit", Color = Colors.White },
                new ColorItem { Name = "Groen", Color = Colors.Green },
                new ColorItem { Name = "Blauw", Color = Colors.Blue }
            };

            RandomColorComboBox1.ItemsSource = colors;
            RandomColorComboBox2.ItemsSource = colors;
            RandomColorComboBox3.ItemsSource = colors;
            RandomColorComboBox4.ItemsSource = colors;
        }

        public class ColorItem
        {
            public required string Name { get; set; }
            public Color Color { get; set; }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            ColorItem selectedItem = comboBox.SelectedItem as ColorItem;

            if (selectedItem != null)
            {
                if (comboBox.Name == "RandomColorComboBox1")
                {
                    SelectedColorLabel1.Background = new SolidColorBrush(selectedItem.Color);
                }
                else if (comboBox.Name == "RandomColorComboBox2")
                {
                    SelectedColorLabel2.Background = new SolidColorBrush(selectedItem.Color);
                }
                else if (comboBox.Name == "RandomColorComboBox3")
                {
                    SelectedColorLabel3.Background = new SolidColorBrush(selectedItem.Color);
                }
                else if (comboBox.Name == "RandomColorComboBox4")
                {
                    SelectedColorLabel4.Background = new SolidColorBrush(selectedItem.Color);
                }
            }
        }

        private void Button_CheckColorCombination(object sender, RoutedEventArgs e)
        {
            userCode = new List<string>
            {
                (RandomColorComboBox1.SelectedItem as ColorItem)?.Name,
                (RandomColorComboBox2.SelectedItem as ColorItem)?.Name,
                (RandomColorComboBox3.SelectedItem as ColorItem)?.Name,
                (RandomColorComboBox4.SelectedItem as ColorItem)?.Name
            };

            for (int i = 0; i < 4; i++)
            {
                if (userCode[i] == secretCode[i])
                {
                    SetBorderColor(i, Colors.DarkRed);
                }
                else if (secretCode.Contains(userCode[i]))
                {
                    SetBorderColor(i, Colors.Wheat);
                }
                else
                {
                    SetBorderColor(i, Colors.Transparent);
                }
            }
        }

        private void SetBorderColor(int index, Color borderColor)
        {
            switch (index)
            {
                case 0:
                    SelectedColorLabel1.BorderBrush = new SolidColorBrush(borderColor);
                    break;
                case 1:
                    SelectedColorLabel2.BorderBrush = new SolidColorBrush(borderColor);
                    break;
                case 2:
                    SelectedColorLabel3.BorderBrush = new SolidColorBrush(borderColor);
                    break;
                case 3:
                    SelectedColorLabel4.BorderBrush = new SolidColorBrush(borderColor);
                    break;
            }
        }

        private void Button_LeaveGame(object sender, RoutedEventArgs e)
        {
            Close();
        }

        int maxAttemptsUser = 10;
        int currentAttempt = 0;
        bool gameEnded = false;

        private void ReduceAttempts()
        {
            currentAttempt++;
            int attemptsLeft = maxAttemptsUser - currentAttempt;

            AttemptsLeftLabel.Content = $"Pogingen over: {attemptsLeft}";

            if (attemptsLeft <= 0)
            {
                MessageBox.Show($"Je hebt verloren! De geheime code was: {string.Join(", ", secretCode)}", "Game Over", MessageBoxButton.OK, MessageBoxImage.Information);
                ResetGame();
            }
        }

        private void ResetGame()
        {
            maxAttemptsUser = 10;
            currentAttempt = 0;
            gameEnded = false;
            GenerateRandomKleur();
            ClearUI();
        }

        private void ClearUI()
        {
            RandomColorComboBox1.SelectedItem = null;
            RandomColorComboBox2.SelectedItem = null;
            RandomColorComboBox3.SelectedItem = null;
            RandomColorComboBox4.SelectedItem = null;

            SelectedColorLabel1.Background = new SolidColorBrush(Colors.White);
            SelectedColorLabel2.Background = new SolidColorBrush(Colors.White);
            SelectedColorLabel3.Background = new SolidColorBrush(Colors.White);
            SelectedColorLabel4.Background = new SolidColorBrush(Colors.White);

            SetBorderColor(0, Colors.Transparent);
            SetBorderColor(1, Colors.Transparent);
            SetBorderColor(2, Colors.Transparent);
            SetBorderColor(3, Colors.Transparent);

            AttemptsLeftLabel.Content = $"Pogingen over: {maxAttemptsUser}";
        }

        private void Button_CheckColorCombination2(object sender, RoutedEventArgs e)
        {
            if (userCode.SequenceEqual(secretCode))
            {
                MessageBox.Show("Gefeliciteerd! Je hebt de geheime code geraden!", "Winnaar", MessageBoxButton.OK, MessageBoxImage.Information);
                gameEnded = true;
                return;
            }

            ReduceAttempts();
        }
    }
}

    
