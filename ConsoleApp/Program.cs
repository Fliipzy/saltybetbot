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
using SaltyLibrary.Twitch;
using SaltyLibrary.Users;
using WebSocketSharp;

namespace ConsoleApp
{
    class Program
    {
        private static DBConnection db;
        private static SaltyBetWebSocketListener stateListener;
        private static TwitchChatWebSocketListener chatListener;
        private static FighterRepository fighterRepository;
        private static StateExtractor stateExtractor;
        private static BetInformationExtractor betInfoExtractor;
        private static State currentState;
        private static Match currentMatch;
        private static bool newRound = false;
        private static GameMode gameMode = GameMode.UNKNOWN;

        private static long matchStartTicks;
        private static long matchDoneTicks;

        private static void OnTwitchChatModMessageReceived(object sender, MessageEventArgs e)
        {
            if (e.Data.Contains("Tier") && currentMatch.P1Tier == null)
            {
                int i = e.Data.IndexOf("Tier") - 2;
                string tier = e.Data.Substring(i, 1);

                currentMatch.P1Tier = tier;
                currentMatch.P2Tier = tier;
            }
        }

        static void Main(string[] args)
        {
            fighterRepository = new FighterRepository();
            stateExtractor = new StateExtractor();
            betInfoExtractor = new BetInformationExtractor();
            currentState = new State();
            currentMatch = new Match();
            InitializeDB();
            InitializeWebSocketListener();
            InitializeTwitchChatListener();

            Console.ReadLine();
            stateListener.Dispose();
            db.Close();
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
            switch (currentState.Alert)
            {
                case "Tournament mode start!":

                    if (gameMode != GameMode.TOURNAMENT)
                    {
                        gameMode = GameMode.TOURNAMENT;
                        WriteLineColor("%cSWITCHED TO TOURNAMENT MODE!", ConsoleColor.Cyan);
                    }
                    break;

                case "Exhibition mode start!":

                    if (gameMode != GameMode.EXHIBITION)
                    {
                        gameMode = GameMode.EXHIBITION;
                        WriteLineColor("%cSWITCHED TO EXHIBITION MODE!", ConsoleColor.Cyan);
                    }
                    break;

                default:

                    string remaining = currentState.Remaining;
                    if (gameMode == GameMode.UNKNOWN)
                    {
                        if (remaining.Contains("exhibition matches left!") || remaining.Contains("after the next exhibition match!"))
                        {
                            WriteLineColor("%cIN EXHIBITION MODE!", ConsoleColor.Cyan);
                            gameMode = GameMode.EXHIBITION;
                            break;
                        }

                        if (remaining.Contains("until the next tournament!"))
                        {
                            WriteLineColor("%cIN MATCHMAKING MODE!", ConsoleColor.Cyan);
                            gameMode = GameMode.MATCHMAKING;
                            break;
                        }

                        if (remaining.Contains("left in the bracket!"))
                        {
                            WriteLineColor("%cIN TOURNAMENT MODE!", ConsoleColor.Cyan);
                            gameMode = GameMode.MATCHMAKING;
                            break;
                        }
                    }

                    if (gameMode == GameMode.EXHIBITION && remaining.Contains("left in the bracket!"))
                    {
                        gameMode = GameMode.MATCHMAKING;
                        WriteLineColor("%cSWITCHED TO MATCHMAKING MODE!", ConsoleColor.Cyan);
                    }
                    break;
            }

            switch (currentState.Status)
            {
                case "open":

                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("Update: ");
                    Console.ResetColor();
                    Console.Write("New round starting soon!\n");

                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("Fighters: ");
                    Console.ResetColor();

                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write(currentState.P1Name);
                    Console.ResetColor();
                    Console.Write(" vs ");
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.Write(currentState.P2Name + "\n");
                    Console.ResetColor();

                    if (!newRound)
                    {
                        newRound = true;
                    }
                    break;

                case "locked":

                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("Update: ");
                    Console.ResetColor();
                    Console.Write("Round has started!\n");

                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("TotalBets: ");

                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write(currentState.P1Total);
                    Console.ResetColor();

                    Console.Write(", ");

                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.Write(currentState.P2Total + "\n");
                    Console.ResetColor();


                    if (newRound)
                    {
                        matchStartTicks = DateTime.Now.Ticks;

                        currentMatch.P1Name = currentState.P1Name;
                        currentMatch.P2Name = currentState.P2Name;
                        currentMatch.P1Total = currentState.P1Total;
                        currentMatch.P2Total = currentMatch.P2Total;

                        Tuple<int, int> pTotalBetsTuple = betInfoExtractor.GetBetInformation();
                        currentMatch.RedTotalBetters = pTotalBetsTuple.Item1;
                        currentMatch.BlueTotalBetters = pTotalBetsTuple.Item2;
                    }
                    break;

                case "1":

                    matchDoneTicks = DateTime.Now.Ticks;

                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("Update: ");
                    Console.ResetColor();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write(currentState.P1Name + " ");
                    Console.ResetColor();
                    Console.Write("won!\n");

                    if (newRound)
                    {
                        currentMatch.Winner = TeamColor.RED;
                        UpdateDatabase(currentMatch);
                        newRound = false;
                        currentMatch = new Match();
                    }
                    break;

                case "2":

                    matchDoneTicks = DateTime.Now.Ticks;

                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("Update: ");
                    Console.ResetColor();
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.Write(currentState.P2Name + " ");
                    Console.ResetColor();
                    Console.Write("won!\n");

                    if (newRound)
                    {
                        currentMatch.Winner = TeamColor.BLUE;
                        UpdateDatabase(currentMatch);
                        newRound = false;
                        currentMatch = new Match();
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

        private static void InitializeTwitchChatListener()
        {
            chatListener = new TwitchChatWebSocketListener(OnTwitchChatModMessageReceived);
            chatListener.Connect();
        }

        private static void InitializeWebSocketListener()
        {
            stateListener = new SaltyBetWebSocketListener(SaltyWS_OnMessageReceived);
            stateListener.Connect();
        }

        private static void InitializeDB()
        {
            var credentials = DBCredentials.ParseFromJsonFile(@"Resources\dbcredentials.json");
            db = DBConnection.Instance();
            db.SetDBCredentials(credentials);
        }

        private static void UpdateDatabase(Match match)
        {
            int duration = (int)(TimeSpan.FromTicks(matchDoneTicks - matchStartTicks).TotalSeconds);

            if (duration > 12)
            {
                duration -= 12;
            }

            match.Duration = duration;

            Console.WriteLine("Match was approx. " + (int)Math.Floor((decimal)duration / 60) + " minute(s) and " + (duration % 60) +" second(s) long!"); ;

            if (gameMode != GameMode.EXHIBITION)
            {
                if (match.Winner == TeamColor.RED)
                {
                    fighterRepository.UpdateFighterWin(match.P1Name, match.P1Tier);
                    fighterRepository.UpdateFighterLoss(match.P2Name, match.P2Tier);
                }
                else
                {
                    fighterRepository.UpdateFighterWin(match.P2Name, match.P2Tier);
                    fighterRepository.UpdateFighterLoss(match.P1Name, match.P1Tier);
                }
            }
        }

        private static void WriteLineColor(string formattedStr, params ConsoleColor[] colors)
        {
            if (formattedStr.Contains("%c") && formattedStr.Length > 2)
            {
                List<int> c_indexes = new List<int>();
                int start = 0;

                while (true)
                {
                    start = formattedStr.IndexOf("%c", start);

                    if (start == -1)
                    {
                        break;
                    }
                    else
                    {
                        c_indexes.Add(start);
                        start += 2;
                    }
                }

                for (int i = 0; i < c_indexes.Count; i++)
                {
                    Console.ForegroundColor = colors[i];
                    string part = "";

                    if (i < c_indexes.Count-1)
                    {
                        part = formattedStr.Substring(c_indexes[i],  c_indexes[i + 1] - c_indexes[i]);
                    }
                    else
                    {
                        part = formattedStr.Substring(c_indexes[i], formattedStr.Length - c_indexes[i]);
                    }

                    int resetIndex = part.IndexOf("%r");
                    
                    if (resetIndex != -1)
                    {
                        Console.Write(part.Substring(2, resetIndex-2));
                        Console.ResetColor();
                        Console.Write(part.Substring(resetIndex+2));
                    }
                    else
                    {
                        Console.Write(part.Substring(2));
                    }
                }
                Console.ResetColor();
                Console.WriteLine();
            }
        }
    }
}
