using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Piano : MonoBehaviour
{
    //Bools of show and hide ui parts
    public bool blockOpened = true;

    //Position buffer list and vars
    private List<Vector2> inventoryPosition = new List<Vector2>();

    //Resources for buttons
    public GameObject[] piano;
    public Sprite[] pianoKeys;
    public GameObject musicControler;
    public GameObject noteControler;
    public Dropdown dropdown;
    public GameObject pianoBg;
    public AudioClip[] Instrument;
    public AudioClip[] PianoNotes;

    //Other
    private GameObject handlePianoBlock;
    // Start is called before the first frame update
    void Start()
    {
        //Add mainpos to list Vt2
        foreach (GameObject handle in piano)
        {
            inventoryPosition.Add(handle.transform.position);
            handle.transform.position = new Vector2(-1000, -1000);
        }

        //Hide menu
        blockOpened = false;

    }

    public void BlockUIClick(GameObject handlePiano)
    {
        musicControler.GetComponent<Sounds>().PlaySound(3);
        if (blockOpened == true)
        {
            foreach (GameObject handle in piano)
            {
                handle.transform.position = new Vector2(-1000, -1000);
            }
            blockOpened = false;
            //gameObject.GetComponent<Controler>().focusObject.GetComponent<Focus>().show = false;
        }
        else
        {
            int i = 0;
            foreach (GameObject handle in piano)
            {
                handle.transform.position = inventoryPosition[i];
                i++;
            }
            handlePianoBlock = handlePiano;
            
            pianoBg.GetComponent<Image>().sprite = pianoKeys[handlePianoBlock.GetComponent<Block>().blockStatus];
            if (handlePianoBlock.GetComponent<Block>().internalInfo == "")
            {
                handlePianoBlock.GetComponent<Block>().internalInfo = "0";
            }
            dropdown.value = Convert.ToInt32(handlePianoBlock.GetComponent<Block>().internalInfo);
            blockOpened = true;
        }
    }

    public void Close()
    {
        foreach (GameObject handle in piano)
        {
            handle.transform.position = new Vector2(-1000, -1000);
        }
        blockOpened = false;
    }

    public void SetMusicalNote(int num)
    {
        pianoBg.GetComponent<Image>().sprite = pianoKeys[num];
        handlePianoBlock.GetComponent<Block>().blockStatus = num;
        PlayNote(num, handlePianoBlock.GetComponent<Block>().internalInfo);


    }

    public void PlayNote(int num,string note)
    {
        if (note == "")
        {
            note = "0";
        }
        int notePlay = Convert.ToInt32(note);
        if (num != 0)
        {
            IEnumerator playSound()
            {
                GameObject handle = Instantiate(noteControler);
                if (notePlay != 0)
                {
                    handle.GetComponent<AudioSource>().pitch = 0.5f + (float)num / 10;
                    handle.GetComponent<AudioSource>().clip = Instrument[notePlay];
                }
                else
                {
                    handle.GetComponent<AudioSource>().clip = PianoNotes[num - 1];
                }
                handle.GetComponent<AudioSource>().Play();
                yield return new WaitForSeconds(2.5f);
                Destroy(handle);
            }
            StartCoroutine(playSound());
        }
    }

    public void NoteType()
    {
        int type = 0;
        switch (dropdown.captionText.text)
        {
            case "Piano": type = 0; break;
            case "Violin": type = 1; break;
            case "Organ": type = 2; break;
            case "Xilophone": type = 3; break;
            case "Cello": type = 4; break;
            case "Kick": type = 5; break;
            case "8 Bit": type = 6; break;
        }
        handlePianoBlock.GetComponent<Block>().internalInfo = type.ToString();
        musicControler.GetComponent<Sounds>().PlaySound(3);
    }
}
