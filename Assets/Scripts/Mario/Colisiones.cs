using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Colisiones : MonoBehaviour
{
    //Variables para ver si el jugador está en el suelo y detectar colisiones
    public bool isGrounded;
    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask groundLayer;

    //Referencias a componentes necesarios
    BoxCollider2D col2D;
    Mario mario;
    Mover mover;
    public LayerMask sideColisions;

    private void Awake()
    {
        col2D = GetComponent<BoxCollider2D>();
        mario = GetComponent<Mario>();
        mover = GetComponent<Mover>();
    }

    //Método que comprueba si el jugador está en el suelo
    public bool Grounded()
    {
        //Comprobar si el jugador está en el suelo mediante dos rayos lanzados hacia abajo desde sus pies
        Vector2 footLeft = new Vector2(col2D.bounds.center.x - col2D.bounds.extents.x, col2D.bounds.center.y);
        Vector2 footRight = new Vector2(col2D.bounds.center.x + col2D.bounds.extents.x, col2D.bounds.center.y);

        //Visualización de los rayos con objetivo de testeo
        Debug.DrawRay(footLeft, Vector2.down * col2D.bounds.extents.y * 1.5f, Color.magenta);
        Debug.DrawRay(footRight, Vector2.down * col2D.bounds.extents.y * 1.5f, Color.magenta);

        //Comprobación de colisión con el suelo
        if (Physics2D.Raycast(footLeft, Vector2.down, col2D.bounds.extents.y * 1.25f, groundLayer))
        {
            isGrounded = true;
        }
        else if (Physics2D.Raycast(footRight, Vector2.down, col2D.bounds.extents.y * 1.25f, groundLayer))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }

        return isGrounded;
    }
    private void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }
    //Método para comprobar colisiones de los lados
    public bool CheckCollsion(int direction)
    {
        //Tamaño y posición
        Vector2 size = new Vector2(0.1f, col2D.bounds.size.y * 0.8f);
        return Physics2D.OverlapBox(col2D.bounds.center + Vector3.right * direction * col2D.bounds.extents.x,
            size, 0, sideColisions);
    }
    //Mëtodo utilizado para testeo en la escena
    private void OnDrawGizmos()
    {
        if(col2D != null)
        {
            //Cubo que representa la zona de colisión
            Vector2 size = new Vector2(0.1f, col2D.bounds.size.y * 0.8f);
            Gizmos.DrawWireCube(col2D.bounds.center + Vector3.right * transform.localScale.x * col2D.bounds.extents.x,
                size);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Comprobar la colisión con enemigos y lava
        if(collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            if(mario.isInvincible)
            {
                Enemy enemy = collision.gameObject.GetComponent<Enemy>();
                enemy.HitStarman();
            }
            else
            {
                mario.Hit();
            }  
        }
        if(collision.gameObject.layer == LayerMask.NameToLayer("Lava"))
        {
            if(!mario.isInvincible)
            {
                mario.Hit();
            }
        }
        if(collision.gameObject.layer == LayerMask.NameToLayer("DamagePlayer"))
        {
            if(!mario.isInvincible)
            {
                mario.Hit();
            }
        }
    }
    //Método para cambiar la capa del jugador al morir
    public void Dead()
    {
        gameObject.layer = LayerMask.NameToLayer("PlayerDead");
        foreach(Transform t in transform)
        {
            t.gameObject.layer = LayerMask.NameToLayer("PlayerDead");
        }
    }
    //Método que cambia de nuevo la capa del jugador a su estado normal al reaparecer
    public void Respawn()
    {
        gameObject.layer = LayerMask.NameToLayer("Player");
        foreach (Transform t in transform)
        {
            t.gameObject.layer = LayerMask.NameToLayer("Player");
        }
    }
    //Método que activa/desactiva las colisiones cuando el jugador es golpeado
    public void HurtCollision(bool activate)
    {
        if(activate)
        {
            gameObject.layer = LayerMask.NameToLayer("OnlyGround");
            transform.GetChild(0).gameObject.layer = LayerMask.NameToLayer("OnlyGround");
        }
        else
        {
            gameObject.layer = LayerMask.NameToLayer("Player");
            transform.GetChild(0).gameObject.layer = LayerMask.NameToLayer("Player");
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Manejo de colisiones con enemigos y plataformas
        Enemy enemy = collision.GetComponent<Enemy>();
        if (enemy != null)
        {
            if(mario.isInvincible)
            {
                enemy.HitStarman();
            }
            else
            {
                //La planta es el único enemigo que el jugador no puede aplastar
                if(collision.CompareTag("Plant"))
                {
                    mario.Hit();
                }
                else
                {
                    //Todos los demás enemigos serán aplastados y al hacerlo el jugador será enviado ligeramente hacia arriba
                    enemy.Stomped(transform);
                    mover.BounceUp();
                }
                
            }            
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        //Si el jugador está en una plataforma, pasa a ser hijo de ella
        if(collision.CompareTag("Platform") && isGrounded)
        {
            transform.parent = collision.transform;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        //Al salir de la plataforma, deja de ser hijo de ella
        if(collision.CompareTag("Platform"))
        {
            transform.parent = null;
            DontDestroyOnLoad(gameObject);
        }
    }
    //Método para romper bloques
    public void StompBlock()
    {
        Collider2D collider2D = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        if(collider2D.gameObject.CompareTag("Block"))
        {
            collider2D.gameObject.GetComponent<Block>().BreakFromTop();
        }
    }
}
