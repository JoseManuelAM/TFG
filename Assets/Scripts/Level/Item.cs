using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Enumeraci�n con los tipos de objetos existentes
public enum ItemType { MagicMushroom, FireFlower, Coin, Life, Star }

public class Item : MonoBehaviour
{
    //Tipo de objeto
    public ItemType type;
    //Booleano para saber si ya ha sido recogido por el jugador
    bool isCatched;
    //Puntos que se obtienen al recoger el objeto
    public int points;

    //Velocidad inicial del objeto
    public Vector2 startVelocity;
    //Referencia al componente AutoMovement
    AutoMovement autoMovement;

    //Objeto de puntuaci�n que se mostrar� al recoger el objeto
    public GameObject floatPointsPrefab;

    private void Awake()
    {
        autoMovement = GetComponent<AutoMovement>();
    }
    private void Start()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isCatched)
        {
            //Si el jugador toca el objeto, recogerlo
            if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                isCatched = true;
                collision.gameObject.GetComponent<Mario>().CatchItem(type);
                CatchItem();
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isCatched)
        {
            Mario mario = collision.gameObject.GetComponent<Mario>();
            if (mario != null)
            {
                mario.CatchItem(type);
                CatchItem();
            }
        }
    }
    //M�todo para detener el movimiento
    public void WaitMove()
    {
        if(autoMovement != null)
        {
            autoMovement.enabled = false;
        }
    }
    //M�todo para iniciar el movimiento
    public void StartMove()
    {
        if(autoMovement != null)
        {
            autoMovement.enabled = true;
        }
        else
        {
            if(startVelocity != Vector2.zero)
            {
                GetComponent<Rigidbody2D>().velocity = startVelocity;
            }
        }
    }
    //M�todo para cambiar la direcci�n del objeto si se est� moviendo y el jugador golpea el bloque en el que est�
    public void HitBelowBlock()
    {
        if(autoMovement != null && autoMovement.enabled)
        {
            autoMovement.ChangeDirection();
        }
    }
    //M�todo que maneja la l�gica al recoger el objeto
    void CatchItem()
    {
        //Sumar puntos correspondientes al marcador
        ScoreManager.Instance.SumarPuntos(points);

        //Si hay un objeto de puntuaci�n en la pantalla, configurarlo
        if(floatPointsPrefab != null)
        {
            GameObject newFloatPoints = Instantiate(floatPointsPrefab, transform.position, Quaternion.identity);
            FloatPoints floatPoints = newFloatPoints.GetComponent<FloatPoints>();
            floatPoints.numPoints = points;
        }
        
        //Destruir el objeto
        Destroy(gameObject);
    }
}
