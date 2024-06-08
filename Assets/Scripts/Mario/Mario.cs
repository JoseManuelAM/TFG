using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mario : MonoBehaviour
{
    //Posibles estados del jugador
    enum State { Default=0,Super=1,Fire=2}
    //Estado actual
    State currentState = State.Default;
    //Referencias a componentes y objetos necesarios
    public GameObject stompBox;
    public Mover mover;
    Colisiones colisiones;
    Animaciones animaciones;
    Rigidbody2D rb2D;

    //Objeto de la bola de fuego y posición del disparo
    public GameObject fireBallPrefab;
    public Transform shootPos;

    //Variables que controlan la invencibilidad con el objeto estrella recolectado
    public bool isInvincible;
    public float invincibleTime;
    float invincibleTimer;

    //Variables que controlan el daño del jugador
    public bool isHurt;
    public float hurtTime;
    float hurtTimer;

    //Variable que controla si el jugador está agachado
    public bool isCrouched;
    //booleano para saber si el jugador está muerto o no
    bool isDead;

    //Instancia del jugador
    public static Mario Instance;

    //Método Awake para la inicialización de los componentes
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            mover = GetComponent<Mover>();
            colisiones = GetComponent<Colisiones>();
            animaciones = GetComponent<Animaciones>();
            rb2D = GetComponent<Rigidbody2D>();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    //Método Update para controlar el comportamiento del jugador cada fotograma
    private void Update()
    {
        isCrouched = false;
        if(!isDead)
        {
            if (rb2D.velocity.y < 0)
            {
                stompBox.SetActive(true);
            }
            else
            {
                if(transform.parent == null)
                {
                    stompBox.SetActive(false);
                }
            }
            //Tecla para agacharse
            if(Input.GetKey(KeyCode.DownArrow))
            {
                if(colisiones.Grounded())
                {
                    isCrouched = true;
                }
            }

            //Tecla para disparar
            if(Input.GetKeyDown(KeyCode.Z))
            {
                if(isCrouched && currentState != State.Default)
                {
                    colisiones.StompBlock();
                }
                else
                {
                    Shoot();
                }
                
            }
            //Controlar la invencibilidad
            if(isInvincible)
            {
                invincibleTimer -= Time.deltaTime;
                if(invincibleTimer < 2f)
                {
                    AudioManager.Instance.StopMusicStarman(true);
                }
                if(invincibleTimer <= 0)
                {
                    isInvincible = false;
                    animaciones.InvincibleMode(false);
                }
            }
            //Controlar el tiempo que el jugador no puede volver a ser golpeado tras recibir un golpe
            if(isHurt)
            {
                hurtTimer -= Time.deltaTime;
                if(hurtTimer <= 0)
                {
                    EndHurt();
                }
            }
        }
        animaciones.Crouch(isCrouched);

        //Tecla para recibir un golpe (usado para testing)
        if (Input.GetKeyDown(KeyCode.H))
        {
            Hit();
        }
    }
    //Método que maneja el golpe recibido
    public void Hit()
    {
        if(!isHurt)
        {
            //Si el jugador no posee power-ups, muere directamente
            if (currentState == State.Default)
            {
                Dead(true);
            }
            else
            {
                //SFX que se emitirá al ser golpeado
                AudioManager.Instance.PlayPowerDown();
                Time.timeScale = 0;
                animaciones.Hit();
                StartHurt();
            }
        }   
    }
    //Inicio del golpe
    void StartHurt()
    {
        isHurt = true;
        animaciones.Hurt(true);
        hurtTimer = hurtTime;
        colisiones.HurtCollision(true);
    }
    //Fin del golpe
    void EndHurt()
    {
        isHurt = false;
        animaciones.Hurt(false);
        colisiones.HurtCollision(false);
    }
    //Método que maneja la muerte del jugador
    public void Dead(bool bounce)
    {
        if(!isDead)
        {
            //SFX que se emite al morir
            AudioManager.Instance.PlayDie();
            isDead = true;
            colisiones.Dead();
            mover.Dead(bounce);
            animaciones.Dead();
            isInvincible = false;
            GameManager.Instance.LoseLife();
        }      
    }
    //Método para hacer reaparecer al jugador
    public void Respawn(Vector2 pos)
    {
        if(isDead)
        {
            animaciones.Reset();
            currentState = State.Default;
        }
        isDead = false;
        colisiones.Respawn();
        mover.Respawn();
        
        transform.position = pos;
    }
    //Método que cambia el estado del jugador
    void ChangeState(int newState)
    {
        currentState = (State)newState;
        animaciones.NewState(newState);
        Time.timeScale = 1;
    }
    //Método que maneja la obtención de objetos
    public void CatchItem(ItemType type)
    {
        //Casos de los distintos objetos del juego
        switch(type)
        {
            case ItemType.MagicMushroom:
                AudioManager.Instance.PlayPowerUp();
                //MagicMushroom
                if(currentState == State.Default)
                {
                    animaciones.PowerUp();
                    Time.timeScale = 0;
                    rb2D.velocity = Vector2.zero;
                }
                break;
            case ItemType.FireFlower:
                AudioManager.Instance.PlayPowerUp();
                if (currentState != State.Fire)
                {
                    animaciones.PowerUp();
                    Time.timeScale = 0;
                    rb2D.velocity = Vector2.zero;
                }
                break;
            case ItemType.Coin:
                AudioManager.Instance.PlayCoin();
                GameManager.Instance.AddCoins();
                break;
            case ItemType.Life:
                GameManager.Instance.NewLife();
                break;
            case ItemType.Star:
                AudioManager.Instance.PlayPowerUp();
                isInvincible = true;
                animaciones.InvincibleMode(true);
                invincibleTimer = invincibleTime;
                EndHurt();
                AudioManager.Instance.MusicStarman();
                break;
        }
    }
    //Método para disparar bolas de fuego
    void Shoot()
    {
        //Si el jugador no está agachado y tiene el estado fuego, puede disparar
        if(currentState == State.Fire && !isCrouched)
        {
            AudioManager.Instance.PlayShoot();
            GameObject newFireBall = Instantiate(fireBallPrefab, shootPos.position, Quaternion.identity);
            newFireBall.GetComponent<Fireball>().direction = transform.localScale.x;
            animaciones.Shoot();
        }
    }
    //Método para comprobar si el jugador está grande
    public bool IsBig()
    {
        return currentState != State.Default;
    }
    //Método para controlar la finalización del nivel al tocar la bandera
    public void Goal()
    {
        AudioManager.Instance.StopMusicStarman(false);
        invincibleTimer = 0;
        AudioManager.Instance.PlayFlagPole();
        mover.DownFlagPole();
        LevelManager.Instance.LevelFinished();
    }
}
