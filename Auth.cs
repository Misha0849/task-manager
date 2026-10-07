using System.Security.Cryptography;
using System.Text;
using Microsoft.Data.Sqlite;

namespace TaskManager;

public static class Auth
{
    public static string HashPassword(string password)
    {
        using var sha = SHA256.Create();
        var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToHexString(bytes);
    }

    public static bool Register(string username, string password)
    {
        using var conn = Database.GetConnection();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = "INSERT INTO users (username, password) VALUES ($u, $p)";
        cmd.Parameters.AddWithValue("$u", username);
        cmd.Parameters.AddWithValue("$p", HashPassword(password));
        try
        {
            cmd.ExecuteNonQuery();
            return true;
        }
        catch (SqliteException)
        {
            return false;
        }
    }

    public static User? Login(string username, string password)
    {
        using var conn = Database.GetConnection();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT id, username, password FROM users WHERE username = $u";
        cmd.Parameters.AddWithValue("$u", username);
        using var reader = cmd.ExecuteReader();
        if (!reader.Read()) return null;

        var stored = reader.GetString(2);
        if (stored != HashPassword(password)) return null;

        return new User
        {
            Id = reader.GetInt32(0),
            Username = reader.GetString(1)
        };
    }
}

public class User
{
    public int Id { get; set; }
    public string Username { get; set; } = "";
}