using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    //Referencias y parámetros para el seguimiento de la cámara
    public Transform target;
    public float followAhead = 2.5f;
    public float minPosX;
    public float maxPosX;

    //Límites del movimiento
    public Transform limitLeft;
    public Transform limitRight;

    //Colliders para detectar los límites
    public Transform colLeft;
    public Transform colRight;


    public bool canMove;
    float camWidth;
    public float lastPos;

    //Punto más bajo de la cámara
    public float lowestPoint;

    // Start is called before the first frame update
    void Start()
    {
        camWidth = Camera.main.orthographicSize * Camera.main.aspect;
        //Límites de la posición x de la cámara
        minPosX = limitLeft.position.x + camWidth;
        maxPosX = limitRight.position.x - camWidth;
        //Última posición conocida
        lastPos = minPosX;
    }

    // Actualización de la cámara cada fotograma
    void Update()
    {
        if(target != null && canMove)
        {
            float newPosX = target.position.x + followAhead;
            newPosX = Mathf.Clamp(newPosX, lastPos, maxPosX);

            transform.position = new Vector3(newPosX, transform.position.y, transform.position.z);
            lastPos = newPosX;
        }
        
    }
    //Método para seguir al objetivo
    public void StartFollow(Transform t)
    {
        target = t;
        float newPosX = target.position.x + followAhead;
        newPosX = Mathf.Clamp(newPosX, lastPos, maxPosX);
        transform.position = new Vector3(newPosX, transform.position.y, transform.position.z);
        lastPos = newPosX;
        canMove = true;
        SearchHeightPos();
    }
    //Método para posicionar la cámara en el eje y
    void SearchHeightPos()
    {
        bool foundPos = false;
        float checkPosition = lowestPoint;

        do
        {
            if (target.position.y < checkPosition + Camera.main.orthographicSize
                && target.position.y > checkPosition - Camera.main.orthographicSize)
            {
                transform.position = new Vector3(transform.position.x, checkPosition, transform.position.z);
                foundPos = true;
            }
            else
            {
                checkPosition += Camera.main.orthographicSize * 2;
            }
        } while (!foundPos);
    }
    //Método para obtener la posición y ver los límites
    public float PositionInCamera(float pos, float width, out bool limitRight, out bool limitLeft)
    {
        if(pos+width > maxPosX + camWidth)
        {
            limitLeft = false;
            limitRight = true;
            return (maxPosX + camWidth - width);
        }
        else if(pos-width < lastPos-camWidth)
        {
            limitLeft = true;
            limitRight = false;
            return (lastPos - camWidth + width);
        }
        limitLeft = false;
        limitRight = false;
        return pos;
 }
    //Método para actualizar la posición del eje x
    public void UpdateMaxPos(float newMaxLimit)
    {
        maxPosX = newMaxLimit - camWidth;
    }
}
