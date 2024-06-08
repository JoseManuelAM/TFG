using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HUD : MonoBehaviour
{
    //Elementos de texto presentes en la interfaz
    public TextMeshProUGUI score;
    public TextMeshProUGUI numCoins;
    public TextMeshProUGUI time;
    public TextMeshProUGUI worldLevel;

    // Update is called once per frame
    void Update()
    {
        //Actualizar los puntos con el valor del script "ScoreManager", que determina los puntos del jugador
        score.text = ScoreManager.Instance.puntos.ToString("D6");
    }

    //Mëtodo que actualiza el número de monedas recogidas
    public void UpdateCoins(int totalCoins)
    {
        //Este número es un string de dos dígitos
        numCoins.text = "x" + totalCoins.ToString("D2");
    }
    //Método que actualiza el tiempo restante
    public void UpdateTime(float timeLeft)
    {
        //El tiempo se aproximará siempre a número entero
        int timeLeftInt = Mathf.RoundToInt(timeLeft);
        //Formatearlo como string de tres dígitos
        time.text = timeLeftInt.ToString("D3");
    }
    //Método para actualizar el nivel actual
    public void UpdateWorld(int world, int level)
    {
        worldLevel.text = world + "-" + level;
    }
}
