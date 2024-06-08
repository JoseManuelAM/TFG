using System;
using MySql.Data.MySqlClient;

class AAA
{
    static void Main(string[] args)
    {
        string connectionString = "server=127.0.0.1;uid=root;pwd=admin;database=videojuego;";
        MySqlConnection conn = new MySqlConnection(connectionString);

        try
        {
            Console.WriteLine("Conectando a la base de datos...");
            conn.Open();

            // Si llegamos aquí, la conexión fue exitosa
            Console.WriteLine("Conexión exitosa.");

            // Aquí puedes ejecutar comandos o consultas si lo deseas
            // Por ejemplo, para obtener la puntuación máxima:
            string query = "SELECT MAX(puntos) FROM TEstadisticas";
            MySqlCommand cmd = new MySqlCommand(query, conn);
            object result = cmd.ExecuteScalar();
            if (result != null)
            {
                int maxScore = Convert.ToInt32(result);
                Console.WriteLine($"La puntuación máxima es: {maxScore}");
            }
            else
            {
                Console.WriteLine("No se encontraron puntuaciones.");
            }
        }
        catch (MySqlException ex)
        {
            // Si hay un error, se mostrará aquí
            Console.WriteLine($"Error al conectar a la base de datos: {ex.Message}");
        }
        finally
        {
            // Asegúrate de cerrar la conexión cuando termines
            if (conn.State == System.Data.ConnectionState.Open)
            {
                conn.Close();
            }
        }

        Console.WriteLine("Presiona cualquier tecla para salir...");
        Console.ReadKey();
    }
}
