using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Command : MonoBehaviour
{
    public bool loadOpened = true;
    public AudioSource musicControler;
    public GameObject[] loadui;
    public GameObject input;
    public Dropdown dropdown;
    private GameObject handle = null;
    private List<float> loadX = new List<float>();
    private List<float> loadY = new List<float>();
    private string copiedText = "";
    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject loadComponent in loadui)
        {
            loadX.Add(loadComponent.transform.position.x);
            loadY.Add(loadComponent.transform.position.y);
            loadComponent.transform.position = new Vector2(-1000, -1000);
        }
        Close();
    }

    // Update is called once per frame
    void Update()
    {
        if (handle != null)
        {
            if (handle.GetComponent<Block>().internalInfo != input.GetComponent<InputField>().text)
            {
                handle.GetComponent<Block>().internalInfo = input.GetComponent<InputField>().text;
            }
        }
    }

    public void LoadUIClick(GameObject handle)
    {
            this.handle = handle;
            musicControler.GetComponent<Sounds>().PlaySound(3);
            loadOpened = true;
            int i = 0;
            input.GetComponent<InputField>().text = handle.GetComponent<Block>().internalInfo;
            dropdown.value = handle.GetComponent<Block>().blockStatus;
            foreach (GameObject loadComponent in loadui)
            {
                loadComponent.transform.position = new Vector2(loadX[i], loadY[i]);
                i++;
            }
        //gameObject.GetComponent<Controler>().focusObject.GetComponent<Focus>().show = true;
    }

    public void Close()
    { 
        loadOpened = false;
        foreach (GameObject loadComponent in loadui)
        {
            loadComponent.transform.position = new Vector2(-1000, -1000);
        }
        //gameObject.GetComponent<Controler>().focusObject.GetComponent<Focus>().show = false;
        musicControler.GetComponent<Sounds>().PlaySound(3);
    }

    public void CommandType()
    {
        int type = 0;
        switch(dropdown.captionText.text)
        {
            case "Normal":type = 0;break;
            case "Repeated":type = 1;break;
            case "Chain": type = 2; break;
        }
        handle.GetComponent<Block>().blockStatus = type;
        musicControler.GetComponent<Sounds>().PlaySound(3);
    }

    public void Copy()
    {
        copiedText = input.GetComponent<InputField>().text;
        GUIUtility.systemCopyBuffer = copiedText;
        musicControler.GetComponent<Sounds>().PlaySound(3);
    }

    public void Paste()
    {
        input.GetComponent<InputField>().text += copiedText;
        musicControler.GetComponent<Sounds>().PlaySound(3);
    }

    public void AddPoints()
    {
        PlayerPrefs.SetInt("commandCount", PlayerPrefs.GetInt("commandCount") + 1);
    }
}
