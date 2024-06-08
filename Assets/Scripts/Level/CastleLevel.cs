using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastleLevel : MonoBehaviour
{
    //Booleano para saber si el jugador está en el castillo al final de cada nivel
    bool marioInCastle;

    void Update()
    {
        //Si está en el castillo y ya se han contado los puntos, avanzar al siguiente nivel
        if(marioInCastle && LevelManager.Instance.countPoints)
        {
            GameManager.Instance.NextLevel();
            marioInCastle = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Mario mario = collision.gameObject.GetComponent<Mario>();
        if(mario != null)
        {
            //Al entrar al castillo, mover al jugador fuera de la cámara
            mario.transform.position = new Vector3(1000, 1000, 1000);
            marioInCastle = true;
            LevelManager.Instance.MarioInCastle();
        }
    }
}
