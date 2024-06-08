using UnityEngine;
using MySql.Data.MySqlClient;
using System;

public class DatabaseManager : MonoBehaviour
{
    private MySqlConnection connection;

    //M�todo para conectar con la base de datos
    public void ConnectToDatabase(string server, string database, string uid, string password, string port)
    {
        string connectionString = $"Encrypt=false;SERVER={server};DATABASE={database};UID={uid};PASSWORD={password};PORT={port}";
        connection = new MySqlConnection(connectionString);

        try
        {
            connection.Open();
            Debug.Log("Conexi�n realizada a la base de datos.");
        }
        catch (Exception ex)
        {
            Debug.LogError("Error al conectar a la base de datos: " + ex.ToString());
        }
    }
    //M�todo para insertar una nueva puntuaci�n
    public void InsertScore(int newScore)
    {
        if (connection.State == System.Data.ConnectionState.Open)
        {
            string query = "INSERT INTO TEstadisticas (puntos) VALUES (@puntos)";
            using (MySqlCommand cmd = new MySqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@puntos", newScore);

                try
                {
                    cmd.ExecuteNonQuery();
                    Debug.Log($"Puntuaci�n de {newScore} insertada correctamente.");
                }
                catch (MySqlException ex)
                {
                    Debug.LogError("Error al insertar la puntuaci�n: " + ex.Message);
                }
            }
        }
        else
        {
            Debug.LogError("La conexi�n no est� abierta.");
        }
    }
    //M�todo para obtener la puntuaci�n m�xima
    public int GetMaxScore()
    {
        if (connection.State == System.Data.ConnectionState.Open)
        {
            string query = "SELECT MAX(puntos) FROM TEstadisticas";
            using (MySqlCommand cmd = new MySqlCommand(query, connection))
            {
                object result = cmd.ExecuteScalar();
                if (result != DBNull.Value)
                {
                    return Convert.ToInt32(result);
                }
                else
                {
                    return 0;
                }
            }
        }
        else
        {
            Debug.LogError("La conexi�n no est� abierta.");
            return 0;
        }
    }

    private void OnDestroy()
    {
        if (connection != null)
        {
            connection.Close();
            Debug.Log("Conexi�n cerrada.");
        }
    }
}
