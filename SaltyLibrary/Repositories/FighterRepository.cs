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
                string sql = $"INSERT INTO fighter (name, wins, losses) VALUES (@name, @wins, @losses)";
                var cmd = new MySqlCommand(sql, dbConnection.Connection);
                cmd.Parameters.AddWithValue("name", fighter.Name);
                cmd.Parameters.AddWithValue("wins", fighter.Wins);
                cmd.Parameters.AddWithValue("losses", fighter.Losses);
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (MySqlException e)
                {
                    Console.WriteLine("DB error: " + e.Message);
                }
                cmd.Dispose();
                dbConnection.Close();
            }
        }

        public Fighter FindFighterByName(string name)
        {
            Fighter fighter = null;
            if (dbConnection.IsConnected())
            {
                string sql = $"SELECT * FROM fighter WHERE name=@name";
                var cmd = new MySqlCommand(sql, dbConnection.Connection);
                cmd.Parameters.AddWithValue("name", name);
                var reader = cmd.ExecuteReader();
                var fighterData = new object[4];

                if (reader.Read())
                {
                    reader.GetValues(fighterData);
                    fighter = MapRowToFighter(fighterData);
                } 
                reader.Close();
                cmd.Dispose();
                dbConnection.Close();
            }
            return fighter;
        }

        public Fighter FindFighterByID(int id)
        {
            Fighter fighter = null;
            if (dbConnection.IsConnected())
            {
                string sql = $"SELECT * FROM fighter WHERE id=@id";
                var cmd = new MySqlCommand(sql, dbConnection.Connection);
                cmd.Parameters.AddWithValue("id", id);
                var reader = cmd.ExecuteReader();
                var fighterData = new object[4];

                if (reader.Read())
                {
                    reader.GetValues(fighterData);
                    fighter = MapRowToFighter(fighterData);
                }
                reader.Close();
                cmd.Dispose();
                dbConnection.Close();
            }
            return fighter;
        }

        public List<Fighter> FindAllFighters()
        {
            List<Fighter> fighters = new List<Fighter>();

            if (dbConnection.IsConnected())
            {
                string sql = "SELECT * FROM fighter;";
                var cmd = new MySqlCommand(sql, dbConnection.Connection);
                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    var fighter = new Fighter();
                    var fdata = new object[4];
                    reader.GetValues(fdata);
                    fighter = MapRowToFighter(fdata);
                    fighters.Add(fighter);
                }
                reader.Close();
                cmd.Dispose();
                dbConnection.Close();
            }
            return fighters;
        }
        public void UpdateFighter(int id, Fighter fighter)
        {
            if (dbConnection.IsConnected())
            {
                string sql = $"UPDATE fighter SET name = @name, wins = @wins, losses = @losses WHERE id = @id";

                var cmd = new MySqlCommand(sql, dbConnection.Connection);
                cmd.Parameters.AddWithValue("name", fighter.Name);
                cmd.Parameters.AddWithValue("wins", fighter.Wins);
                cmd.Parameters.AddWithValue("losses", fighter.Losses);
                cmd.Parameters.AddWithValue("id", id);

                cmd.ExecuteNonQuery();
                cmd.Dispose();
                dbConnection.Close();
            }
        }

        public void DeleteFighterByID(int id)
        {
            if (dbConnection.IsConnected())
            {
                string sql = $"DELETE FROM fighter WHERE id = @id";
                var cmd = new MySqlCommand(sql, dbConnection.Connection);
                cmd.Parameters.AddWithValue("id", id);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                dbConnection.Close();
            }
        }

        public void DeleteFighterByName(string name)
        {
            if (dbConnection.IsConnected())
            {
                string sql = $"DELETE FROM fighter WHERE name = @name";
                var cmd = new MySqlCommand(sql, dbConnection.Connection);
                cmd.Parameters.AddWithValue("name", name);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                dbConnection.Close();
            }
        }

        public Fighter FighterExists(string name)
        {
            Fighter fighter = null;
            if (dbConnection.IsConnected())
            {
                string sql = $"SELECT * FROM fighter WHERE name = @name";
                var cmd = new MySqlCommand(sql, dbConnection.Connection);
                cmd.Parameters.AddWithValue("name", name);
                var reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    object[] fdata = new object[4];
                    reader.GetValues(fdata);
                    fighter = MapRowToFighter(fdata);
                }
                reader.Close();
                cmd.Dispose();
                dbConnection.Close();
            }
                return fighter;
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
