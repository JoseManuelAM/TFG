using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Transition : MonoBehaviour
{
    public TextMeshProUGUI numLives;
    public TextMeshProUGUI world;
    public GameObject panelLevel;
    public GameObject panelGameOver;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.HideTimer();
        if(GameManager.Instance.isGameOver)
        {
            panelGameOver.SetActive(true);
            panelLevel.SetActive(false);
        }
        else
        {
            numLives.text = GameManager.Instance.lives.ToString();
            world.text = GameManager.Instance.currentWorld + "-" + GameManager.Instance.currentLevel;
            panelGameOver.SetActive(false);
            panelLevel.SetActive(true);
        }
    }

}
