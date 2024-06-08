using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int points;
    protected Animator animator;
    protected AutoMovement autoMovement;
    protected Rigidbody2D rb2D;

    public GameObject floatPointsPrefab;

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
        autoMovement = GetComponent<AutoMovement>();
        rb2D = GetComponent<Rigidbody2D>();
    }
    protected virtual void Start()
    {

    }
    protected virtual void Update()
    {

    }
    //Método para detectar colisión con otros enemigos y cambiar de dirección
    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == gameObject.layer)
        {
            autoMovement.ChangeDirection();
        }
    }
    //Métodos para diferetes interacciones con el jugador
    public virtual void Stomped(Transform player)
    {

    }
    public virtual void HitFireball()
    {
        FlipDie();
    }
    public virtual void HitStarman()
    {
        FlipDie();
    }
    public virtual void HitBelowBlock()
    {
        FlipDie();
    }
    public virtual void HitRollingShell()
    {
        FlipDie();
    }
    //Método de animación al morir
    protected void FlipDie()
    {
        AudioManager.Instance.PlayFlipDie();
        animator.SetTrigger("Flip");
        rb2D.velocity = Vector2.zero;
        rb2D.AddForce(Vector2.up * 6, ForceMode2D.Impulse);
        if(autoMovement != null)
        {
            autoMovement.enabled = false;
        }
        GetComponent<Collider2D>().enabled = false;
        Dead();
    }
    //Método que maneja la lógica del enemigo al morir, otorgando puntos y mostrándolos en pantalla
    protected void Dead()
    {
        ScoreManager.Instance.SumarPuntos(points);
        GameObject newFloatPoints = Instantiate(floatPointsPrefab, transform.position, Quaternion.identity);
        FloatPoints floatPoints = newFloatPoints.GetComponent<FloatPoints>();
        floatPoints.numPoints = points;
    }
}
