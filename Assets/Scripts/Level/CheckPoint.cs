using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    //Identificador del punto de control
    public int id;
    //Punto en el que aparece el jugador
    public Transform startPointPlayer;
    //Referencia al nivel actual
    public Stage stage;
    public Color backgroundColor;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Comprobar que el objeto que entra en el trigger es el jugador
        if(collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            //Actualizar el punto de control actual en el GameManager
            GameManager.Instance.currentPoint = id;
        }
    }
}
