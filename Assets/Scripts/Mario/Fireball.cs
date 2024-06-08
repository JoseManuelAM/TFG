using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    //Dirección y velocidad de los proyectiles, además de su fuerza de rebote
    public float direction;
    public float speed;
    public float bounceForce;

    //Objeto de explosión para cuando el proyectil colisiona
    public GameObject explosionPrefab;
    Rigidbody2D rb2D;

    //Booleano para saber si ha colisionado o no
    bool colision;
    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        //Inicializar la velocidad en la dirección en la que se disparó el proyectil
        speed *= direction;
        rb2D.velocity = new Vector2(speed, 0);
    }

    // Update is called once per frame
    void Update()
    {
        //Actualizar la velocidad horizontal del proyectil
        rb2D.velocity = new Vector2(speed, rb2D.velocity.y);
    }
    //Método que maneja la colisión del proyectil con el entorno
    private void OnCollisionEnter2D(Collision2D collision)
    {
        colision = true;
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();
        if(enemy != null)
        {
            enemy.HitFireball();
            Explode(collision.GetContact(0).point);
        }
        else
        {
            Vector2 sidePoint = collision.GetContact(0).normal;

            if (Mathf.Abs(sidePoint.x) > 0.01f) //Hay colision lateral
            {
                Explode(collision.GetContact(0).point);
            }
            else if (sidePoint.y > 0) //colisiona por abajo
            {
                rb2D.AddForce(Vector2.up * bounceForce, ForceMode2D.Impulse);
            }
            else if (sidePoint.y < 0) //colisiona por arriba
            {
                rb2D.AddForce(Vector2.down * bounceForce, ForceMode2D.Impulse);
            }
            else
            {
                Explode(collision.GetContact(0).point);
            }
        }  
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if(colision)
        {
            colision = false;
        }
        else
        {
            Explode(collision.GetContact(0).point);
        }
    }
    //Método que crea una exploxión y destruye el proyectil
    void Explode(Vector2 point)
    {
        //SFX de golpe del proyectil
        AudioManager.Instance.PlayBump();
        //Instanciar explosión en el punto de contacto
        Instantiate(explosionPrefab, point, Quaternion.identity);
        //Destruir el objeto
        Destroy(gameObject);
    }
}
