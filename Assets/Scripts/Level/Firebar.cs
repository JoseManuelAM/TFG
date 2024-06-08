using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Firebar : MonoBehaviour
{
    //Velocidad de rotaci�n de las barras de fuego del �ltimo nivel
    public float rotateSpeed = 75;
    void Update()
    {
        //Actualizar su posici�n cada fotograma
        transform.Rotate(Vector3.forward * rotateSpeed * Time.deltaTime);
    }
}
