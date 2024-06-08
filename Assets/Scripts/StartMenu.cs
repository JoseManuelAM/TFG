using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using MySql.Data.MySqlClient;

public class StartMenu : MonoBehaviour
{
    //Referencias a los elementos presentes en el menú inicial
    public TextMeshProUGUI topPoints;
    public Vector2 marioStartPos;
    public GameObject buttonNewGame;
    public GameObject buttonContinue;
    public GameObject buttonOptions;
    public GameObject buttonExit;

    private ScoreManager scoreManager;
    // Start is called before the first frame update
    void Start()
    {
        scoreManager = FindObjectOfType<ScoreManager>();
        if (scoreManager != null)
        {
            // Usar la puntuación máxima del ScoreManager para actualizar la interfaz
            topPoints.text = "TOP-" + scoreManager.maxPuntos.ToString("D6");
        }
        else
        {
            Debug.LogError("ScoreManager no encontrado en la escena.");
        }
        Mario.Instance.Respawn(marioStartPos);

        //Por defecto, el cursor se posiciona sobre "Nueva Partida"
        EventSystem.current.SetSelectedGameObject(buttonNewGame);

        //Comprobar si hay alguna partida guardada, para poder interactuar con el botón "Continuar"
        int savedWorld = PlayerPrefs.GetInt("World", 1);
        int savedLevel = PlayerPrefs.GetInt("Level", 1);
        if(savedWorld == 1 && savedLevel == 1)
        {
            buttonContinue.GetComponent<Button>().interactable = false;
            buttonContinue.GetComponentInChildren<TextMeshProUGUI>().color = new Color(1, 1, 1, 0.5f);
        }
    }
    //Mëtodos que se llaman al hacer click en su botón correspondiente
    public void ClickNewGame()
    {
        GameManager.Instance.StartGame();
    }
    public void ClickContinue()
    {
        GameManager.Instance.ContinueGame();
    }
    public void ClickOptions()
    {
        GameManager.Instance.Options();
    }
    public void ClickExit()
    {
        GameManager.Instance.Exit();
    }
    public void ClickOk()
    {
        GameManager.Instance.Ok();
    }
}
