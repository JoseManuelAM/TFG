using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
//Clase que implementa las interfaces que manejan elementos de selecci�n
public class MenuButton : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    //Referencia al objeto que ser� el cursor
    public GameObject icon;
    //M�todo para cuando el bot�n se deja de seleccionar
    public void OnDeselect(BaseEventData eventData)
    {
        icon.SetActive(false);
    }
    //M�todo para cuando el bot�n se selecciona
    public void OnSelect(BaseEventData eventData)
    {
        icon.SetActive(true);
    }
}
