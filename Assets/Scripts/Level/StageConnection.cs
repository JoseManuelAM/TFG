using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ConnectDirection { Up, Down, Left, Right }
public class StageConnection : MonoBehaviour
{
    //Dirección de salida de la conexión actual
    public ConnectDirection exitDirection;

    //Referencia al componente que controla el seguimiento de la cámara
    public CameraFollow cam;

    //Booleanos para controlar el inicio y la permanencia de la conexión
    bool connectionStarted;
    bool stayConnection;

    //Referencia al área con el que se conectará
    public Stage stage;

    //Booleano para indicar si la conexión se inicia automáticamente sin interacción del jugador
    public bool auto;
    private void Update()
    {
        //Detecta si el jugador presiona las teclas para iniciar la conexión entre áreas
        if(Input.GetKey(KeyCode.DownArrow) && exitDirection == ConnectDirection.Down)
        {
            if(stayConnection && !connectionStarted)
            {
                StartCoroutine(StartConnection());
            }
        }
        if(Input.GetKey(KeyCode.RightArrow) && exitDirection == ConnectDirection.Right)
        {
            if(stayConnection && !connectionStarted)
            {
                StartCoroutine(StartConnection());
            }
        }
    }

    //Corutina que maneja el proceso de conexión entre áreas
    IEnumerator StartConnection()
    {
        //Reproduce el sonido de entrar a la tubería y actualiza la zona del nivel
        AudioManager.Instance.PlayPipe();
        connectionStarted = true;
        LevelManager.Instance.levelPaused = true;
        cam.canMove = false;
        //Movimiento automático hasta la salida, y espera hasta que el personaje complete dicho movimiento
        Mario.Instance.mover.AutoMoveConnection(exitDirection);
        while(!Mario.Instance.mover.moveConnectionCompleted)
        {
            yield return null;
        }
        //Carga la nueva zona del nivel
        stage.EnterStage();
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        //Si el jugador entra en el trigger, inicia la conexión
        if(collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if(!connectionStarted && auto)
            {
                StartCoroutine(StartConnection());
            }
            stayConnection = true;
        }
    }

    //Método que se activa cuando el jugador se mueve fuera del área donde se realiza la conexión
    private void OnTriggerExit2D(Collider2D collision)
    {
        stayConnection = false;
    }
}
