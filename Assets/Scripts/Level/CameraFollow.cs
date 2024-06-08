using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    //Referencias y par�metros para el seguimiento de la c�mara
    public Transform target;
    public float followAhead = 2.5f;
    public float minPosX;
    public float maxPosX;

    //L�mites del movimiento
    public Transform limitLeft;
    public Transform limitRight;

    //Colliders para detectar los l�mites
    public Transform colLeft;
    public Transform colRight;


    public bool canMove;
    float camWidth;
    public float lastPos;

    //Punto m�s bajo de la c�mara
    public float lowestPoint;

    // Start is called before the first frame update
    void Start()
    {
        camWidth = Camera.main.orthographicSize * Camera.main.aspect;
        //L�mites de la posici�n x de la c�mara
        minPosX = limitLeft.position.x + camWidth;
        maxPosX = limitRight.position.x - camWidth;
        //�ltima posici�n conocida
        lastPos = minPosX;
    }

    // Actualizaci�n de la c�mara cada fotograma
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
    //M�todo para seguir al objetivo
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
    //M�todo para posicionar la c�mara en el eje y
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
    //M�todo para obtener la posici�n y ver los l�mites
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
    //M�todo para actualizar la posici�n del eje x
    public void UpdateMaxPos(float newMaxLimit)
    {
        maxPosX = newMaxLimit - camWidth;
    }
}
