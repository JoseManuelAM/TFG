using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public World[] worlds;

    public int currentWorld;
    public int currentLevel;

    public HUD hud;
    int coins;
    public Mario mario;

    public int lives;

    bool isRespawning;
    public bool isGameOver;

    public int currentPoint;
    public static GameManager Instance;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        HideTimer();
        isGameOver = true;
        currentWorld = PlayerPrefs.GetInt("World", 1);
        currentLevel = PlayerPrefs.GetInt("Level", 1);
    }
    //M�todo para a�adir monedas y dar una vida extra al jugador si supera las 99 monedas recogidas
    public void AddCoins()
    {
        coins++;
        if(coins > 99)
        {
            coins = 0;
            //lives++;
            NewLife();
        }
        hud.UpdateCoins(coins);
    }
    //M�todo que mata al jugador si se acaba el tiempo
    public void OutOfTime()
    {
        Mario.Instance.Dead(true);
    }
    //M�todo que mata al jugador en algunas zonas en las que debe morir
    public void KillZone()
    {
        if(!isRespawning)
        {
            Mario.Instance.Dead(false);
        } 
    }
    //M�todo para restar una vida al jugador
    public void LoseLife()
    {
        if(!isRespawning)
        {
            lives--;
            isRespawning = true;
            if(lives > 0)
            {
                StartCoroutine(Respawn());
            }
            else
            {
                GameOver();
            }
        }
        
    }
    //M�todo para sumar una vida al jugador
    public void NewLife()
    {
        lives++;
        AudioManager.Instance.PlayOneUp();
    }
    //M�todo para iniciar el juego desde el nivel 1
    public void StartGame()
    {
        currentLevel = 1;
        currentWorld = 1;
        LoadLevel();
    }
    //M�todo para continuar el juego por el nivel que el jugador lo dej�
    public void ContinueGame()
    {
        LoadLevel();
    }
    //M�todo que lleva a la pantalla de opciones
    public void Options()
    {
        SceneManager.LoadScene("Options");
    }
    //M�todo para salir del juego
    public void Exit()
    {
        Application.Quit();
    }
    //M�todo para volver al men�, desde "Opciones"
    public void Ok()
    {
        SceneManager.LoadScene("StartMenu");
    }
    //M�todo para iniciar una nueva partida
    void NewGame()
    {
        lives = 3;
        coins = 0;
        isGameOver = false;
        ScoreManager.Instance.NewGame();
        currentPoint = 0;
    }
    //M�todo que maneja cuando el jugador se queda sin vidas
    void GameOver()
    {
        Debug.Log("Fin del juego");
        ScoreManager.Instance.GameOver();
        isGameOver = true;
        PlayerPrefs.SetInt("World", currentWorld);
        PlayerPrefs.SetInt("Level", currentLevel);

        StartCoroutine(Respawn());
    }
    //Corutina que maneja la reaparici�n del jugador
    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(3f);
        isRespawning = false;
        SceneManager.LoadScene("Transition");
        if (isGameOver)
        {
            AudioManager.Instance.PlayGameOver();
            yield return new WaitForSeconds(5f);
            SceneManager.LoadScene("StartMenu");
        }
        else
        {
            yield return new WaitForSeconds(5f);
            LoadLevel();
        }
    }
    //Funci�n para cuando un nivel se ha cargado, actualizar la interfaz
    public void LevelLoaded()
    {
        hud.UpdateWorld(currentWorld, currentLevel);
        ShowTimer();
        if(isGameOver)
        {
            NewGame();
        }
        hud.UpdateCoins(coins);
        Vector3 position = LevelManager.Instance.checkPoints[currentPoint].startPointPlayer.position;
        Mario.Instance.Respawn(position);
        LevelManager.Instance.StartLevel(currentPoint);
        LevelManager.Instance.cameraFollow.StartFollow(Mario.Instance.transform);
    }
    //M�todo para ir a una escena espec�fica
    public void GoToLevel(string sceneName)
    {
        currentPoint = 0;
        SceneManager.LoadScene(sceneName);
    }
    //M�todo para ir a un nivel espec�fico en base al mundo y nivel
    public void GoToLevel(int world, int level)
    {
        currentPoint = 0;
        currentLevel = level;
        currentWorld = world;
        hud.UpdateWorld(world, level);
        LoadTransition();
    }
    //M�todo que llama a la escena de transici�n entre niveles para continuar con el siguiente nivel
    void LoadTransition()
    {
        SceneManager.LoadScene("Transition");
        Invoke("LoadLevel", 5f);
    }
    //M�todo que carga el nivel actual por los �ndices de mundo y nivel
    void LoadLevel()
    {
        int worldIndex = currentWorld - 1;
        int levelIndex = currentLevel - 1;

        string sceneName = worlds[worldIndex].levels[levelIndex].sceneName;
        SceneManager.LoadScene(sceneName);
    }
    //M�todo que avanza al siguiente nivel, actualizando el �ndice del nuevo nivel
    public void NextLevel()
    {
        int worldIndex = currentWorld - 1;
        int levelIndex = currentLevel - 1;

        levelIndex++;
        if(levelIndex >= worlds[worldIndex].levels.Length)
        {
            worldIndex++;
            if(worldIndex >= worlds.Length)
            {
                Debug.Log("Juego completado");
                return;
            }
            else
            {
                levelIndex = 0;
            }
        }

        currentWorld = worldIndex + 1;
        currentLevel = levelIndex + 1;
        currentPoint = 0;
        hud.UpdateWorld(currentWorld, currentLevel);
        LoadTransition();
    }
    //M�todo para esconder el temporizador en la interfaz
    public void HideTimer()
    {
        hud.time.enabled = false;
    }
    //M�todo para mostrar el temporizador en la interfaz
    public void ShowTimer()
    {
        hud.time.enabled = true;
    }
}
//Estructuras de mundo y nivel
[System.Serializable]
public struct World
{
    public int id;
    public Level[] levels;
}
[System.Serializable]
public struct Level
{
    public int id;
    public string sceneName;
}
