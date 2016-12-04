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
        public string opponnentNickname { get; set; }
        private int opponnentDescriptor;
        //public Button a1 { get; set; }
        //public Button a2 { get; set; }
        //public Button a3 { get; set; }
        public Grid LayoutRoot { get; set; }

        public int isMyTurn;
        public char myTurn { get; set; }
        public char opponnentsTurn { get; set; }

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
                    myTurn = 'X';
                    opponnentsTurn = 'O';

                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        foreach (Control c in LayoutRoot.Children)
                        {
                            if (c is Label)
                            {
                                Label myNickLabel = (Label)LayoutRoot.FindName("myNickLabel");
                                Label myTurnLabel = (Label)LayoutRoot.FindName("myTurnLabel");
                                Label opponnentsTurnLabel = (Label)LayoutRoot.FindName("opponnentsTurnLabel");
                                Label opponnentsNickLabel = (Label)LayoutRoot.FindName("opponnentsNickLabel");
                                myNickLabel.Content = login;
                                myTurnLabel.Content = myTurn;
                                opponnentsNickLabel.Content = opponnentNickname;
                                opponnentsTurnLabel.Content = opponnentsTurn;
                            }
                        }
                    });

                    cHandler.Receive((res) =>
                    {
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            foreach (Control c in LayoutRoot.Children)
                            {
                                if (c is Button)
                                {
                                    Button b = (Button)c;
                                    if (b.Content == "")
                                    {
                                        b.IsEnabled = true;
                                    }
                                }
                            }
                            Button opponnentsTurnButton = (Button)LayoutRoot.FindName(res.Substring(0, 2));
                            opponnentsTurnButton.Content = opponnentsTurn;
                            opponnentsTurnButton.IsEnabled = false;
                            isMyTurn = 1;
                        });
                    });
                } else
                {
                    myTurn = 'O';
                    opponnentsTurn = 'X';

                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        foreach (Control c in LayoutRoot.Children)
                        {
                            if (c is Button)
                            {
                                c.IsEnabled = true;
                            }
                            else if (c is Label)
                            {
                                Label myNickLabel = (Label)LayoutRoot.FindName("myNickLabel");
                                Label myTurnLabel = (Label)LayoutRoot.FindName("myTurnLabel");
                                Label opponnentsTurnLabel = (Label)LayoutRoot.FindName("opponnentsTurnLabel");
                                Label opponnentsNickLabel = (Label)LayoutRoot.FindName("opponnentsNickLabel");
                                myNickLabel.Content = login;
                                myTurnLabel.Content = myTurn;
                                opponnentsNickLabel.Content = opponnentNickname;
                                opponnentsTurnLabel.Content = opponnentsTurn;
                            }
                        }
                    });
                }
            });
            
        }

        public void sendClickedField(string field)
        {
            if (isMyTurn == 1)
            {
                foreach (Control c in LayoutRoot.Children)
                {
                    if (c is Button)
                    {
                        c.IsEnabled = false;
                    }
                }

                cHandler.SendData("2 " + field + " " + opponnentDescriptor);
                isMyTurn = 0;
                
                cHandler.Receive((res) =>
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        foreach (Control c in LayoutRoot.Children)
                        {
                            if (c is Button)
                            {
                                Button bb = (Button)c;
                                if (bb.Content == "")
                                {
                                    bb.IsEnabled = true;
                                }
                            }
                        }
                        if (res.Substring(0, 1) != 4.ToString())
                        {
                            Button opponnentsTurnButton = (Button)LayoutRoot.FindName(res.Substring(0, 2));
                            opponnentsTurnButton.Content = opponnentsTurn;
                            opponnentsTurnButton.IsEnabled = false;
                        } else
                        {
                            string[] splittedResponse = res.Split(null);
                            //MessageBox.Show(splittedResponse[3]);
                            if (splittedResponse.Length > 3)
                            {
                                Button opponnentsTurnButton = (Button)LayoutRoot.FindName(splittedResponse[3].Substring(0, 2));
                                opponnentsTurnButton.Content = opponnentsTurn;
                                opponnentsTurnButton.IsEnabled = false;
                            }
                            MessageBox.Show("Mamy zwycięzcę! Wygrywa " + opponnentNickname);

                            foreach (Control c in LayoutRoot.Children)
                            {
                                if (c is Button)
                                {
                                    Button bbb = (Button)c;
                                    bbb.IsEnabled = false;
                                }
                            }
                        }
                        
                        isMyTurn = 1;
                    });
                });
            }
        
        }

        public void Connect(string ip)
        {
            cHandler.Connect(ip);
        }
    }
}
