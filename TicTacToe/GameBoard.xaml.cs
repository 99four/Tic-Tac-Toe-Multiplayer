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

        public GameBoard(GameLogicLayer gameLogicLayer, string myLogin, ConnectionHandler cHandler)
        {
            this.gameLogicLayer = gameLogicLayer;
            
            this.myLogin = myLogin;
            InitializeComponent();
        }

        private void a1_Click(object sender, RoutedEventArgs e)
        {
            //a1.SetValue(NameProperty, 'X');
            gameLogicLayer.sendClickedField("a1");
            a1.Content = gameLogicLayer.myTurn;
        }

        private void a2_Click(object sender, RoutedEventArgs e)
        {
            //a1.SetValue(NameProperty, 'X');
            gameLogicLayer.sendClickedField("a2");
            a2.Content = gameLogicLayer.myTurn;
        }

        private void a3_Click(object sender, RoutedEventArgs e)
        {
            //a1.SetValue(NameProperty, 'X');
            gameLogicLayer.sendClickedField("a3");
            a3.Content = gameLogicLayer.myTurn;
        }

        private void b1_Click(object sender, RoutedEventArgs e)
        {
            //a1.SetValue(NameProperty, 'X');
            gameLogicLayer.sendClickedField("b1");
            b1.Content = gameLogicLayer.myTurn;
        }

        private void b2_Click(object sender, RoutedEventArgs e)
        {
            //a1.SetValue(NameProperty, 'X');
            gameLogicLayer.sendClickedField("b2");
            b2.Content = gameLogicLayer.myTurn;
        }

        private void b3_Click(object sender, RoutedEventArgs e)
        {
            //a1.SetValue(NameProperty, 'X');
            gameLogicLayer.sendClickedField("b3");
            b3.Content = gameLogicLayer.myTurn;
        }

        private void c1_Click(object sender, RoutedEventArgs e)
        {
            //a1.SetValue(NameProperty, 'X');
            gameLogicLayer.sendClickedField("c1");
            c1.Content = gameLogicLayer.myTurn;
        }

        private void c2_Click(object sender, RoutedEventArgs e)
        {
            //a1.SetValue(NameProperty, 'X');
            gameLogicLayer.sendClickedField("c2");
            c2.Content = gameLogicLayer.myTurn;
        }

        private void c3_Click(object sender, RoutedEventArgs e)
        {
            //a1.SetValue(NameProperty, 'X');
            gameLogicLayer.sendClickedField("c3");
            c3.Content = gameLogicLayer.myTurn;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.gameLogicLayer.a1 = a1;
            this.gameLogicLayer.a2 = a2;
        }
    }
}
