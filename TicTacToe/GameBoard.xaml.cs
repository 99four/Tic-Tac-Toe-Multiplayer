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
    }
}
