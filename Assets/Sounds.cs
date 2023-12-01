using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Sounds : MonoBehaviour
{
    public AudioClip[] AudioClips;
    public bool sounds = true;
    // Start is called before the first frame update
    public void PlaySound(int soundID,float volume = 1f)
    {
        if (PlayerPrefs.GetInt("sounds") != 0)
        {
            IEnumerator playSound()
            {
                if (gameObject.GetComponent<AudioSource>().isPlaying == true)
                {
                    GameObject handle = Instantiate(gameObject);
                    gameObject.GetComponent<AudioSource>().volume = volume;
                    handle.GetComponent<AudioSource>().clip = AudioClips[soundID];
                    handle.GetComponent<AudioSource>().Play();
                    yield return new WaitForSeconds(2.5f);
                    Destroy(handle);
                }
                else
                {
                    gameObject.GetComponent<AudioSource>().volume = volume;
                    gameObject.GetComponent<AudioSource>().clip = AudioClips[soundID];
                    gameObject.GetComponent<AudioSource>().Play();
                }
            }
            StartCoroutine(playSound());
        }
        
        
    }
    /*
    ID:0 Dirt
    ID:1 Sand
    ID:2 Delete
    ID:3 Click
    ID:4 Stone
    ID:5 Wood
    ID:6 Leaves
    ID:7 Farts
    ID:8 Ice
    ID:9 Energy
    ID:10 Lights
    ID:11 Switch
    */

}
