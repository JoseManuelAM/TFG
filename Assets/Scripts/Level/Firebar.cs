using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Firebar : MonoBehaviour
{
    //Velocidad de rotación de las barras de fuego del último nivel
    public float rotateSpeed = 75;
    void Update()
    {
        //Actualizar su posición cada fotograma
        transform.Rotate(Vector3.forward * rotateSpeed * Time.deltaTime);
    }
}
