using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using SaltyLibrary;
using SaltyLibrary.Data;
using SaltyLibrary.Repositories;
using SaltyLibrary.Saltybet;
using SaltyLibrary.Saltybet.Enums;
using SaltyLibrary.Services;
using SaltyLibrary.Users;
using WebSocketSharp;

namespace ConsoleApp
{
    class Program
    {
        private static DBConnection db;
        private static SaltyWebSocketListener wsListener;
        private static FighterRepository fighterRepository;
        private static StateExtractor stateExtractor;
        private static State currentState;
        private static Match currentMatch;
        private static bool newRound = false;

        static void Main(string[] args)
        {
            fighterRepository = new FighterRepository();
            stateExtractor = new StateExtractor();
            currentState = new State();
            InitializeDB();
            InitializeWebSocketListener();

            Console.ReadLine();
        }

        private static void UpdateState()
        {
            State newState = stateExtractor.GetCurrentState();
            if (newState.Status != currentState.Status)
            {
                currentState = newState;
                EvaluateNewState();
            }
        }

        private static void EvaluateNewState()
        {
            switch (currentState.Status)
            {
                case "open":

                    newRound = true;

                    if (currentMatch == null)
                    {
                        currentMatch = new Match()
                        {
                            P1Name = currentState.P1Name,
                            P2Name = currentState.P2Name,
                        };
                    }

                    break;

                case "closed":

                    if (currentMatch.P1Total == null || currentMatch.P2Total == null)
                    {
                        currentMatch.P1Total = currentState.P1Total;
                        currentMatch.P2Total = currentMatch.P2Total;
                    }

                    break;

                case "1":

                    if (newRound)
                    {
                        currentMatch.Winner = TeamColor.RED;
                        UpdateDatabase(currentMatch);
                        newRound = false;
                    }
                    break;

                case "2":

                    if (newRound)
                    {
                        currentMatch.Winner = TeamColor.BLUE;
                        UpdateDatabase(currentMatch);
                        newRound = false;
                    }
                    break;

                default:
                    break;
            }
        }

        private static void SaltyWS_OnMessageReceived(object sender, MessageEventArgs e)
        {
            if (e.Data.Contains("42"))
            {
                UpdateState();
            }
        }

        private static void InitializeWebSocketListener()
        {
            wsListener = new SaltyWebSocketListener(SaltyWS_OnMessageReceived);
            wsListener.Connect();
        }

        private static void InitializeDB()
        {
            var credentials = DBCredentials.ParseFromJsonFile(@"Resources\dbcredentials.json");
            db = DBConnection.Instance();
            db.SetDBCredentials(credentials);
        }

        private static void UpdateDatabase(Match match)
        {
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
        }
    }
}
