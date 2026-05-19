using Microsoft.Data.Sqlite;

namespace TodoMVC.Data;

public static class Database
{
    private const string ConnectionString = "Data Source=peliculasyseries.db";

    public static void Inicializar()
    {
        using var connection = AbrirConexion();
        var sql = @"
            CREATE TABLE IF NOT EXISTS PeliculasySeries (
                Id         INTEGER PRIMARY KEY AUTOINCREMENT,
                Titulo     TEXT    NOT NULL,
                Genero TEXT NOT NULL,
                Estreno DATETIME NOT NULL,
                Completada INTEGER NOT NULL DEFAULT 0
            )";
        using var cmd = new SqliteCommand(sql, connection);
        cmd.ExecuteNonQuery();
    }

    public static SqliteConnection AbrirConexion()
    {
        var connection = new SqliteConnection(ConnectionString);
        connection.Open();
        return connection;
    }
}
