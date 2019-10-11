using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using MySql.Data.MySqlClient;
using SaltyLibrary;
using SaltyLibrary.Data;
using SaltyLibrary.Repositories;
using SaltyLibrary.Saltybet;
using SaltyLibrary.Saltybet.Enums;
using SaltyLibrary.Services;
using SaltyLibrary.Users;

namespace ConsoleApp
{
    class Program
    {
        private static DBConnection db = DBConnection.Instance();
        private static FighterRepository fighterRepository = new FighterRepository();
        static void Main(string[] args)
        {
            Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            client.Connect("www-cdn-twitch.saltybet.com", 1337);

            

            Console.WriteLine("hello");

            /*
            Console.CursorVisible = false;

            Console.WriteLine("[WELCOME TO SALTY BET BOT!]\n");
            Console.WriteLine("Current state:");
    
            var credentials = DBCredentials.ParseFromJsonFile(@"Resources\dbcredentials.json");
            db.SetDBCredentials(credentials);

            State currentState = new State();
            Match match = new Match();

            //Start data extractor thread
            Thread dataExtractorThread = new Thread(() => { DataGatheringLoop(out currentState); });
            dataExtractorThread.Start();

            bool matchEnded = true;

            while (true)
            {
                if (currentState != null)
                {
                    if (match.P1Name != currentState.P1Name || match.P2Name != currentState.P2Name)
                    {
                        match = new Match()
                        {
                            P1Name = currentState.P1Name,
                            P2Name = currentState.P2Name
                        };
                        matchEnded = false;
                    }

                    switch (currentState.Status)
                    {
                        case "open":
                            break;

                        case "locked":
                            break;

                        case "1":

                            if (!matchEnded)
                            {
                                match.Winner = TeamColor.RED;
                                UpdateDatabase(match);
                                matchEnded = true;
                            }
                            break;
                        case "2":

                            if (!matchEnded)
                            {
                                match.Winner = TeamColor.BLUE;
                                UpdateDatabase(match);
                                matchEnded = true;
                            }
                            break;

                        default:
                            break;
                    }
                }
                Thread.Sleep(1000);
            }*/
        }

        private static void DataGatheringLoop(out State state)
        {
            var stateExtractor = new StateExtractor();

            while (true)
            {
                state = stateExtractor.GetCurrentState();
                Thread.Sleep(3000);
            }
        }

        private static void UpdateDatabase(Match match)
        {
            if (!db.IsConnected())
            {
                return;
            }

            TeamColor winner = match.Winner;
            Fighter redFighter = fighterRepository.FighterExists(match.P1Name);
            Fighter blueFighter = fighterRepository.FighterExists(match.P2Name);

            //Check if red fighter exists
            if (redFighter == null)
            {
                Fighter tFighter = new Fighter(match.P1Name, (winner == TeamColor.RED ? 1 : 0), (winner == TeamColor.RED ? 0 : 1));
                fighterRepository.CreateFighter(tFighter);
            }
            else
            {
                Fighter updatedFighter = redFighter;

                if (winner == TeamColor.RED)
                {
                    updatedFighter.Wins++;
                }
                else
                {
                    updatedFighter.Losses++;
                }

                fighterRepository.UpdateFighter(redFighter.ID, updatedFighter);
            }

            //Check if blue fighter exists
            if (blueFighter == null)
            {
                Fighter tFighter = new Fighter(match.P2Name, (winner == TeamColor.BLUE ? 1 : 0), (winner == TeamColor.BLUE ? 0 : 1));
                fighterRepository.CreateFighter(tFighter);
            }
            else
            {
                Fighter updatedFighter = blueFighter;

                if (winner == TeamColor.BLUE)
                {
                    updatedFighter.Wins++;
                }
                else
                {
                    updatedFighter.Losses++;
                }

                fighterRepository.UpdateFighter(blueFighter.ID, updatedFighter);
            }
            db.Connection.Close();
        }
    }
}
