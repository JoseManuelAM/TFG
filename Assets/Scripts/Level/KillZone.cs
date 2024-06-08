using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Zona de muerte que si la toca el jugador, muere instant�neamente (vac�os, lava)
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            GameManager.Instance.KillZone();
        }
    }
}
