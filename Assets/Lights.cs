using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lights : MonoBehaviour
{
    bool isGradEnabled = false;
    byte[] gradValues = new byte[2] {0,0};
    float timer = 1f;
    int currentAlpha = 0;
    int increment = -2;
    public Color32 definedColor;
    void Update()
    {
        if (PlayerPrefs.GetInt("realisticLights") == 1)
        {
            if (isGradEnabled == true)
            {
                timer -= Time.deltaTime;
                if (timer <= 0)
                {
                    timer = 0.5f;
                    currentAlpha = currentAlpha + increment;
                    if (currentAlpha <= gradValues[0])
                    {
                        increment = 2;
                    }
                    if (currentAlpha >= gradValues[1])
                    {
                        increment = -2;
                    }
                    gameObject.GetComponent<SpriteRenderer>().color = new Color32(definedColor.r, definedColor.g, definedColor.b, byte.Parse(currentAlpha.ToString()));
                }
            }
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().color = new Color32(definedColor.r, definedColor.g, definedColor.b, 0);
        }
    }
    public void Create(float x,float y)
    {
        gameObject.transform.position = new Vector3(x, y, -100);
    }

    public void SetLight(Color32 color)
    {
        gameObject.GetComponent<SpriteRenderer>().color = color;
        definedColor = color;
        isGradEnabled = false;
    }

    public void SetGradLight(Color32 color,byte minAlpha,byte maxAlpha)
    {
        gameObject.GetComponent<SpriteRenderer>().color = color;
        gradValues[0] = minAlpha;
        gradValues[1] = maxAlpha;
        isGradEnabled = true;
        currentAlpha = maxAlpha;
        definedColor = color;
    }

    public void ChangeGradColor(Color32 color)
    {
        definedColor = color;
    }
}
