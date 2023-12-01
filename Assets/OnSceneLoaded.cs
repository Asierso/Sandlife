using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnSceneLoaded : MonoBehaviour
{
    public GameObject Background;
    public GameObject[] GameNameIcon = new GameObject[2];
    int Alpha = 255;
    // Start is called before the first frame update
    void Start()
    {
        Background.transform.position = new Vector2(gameObject.transform.position.x * 2 - Screen.width, gameObject.transform.position.y * 2 - Screen.height);
        Background.transform.localScale = new Vector2(Screen.width,Screen.height);
        Cursor.lockState = CursorLockMode.None;
        // Show the cursor
        Cursor.visible = true;
        StartCoroutine(OnLoaded(Background.GetComponent<Image>()));
    }

    IEnumerator OnLoaded(Image EnfasisImage)
    {
        while(Alpha > 5)
        {
            EnfasisImage.color = new Color32(255, 255, 255, Byte.Parse(Alpha.ToString()));
            GameNameIcon[0].GetComponent<TMPro.TextMeshProUGUI>().color = new Color32(32, 32, 32, Byte.Parse(Alpha.ToString()));
            GameNameIcon[1].GetComponent<Image>().color = new Color32(255, 255, 255, Byte.Parse(Alpha.ToString()));

            yield return new WaitForSeconds(0.02f);
            Alpha = Alpha - 10;
        }
        Destroy(gameObject);
    }
}
