using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    //Tiempo al inicio del nivel
    public int time;
    //Temporizador del nivel
    public float timer;

    //Referencia al jugador
    Mario mario;

    //Booleanos para indicar si el nivel ha acabado o está pausado
    public bool levelFinished;
    public bool levelPaused;

    //Array con los puntos de control del nivel
    public CheckPoint[] checkPoints;
    //Booleano para indicar si el nivel ha comenzado
    public bool hasLevelStart;

    //Referencia al componente que hace que la cámara siga al jugador
    public CameraFollow cameraFollow;

    //Booleano para saber si se están contando los puntos al final del nivel
    public bool countPoints;

    //Música de fondo del nivel
    public LevelStageMusic musicBackground;

    public static LevelManager Instance;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        //Iniciar el tiempo y actualizar la interfaz
        timer = time;
        GameManager.Instance.hud.UpdateTime(timer);

        //Referencias necesarias
        mario = FindAnyObjectByType<Mario>();
        cameraFollow = Camera.main.GetComponent<CameraFollow>();
        GameManager.Instance.LevelLoaded();
    }

    //Método para empezar el nivel desde un punto de control
    public void StartLevel(int currentPoint)
    {
        if(hasLevelStart && currentPoint == 0)
        {
            levelPaused = true;
            Mario.Instance.mover.AutoWalk();
        }
        if (checkPoints[currentPoint].stage != null)
        {
            checkPoints[currentPoint].stage.EnterStage();
        }
        else
        {
            AudioManager.Instance.PlayLevelStageMusic(musicBackground);
        }
        Camera.main.backgroundColor = checkPoints[currentPoint].backgroundColor;
    }
    //Método para detener el seguimiento de la cámara en el final del nivel
    public void MarioInCastle()
    {
        cameraFollow.canMove = false;
    }
    // Update is called once per frame
    void Update()
    {
        if(!levelFinished && !levelPaused)
        {
            timer -= Time.deltaTime / 0.4f; //1 segundo en el temporizador del juego son 0.4 segundos en la realidad
            //Si quedan menos de 100 segundos en el juego, llamar a la función que acelera la música
            if(timer <= 100)
            {
                AudioManager.Instance.HurryUp();
            }

            //Si el tiempo llega a 0, llamar a la función que controla este caso
            if (timer <= 0)
            {
                GameManager.Instance.OutOfTime();
                timer = 0;
            }
            //Función que actualiza el tiempo del contador
            GameManager.Instance.hud.UpdateTime(timer);
        }  
    }
    //Al finalizar el nivel, indicarlo y transformar los segundos restantes en puntos
    public void LevelFinished()
    {
        levelFinished = true;
        StartCoroutine(SecondsToPoints());
    }
    IEnumerator SecondsToPoints()
    {
        yield return new WaitForSeconds(1f);

        int timeLeft = Mathf.RoundToInt(timer);

        while(timeLeft > 0)
        {
            //Mientras queden segundos en el contador, restarlos e ir sumando puntos de 50 en 50
            timeLeft--;
            GameManager.Instance.hud.UpdateTime(timeLeft);
            ScoreManager.Instance.SumarPuntos(50);
            AudioManager.Instance.PlayCoin();
            yield return new WaitForSeconds(0.05f);
        }

        countPoints = true;
    }
}
