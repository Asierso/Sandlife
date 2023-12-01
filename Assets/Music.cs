using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour
{
    public AudioClip Base;
    public AudioClip[] Relax;
    public bool music = true;
    public int lastOST = -1;
    public enum MusicPlayer {Main,Other};
    public MusicPlayer MusicPlayerType;
    // Start is called before the first frame update
    void Start()
    {
        //
        
        
    }
    private void Update()
    {
        if (PlayerPrefs.GetInt("music") == 1)
        {
            if (MusicPlayerType == MusicPlayer.Main)
            {
                if (gameObject.GetComponent<AudioSource>().isPlaying == false)
                {
                    gameObject.GetComponent<AudioSource>().clip = Base;
                    gameObject.GetComponent<AudioSource>().Play();
                }

            }
            else if (MusicPlayerType == MusicPlayer.Other)
            {
                if (gameObject.GetComponent<AudioSource>().isPlaying == false)
                {
                    System.Random rdn = new System.Random();
                    var random = rdn.Next(0, 1000);
                    if (random < Relax.Length && lastOST != random)
                    {
                        Debug.Log("Playing Relax" + random);
                        lastOST = random;
                        if (random == 0) gameObject.GetComponent<AudioSource>().volume = 0.4f;
                        gameObject.GetComponent<AudioSource>().clip = Relax[random];
                        gameObject.GetComponent<AudioSource>().loop = false;
                        gameObject.GetComponent<AudioSource>().Play();
                    }
                }

            }
        }
        else
        {
            gameObject.GetComponent<AudioSource>().Stop();
        }
        
    }
}
