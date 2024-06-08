using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
    //Enumeración con las distintas opciones que puede adquirir el movimiento del jugador
    enum Direction { Left = -1,None = 0,Right = 1};
    //Dirección actual del movimiento, inicializada a cero.
    Direction currentDirection = Direction.None;

    //Variables para la velocidad, aceleración, velocidad máxima y fricción del jugador
    public float speed;
    public float acceleration;
    public float maxVelocity;
    public float friction;
    //Velocidad actual inicializada a 0
    float currentVelocity = 0f;

    //Variables para el control del salto
    public float jumpForce;
    public float maxJumpingTime = 1f;
    public bool isJumping;
    float jumpTimer = 0;
    float defaultGravity;

    //Variable que controlará si el jugador se está deslizando o no
    public bool isSkidding;

    //Referencia al componente Rigidbody que tiene el personaje principal en Unity
    public Rigidbody2D rb2D;
    //Referencia al script "Colisiones"
    Colisiones colisiones;

    //Variable que permitirá controlar cuando queremos que el usuario pueda mover o no al personaje
    public bool inputMoveEnabled = true;

    //Referencia al objeto que representa la caja de colisión de la cabeza del jugador
    public GameObject headBox;
    //Referencia al script "Animaciones"
    Animaciones animaciones;

    //Variables para controlar la lógica de la bandera del final del nivel
    bool isClimbingFlagPole = false;
    public float climbPoleSpeed = 5;
    public bool isFlagDown;

    //Variables para controlar el movimiento automático del jugador una vez baja la bandera del final del nivel
    bool isAutoWalk;
    public float autoWalkSpeed = 5f;

    //Referencia al script "Mario"
    Mario mario;

    public bool moveConnectionCompleted = true;

    //Referencia al componente SpriteRenderer que tiene el personaje principal en Unity
    SpriteRenderer spriteRenderer;

    //Método Awake para inicializar las referencias declaradas anteriormente
    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        colisiones = GetComponent<Colisiones>();
        animaciones = GetComponent<Animaciones>();
        mario = GetComponent<Mario>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        defaultGravity = rb2D.gravityScale;
    }

    void Start()
    {
        
    }

    //Método Update que controla tanto el movimiento como las animaciones del jugador
    void Update()
    {
        if(GameManager.Instance.isGameOver)
        {
            animaciones.Grounded(true);
            return;
        }
        bool grounded = colisiones.Grounded();
        animaciones.Grounded(grounded);

        if (grounded)
        {
            animaciones.Jumping(isJumping);
        }

        //Si el nivel ha finalizado, iniciar la corutina de la bandera de final del nivel
        if (LevelManager.Instance.levelFinished)
        {
            if (grounded && isClimbingFlagPole)
            {
                StartCoroutine(JumpOffPole());
            }
        }
        //Si el nivel no ha finalizado, controlar el salto y el movimiento del jugador
        else
        {
            headBox.SetActive(false);
            if (isJumping)
            {
                if (rb2D.velocity.y > 0f)
                {
                    headBox.SetActive(true);
                    if (Input.GetKey(KeyCode.Space))
                    {
                        jumpTimer += Time.deltaTime;
                    }
                    if (Input.GetKeyUp(KeyCode.Space))
                    {
                        if (jumpTimer < maxJumpingTime)
                        {
                            rb2D.gravityScale = defaultGravity * 3f;
                        }
                    }
                }
                else
                {
                    rb2D.gravityScale = defaultGravity;
                    isJumping = false;
                    jumpTimer = 0;
                }
            }

            //Controlar la dirección del movimiento
            currentDirection = Direction.None;
            if (inputMoveEnabled)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    if (grounded)
                    {
                        Jump();
                    }
                }
                if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
                {
                    currentDirection = Direction.Left;
                }
                if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
                {
                    currentDirection = Direction.Right;
                }
            }
            bool limitRight;
            bool limitLeft;
            if (LevelManager.Instance.cameraFollow != null)
            {
                float posX = LevelManager.Instance.cameraFollow.PositionInCamera(transform.position.x, spriteRenderer.bounds.extents.x,
                out limitRight, out limitLeft);
                if (limitRight && (currentDirection == Direction.Right || currentDirection == Direction.None))
                {
                    rb2D.velocity = new Vector2(0, rb2D.velocity.y);
                }
                else if (limitLeft && (currentDirection == Direction.Left || currentDirection == Direction.None))
                {
                    rb2D.velocity = new Vector2(0, rb2D.velocity.y);
                }

                transform.position = new Vector2(posX, transform.position.y);
            }
        }
        
    }
    private void FixedUpdate()
    {
        if (GameManager.Instance.isGameOver)
            return;
            //Movimiento al bajar la bandera
            if (isClimbingFlagPole)
            {
                rb2D.MovePosition(rb2D.position + Vector2.down * climbPoleSpeed * Time.fixedDeltaTime);
            }
            //Movimiento tras bajar la bandera
            else if(isAutoWalk)
            {
                Vector2 velocity = new Vector2(currentVelocity, rb2D.velocity.y);
                rb2D.velocity = velocity;
                animaciones.Velocity(Mathf.Abs(currentVelocity));
            }
        //Controlar movimiento y animaciones mientras el jugador controle a su personaje
        else if(!rb2D.isKinematic && !LevelManager.Instance.levelFinished)
        {
                isSkidding = false;
                currentVelocity = rb2D.velocity.x;
                if (colisiones.CheckCollsion((int)currentDirection))
                {
                    currentVelocity = 0;
                }
                else
                {
                    if (currentDirection > 0)
                    {
                        if (currentVelocity < 0)
                        {
                            currentVelocity += (acceleration + friction) * Time.deltaTime;
                            isSkidding = true;
                        }
                        else if (currentVelocity < maxVelocity)
                        {
                            currentVelocity += acceleration * Time.deltaTime;
                            transform.localScale = new Vector2(1, 1);
                        }
                    }
                    else if (currentDirection < 0)
                    {
                        if (currentVelocity > 0)
                        {
                            currentVelocity -= (acceleration + friction) * Time.deltaTime;
                            isSkidding = true;
                        }
                        else if (currentVelocity > -maxVelocity)
                        {
                            currentVelocity -= acceleration * Time.deltaTime;
                            transform.localScale = new Vector2(-1, 1);
                        }
                    }
                    else
                    {
                        if (currentVelocity > 1f)
                        {
                            currentVelocity -= friction * Time.deltaTime;
                        }
                        else if (currentVelocity < -1f)
                        {
                            currentVelocity += friction * Time.deltaTime;
                        }
                        else
                        {
                            currentVelocity = 0;
                        }
                    }
                }


                if (mario.isCrouched)
                {
                    currentVelocity = 0;
                }
                Vector2 velocity = new Vector2(currentVelocity, rb2D.velocity.y);
                rb2D.velocity = velocity;

                animaciones.Velocity(currentVelocity);
                animaciones.Skid(isSkidding);
        }

        
    }
    //Método de salto del jugador
    void Jump()
    {
        if(!isJumping)
        {
            //Dependiendo de si el jugador está transformado en grande o no, reproducir un SFX de salto u otro
            if(mario.IsBig())
            {
                AudioManager.Instance.PlayBigJump();
            }
            else
            {
                AudioManager.Instance.PlayJump();
            }
            isJumping = true;
            Vector2 fuerza = new Vector2(0, jumpForce);
            rb2D.AddForce(fuerza, ForceMode2D.Impulse);
            animaciones.Jumping(true);
        }
        
    }
    //Método de movimiento
    void MoveRight()
    {
        Vector2 velocity = new Vector2(1f, 0f);
        rb2D.velocity = velocity;
    }
    //Método para controlar la muerte del jugador
    public void Dead(bool bounce)
    {
        //Bloqueo del movimiento para que el jugador no pueda moverse
        inputMoveEnabled = false;
        //Pequeño impulso hacia arriba, después cae hacia abajo
        if(bounce)
        {
            rb2D.velocity = Vector2.zero;
            rb2D.gravityScale = 1;
            rb2D.AddForce(Vector2.up * 5f, ForceMode2D.Impulse);
        }
       
    }
    //Método para hacer reaparecer al jugador
    public void Respawn()
    {
        isAutoWalk = false;
        inputMoveEnabled = true;
        rb2D.velocity = Vector2.zero;
        rb2D.gravityScale = defaultGravity;
        transform.localScale = Vector2.one;
    }
    //Método de impulso hacia arriba
    public void BounceUp()
    {
        rb2D.velocity = Vector2.zero;
        rb2D.AddForce(Vector2.up * 10f, ForceMode2D.Impulse);
    }
    //Método de movimiento automático
    public void AutoWalk()
    {
        rb2D.isKinematic = false;
        inputMoveEnabled = false;
        isAutoWalk = true;
        currentVelocity = autoWalkSpeed;
    }
    //Método para bajar la bandera de final del nivel
    public void DownFlagPole()
    {
        if(!isClimbingFlagPole)
        {
            inputMoveEnabled = false;
            rb2D.isKinematic = true;
            rb2D.velocity = new Vector2(0, 0f);
            isClimbingFlagPole = true;
            isJumping = false;
            animaciones.Jumping(false);
            animaciones.Climb(true);
            transform.position = new Vector2(transform.position.x + 0.1f, transform.position.y);
        }
        
    }
    //Corutina que maneja el comportamiento del jugador al bajar la bandera
    IEnumerator JumpOffPole()
    {
        isAutoWalk = false;
        isClimbingFlagPole = false;
        rb2D.velocity = Vector2.zero;
        animaciones.Pause();
        yield return new WaitForSeconds(0.25f);

        while(!isFlagDown)
        {
            yield return null;
        }

        //Ajustar la posición del jugador para que mire a la izquierda
        transform.position = new Vector2(transform.position.x + 0.5f, transform.position.y);
        GetComponent<SpriteRenderer>().flipX = true;
        yield return new WaitForSeconds(0.25f);

        //Actualizar las animaciones para que, una vez en el suelo, continúe con su animación de andar
        animaciones.Climb(false);
        animaciones.Continue();
        GetComponent<SpriteRenderer>().flipX = false;
        AutoWalk();
        //Música de nivel completado
        AudioManager.Instance.PlayLevelCompleted();
    }
    //Método para el movimiento automático
    public void AutoMoveConnection(ConnectDirection direction)
    {
        isAutoWalk = false;
        moveConnectionCompleted = false;
        inputMoveEnabled = false;
        rb2D.isKinematic = true;
        rb2D.velocity = Vector2.zero;
        spriteRenderer.sortingOrder = -100;

        switch(direction)
        {
            case ConnectDirection.Up:
                StartCoroutine(AutoMoveConnectionUp());
                break;
            case ConnectDirection.Down:
                StartCoroutine(AutoMoveConnectionDown());
                break;
            case ConnectDirection.Left:
                moveConnectionCompleted = true;
                break;
            case ConnectDirection.Right:
                StartCoroutine(AutoMoveConnectionRight());
                break;
        }
    }
    //Método para restablecer el movimiento del jugador
    public void ResetMove()
    {
        rb2D.isKinematic = false;
        inputMoveEnabled = true;
        spriteRenderer.sortingOrder = 20;
    }
    //Corutina para mover al jugador abajo
    IEnumerator AutoMoveConnectionDown()
    {
        float targetDown = transform.position.y - spriteRenderer.bounds.size.y;
        while(transform.position.y > targetDown)
        {
            transform.position += Vector3.down * Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        moveConnectionCompleted = true;
    }
    //Corutina para mover al jugador arriba
    IEnumerator AutoMoveConnectionUp()
    {
        float targetUp = transform.position.y + spriteRenderer.bounds.size.y;
        while (transform.position.y < targetUp)
        {
            transform.position += Vector3.up * Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        moveConnectionCompleted = true;
    }
    //Corutina para mover al jugador a la derecha
    IEnumerator AutoMoveConnectionRight()
    {
        float targetRight = transform.position.x + spriteRenderer.bounds.size.x*2;
        animaciones.Velocity(1);
        while (transform.position.x < targetRight)
        {
            transform.position += Vector3.right * Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        animaciones.Velocity(0);
        moveConnectionCompleted = true;
    }
    //Método para detener el movimiento del jugador
    public void StopMove()
    {
        inputMoveEnabled = false;
        rb2D.isKinematic = true;
        rb2D.velocity = Vector2.zero;
        isAutoWalk = false;
        animaciones.Velocity(0);
    }
}
