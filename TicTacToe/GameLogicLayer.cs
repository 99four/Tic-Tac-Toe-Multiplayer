using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace TicTacToe
{
    public class GameLogicLayer
    {
        public ConnectionHandler cHandler;
        private string opponnentNickname;
        private int opponnentDescriptor;
        public Button ZMienna { get; set; }

        public int isMyTurn;
        public char myTurn { get; set; }

        public GameLogicLayer()
        {
            cHandler = new ConnectionHandler();
        }

        public void Join(string login)
        {
            //Console.WriteLine(login);
            cHandler.SendData("1 " + login);
            //int status = Int32.Parse(cHandler.Receive());
            cHandler.Receive((response) => {
                string[] splittedResponse = response.Split(null);
                Console.WriteLine("response to: " + response);

                foreach(string elem in splittedResponse)
                {
                    Console.WriteLine("splitted elem to " + elem);
                }

                opponnentNickname = splittedResponse[0];
                opponnentDescriptor = Int32.Parse(splittedResponse[1]);
                isMyTurn = Int32.Parse(splittedResponse[2]);
                 // jezeli nie moja tura to nasluchuje na odpowiedz , jezeli dostane wiadomosc to uaktualniam plansze i isMyturn=1

                if (isMyTurn == 0)
                {
                    cHandler.Receive((res) =>
                    {
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            ZMienna.Content = "O";
                            isMyTurn = 1;
                        });
                    });
                }
            });
            
        }

        private void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                /* retrieve the SocketStateObject */
                SocketStateObject state = (SocketStateObject)ar.AsyncState;
                Socket socketFd = state.m_SocketFd;

                /* read data */
                int size = socketFd.EndReceive(ar);

                if (size > 0)
                {
                    state.m_StringBuilder.Append(Encoding.ASCII.GetString(state.m_DataBuf, 0, size));

                    /* get the rest of the data */
                    socketFd.BeginReceive(state.m_DataBuf, 0, SocketStateObject.BUF_SIZE, 0,
                                          new AsyncCallback(ReceiveCallback), state);
                }
                else
                {
                    /* all the data has arrived */
                    if (state.m_StringBuilder.Length > 1)
                    {
                        /* shutdown and close socket */
                        socketFd.Shutdown(SocketShutdown.Both);
                        socketFd.Close();
                    }
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show("Exception:\t\n" + exc.Message.ToString());
            }
        }

        public void sendClickedField(string field)
        {
            //string opponnentTurn;
            if (isMyTurn == 1)
            {
                cHandler.SendData("2 " + field + " " + opponnentDescriptor);
                
                isMyTurn = 0;
                cHandler.Receive((res) => Console.WriteLine("Hello "+ res));
            }
        
        }

        public void Connect(string ip)
        {
            cHandler.Connect(ip);
        }
    }
}
