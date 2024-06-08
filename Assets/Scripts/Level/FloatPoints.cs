using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FloatPoints : MonoBehaviour
{
    //N�mero de puntos que se mostrar�n
    public int numPoints = 0;
    //Distancia que los puntos flotar�n hacia arriba
    public float distance = 2f;
    //Velocidad a la que flotar�n
    public float speed = 2f;
    //Destruir el objeto despu�s de flotar
    public bool destroy = true;

    //Posici�n a la que los puntos llegar�n
    float targetPos;

    //Array de posibles puntos con sus sprites
    public Points[] points;

    //Referencia al componente SpriteRenderer
    SpriteRenderer spriteRenderer;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    // Start is called before the first frame update
    void Start()
    {
        //Mostrar puntos y calcular su posici�n objetivo
        Show(numPoints);
        targetPos = transform.position.y + distance;
    }
    private void Update()
    {
        //Mover los puntos hacia arriba
        if(transform.position.y < targetPos)
        {
            transform.position = new Vector2(transform.position.x,
                transform.position.y + (speed * Time.deltaTime));
        }
        //Al alcanzar su posici�n objetivo, destruir el objeto
        else if(destroy)
        {
            Destroy(gameObject);
        }
    }
    //M�todo para mostrar el sprite acorde a los puntos obtenidos
    void Show(int points)
    {
        //Iterar el array de puntos para encontrar su sprite
        for(int i = 0; i < this.points.Length; i++)
        {
            if (this.points[i].numPoints == points)
            {
                spriteRenderer.sprite = this.points[i].sprite;
                break;
            }
        }
    }

}
[System.Serializable]
//Clase para asociar el n�mero de puntos con su sprite
public class Points
{
    public int numPoints;
    public Sprite sprite;
}
