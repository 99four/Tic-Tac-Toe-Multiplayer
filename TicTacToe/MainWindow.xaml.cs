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
        private GameLogicLayer gameLogicLayer;
        private GameBoard gameBoard;
        public MainWindow()
        {
            gameLogicLayer = new GameLogicLayer();
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            string login = nickNameTextBox.Text;
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
                this.Close();

                //int status = gameLogicLayer.Join(login);
                //Console.WriteLine(status);
                //if (status == 1)
                //{
                //    var gameBoard = new GameBoard(gameLogicLayer, login);
                //    gameBoard.Show();
                //    this.Close();
                //}
                //else
                //{
                //    alertLabel.Content = "Wystąpił błąd";
                //}
            }
        }

        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
