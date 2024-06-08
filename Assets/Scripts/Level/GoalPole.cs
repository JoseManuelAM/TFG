using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalPole : MonoBehaviour
{
    //Referencia a la bandera y su punto más bajo
    public Transform flag;
    public Transform bottom;
    //Velocidad de descenso
    public float flagVelocity = 5f;

    //Puntos que se mostrarán al tocar la bandera
    public GameObject floatPointsPrefab;
    //Booleano para indicar cuándo bajar la bandera
    bool downFlag;
    //Referencia al componente "Mover" del jugador
    Mover mover;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Mario mario = collision.GetComponent<Mario>();
        if(mario != null)
        {
            //Si el jugador entra al trigger, bajar la bandera
            downFlag = true;
            mario.Goal();
            mover = collision.GetComponent<Mover>();
            //Calcular la altura a la que el jugador tocó la bandera para asignar los puntos
            Vector2 contactPoint = collision.ClosestPoint(transform.position);
            CalculateHeight(contactPoint.y);
        }
    }

    private void FixedUpdate()
    {
        if(downFlag)
        {
            //Bajar la bandera hasta que baje del todo
            if(flag.position.y > bottom.position.y)
            {
                flag.position = new Vector2(flag.position.x, flag.position.y - (flagVelocity * Time.fixedDeltaTime));
            }
            //Indicar que ha bajado completamente
            else
            {
                mover.isFlagDown = true;
            }
        }
    }
    //Método que calcula los puntos obtenidos en base a la altura a la que el jugador tocó la bandera
    void CalculateHeight(float marioPosition)
    {
        //Diferentes alturas con sus puntos
        float size = GetComponent<BoxCollider2D>().bounds.size.y;

        float minPosition1 = transform.position.y + (size - size / 5f);//5000

        float minPosition2 = transform.position.y + (size - 2*size / 5f);//2000
       
        float minPosition3 = transform.position.y + (size - 3 * size / 5f);//800
        
        float minPosition4 = transform.position.y + (size - 4 * size / 5f);//400
        
        int numPoints = 0;
        //Asignar los puntos según la posición del jugador
        if(marioPosition >= minPosition1)
        {
            numPoints = 5000;
        }
        else if(marioPosition >= minPosition2)
        {
            numPoints = 2000;
        }
        else if(marioPosition >= minPosition3)
        {
            numPoints = 800;
        }
        else if(marioPosition >= minPosition4)
        {
            numPoints = 400;
        }
        else
        {
            numPoints = 100;
        }
        //Sumar puntos al marcador
        ScoreManager.Instance.SumarPuntos(numPoints);

        //Crear los puntos que aparecerán en la pantalla
        Vector2 positionFloatPoints = new Vector2(transform.position.x + 0.65f, bottom.position.y);
        GameObject newFloatPoints = Instantiate(floatPointsPrefab, positionFloatPoints, Quaternion.identity);
        FloatPoints floatPoints = newFloatPoints.GetComponent<FloatPoints>();
        floatPoints.numPoints = numPoints;
        floatPoints.speed = flagVelocity;
        floatPoints.distance = flag.position.y - bottom.position.y;
        floatPoints.destroy = false;
    }
}
