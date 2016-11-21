using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace TicTacToe
{
    public class GameLogicLayer
    {
        private ConnectionHandler cHandler;
        private string opponnentNickname;
        private int opponnentDescriptor;
        private int isMyTurn;
        public char myTurn { get; set; }

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
            string[] splittedResponse;
            
            splittedResponse = response.Split(' ');

            opponnentNickname = splittedResponse[0];
            opponnentDescriptor = Int32.Parse(splittedResponse[1]);
            isMyTurn = Int32.Parse(splittedResponse[2]);

            Console.WriteLine("nick przeciwnika to: " + opponnentNickname);
            Console.WriteLine("deskryptor przeciwnika to: " + opponnentDescriptor);
            Console.WriteLine("moja kolejnosc? : " + isMyTurn);

            if (isMyTurn == 1)
            {
                myTurn = 'O';
                MessageBox.Show("Twoj ruch to " + myTurn);
            }
            else
            {
                myTurn = 'X';
                MessageBox.Show("Twoj ruch to " + myTurn);
            }

            //if (isMyTurn == 1)
            //{

            //}
            //else
            //{

            //}

            //return status;
        }

        public void sendClickedField(string field)
        {
            if(isMyTurn == 1)
            {
                cHandler.SendData("2 " + field + " " + opponnentDescriptor);
                isMyTurn = 0;
            }
            else
            {
                string opponnentTurn = cHandler.Receive();

                Console.WriteLine(opponnentTurn);
                isMyTurn = 1;
            }
        }

        public void Connect(string ip)
        {
            cHandler.Connect(ip);
        }
    }
}
