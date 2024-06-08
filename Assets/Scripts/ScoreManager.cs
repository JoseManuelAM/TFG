using System.Collections;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public int puntos;
    public int maxPuntos;
    public TextMeshProUGUI topPointsText; // Referencia al componente TextMeshProUGUI de la UI

    public static ScoreManager Instance;
    private DatabaseManager dbManager;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            dbManager = gameObject.AddComponent<DatabaseManager>();
            dbManager.ConnectToDatabase("localhost", "videojuego", "root", "admin", "3306");
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        puntos = 0;
        StartCoroutine(GetMaxScoreFromDB());
    }

    IEnumerator GetMaxScoreFromDB()
    {
        yield return new WaitForSeconds(0);
        maxPuntos = dbManager.GetMaxScore();
        topPointsText.text = "TOP-" + maxPuntos.ToString("D6"); // Actualiza el texto con la puntuaci�n m�xima
    }

    public void NewGame()
    {
        puntos = 0;
    }

    public void GameOver()
    {
        if (puntos > maxPuntos)
        {
            maxPuntos = puntos;
            dbManager.InsertScore(maxPuntos);
            topPointsText.text = "TOP-" + maxPuntos.ToString("D6"); // Actualizaci�n el texto con la nueva puntuaci�n m�xima
        }
    }

    public void SumarPuntos(int cantidad)
    {
        puntos += cantidad;
    }
}
