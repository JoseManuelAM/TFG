using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOutCamera : MonoBehaviour
{
    //Referencia al componente SpriteRenderer
    SpriteRenderer spriteRenderer;
    //Booleano que identifica si el objeto ha estado visible en algún momento
    bool hasBeenVisible;

    //Booleano para detectar si el objeto está detrás de cámara
    public bool onlyBack;
    //Distancia mínima desde la cámara para destruir el objeto
    public float minDistance = 0;

    public GameObject parent;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(spriteRenderer.isVisible)
        {
            hasBeenVisible = true;
        }
        else
        {
            //Si el objeto ha sido visible, comprobar su distancia y destruirlo si está detrás de la cámara
            if(hasBeenVisible)
            {
                if (Mathf.Abs(transform.position.x - Camera.main.transform.position.x) > minDistance)
                {

                    if(onlyBack)
                    {
                        //Si está delante el objeto, no hacer nada
                        if (transform.position.x > Camera.main.transform.position.x)
                        {
                            return;
                        }
                    }

                    //Si no tiene objeto padre, destruir el objeto
                    if(parent == null)
                    {
                        Destroy(gameObject);
                    }
                    //En caso de que sí tenga, destruir el objeto padre
                    else
                    {
                        Destroy(parent);
                    }
                    
                }

            }
        }
    }
}
