using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Keycups : MonoBehaviour
{
    //Gameobjects for keycups
    public List<GameObject> KeyCups;
    public GameObject KeyCupX;
    public GameObject KeyCupLft;

    //Position buffer list and vars
    public List<Vector2> KeysPos;

    //Other
    public bool finishedLoad = false;

    //Show or hide keycups if..
    void Start()
    {
        //Detect input and device
        try
        {
            if (SystemInfo.operatingSystemFamily != OperatingSystemFamily.Windows)
            {
                KeyCups.Add(KeyCupX);
            }
        }
        catch
        {

        }

        //Add mainpos to list Vt2
        foreach (GameObject key in KeyCups)
        {
            KeysPos.Add(key.transform.position);
        }

        //Hide all
        finishedLoad = true;
        HideKeyCups();
    }

    //Control the hid of the keycups 
    #region KeycupsControl
    public void ShowKeyCups()
    {
        if(SystemInfo.operatingSystemFamily == OperatingSystemFamily.Windows)
        {
            int i = 0;
            foreach (GameObject key in KeyCups)
            {
                key.transform.position = KeysPos[i];
                i++;
            }
        }
    }
    public void HideKeyCups()
    {
        if (finishedLoad == true)
        {
            foreach (GameObject key in KeyCups)
            {
                key.transform.position = new Vector2(-1000, -1000);
            }
        }
    }
    #endregion
}
