using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Message : MonoBehaviour
{
    public GameObject[] message;
    public List<Vector2> messageVct = new List<Vector2>();
    public List<Vector2> viewButtonsPosition = new List<Vector2>();
    public bool blockAlert = true;
    public GameObject musicControler;
    public Action ButtonAClickedDelegate;
    public Action ButtonBClickedDelegate;

    public GameObject title;
    public GameObject[] KeyCups;
    public GameObject content;
    public GameObject buttonA;
    public GameObject buttonB;
    public InputField input;
    bool inputAviable = false;
    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject inventoryComponent in message)
        {
            messageVct.Add(inventoryComponent.transform.position);
            inventoryComponent.transform.position = new Vector2(-1000, -1000);
        }
        blockAlert = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BlockButton()
    {
        musicControler.GetComponent<Sounds>().PlaySound(3);
        BlockUIClick();
    }
    public void BlockUIClick()
    {
        if (blockAlert == true)
        {
            foreach (GameObject inventoryComponent in message)
            {
                inventoryComponent.transform.position = new Vector2(-1000, -1000);
            }
            blockAlert = false;
            //gameObject.GetComponent<Controler>().focusObject.GetComponent<Focus>().show = false;
        }
        else
        {
            int i = 0;
            foreach (GameObject inventoryComponent in message)
            {
                if(inputAviable == false && inventoryComponent.transform.name == "input")
                {
                    
                }
                else
                {
                    inventoryComponent.transform.position = messageVct[i];
                }
                i++;
            }
            if (SystemInfo.operatingSystemFamily != OperatingSystemFamily.Windows)
            {
                foreach (GameObject key in KeyCups)
                {
                    key.transform.position = new Vector2(-1000, -1000);
                }
            }
            blockAlert = true;
            //gameObject.GetComponent<Controler>().focusObject.GetComponent<Focus>().show = true;
        }
    }
    public void ShowAlert(string title,string content,string buttonA,string buttonB,Action buttonAAction,Action buttonBAction,bool haveTxtBox = false,string txtBoxPlaceholder = "")
    {
        this.title.GetComponent<TMPro.TextMeshProUGUI>().text = title;
        this.content.GetComponent<TMPro.TextMeshProUGUI>().text = content;
        this.buttonA.GetComponent<Text>().text = buttonA;
        this.buttonB.GetComponent<Text>().text = buttonB;
        ButtonAClickedDelegate = buttonAAction;
        ButtonBClickedDelegate = buttonBAction;
        input.placeholder.GetComponent<Text>().text = txtBoxPlaceholder;
        inputAviable = haveTxtBox;
        BlockUIClick();
    }
    public void OptionAClicked()
    {
        //gameObject.GetComponent<SaveLoad>().musicControler.GetComponent<Sounds>().PlaySound(3);
        ButtonAClickedDelegate?.Invoke();
        BlockUIClick();
    }
    public void OptionBClicked()
    {
        //gameObject.GetComponent<SaveLoad>().musicControler.GetComponent<Sounds>().PlaySound(3);
        ButtonBClickedDelegate?.Invoke();
        BlockUIClick();
    }
}
