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

        string connectionString = "Server=tcp:owen-ditore-personal-projects.database.windows.net,1433;Initial Catalog=ChessPersonalProject;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;Authentication=\"Active Directory Default\";";

        /*
        using(SqlConnection connection = new SqlConnection( connectionString ))
        {
            connection.Open();
            Console.WriteLine( "Connection Open successful!" );

            int userID = LoginUser( connection );


        }
        Console.WriteLine( "Connection closed. Press any key to finish..." );
        Console.ReadKey();

        */

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

            string query = $"INSERT INTO Users (UserID, Username) VALUES ({userID}, '{username}');";
            SqlCommand command = new SqlCommand( query, connection );

            command.ExecuteNonQuery();

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

