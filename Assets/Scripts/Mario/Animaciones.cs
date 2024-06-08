using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animaciones : MonoBehaviour
{
    //Referencia al componente animator
    Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    //M�todo para actualizar el estado si el jugador est� en el suelo
    public void Grounded(bool isGrounded)
    {
        animator.SetBool("Grounded", isGrounded);
    }
    //M�todo para actualizar la velocidad del jugador
    public void Velocity(float velocityX)
    {
        animator.SetFloat("VelocityX", Mathf.Abs(velocityX));
    }
    //M�todo para actualizar si el jugador est� saltando o no
    public void Jumping(bool isJumping)
    {
        animator.SetBool("Jumping", isJumping);
    }
    //M�todo para actualizar si el jugador se est� deslizando o no
    public void Skid(bool isSkidding)
    {
        animator.SetBool("Skid", isSkidding);
    }
    //M�todo para activar la animaci�n de muerte
    public void Dead()
    {
        animator.SetTrigger("Dead");
    }
    //M�todo para cambiar el estado del jugador
    public void NewState(int state)
    {
        animator.SetInteger("State", state);
    }
    //M�todo que activa la animaci�n power-up
    public void PowerUp()
    {
        animator.SetTrigger("PowerUp");
    }
    //M�todo que activa la animaci�n de golpe
    public void Hit()
    {
        animator.SetTrigger("Hit");
    }
    //M�todo que activa la animaci�n de disparo
    public void Shoot()
    {
        animator.SetTrigger("Shoot");
    }
    //M�todo que activa o desactiva el modo invencible
    public void InvincibleMode(bool activate)
    {
        animator.SetBool("Invincible", activate);
    }
    //M�todo que activa la animaci�n al ser golpeado
    public void Hurt(bool activate)
    {
        animator.SetBool("Hurt", activate);
    }
    //M�todo para activar la animaci�n de agachado
    public void Crouch(bool activate)
    {
        animator.SetBool("Crouched", activate);
    }
    //M�todo que activa la animaci�n de escalar
    public void Climb(bool activate)
    {
        animator.SetBool("Climb", activate);
    }
    //M�todo para pausar las animaciones
    public void Pause()
    {
        animator.speed = 0;
    }
    //M�todo para continuar con las animaciones
    public void Continue()
    {
        animator.speed = 1;
    }
    //M�todo para resetear todas las animaciones a su estado inicial
    public void Reset()
    {
        animator.SetBool("Grounded", false);
        animator.SetFloat("VelocityX", 0);
        animator.SetBool("Jumping", false);
        animator.SetBool("Skid", false);
        animator.SetBool("Invincible", false);
        animator.SetBool("Hurt", false);
        animator.SetBool("Crouched", false);
        animator.SetBool("Climb", false);

        animator.ResetTrigger("Dead");
        animator.ResetTrigger("PowerUp");
        animator.ResetTrigger("Hit");
        animator.ResetTrigger("Shoot");

        animator.SetInteger("State", 0);
        animator.Play("States");
    }
}
