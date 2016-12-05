using System;
using System.Collections.Generic;
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

namespace TicTacToe
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public GameLogicLayer gameLogicLayer;
        public GameBoard gameBoard;
        public ConnectionHandler cHandler;

        public MainWindow()
        {
            gameLogicLayer = new GameLogicLayer();
            cHandler = new ConnectionHandler();
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            string login = nickNameTextBox.Text;
            //string ip = ipTextBox.Text;
            string ip = "192.168.0.102";

            gameLogicLayer.Connect(ip);

            if (login.Length == 0)
            {
                alertLabel.Content = "Wpisz jakiś login";
            }
            else if (login.Any(x => Char.IsWhiteSpace(x)))
            {
                alertLabel.Content = "Login zawiera białe znaki.";
            }
            else
            {
                gameBoard = new GameBoard(gameLogicLayer, login);
                gameBoard.Show();
                gameLogicLayer.Join(login);
                this.Close();
            }
        }

        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
