using System;
using System.Collections.Generic;
using System.Text;
using System.CodeDom.Compiler;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;

namespace ChessClassLibrary
{
    public class DatabaseConnection
    {

        //string connectionString = "Server=tcp:owen-ditore-personal-projects.database.windows.net,1433;Initial Catalog=ChessPersonalProject;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;Authentication=\"Active Directory Default\";";
        string connectionString = "Data Source=owen-ditore-personal-projects.database.windows.net;Initial Catalog=ChessPersonalProject;Persist Security Info=False;User ID=ChessLogin;Password=chess2026!;Pooling=False;MultipleActiveResultSets=False;Encrypt=Strict;TrustServerCertificate=False;Application Name=\"Microsoft SQL Server Data Tools, SQL Server Object Explorer\";Command Timeout=0";
        public string GetNewFenIfDifferent(int gameID, string oldFen)
        {
            string newFen = "";

            using(SqlConnection connection = new SqlConnection( connectionString ))
            {
                connection.Open();

                string query = $"SELECT * FROM Games WHERE GameID = {gameID};";

                SqlCommand command = new SqlCommand( query, connection );

                using(var reader = command.ExecuteReader())
                {
                    while(reader.Read())
                    {
                        newFen = reader.GetString(3);
                    }
                }

            }

            if(newFen == oldFen)
                return null;

            return newFen;
        }
        public List<GameLog> GetListOfJoinableGames( int userID )
        {
            List<GameLog> userInProgressGameLogs = new List<GameLog>();

            using(SqlConnection connection = new SqlConnection( connectionString ))
            {
                connection.Open();

                string query = $"SELECT * FROM Games WHERE WhitePlayerID = {userID} OR BlackPlayerID = {userID};";

                SqlCommand command = new SqlCommand( query, connection );

                using(var reader = command.ExecuteReader())
                {
                    while(reader.Read())
                    {
                        GameLog gameLog = new GameLog( reader.GetInt32(0), reader.GetInt32(1), reader.GetInt32(2), reader.GetString(3), reader.GetString(4));
                        if( gameLog.StateOfGame == "In Progress")
                        {
                            userInProgressGameLogs.Add( gameLog );
                        }
                    }
                }
            }

            return userInProgressGameLogs;
        }


        public void EndTurnUpdateGame( int gameID, string fen, string stateOfGame )
        {
            using(SqlConnection connection = new SqlConnection( connectionString ))
            {
                connection.Open();

                string query = $"UPDATE Games SET FEN = '{fen}', StateOfGame = '{stateOfGame}' WHERE GameID = {gameID};";

                SqlCommand command = new SqlCommand( query, connection );

                command.ExecuteNonQuery();

            }
        }

        public int CreateNewGame( int userID, int opponentUserId)
        {
            string startingFEN = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";

            using(SqlConnection connection = new SqlConnection( connectionString ))
            {
                connection.Open();

                int gameID = GenerateNewGameID( connection );

                string query = $"INSERT INTO Games (GameID, WhitePlayerID, BlackPlayerID, FEN, StateOfGame)" +
                               $" VALUES ({gameID},{userID},{opponentUserId},'{startingFEN}','In Progress');";

                SqlCommand command = new SqlCommand( query, connection );
                command.ExecuteNonQuery();

                return gameID;
            }
        }

        private int GenerateNewGameID( SqlConnection connection )
        {
            string query = "SELECT COUNT(*) FROM Games;";
            SqlCommand command = new SqlCommand( query, connection );
            int rows = 0;

            using(var reader = command.ExecuteReader())
            {
                while(reader.Read())
                {
                    rows = reader.GetInt32( 0 );
                }
            }

            int gameID = rows + 1;
            return gameID;
        }
        
        public Dictionary<string,int> GetDictionaryOfUsers()
        {
            using(SqlConnection connection = new SqlConnection( connectionString ))
            {
                connection.Open();
                string query = "SELECT * FROM Users";
                SqlCommand command = new SqlCommand( query, connection );

                Dictionary<string, int> users = new Dictionary<string, int>();

                using(var reader = command.ExecuteReader())
                {
                    while(reader.Read())
                    {
                        users.Add( reader.GetString( 1 ), reader.GetInt32( 0 ) );
                    }
                }

                return users;

            }
        }

        public int LoginUser( bool newUser, string username )
        {
            using(SqlConnection connection = new SqlConnection( connectionString ))
            {
                connection.Open();

                if(newUser)
                {
                    return CreateNewUser( username, connection );
                }

                return GetUserID( username, connection );
            }
        }

        int CreateNewUser( string username, SqlConnection connection )
        {
            int userID = GenerateNewUserID( username, connection );

            try
            {
                string query = $"INSERT INTO Users (UserID, Username) VALUES ({userID}, '{username}');";
                SqlCommand command = new SqlCommand( query, connection );

                command.ExecuteNonQuery();
            }

            catch(Exception ex)
            {
                userID = 0;
            }

            return userID;

        }

        int GenerateNewUserID( string username, SqlConnection connection )
        {
            string query = "SELECT COUNT(*) FROM Users;";
            SqlCommand command = new SqlCommand( query, connection );
            int rows = 0;

            using(var reader = command.ExecuteReader())
            {
                while(reader.Read())
                {
                    rows = reader.GetInt32( 0 );
                }
            }

            int userID = rows + 1;
            return userID;
        }

        int GetUserID( string username, SqlConnection connection )
        {
            string query = $"SELECT UserID FROM Users WHERE Username = '{username}'; ";
            SqlCommand command = new SqlCommand( query, connection );
            int userID = 0;

            using(var reader = command.ExecuteReader())
            {
                while(reader.Read())
                {
                    userID = reader.GetInt32( 0 );
                }
            }

            return userID;
        }

    }
}

