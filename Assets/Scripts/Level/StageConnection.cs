using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ConnectDirection { Up, Down, Left, Right }
public class StageConnection : MonoBehaviour
{
    //Direcci�n de salida de la conexi�n actual
    public ConnectDirection exitDirection;

    //Referencia al componente que controla el seguimiento de la c�mara
    public CameraFollow cam;

    //Booleanos para controlar el inicio y la permanencia de la conexi�n
    bool connectionStarted;
    bool stayConnection;

    //Referencia al �rea con el que se conectar�
    public Stage stage;

    //Booleano para indicar si la conexi�n se inicia autom�ticamente sin interacci�n del jugador
    public bool auto;
    private void Update()
    {
        //Detecta si el jugador presiona las teclas para iniciar la conexi�n entre �reas
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

    //Corutina que maneja el proceso de conexi�n entre �reas
    IEnumerator StartConnection()
    {
        //Reproduce el sonido de entrar a la tuber�a y actualiza la zona del nivel
        AudioManager.Instance.PlayPipe();
        connectionStarted = true;
        LevelManager.Instance.levelPaused = true;
        cam.canMove = false;
        //Movimiento autom�tico hasta la salida, y espera hasta que el personaje complete dicho movimiento
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
        //Si el jugador entra en el trigger, inicia la conexi�n
        if(collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if(!connectionStarted && auto)
            {
                StartCoroutine(StartConnection());
            }
            stayConnection = true;
        }
    }

    //M�todo que se activa cuando el jugador se mueve fuera del �rea donde se realiza la conexi�n
    private void OnTriggerExit2D(Collider2D collision)
    {
        stayConnection = false;
    }
}
