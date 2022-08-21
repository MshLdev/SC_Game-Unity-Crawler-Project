using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UI_Fader : MonoBehaviour
{
    Color32 targetColor = new Color32(21, 21, 21, 0);
    float timeLeft = 9.0f;

    void Update()
    {
        if(timeLeft <= 8.5f)
        {
            if (timeLeft <= Time.deltaTime)
            {
                gameObject.GetComponent<Image>().color = targetColor;
            }
            else
            {
                gameObject.GetComponent<Image>().color = Color.Lerp(gameObject.GetComponent<Image>().color, targetColor, Time.deltaTime / timeLeft);
                gameObject.transform.GetChild(0).GetComponent<Text>().color = Color.Lerp(gameObject.transform.GetChild(0).GetComponent<Text>().color, targetColor, Time.deltaTime / timeLeft);
            }
        }

        // update the timer
        timeLeft -= Time.deltaTime;
    }
}
