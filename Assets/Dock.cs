using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dock : MonoBehaviour
{
    //Position buffer list
    private List<Vector2> Position = new List<Vector2>();

    //Dock components list
    public GameObject[] dockObj;
    public Sprite[] dockBtnTextures;
    public GameObject DockBtn;
    public Image dock;
    public bool hiddenDock = false;

    //Controls the dock position and hid
    #region DockLoader
    void Start()
    {
        foreach(GameObject handle in dockObj)
        {
            Position.Add(handle.transform.position);
        }
        DockUI();
    }
    public void DockUI()
    {
        gameObject.GetComponent<SaveLoad>().musicControler.GetComponent<Sounds>().PlaySound(3);
        if (hiddenDock == false)
        {
            dock.sprite = dockBtnTextures[0];
            hiddenDock = true;
            foreach (GameObject handle in dockObj)
            {
                handle.transform.position = new Vector2(9999,9999);
            }
            gameObject.GetComponent<Keycups>().HideKeyCups();
        }
        else
        {
            dock.sprite = dockBtnTextures[1];
            hiddenDock = false;
            for (int i = 0;i<dockObj.Length;i++)
            {
                dockObj[i].transform.position = Position[i];
            }
            gameObject.GetComponent<Keycups>().ShowKeyCups();

        }
    }
    #endregion
}
