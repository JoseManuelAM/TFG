using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KoopaWinged : Koopa
{
    bool isFly;
    public GameObject wingPrefab;

    protected override void Start()
    {
        //Al iniciar, el Koopa Alado se encuentra volando
        base.Start();
        isFly = true;
        animator.SetBool("Fly", true);
    }
    //Método que hace que el Koopa pierda las alas
    void LoseWings()
    {
        isFly = false;
        if(rb2D.velocity.x < 0)
        {
            autoMovement.speed *= -1;
        }
        rb2D.velocity = Vector2.zero;
        rb2D.isKinematic = false;
        autoMovement.useWaypoints = false;
        autoMovement.isFall = true;
        animator.SetBool("Fly", false);
        LoseWingsAnimation();
    }
    //Sobreescritura del método al ser pisado
    public override void Stomped(Transform player)
    {
        //Si está volando, pierde las alas y ya no vuela
        if(isFly)
        {
            AudioManager.Instance.PlayStomp();
            LoseWings();
            NoDamageTemp();
        }
        else
        //Si no está volando, se comporta como un Koopa normal
        {
            base.Stomped(player);
        }
    }
    //Sobreescritura de métodos para las diferentes interacciones con el jugador
    public override void HitFireball()
    {
        if(isFly)
        {
            LoseWings();
        }
        base.HitFireball();
    }
    public override void HitRollingShell()
    {
        if (isFly)
        {
            LoseWings();
        }
        base.HitRollingShell();
    }
    public override void HitStarman()
    {
        if(isFly)
        {
            LoseWings();
        }
        base.HitStarman();
    }
    protected override void Update()
    {
        base.Update();
        //Para fines de testing, al pulsar la L simula que el enemigo es pisado
        if(Input.GetKeyDown(KeyCode.L))
        {
            Stomped(Mario.Instance.transform);
        }
    }
    //Método que contiene la animación de las alas cuando el Koopa las pierde
    void LoseWingsAnimation()
    {
        GameObject wing;
        wing = Instantiate(wingPrefab, transform.position, Quaternion.identity);
        wing.GetComponent<Rigidbody2D>().AddForce(new Vector2(3f, 9f), ForceMode2D.Impulse);

        wing = Instantiate(wingPrefab, transform.position, Quaternion.identity);
        wing.transform.localScale = new Vector3(-1f, 1f, 1f);
        wing.GetComponent<Rigidbody2D>().AddForce(new Vector2(-3f, 9f), ForceMode2D.Impulse);

    }
}
