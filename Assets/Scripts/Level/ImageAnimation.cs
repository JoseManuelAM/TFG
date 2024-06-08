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

    //Referencia al componente Image donde se mostrará la animación
    Image image;
    //Ïndice actual del fotograma de la animación
    int animationFrame = 0;

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
        //Invocar continuamente el método para cada fotograma
        InvokeRepeating("ChangeImage", frameTime, frameTime);
    }

    //Método para cambiar el sprite de la imagen
    void ChangeImage()
    {
        //Actualizar el sprite y avanzar al siguiente sprite
        image.sprite = sprites[animationFrame];
        animationFrame++;
        //Cada vez que recorramos el array de sprites, volver al inicio para repetir la animación en bucle
        if(animationFrame >= sprites.Length)
        {
            animationFrame = 0;
        }
    }
}
