using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Focus : MonoBehaviour
{
    public bool show = false;
    public GameObject canvas;
    private float[] coords = new float[2];
    // Update is called once per frame
    private void Start()
    {
        coords[0] = gameObject.transform.position.x;
        coords[1] = gameObject.transform.position.y;
        gameObject.GetComponent<Transform>().localScale = new Vector2(0.1f, 0.1f);
        gameObject.transform.position = new Vector2(9999, 9999);
    }
    void Update()
    {
        if (show == true)
        {
            if (PlayerPrefs.GetInt("focusEnabled") == 1)
            {
                gameObject.GetComponent<Transform>().localScale = new Vector2(Screen.width, Screen.height);
                gameObject.transform.position = new Vector2(coords[0], coords[1]);
            }
            else
            {
                gameObject.GetComponent<Transform>().localScale = new Vector2(0.1f, 0.1f);
                gameObject.transform.position = new Vector2(9999, 9999);
            }
        }
        else
        {
            gameObject.GetComponent<Transform>().localScale = new Vector2(0.1f, 0.1f);
            gameObject.transform.position = new Vector2(9999, 9999);
        }   
        if(canvas.GetComponent<SaveLoad>().loadOpened == false && canvas.GetComponent<UI>().blockOpened == false && canvas.GetComponent<Settings>().blockSettings == false && canvas.GetComponent<Online>().blockOnline == false && canvas.GetComponent<Command>().loadOpened == false && canvas.GetComponent<Computers>().blockComputer == false && canvas.GetComponent<Trophies>().blockTrophies == false && canvas.GetComponent<Message>().blockAlert == false && canvas.GetComponent<Profile>().blockProfile == false && canvas.GetComponent<Piano>().blockOpened == false)
        {
            show = false;
        }
        else
        {
            show = true;
        }
    }
}
