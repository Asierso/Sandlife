using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMusic : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<AudioSource>().time = PlayerPrefs.GetFloat("stmusic");
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerPrefs.GetInt("music") == 1)
        {
            if (PlayerPrefs.GetFloat("stmusic") == 0f && SceneManager.GetActiveScene().name == "Game")
            {
                gameObject.GetComponent<AudioSource>().Stop();
            }
            PlayerPrefs.SetFloat("stmusic", gameObject.GetComponent<AudioSource>().time);
        }
        else
        {
            gameObject.GetComponent<AudioSource>().Stop();
        }
    }
}
