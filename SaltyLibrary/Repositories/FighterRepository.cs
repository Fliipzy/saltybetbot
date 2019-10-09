using MySql.Data.MySqlClient;
using SaltyLibrary.Data;
using SaltyLibrary.Saltybet;
using System;
using System.Collections.Generic;
using System.Text;

namespace SaltyLibrary.Repositories
{
    public class FighterRepository
    {
        private DBConnection dbConnection = DBConnection.Instance();

        public void CreateFighter(Fighter fighter)
        {
            if (dbConnection.IsConnected())
            {
                string sql = $"INSERT INTO fighter (name, wins, losses) VALUES ('{fighter.Name}', {fighter.Wins}, {fighter.Losses});";
                var cmd = new MySqlCommand(sql, dbConnection.Connection);
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (MySqlException e)
                {
                    Console.WriteLine("DB error: " + e.Message);
                }
            }
        }

        public Fighter FindFighterByName(string name)
        {
            if (dbConnection.IsConnected())
            {
                Fighter fighter = null;
                string sql = $"SELECT * FROM fighter WHERE name='{name}';";
                var cmd = new MySqlCommand(sql, dbConnection.Connection);
                var reader = cmd.ExecuteReader();
                var fighterData = new object[4];

                if (reader.Read())
                {
                    reader.GetValues(fighterData);
                    fighter = MapRowToFighter(fighterData);
                } 
                reader.Close();
                return fighter;
            }
            return null;
        }

        public Fighter FindFighterByID(int id)
        {
            if (dbConnection.IsConnected())
            {
                Fighter fighter = null;
                string sql = $"SELECT * FROM fighter WHERE id={id};";
                var cmd = new MySqlCommand(sql, dbConnection.Connection);
                var reader = cmd.ExecuteReader();
                var fighterData = new object[4];

                if (reader.Read())
                {
                    reader.GetValues(fighterData);
                    fighter = MapRowToFighter(fighterData);
                }
                reader.Close();
                return fighter;
            }
            return null;
        }

        public List<Fighter> FindAllFighters()
        {
            if (dbConnection.IsConnected())
            {
                var fighters = new List<Fighter>();
                string sql = "SELECT * FROM fighter;";
                var cmd = new MySqlCommand(sql, dbConnection.Connection);
                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    var fdata = new object[4];
                    reader.GetValues(fdata);
                    Fighter fighter = MapRowToFighter(fdata);
                    fighters.Add(fighter);
                }
                reader.Close();
                return fighters;
            }
            return null;
        }
        public void UpdateFighter(int id, Fighter fighter)
        {
            if (dbConnection.IsConnected())
            {
                string sql = $"UPDATE fighter SET name = '{fighter.Name}', wins = {fighter.Wins}, losses = {fighter.Losses} WHERE id = {id};";
                var cmd = new MySqlCommand(sql, dbConnection.Connection);
                cmd.ExecuteNonQuery();
            }
        }

        public void DeleteFighterByID(int id)
        {
            if (dbConnection.IsConnected())
            {
                string sql = $"DELETE FROM fighter WHERE id = {id};";
                var cmd = new MySqlCommand(sql, dbConnection.Connection);
                cmd.ExecuteNonQuery();
            }
        }

        public void DeleteFighterByName(string name)
        {
            if (dbConnection.IsConnected())
            {
                string sql = $"DELETE FROM fighter WHERE name = '{name}';";
                var cmd = new MySqlCommand(sql, dbConnection.Connection);
                cmd.ExecuteNonQuery();
            }
        }

        private Fighter MapRowToFighter(object[] fdata)
        {
            Fighter fighter = new Fighter();

            fighter.ID = Int32.Parse(fdata[0].ToString());
            fighter.Name = fdata[1].ToString();
            fighter.Wins = Int32.Parse(fdata[2].ToString());
            fighter.Losses = Int32.Parse(fdata[3].ToString());

            return fighter;
        }
    }
}
