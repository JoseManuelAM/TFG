using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeFinal : MonoBehaviour
{
    //Array de las partes del puente destruibles
    public GameObject[] bridgeParts;
    //Límite de la cámara del juego al romper el puente final
    public Transform finalLimit;
    //Referencias al objeto del puente y a Bowser
    public GameObject bridge;
    public Bowser bowser;

    //Variable para saber si el puente ya ha sido destruido
    bool isBridgeCollapse;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Comprobar si el jugador ha tocado el hacha
        if(collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            //Si aún no se ha destruido el puente, iniciar su destrucción
            if(!isBridgeCollapse)
            {
                bowser.collapseBridge = true;
                isBridgeCollapse = true;
                GetComponent<CircleCollider2D>().enabled = false;
                Mario.Instance.mover.StopMove();
                LevelManager.Instance.levelPaused = true;

                StartCoroutine(CollapseBridge());
            }
        }
    }

    //Corutina que destrulle el puente
    IEnumerator CollapseBridge()
    {
        if(!bowser.isDead)
        {
            //Destruir las partes del puente
            foreach (GameObject bridgePart in bridgeParts)
            {
                Destroy(bridgePart);
                yield return new WaitForSeconds(0.2f);
            }
            //Destruir el puente completo
            Destroy(bridge);
            bowser.FallBridge();
            yield return new WaitForSeconds(1.25f);
        }
        
        Mario.Instance.mover.AutoWalk();
        Camera.main.GetComponent<CameraFollow>().UpdateMaxPos(finalLimit.position.x);
    }
}
