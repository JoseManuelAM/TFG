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
    //Método para actualizar el estado si el jugador está en el suelo
    public void Grounded(bool isGrounded)
    {
        animator.SetBool("Grounded", isGrounded);
    }
    //Método para actualizar la velocidad del jugador
    public void Velocity(float velocityX)
    {
        animator.SetFloat("VelocityX", Mathf.Abs(velocityX));
    }
    //Método para actualizar si el jugador está saltando o no
    public void Jumping(bool isJumping)
    {
        animator.SetBool("Jumping", isJumping);
    }
    //Método para actualizar si el jugador se está deslizando o no
    public void Skid(bool isSkidding)
    {
        animator.SetBool("Skid", isSkidding);
    }
    //Método para activar la animación de muerte
    public void Dead()
    {
        animator.SetTrigger("Dead");
    }
    //Método para cambiar el estado del jugador
    public void NewState(int state)
    {
        animator.SetInteger("State", state);
    }
    //Método que activa la animación power-up
    public void PowerUp()
    {
        animator.SetTrigger("PowerUp");
    }
    //Método que activa la animación de golpe
    public void Hit()
    {
        animator.SetTrigger("Hit");
    }
    //Método que activa la animación de disparo
    public void Shoot()
    {
        animator.SetTrigger("Shoot");
    }
    //Método que activa o desactiva el modo invencible
    public void InvincibleMode(bool activate)
    {
        animator.SetBool("Invincible", activate);
    }
    //Método que activa la animación al ser golpeado
    public void Hurt(bool activate)
    {
        animator.SetBool("Hurt", activate);
    }
    //Método para activar la animación de agachado
    public void Crouch(bool activate)
    {
        animator.SetBool("Crouched", activate);
    }
    //Método que activa la animación de escalar
    public void Climb(bool activate)
    {
        animator.SetBool("Climb", activate);
    }
    //Método para pausar las animaciones
    public void Pause()
    {
        animator.speed = 0;
    }
    //Método para continuar con las animaciones
    public void Continue()
    {
        animator.speed = 1;
    }
    //Método para resetear todas las animaciones a su estado inicial
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
