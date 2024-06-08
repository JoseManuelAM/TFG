using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bowser : Enemy
{
    public int health = 5;
    public bool isDead;


    public float speed = 2f;
    public float minJumpTime = 1f;
    public float maxJumpTime = 5f;
    public float jumpForce = 8f;
    public float minDistanceToMove = 10f;

    //Variables que controlan el fuego que dispara Bowser
    bool canShot;
    public GameObject firePrefab;
    public Transform shootPos;
    public float minShotTime = 1f;
    public float maxShotTime = 5f;
    float shotTimer;
    public float minDistanceToShot = 50f;

    float jumpTimer;
    float direction = -1;
    bool canMove;

    public bool collapseBridge;
    protected override void Start()
    {
        base.Start();
        //Establecer temporizadores aleatorios en un rango para los saltos y disparos
        jumpTimer = Random.Range(minJumpTime, maxJumpTime);
        shotTimer = Random.Range(minShotTime, maxShotTime);
        canMove = false;
        canShot = false;
    }
    protected override void Update()
    {
        //Mientras el puente no haya sido destruido, Bowser puede moverse y disparar
        if(!collapseBridge)
        {
            //Si el jugador se acerca, activa el movimiento de Bowser
            if(!canMove && Mathf.Abs(Mario.Instance.transform.position.x - transform.position.x) <= minDistanceToMove)
            {
                canMove = true;
            }
            if(canMove)
            {
                //Cambia la dirección de Bowser según la posicion del jugador
                if(transform.position.x >= (Mario.Instance.transform.position.x + 2f) && direction == 1)
                {
                    direction = -1;
                    transform.localScale = Vector3.one;
                }
                else if(transform.position.x <= (Mario.Instance.transform.position.x - 2f) && direction == -1)
                {
                    direction = 1;
                    transform.localScale = new Vector3(-1, 1, 1);
                }

                rb2D.velocity = new Vector2(direction * speed, rb2D.velocity.y);

                jumpTimer -= Time.deltaTime;
                if(jumpTimer <=0)
                {
                    Jump();
                }
            }
            //Activa el disparo si el jugador se acerca
            if(!canShot &&
                Mathf.Abs(Mario.Instance.transform.position.x - transform.position.x) <= minDistanceToShot)
            {
                canShot = true;
            }
            if(canShot)
            {
                shotTimer -= Time.deltaTime;
                if(shotTimer <= 0)
                {
                    Shoot();
                }
            }

        }
    }
    //Método de salto
    void Jump()
    {
        Vector2 force = new Vector2(0, jumpForce);
        rb2D.AddForce(force, ForceMode2D.Impulse);
        jumpTimer = Random.Range(minJumpTime, maxJumpTime);
    }
    //Método de disparo
    void Shoot()
    {
        //Crea el objeto y establece su dirección
        GameObject fire = Instantiate(firePrefab, shootPos.position, Quaternion.identity);
        fire.GetComponent<BowserFire>().direction = direction;
        shotTimer = Random.Range(minShotTime, maxShotTime);
    }
    //Mëtodo para que Bowser caiga del puente
    public void FallBridge()
    {
        AudioManager.Instance.PlayBowserFall();
        Dead();
    }
    //Sobreescritura del método al ser pisado, hace que dañe al jugador
    public override void Stomped(Transform player)
    {
        player.GetComponent<Mario>().Hit();
    }
    //Métodos de la clase Enemy que no son necesarios
    public override void HitRollingShell()
    {
        
    }
    public override void HitBelowBlock()
    {
        
    }
    //Mëtodo para cuando el jugador dispara proyectiles a Bowser
    public override void HitFireball()
    {
        
        rb2D.velocity = Vector2.zero;
        health--;
        //Si su salud llega a 0, muere
        if(health <= 0)
        {
            AudioManager.Instance.PlayBowserFall();
            FlipDie();
            isDead = true;
        }
       
    }
    public override void HitStarman()
    {
        
    }
}

