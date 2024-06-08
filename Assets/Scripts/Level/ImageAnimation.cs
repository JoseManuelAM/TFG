using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageAnimation : MonoBehaviour
{
    //Array de sprites
    public Sprite[] sprites;
    //Tiempo entre cada fotograma
    public float frameTime = 0.1f;

    //Referencia al componente Image donde se mostrar� la animaci�n
    Image image;
    //�ndice actual del fotograma de la animaci�n
    int animationFrame = 0;

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
        //Invocar continuamente el m�todo para cada fotograma
        InvokeRepeating("ChangeImage", frameTime, frameTime);
    }

    //M�todo para cambiar el sprite de la imagen
    void ChangeImage()
    {
        //Actualizar el sprite y avanzar al siguiente sprite
        image.sprite = sprites[animationFrame];
        animationFrame++;
        //Cada vez que recorramos el array de sprites, volver al inicio para repetir la animaci�n en bucle
        if(animationFrame >= sprites.Length)
        {
            animationFrame = 0;
        }
    }
}
