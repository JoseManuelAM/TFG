using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toad : MonoBehaviour
{
    //Referencias al texto que se muestra al final del último nivel
    public GameObject text1;
    public GameObject text2;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Detiene el movimiento del jugador al acercarse al NPC
        if(collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Mario.Instance.mover.StopMove();
            StartCoroutine(ShowFinalTexts());
        }
    }
    //Corutina que gestiona los textos del final y la vuelta al menú inicial
    IEnumerator ShowFinalTexts()
    {
        AudioManager.Instance.PlayCastleCompleted();
        yield return new WaitForSeconds(1f);
        text1.SetActive(true);
        yield return new WaitForSeconds(1f);
        text2.SetActive(true);
        LevelManager.Instance.LevelFinished();
        yield return new WaitForSeconds(5f);
        UnityEngine.SceneManagement.SceneManager.LoadScene("StartMenu");
    }
}
