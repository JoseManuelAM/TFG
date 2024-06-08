using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Koopa : Enemy
{
    //Variables para controlar al Koopa cuando el jugador lo pisa y se esconde en su caparazón
    bool isHidden;
    public float maxStoppedTime;
    float stoppedTimer;
   
    public float rollingSpeed;
    public bool isRolling;

    bool isAvoidFall;
    protected override void Start()
    {
        base.Start();
        isAvoidFall = autoMovement.avoidFall;
    }
    protected override void Update()
    {
        base.Update();
        //Reanudar el movimiento del Koopa cuando ha estado oculto unos segundos
        if(isHidden && rb2D.velocity.x == 0f)
        {
            stoppedTimer += Time.deltaTime;
            if(stoppedTimer >= maxStoppedTime)
            {
                ResetMove();
            }
        }
    }
    //Sobreescritura del método al pisar al Koopa
    public override void Stomped(Transform player)
    {
       
        AudioManager.Instance.PlayStomp();
        isRolling = false;
        //Si no está oculto, se oculta y se detiene su movimiento
        if(!isHidden)
        {
            isHidden = true;
            animator.SetBool("Hidden", isHidden);
            autoMovement.PauseMovement();
        }
        else
        {
            //Si está oculto, se comprueba si ya está rodando y se cambia la velocidad
            if(Mathf.Abs(rb2D.velocity.x) > 0f)
            {
                autoMovement.PauseMovement();
            }
            else
            {
                //La dirección en la que se mueva el caparazón depende de la posición del jugador al pisarlo
                if (player.position.x < transform.position.x)
                {
                    autoMovement.speed = rollingSpeed;
                }
                else
                {
                    autoMovement.speed = -rollingSpeed;
                }
                autoMovement.ContinueMovement(new Vector2(autoMovement.speed, 0f));
                isRolling = true;
            }    
        }
        DestroyOutCamera destroyOutCamera = GetComponent<DestroyOutCamera>();
        if(isRolling)
        {
            destroyOutCamera.onlyBack = false;
            autoMovement.avoidFall = false;
        }
        else
        {
            destroyOutCamera.onlyBack = true;
            autoMovement.avoidFall = isAvoidFall;
        }
        NoDamageTemp();
        stoppedTimer = 0;
    }
    //Método para evitar que el Koopa reciba daño mientras está escondido en el caparazón
    protected void NoDamageTemp()
    {
        gameObject.layer = LayerMask.NameToLayer("OnlyGround");
        Invoke("ResetLayer", 0.1f);
    }
    //Método para cuando el Koopa es golpeado por un caparazón
    public override void HitRollingShell()
    {
        if(!isRolling)
        {
            FlipDie();
        }
        else
        {
            autoMovement.ChangeDirection();
        }
    }
    //Método para reestablecer la capa del Koopa a "Enemigo"
    void ResetLayer()
    {
        gameObject.layer = LayerMask.NameToLayer("Enemy");
    }
    //Método para reanudar el movimiento del Koopa cuando deja de estar oculto
    void ResetMove()
    {
        autoMovement.ContinueMovement();
        isHidden = false;
        animator.SetBool("Hidden", isHidden);
        stoppedTimer = 0;
    }
    //Interacciones mientras el Koopa está rodando en su caparazón
    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        if(isRolling)
        {
            if(collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                collision.gameObject.GetComponent<Enemy>().HitRollingShell();
            }
        }
        else if(!isHidden)
        {
            base.OnCollisionEnter2D(collision);
        }
    }
}
