using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System;
using Microsoft.Data.SqlClient;
using System.Data;

Console.WriteLine("1 - Sign in\n2 - Sign up\n3 - Exit");
var choice = Console.ReadLine();

switch (choice)
{
    case "1":
        SignUp.RegisterUser();
        break;
    case "2":
        SignIn.LoginUser();
        break;
    case "3":
        Console.WriteLine("Exit");
        break;

}
public class SignUp
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int Age { get; set; }
    public string Login { get; set; }
    public string Password { get; set; }

    public SignUp(string firstname, string lastname, int age, string login, string password)
    {
        FirstName = firstname;
        LastName = lastname;
        Age = age;
        Login = login;
        Password = password;
    }

    public static void RegisterUser()
    {
        Console.Write("User Name: ");
        string firstName = Console.ReadLine()!;

        Console.Write("User Lastname: ");
        string lastName = Console.ReadLine()!;

        Console.Write("User Age: ");
        int age;
        while (!int.TryParse(Console.ReadLine(), out age))
        {
            Console.Write("Select correct age: ");
        }

        Console.Write("Login: ");
        string login = Console.ReadLine()!;

        Console.Write("Password: ");
        string password = Console.ReadLine()!;

        string connectionString = @"Server=(localdb)\MSSQLLocalDB;Database=Users;Integrated Security=True;Encrypt=False;";

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();


            string insertQuery = $@"INSERT INTO Users (FirstName, LastName, Age, Login, Password) 
                                    VALUES ('{firstName}', '{lastName}', {age}, '{login}', '{password}')";

            using (SqlCommand command = new SqlCommand(insertQuery, connection))
            {
                command.ExecuteNonQuery();
            }
        }

        Console.WriteLine("Registed user data");
    }
}

public class SignIn
{
    public static void LoginUser()
    {

        Console.Write("Login: ");
        string login = Console.ReadLine()!;
        Console.Write("Password: ");
        string password = Console.ReadLine()!;

        string connectionString = @"Server=(localdb)\MSSQLLocalDB;Database=Users;Integrated Security=True;Encrypt=False;";

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();

            string query = "SELECT Login, Password FROM Users WHERE Login = @login";

            using (SqlCommand command = new SqlCommand(query, connection))
            {

                command.Parameters.AddWithValue("@login", login);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read()) 
                    {
                        string dbPassword = reader["Password"].ToString()!;

                        if (dbPassword == password) 
                        {
                            Console.WriteLine("Join success");
                        }
                        else
                        {
                            Console.WriteLine("Login or Password wrong!");
                        }
                    }
                    else
                    {
                        Console.WriteLine("User not found!");
                    }
                }
            }
        }
    }
}
