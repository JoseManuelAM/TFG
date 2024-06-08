using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
//Clase que implementa las interfaces que manejan elementos de selección
public class MenuButton : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    //Referencia al objeto que será el cursor
    public GameObject icon;
    //Método para cuando el botón se deja de seleccionar
    public void OnDeselect(BaseEventData eventData)
    {
        icon.SetActive(false);
    }
    //Método para cuando el botón se selecciona
    public void OnSelect(BaseEventData eventData)
    {
        icon.SetActive(true);
    }
}
