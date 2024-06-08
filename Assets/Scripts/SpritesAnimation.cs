using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpritesAnimation : MonoBehaviour
{
    //Array de sprites para hacer la animaci�n
    public Sprite[] sprites;
    public float frameTime = 0.1f;

    int animationFrame = 0;

    public bool stop;
    public bool loop = true;
    SpriteRenderer spriteRenderer;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Animation());
    }

    IEnumerator Animation()
    {
        //Si la animaci�n es un bucle
        if(loop)
        {
            while (!stop)
            {
                //Asigna el sprite actual al Sprite renderer
                spriteRenderer.sprite = sprites[animationFrame];
                //Incrementa el �ndice del fotograma de la animaci�n
                animationFrame++;
                //Al llegar al final del array, vuelve al fotograma 0, es decir al inicio
                if (animationFrame >= sprites.Length)
                {
                    animationFrame = 0;
                }
                yield return new WaitForSeconds(frameTime);
            }
        }
        else
        {
            //Si no est� en bucle, se recorren todos los sprites una �nica vez
            while (animationFrame < sprites.Length)
            {
                spriteRenderer.sprite = sprites[animationFrame];
                animationFrame++;
                yield return new WaitForSeconds(frameTime);
            }
            Destroy(gameObject);
        }
        
    }


}
