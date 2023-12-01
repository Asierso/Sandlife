using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SingleKeyCup : MonoBehaviour
{
    //Resources and sprites for keycups animation
    public Sprite KeycupHold;
    public Sprite KeycupRelease;
    public KeyCode KeyToDetect;

    //Keys cooldown
    private float timer = 1f;

    //Letters animation Vector and Handle
    private GameObject childText;
    private Vector2 normalPos;
    private Vector2 holdPos;
    private void Start()
    {
        childText = gameObject.GetComponentsInChildren<Transform>()[1].gameObject;
        normalPos = new Vector2(childText.transform.localPosition.x, childText.transform.localPosition.y);
        holdPos = new Vector2(childText.transform.localPosition.x, childText.transform.localPosition.y - 3f);
    }
    private void Update()
    {
        if (SystemInfo.operatingSystemFamily == OperatingSystemFamily.Windows)
        {
            if (timer < 1)
            {
                timer += Time.deltaTime;
            }
            if (timer > 0.2f)
            {
                if (Input.GetKeyDown(KeyToDetect))
                {
                    gameObject.GetComponent<Image>().sprite = KeycupHold;
                    childText.transform.localPosition = holdPos;
                    timer = 0f;
                }
                
            }
            if (Input.GetKeyUp(KeyToDetect))
            {
                gameObject.GetComponent<Image>().sprite = KeycupRelease;
                childText.transform.localPosition = normalPos;
            }
        }
    }
}
