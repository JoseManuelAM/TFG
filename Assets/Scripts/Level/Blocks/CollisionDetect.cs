using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetect : MonoBehaviour
{
    //Referencia al componente bloque del objeto padre
    Block block;
    private void Awake()
    {
        block = GetComponentInParent<Block>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Verifica que el objeto con el que colisionó el bloque es la cabeza del jugador
        if (collision.CompareTag("HeadMario"))
        {
            collision.transform.parent.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            //Maneja si el bloque ha sido golpeado por el jugador en estado grande o pequeño
            if(collision.GetComponentInParent<Mario>().IsBig())
            {
                block.HeadCollision(true);
            }
            else
            {
                block.HeadCollision(false);
            }
            
        }
    }
}
