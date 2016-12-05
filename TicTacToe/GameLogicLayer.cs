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
        public int isMyTurn;
        public char myTurn { get; set; }
        public char opponnentsTurn { get; set; }
        public Grid LayoutRoot { get; set; }
        System.Timers.Timer timer = new System.Timers.Timer(10000);
        System.Timers.Timer myTimer = new System.Timers.Timer(10000);

        public GameLogicLayer()
        {
            cHandler = new ConnectionHandler();
            timer.AutoReset = false;
            myTimer.AutoReset = false;
            timer.Elapsed += (source, e) => {
                MessageBox.Show("Nie dostalem odpowiedzi od 10 sekund :(");
                Environment.Exit(0);
            };
            myTimer.Elapsed += (source, e) => {
                MessageBox.Show("Jestem zbyt dlugo nieaktywny");
                Environment.Exit(0);
            };
        }

        public void Join(string login)
        {
            cHandler.SendData("1 " + login);
            cHandler.Receive((response) => {
                string[] splittedResponse = response.Split(null);
                opponnentNickname = splittedResponse[0];
                opponnentDescriptor = Int32.Parse(splittedResponse[1]);
                isMyTurn = Int32.Parse(splittedResponse[2]);

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
                    timer.Start();
                    cHandler.Receive((res) =>
                    {
                        timer.Close();
                        myTimer.Start();
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
                    myTimer.Start();
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
                myTimer.Close();
                
                timer.Start();
                
                isMyTurn = 0;
                
                cHandler.Receive((res) =>
                {
                    myTimer.Start();
                    timer.Close();
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
