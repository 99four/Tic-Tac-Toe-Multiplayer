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
using System.Windows.Shapes;

namespace TicTacToe
{
    /// <summary>
    /// Interaction logic for GameBoard.xaml
    /// </summary>
    public partial class GameBoard : Window
    {
        private GameLogicLayer gameLogicLayer;
        private string myLogin;

        public GameBoard(GameLogicLayer gameLogicLayer, string myLogin)
        {
            this.gameLogicLayer = gameLogicLayer;
            this.myLogin = myLogin;
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            Button clickedButton = (Button)sender;
            clickedButton.Content = gameLogicLayer.myTurn;
            gameLogicLayer.sendClickedField(clickedButton.Name);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.gameLogicLayer.a1 = a1;
            this.gameLogicLayer.a2 = a2;
            this.gameLogicLayer.a3 = a3;
        }
    }
}
