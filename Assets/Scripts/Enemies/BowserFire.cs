using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowserFire : MonoBehaviour
{
    public float direction;
    public float speed = 10f;

    //Componentes del movimiento y sprite del fuego
    Rigidbody2D rb2D;
    SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        //Ajustar la velocidad y dirección del fuego
        speed *= direction;
        transform.localScale = new Vector3(-direction, 1, 1);
        rb2D.velocity = new Vector2(speed, 0);
        StartCoroutine(Animation());
    }
    //Corutina para la animación del fuego
    IEnumerator Animation()
    {
        //Bucle que anima el fuego mientras exista el objeto en la escena
        while(true)
        {
            spriteRenderer.flipY = !spriteRenderer.flipY;
            yield return new WaitForSeconds(0.1f);
        }
    }
}
