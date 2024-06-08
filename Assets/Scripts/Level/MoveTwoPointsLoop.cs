using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTwoPointsLoop : MonoBehaviour
{
    //Enumeración que define la dirección del movimiento
    public enum Direction
    {
        NONE,
        HORIZONTAL,
        VERTICAL
    }
    public Direction direction; //Dirección actual
    public Transform startPoint; //Punto de inicio
    public Transform endPoint; //Punto de fin
    public float speed; //Velocidad de movimiento

    public bool reverse; //Booleano para realizar el mismo movimiento a la inversa

    Vector3 currentTarget; //Objetivo al que se mueve el objeto
    Vector3 startPos; //Posición de inicio
    Vector3 endPos; //Posición de fin

    // Start is called before the first frame update
    void Start()
    {
        //Inicialización de las posiciones inicio y fin en base a la dirección
        switch(direction)
        {
            case Direction.NONE:
                startPos = startPoint.position;
                endPos = endPoint.position;
                break;
            case Direction.HORIZONTAL:
                startPos = new Vector3(startPoint.position.x, transform.position.y, transform.position.z);
                endPos = new Vector3(endPoint.position.x, transform.position.y, transform.position.z);
                break;
            case Direction.VERTICAL:
                startPos = new Vector3(transform.position.x, startPoint.position.y, transform.position.z);
                endPos = new Vector3(transform.position.x, endPoint.position.y, transform.position.z);
                break;
        }

        currentTarget = endPos;
    }

    // Update is called once per frame
    void Update()
    {
        float fixedSpeed = speed * Time.deltaTime;
        //Mover el objeto hacia el objetivo
        transform.position = Vector3.MoveTowards(transform.position, currentTarget, fixedSpeed);
        if(transform.position == currentTarget)
        {
            //Si es verdadero, cambiar el objetivo
            if(reverse)
            {
                if(currentTarget == startPos)
                {
                    currentTarget = endPos;
                }
                else
                {
                    currentTarget = startPos;
                }
            }
            //En caso contrario, reiniciar su posición al punto inicial
            else
            {
                transform.position = startPos;
            }
            
        }
    }
}
