
// MasterMind game by SpicyGames, schoolproject

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
        // Lijst van beschikbare kleuren voor het spel
        private readonly List<ColorItem> availableColors = new List<ColorItem>
        {
            new ColorItem { Name = "Rood", Color = Colors.Red },
            new ColorItem { Name = "Geel", Color = Colors.Yellow },
            new ColorItem { Name = "Oranje", Color = Colors.Orange },
            new ColorItem { Name = "Wit", Color = Colors.White },
            new ColorItem { Name = "Groen", Color = Colors.Green },
            new ColorItem { Name = "Blauw", Color = Colors.Blue }
        };

        // Geheime kleurcombinatie
        private List<string> secretCode = new List<string>();

        // Door de gebruiker gekozen kleurcombinatie
        private List<string> userCode = new List<string>();

        // Variabelen gerelateerd aan pogingen
        private int remainingAttempts = 10;
        private int currentAttempt = 0;
        private bool gameEnded = false;
        private int maxAttempts = 10;

        // Lijsten voor ComboBox-besturingselementen en hun bijbehorende labels
        private List<ComboBox> comboBoxes;
        private List<Label> selectedLabels;

        public MainWindow()
        {
            InitializeComponent();

            // Initialiseer ComboBox- en Label-lijsten
            comboBoxes = new List<ComboBox> { RandomColorComboBox1, RandomColorComboBox2, RandomColorComboBox3, RandomColorComboBox4 };
            selectedLabels = new List<Label> { SelectedColorLabel1, SelectedColorLabel2, SelectedColorLabel3, SelectedColorLabel4 };

            // Genereer een willekeurige geheime kleurcombinatie
            GenerateRandomKleur();

            // Vul de ComboBoxen met beschikbare kleuren
            PopulateComboBoxesWithColors();

            // Werk het label met pogingen bij om de beginstatus weer te geven
            UpdateAttemptsLabel();
        }

        /*==================== Kleurgerelateerde Functies ====================*/

        // Genereert een willekeurige kleurcombinatie voor de geheime code
        private void GenerateRandomKleur()
        {
            Random randomColorGenerator = new Random();

            // Schud de lijst van beschikbare kleuren en selecteer de eerste 4
            secretCode = availableColors
                .OrderBy(_ => randomColorGenerator.Next())
                .Take(4)
                .Select(c => c.Name)
                .ToList();

            // Toon de geheime code in de venstertitel (voor debugging)
            this.Title = "Geheime kleurcombinatie is: " + string.Join(", ", secretCode);
        }

        // Vult de ComboBox-besturingselementen met de lijst van beschikbare kleuren
        private void PopulateComboBoxesWithColors()
        {
            foreach (var comboBox in comboBoxes)
            {
                comboBox.ItemsSource = availableColors;
            }
        }

        // Vertegenwoordigt een kleuritem met een naam en kleurwaarde
        public class ColorItem
        {
            public string Name { get; set; } // Naam van de kleur
            public Color Color { get; set; } // Kleurwaarde
        }

        /*==================== Pogingsgerelateerde Functies ====================*/

        // Werkt het label bij dat het aantal resterende pogingen toont
        private void UpdateAttemptsLabel()
        {
            AttemptsLeftLabel.Content = $"POGINGEN OVER: {maxAttempts - currentAttempt}";
        }

        // Vermindert het aantal resterende pogingen en controleert op game over
        private void ReduceAttempts()
        {
            currentAttempt++;
            UpdateAttemptsLabel();

            if (currentAttempt >= maxAttempts)
            {
                // Toon een game over-bericht en reset het spel
                MessageBox.Show($"Je hebt verloren! De geheime code was: {string.Join(", ", secretCode)}", "Game Over", MessageBoxButton.OK, MessageBoxImage.Information);
                ResetGame();
            }
        }

        /*==================== Gebruikersinteractie Functies ====================*/

        // Behandelt de selectie wijzigingsevent voor ComboBox-besturingselementen
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (gameEnded) // Als het spel al voorbij is, informeer de gebruiker
            {
                MessageBox.Show("Het spel is al voorbij. Start een nieuw spel om verder te spelen.", "Informatie", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            ComboBox comboBox = sender as ComboBox;
            if (comboBox == null) return;

            // Haal de index van de ComboBox op en werk het bijbehorende label bij
            int index = comboBoxes.IndexOf(comboBox);
            if (index >= 0)
            {
                ColorItem selectedItem = comboBox.SelectedItem as ColorItem;
                selectedLabels[index].Background = new SolidColorBrush(selectedItem?.Color ?? Colors.White);
            }
        }

        // Behandelt de knopklik om de kleurcombinatie van de gebruiker te controleren
        private void Button_CheckColorCombination(object sender, RoutedEventArgs e)
        {
            CheckColorCombination();
        }

        // Controleert de kleurcombinatie van de gebruiker tegen de geheime code
        private void CheckColorCombination()
        {
            if (gameEnded) // Als het spel al voorbij is, informeer de gebruiker
            {
                MessageBox.Show("Het spel is al voorbij. Start een nieuw spel om verder te spelen.", "Informatie", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            // Haal de door de gebruiker geselecteerde kleurennamen op
            userCode = comboBoxes
                .Select(cb => (cb.SelectedItem as ColorItem)?.Name ?? "")
                .ToList();

            // Zorg ervoor dat alle ComboBoxen een geselecteerde waarde hebben
            if (userCode.Contains(""))
            {
                MessageBox.Show("Selecteer een kleur in alle velden!", "Fout", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            bool allCorrect = true;

            // Vergelijk de code van de gebruiker met de geheime code
            for (int i = 0; i < 4; i++)
            {
                if (userCode[i] == secretCode[i]) // Correcte kleur en positie
                {
                    SetBorderColor(i, Colors.DarkRed);
                }
                else if (secretCode.Contains(userCode[i])) // Correcte kleur, verkeerde positie
                {
                    SetBorderColor(i, Colors.Wheat);
                    allCorrect = false;
                }
                else // Onjuiste kleur
                {
                    SetBorderColor(i, Colors.Transparent);
                    allCorrect = false;
                }
            }

            if (allCorrect) // Als alle kleuren correct zijn, wint de gebruiker
            {
                MessageBox.Show("Gefeliciteerd! Je hebt de geheime code geraden!", "Winnaar", MessageBoxButton.OK, MessageBoxImage.Information);
                gameEnded = true;
            }
            else
            {
                ReduceAttempts(); // Anders, verminder pogingen
            }
        }

        // Stelt de randkleur in voor het label dat aan de opgegeven index is gekoppeld
        private void SetBorderColor(int index, Color borderColor)
        {
            if (index >= 0 && index < selectedLabels.Count)
            {
                selectedLabels[index].BorderBrush = new SolidColorBrush(borderColor);
            }
        }

        // Reset het spel naar de beginstatus
        private void ResetGame()
        {
            currentAttempt = 0;
            gameEnded = false;
            GenerateRandomKleur();
            ClearUI();
        }

        // Leegt de UI-componenten en reset labels en ComboBoxen
        private void ClearUI()
        {
            foreach (var comboBox in comboBoxes)
            {
                comboBox.SelectedItem = null;
            }

            foreach (var label in selectedLabels)
            {
                label.Background = new SolidColorBrush(Colors.White);
                label.BorderBrush = new SolidColorBrush(Colors.Transparent);
            }

            UpdateAttemptsLabel();
        }

        // Behandelt de knopklik om de applicatie te sluiten
        private void Button_LeaveGame(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
