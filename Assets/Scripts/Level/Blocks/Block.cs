using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    //Booleano que determina si el bloque es rompible o no
    public bool isBreakable;
    //Referencia al objeto de las piezas de ladrillo rotas
    public GameObject brickPiecePrefab;

    //Monedas que contendr� el bloque y refeencia al bloque de monedas
    public int numCoins;
    public GameObject coinBlockPrefab;

    //Booleano para controlar si el bloque est� rebotando
    bool bouncing;

    //Referencias a los sprites de los distintos estados por los que puede pasar el bloque
    public Sprite defaultBlock;
    public Sprite hitBlock;
    public Sprite hitEmptyBlock;
    public Sprite emptyBlock;
    bool isEmpty;

    //Referencias a los objetos de poder que pueden contener los bloques
    public GameObject itemPreab;
    public GameObject itemFlowerPrefab;

    //Detecci�n de colisiones y sprite del bloque
    public LayerMask onBlockLayers;
    BoxCollider2D boxCollider2D;
    SpriteRenderer spriteRenderer;
    private void Awake()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void Start()
    {
        if(spriteRenderer.sprite == null)
        {
            boxCollider2D.enabled = false;
        }
    }
    //M�todo que gestiona la interacci�n con objetos que est�n encima del bloque
    void OnTheBlock()
    {
        //Detecta colisiones con objetos sobre el bloque
        Collider2D[] colliders = Physics2D.OverlapBoxAll(boxCollider2D.bounds.center +
            Vector3.up * boxCollider2D.bounds.extents.y, boxCollider2D.bounds.size * 0.5f, 0, onBlockLayers);

        //Gestiona las interacciones con enemigos y objetos
        foreach(Collider2D c in colliders)
        {
            Enemy enemy = c.GetComponent<Enemy>();
            if(enemy != null)
            {
                enemy.HitBelowBlock();
            }
            else
            {
                Item item = c.GetComponent<Item>();
                if(item != null)
                {
                    if(item.type == ItemType.Coin)
                    {
                        Instantiate(coinBlockPrefab, transform.position, Quaternion.identity);
                        Destroy(item.gameObject);
                    }
                    else
                    {
                        item.HitBelowBlock();
                    }
                }
            }
        }
    }
    //M�todo para mostrar en el editor de Unity los colliders con fines de depuraci�n
    private void OnDrawGizmos()
    {
        if(boxCollider2D != null)
        {
            Gizmos.DrawWireCube(boxCollider2D.bounds.center +
                        Vector3.up * boxCollider2D.bounds.extents.y, boxCollider2D.bounds.size * 0.5f);
        }        
    }
    //M�todo que gestiona la colisi�n de la cabeza del jugador con el bloque
    public void HeadCollision(bool marioBig)
    {
        //Activa el collider y cambia el sprite al de bloque vac�o
        if(spriteRenderer.sprite == null)
        {
            boxCollider2D.enabled = true;
            spriteRenderer.sprite = emptyBlock;
        }
            //Si es rompible y el jugador es grande, el bloque se rompe
            if(isBreakable)
            {
            if(marioBig)
            {
                OnTheBlock();
                Break();
            }
            //Si el jugador es peque�o, tiene una animaci�n de rebote pero no se rompe
            else
            {
                Bounce();
            }
               
            }
            //Por �ltimo, si el bloque no est� vac�o, se gestiona su interacci�n con monedas u otros objetos que �ste puede contener
            else if(!isEmpty)
            {
                if (numCoins > 0)
                {
                    if (!bouncing)
                    {
                        Instantiate(coinBlockPrefab, transform.position, Quaternion.identity);
                        numCoins--;
                        if(numCoins <= 0)
                        {
                            isEmpty = true;
                        }
                        Bounce();
                    }
                }
                else if(itemPreab != null)
                {
                    if(!bouncing)
                    {
                        StartCoroutine(ShowItem());
                        
                        isEmpty = true;
                        Bounce();
                    }
                }
            }
            if(!isEmpty)
            {
                OnTheBlock();
            }
    }
    //M�todo para romper un bloque
    public void BreakFromTop()
    {
        if(isBreakable)
        {
            Break();
        }
    }
    //M�todo para la animaci�n de rebote del bloque
    void Bounce()
    {
        if(!bouncing)
        {
            StartCoroutine(BounceAnimation());
        }
    }
    //Corutina que gestiona esta animaci�n
    IEnumerator BounceAnimation()
    {
        //Cambia el bloque dependiendo de si est� vac�o o no
        if(isEmpty)
        {
            SpritesAnimation spritesAnimation = GetComponent<SpritesAnimation>();
            if (spritesAnimation != null)
            {
                spritesAnimation.stop = true;
            }
            spriteRenderer.sprite = hitEmptyBlock;
        }
        else
        {
            if(hitBlock != null)
            {
                spriteRenderer.sprite = hitBlock;
            }          
        }
        //Variables para manejar la animaci�n
        AudioManager.Instance.PlayBump();
        bouncing = true;
        float time = 0;
        float duration = 0.1f;

        Vector2 startPosition = transform.position;
        Vector2 targetPosition = (Vector2)transform.position + Vector2.up * 0.25f;

        //Mueve el bloque hasta la posici�n objetivo
        while (time < duration)
        {
            transform.position = Vector2.Lerp(startPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPosition;
        time = 0;
        //El bloque vuelve a su posici�n original
        while (time < duration)
        {
            transform.position = Vector2.Lerp(targetPosition, startPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        transform.position = startPosition;
        bouncing = false;

        if(isEmpty)
        {
            SpritesAnimation spritesAnimation = GetComponent<SpritesAnimation>();
            if(spritesAnimation != null)
            {
                spritesAnimation.stop = true;
            }
            spriteRenderer.sprite = emptyBlock;
        }
        else
        {
            SpritesAnimation spritesAnimation = GetComponent<SpritesAnimation>();
            if (spritesAnimation != null)
            {
                spritesAnimation.stop = false;
            }
            else
            {
                spriteRenderer.sprite = defaultBlock;
            }
        }
    }
    //M�todo para romper el bloque y generar los fragmentos de ladrillo
    void Break()
    {
        AudioManager.Instance.PlayBreak();
        ScoreManager.Instance.SumarPuntos(50);
        GameObject brickPiece;
        //Parte arriba a la derecha
        brickPiece = Instantiate(brickPiecePrefab, transform.position,
            Quaternion.identity);
        brickPiece.GetComponent<Rigidbody2D>().velocity = new Vector2(3f, 12f);
        //Parte arriba a la izquierda
        brickPiece = Instantiate(brickPiecePrefab, transform.position,
            Quaternion.identity);
        brickPiece.transform.localScale = new Vector3(-1f, 1f, 1f);
        brickPiece.GetComponent<Rigidbody2D>().velocity = new Vector2(-3f, 12f);
        //Parte abajo a la derecha
        brickPiece = Instantiate(brickPiecePrefab, transform.position,
           Quaternion.identity);
        brickPiece.transform.localScale = new Vector3(1f, -1f, 1f);
        brickPiece.GetComponent<Rigidbody2D>().velocity = new Vector2(3f, 8f);
        //Parte abajo a la izquierda
        brickPiece = Instantiate(brickPiecePrefab, transform.position,
           Quaternion.identity);
        brickPiece.transform.localScale = new Vector3(-1f, -1f, 1f);
        brickPiece.GetComponent<Rigidbody2D>().velocity = new Vector2(-3f, 8f);

        Destroy(gameObject);
    }
    //Corutina que muestra un objeto saliendo del bloque
    IEnumerator ShowItem()
    {
        AudioManager.Instance.PlayPowerUpAppear();
        GameObject newItem;

        if(itemFlowerPrefab != null && Mario.Instance.IsBig())
        {
            newItem = Instantiate(itemFlowerPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            newItem = Instantiate(itemPreab, transform.position, Quaternion.identity);
        }
       
        Item item = newItem.GetComponent<Item>();
        item.WaitMove();
        //Variables para la animaci�n hacia arriba
        float time = 0;
        float duration = 1f;
        Vector2 startPosition = newItem.transform.position;
        Vector2 targetPostion = (Vector2)transform.position + Vector2.up * 0.5f;

        //Movimiento hacia arriba, creando el efecto del objeto apareciendo
        while(time < duration)
        {
            newItem.transform.position = Vector2.Lerp(startPosition, targetPostion, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        newItem.transform.position = targetPostion;
        item.StartMove();
    }
}
