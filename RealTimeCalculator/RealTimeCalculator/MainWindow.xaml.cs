
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using CalculatorEngine;

namespace RealTimeCalculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    public partial class MainWindow : Window
    {
        public ClassCalculator taschenrechner;
        Button button;

        // History 
        private DispatcherTimer longPressTimer;
        private bool isLongPressActive = false;

        public MainWindow()
        {
            InitializeComponent();

            txtDisplay.Text = "0";

            taschenrechner = new ClassCalculator();
            longPressTimer = new DispatcherTimer();
            longPressTimer.Interval = TimeSpan.FromSeconds(1);
            longPressTimer.Tick += LongPressTimer_Tick;

        }

        private void Button_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.D0:
                case Key.NumPad0:
                    btn0.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                    break;
                case Key.D1:
                case Key.NumPad1:
                    btn1.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                    break;
                case Key.D2:
                case Key.NumPad2:
                    btn2.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                    break;
                case Key.D3:
                case Key.NumPad3:
                    btn3.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                    break;
                case Key.D4:
                case Key.NumPad4:
                    btn4.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                    break;
                case Key.D5:
                case Key.NumPad5:
                    btn5.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                    break;
                case Key.D6:
                case Key.NumPad6:
                    btn6.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                    break;
                case Key.D7:
                case Key.NumPad7:
                    btn7.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                    break;
                case Key.D8:
                case Key.NumPad8:
                    btn8.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                    break;
                case Key.D9:
                case Key.NumPad9:
                    btn9.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                    break;
                case Key.Add:
                    btnAdd.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                    break;
                case Key.Subtract:
                    btnSubtract.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                    break;
                case Key.Multiply:
                    btnMultiply.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                    break;
                case Key.Divide:
                    btnDivide.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                    break;
                case Key.Decimal:
                    btnDot.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                    break;
                case Key.Back:
                    btnBackspace.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                    break;
                case Key.C:
                    btnClear.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                    break;
                case Key.S:
                    btnSin.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                    break;
                case Key.O:
                    btnCos.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                    break;
                case Key.T:
                    btnTan.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                    break;
                case Key.OemPlus:
                    btnPlusMinus.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                    break;
                // Add more cases for other keys as needed
                default:
                    break;
            }
        }

        private void UpdateDisplay()
        {
            txtDisplay.Text = taschenrechner.GetInputString();
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            txtDisplay.Clear();
            taschenrechner.operandsStack.Clear();
            taschenrechner.operatorsStack.Clear();
        }

        private void btnNum_Click(object sender, RoutedEventArgs e)
        {
            button = sender as Button;
            if (button != null)
            {
                txtDisplay.Text += button.Content;
                taschenrechner.SetInput();
                taschenrechner.SetNumber(button.Content.ToString());
                UpdateDisplay();
            }
        }

        private void btnOperator_Click(object sender, RoutedEventArgs e)
        {
            button = sender as Button;

            if (button != null)
            {
                taschenrechner.AddOperator(Convert.ToChar(button.Content));
                UpdateDisplay();
            }
        }

        private void btnDot_Click(object sender, RoutedEventArgs e)
        {
            button = sender as Button;

            if (button != null && !txtDisplay.Text.Contains('.'))
            {
                taschenrechner.SetInput();
                taschenrechner.SetComma();
                UpdateDisplay();
            }
        }

        // Löschen geht nur am Anfang 
        // TO CHANGE IT: Playing around with stacks
        private void btnBackspace_Click(object sender, RoutedEventArgs e)
        {
            if (txtDisplay.Text.Length > 0)
            {
                txtDisplay.Text = txtDisplay.Text.Remove(txtDisplay.Text.Length - 1, 1);
                taschenrechner.SetNumber(txtDisplay.Text);
            }
            UpdateDisplay();
        }

        private void btnPlusMinus_Click(object sender, RoutedEventArgs e)
        {
            button = sender as Button;

            if (taschenrechner.isNumber && button != null && button.Content == "+/-")
            {
                taschenrechner.SetInput();

                if (txtDisplay.Text.StartsWith("-"))
                {
                    txtDisplay.Text = txtDisplay.Text.Substring(1);
                }
                else
                {
                    txtDisplay.Text = "-" + txtDisplay.Text;
                }

                taschenrechner.SetNumber(txtDisplay.Text);
                UpdateDisplay();
            }
        }

        // Trigonometrie
        private void btnSin_Click(object sender, RoutedEventArgs e)
        {
            txtDisplay.Text = taschenrechner.CalculateSine(txtDisplay.Text);
            UpdateDisplay();
        }

        private void btnCos_Click(object sender, RoutedEventArgs e)
        {
            txtDisplay.Text = taschenrechner.CalculateCosine(txtDisplay.Text);
            UpdateDisplay();
        }

        private void btnTan_Click(object sender, RoutedEventArgs e)
        {
            txtDisplay.Text = taschenrechner.CalculateTangent(txtDisplay.Text);
            UpdateDisplay();
        }

        // History methods
        private void btn0_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            isLongPressActive = true;
            LongPressProgressBar.Visibility = Visibility.Visible;
            Storyboard sb = this.FindResource("LongPressAnimation") as Storyboard;
            if (sb != null) sb.Begin();
            longPressTimer.Start();
        }

        private void btn0_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (isLongPressActive)
            {
                isLongPressActive = false;
                LongPressProgressBar.Visibility = Visibility.Collapsed;
                Storyboard sb = this.FindResource("LongPressAnimation") as Storyboard;
                if (sb != null) sb.Stop();
                longPressTimer.Stop();
                LongPressProgressBar.Value = 0;
            }
        }

        private void LongPressTimer_Tick(object sender, EventArgs e)
        {
            longPressTimer.Stop();
            isLongPressActive = false;
            LongPressProgressBar.Visibility = Visibility.Collapsed;
            Storyboard sb = this.FindResource("LongPressAnimation") as Storyboard;
            if (sb != null) sb.Stop();
            LongPressProgressBar.Value = 0;

            ShowHistory();
        }

        private void ShowHistory()
        {
            // Implement history showing logic here
            List<string> history = taschenrechner.GetHistory();
            StringBuilder historyText = new StringBuilder();
            foreach (string entry in history)
            {
                historyText.AppendLine(entry);
            }
            MessageBox.Show(historyText.ToString(), "Calculation History");
        }
    }
}
