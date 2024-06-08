using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shake : MonoBehaviour
{
    public float angle = 45f;
    public float time = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ShakeObject());
    }

    IEnumerator ShakeObject()
    {
        int direction = 1;
        float timer = 0;
        while(true)
        {
            timer += Time.deltaTime;
            float zAngle = direction * timer * (angle / time);
            transform.rotation = Quaternion.Euler(0, 0, zAngle);
            if(timer >= time)
            {
                direction *= -1;
                timer = 0;
            }
            yield return null;
        }
    }
}
