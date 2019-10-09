using System;
using System.Collections.Generic;
using System.Net;
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
        static void Main(string[] args)
        {
            DBConnection conn = DBConnection.Instance();
            DBCredentials credentials = DBCredentials.ParseFromJsonFile(@"Resources\dbcredentials.json");
            conn.SetDBCredentials(credentials);
            conn.IsConnected();

            var stateExtractor = new StateExtractor();

            string input = "";
            while (!input.Equals("quit", StringComparison.OrdinalIgnoreCase))
            {
                Console.Clear();
                var currentState = stateExtractor.GetCurrentState();
                Console.WriteLine($"P1Name: {currentState.P1Name}\n" +
                                  $"P2Name: {currentState.P2Name}\n" +
                                  $"P1Total: {currentState.P1Total}\n" +
                                  $"P2Total: {currentState.P2Total}\n" +
                                  $"Status: {currentState.Status}\n" +
                                  $"Alert: {currentState.Alert}\n" +
                                  $"X: {currentState.X}\n" +
                                  $"Remaining: {currentState.Remaining}\n");

                input = Console.ReadLine();
            }


        }

        private static void LoginService_OnLoggedIn(object sender, EventArgs e)
        {
            Console.WriteLine("Successfully logged in!");
        }

        private static void BetService_OnBetPlaced(object sender, EventArgs e)
        {
            Console.WriteLine("Bet successfully placed!");
        }
    }
}
