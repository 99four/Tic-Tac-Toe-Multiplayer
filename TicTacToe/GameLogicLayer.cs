using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe
{
    public class GameLogicLayer
    {
        private ConnectionHandler cHandler;

        public GameLogicLayer()
        {
            cHandler = new ConnectionHandler();
        }

        public void Join(string login)
        {
            Console.WriteLine(login);
            cHandler.SendData("1 " + login);
            //int status = Int32.Parse(cHandler.Receive());
            string response = cHandler.Receive();
            Console.WriteLine(response);

            //return status;
        }

        public void Connect(string ip)
        {
            cHandler.Connect(ip);
        }
    }
}
