using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage : MonoBehaviour
{
    public Transform enterPoint;
    public ConnectDirection enterDirection;

    //Referencia al componente que permite que la cámara siga al jugador
    public CameraFollow cam;
    //Booleano para controlar el movimiento de la cámara
    public bool cameraMove;

    public Color backgroundColor;
    public LevelStageMusic musicBackground;

    //Método para iniciar un nivel
    void StartStage()
    {
        //Resetear movimiento del jugador e iniciar el nivel
        Mario.Instance.mover.ResetMove();
        LevelManager.Instance.levelPaused = false;
        //Si la cámara tiene que moverse, hacemos que siga al jugador
        if(cameraMove)
        {
            cam.StartFollow(Mario.Instance.transform);
        }
    }
    //Método al entrar en un nivel
    public void EnterStage()
    {
        AudioManager.Instance.PlayLevelStageMusic(musicBackground);
        Camera.main.backgroundColor = backgroundColor;
        //Posicionar al jugador y a la cámara en el punto inicial
        Mario.Instance.transform.position = enterPoint.position;
        cam.transform.position = new Vector3(transform.position.x, transform.position.y, cam.transform.position.z);
        cam.lastPos = transform.position.x;
        //Dependiendo de la dirección, se inicia el nivel de distinta forma
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
    //Corutina que espera un segundo antes de iniciar el nivel para dar tiempo a que se completen las animaciones que tienen que ocurrir después de que el jugador alcanza la bandera
    IEnumerator StartStageDown()
    {
        yield return new WaitForSeconds(1f);
        StartStage();
    }
    //Corutina que coloca al jugador automáticamente al salir de una tubería
    IEnumerator StartStageUp()
    {
        //Calcular el tamaño del jugador y ajustar su posición
        float sizeMario = Mario.Instance.GetComponent<SpriteRenderer>().bounds.size.y;
        Mario.Instance.transform.position = enterPoint.position + Vector3.down * sizeMario;
        //Iniciar el movimiento automático en la dirección de entrada
        Mario.Instance.mover.AutoMoveConnection(enterDirection);
        //Espera hasta que el personaje termine de moverse de manera automática para volver a cederle el control al jugador
        while(!Mario.Instance.mover.moveConnectionCompleted)
        {
            yield return null;
        }
        StartStage();
    }
}
