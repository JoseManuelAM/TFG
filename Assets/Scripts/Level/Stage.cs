using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage : MonoBehaviour
{
    public Transform enterPoint;
    public ConnectDirection enterDirection;

    //Referencia al componente que permite que la c�mara siga al jugador
    public CameraFollow cam;
    //Booleano para controlar el movimiento de la c�mara
    public bool cameraMove;

    public Color backgroundColor;
    public LevelStageMusic musicBackground;

    //M�todo para iniciar un nivel
    void StartStage()
    {
        //Resetear movimiento del jugador e iniciar el nivel
        Mario.Instance.mover.ResetMove();
        LevelManager.Instance.levelPaused = false;
        //Si la c�mara tiene que moverse, hacemos que siga al jugador
        if(cameraMove)
        {
            cam.StartFollow(Mario.Instance.transform);
        }
    }
    //M�todo al entrar en un nivel
    public void EnterStage()
    {
        AudioManager.Instance.PlayLevelStageMusic(musicBackground);
        Camera.main.backgroundColor = backgroundColor;
        //Posicionar al jugador y a la c�mara en el punto inicial
        Mario.Instance.transform.position = enterPoint.position;
        cam.transform.position = new Vector3(transform.position.x, transform.position.y, cam.transform.position.z);
        cam.lastPos = transform.position.x;
        //Dependiendo de la direcci�n, se inicia el nivel de distinta forma
        switch(enterDirection)
        {
            case ConnectDirection.Down:
                StartCoroutine(StartStageDown());
                break;
            case ConnectDirection.Up:
                StartCoroutine(StartStageUp());
                break;
            case ConnectDirection.Left:
                StartStage();
                break;
            case ConnectDirection.Right:
                StartStage();
                break;
        }
    }
    //Corutina que espera un segundo antes de iniciar el nivel para dar tiempo a que se completen las animaciones que tienen que ocurrir despu�s de que el jugador alcanza la bandera
    IEnumerator StartStageDown()
    {
        yield return new WaitForSeconds(1f);
        StartStage();
    }
    //Corutina que coloca al jugador autom�ticamente al salir de una tuber�a
    IEnumerator StartStageUp()
    {
        //Calcular el tama�o del jugador y ajustar su posici�n
        float sizeMario = Mario.Instance.GetComponent<SpriteRenderer>().bounds.size.y;
        Mario.Instance.transform.position = enterPoint.position + Vector3.down * sizeMario;
        //Iniciar el movimiento autom�tico en la direcci�n de entrada
        Mario.Instance.mover.AutoMoveConnection(enterDirection);
        //Espera hasta que el personaje termine de moverse de manera autom�tica para volver a cederle el control al jugador
        while(!Mario.Instance.mover.moveConnectionCompleted)
        {
            yield return null;
        }
        StartStage();
    }
}
