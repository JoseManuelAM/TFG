using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockCoin : MonoBehaviour
{
    //Referencia al objeto que muestra los puntos al recoger una moneda
    public GameObject floatPointsPrefab;
    void Start()
    {
        GameManager.Instance.AddCoins();
        AudioManager.Instance.PlayCoin();
        ScoreManager.Instance.SumarPuntos(200);

        //Posición de los puntos encima del bloque de monedas
        Vector2 postionFloatPoints = new Vector2(transform.position.x, transform.position.y + 1f);
        GameObject newFloatPoints = Instantiate(floatPointsPrefab, postionFloatPoints, Quaternion.identity);
        FloatPoints floatPoints = newFloatPoints.GetComponent<FloatPoints>();
        floatPoints.numPoints = 200;

        transform.position = new Vector2(transform.position.x, transform.position.y + 0.5f);
        StartCoroutine(Animation());
    }

    //Corutina que anima el bloque de monedas hacia arriba y abajo
    IEnumerator Animation()
    {
        float time = 0;
        float duration = 0.25f;
        Vector2 startPosition = transform.position;
        Vector2 targetPosition = (Vector2)transform.position + (Vector2.up * 2f);

        while(time < duration)
        {
            transform.position = Vector2.Lerp(startPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPosition;
        time = 0;
        while(time < duration)
        {
            transform.position = Vector2.Lerp(targetPosition, startPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        transform.position = startPosition;
        Destroy(gameObject);
    }
}
