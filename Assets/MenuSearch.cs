using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MenuSearch : MonoBehaviour
{
    //Public vars of elements to make search
    public GameObject input;
    public List<GameObject> buttons;
    public GameObject container;

    //Dictionary of the name of all block in Search tab (ordered by IndexNum) in a list
    public List<string> objects;

    //Other
    private int firstMatch = -1;
    private Vector2 mainPostion;
    #region UpdateDictionary
    private void Start()
    {
        mainPostion = container.transform.localPosition;
        Transform[] buttonTransformComponents = container.GetComponentsInChildren<Transform>();
        buttonTransformComponents.ToList().ForEach((obj) => { try { if (obj.GetComponent<Button>().enabled == true) { buttons.Add(obj.gameObject); objects.Add(obj.gameObject.name); } } catch { } });    
    }
    #endregion

    //To make the dynamic search
    void Update()
    {
        string text = input.GetComponent<InputField>().text;
        int i = 0;
        if(text != "")
        {
            foreach (string obj in objects)
            {
                if (!obj.ToLower().StartsWith(text.ToLower()))
                {
                    buttons[i].GetComponent<Image>().color = new Color32(128, 128, 128,255);
                }
                else
                {
                    buttons[i].GetComponent<Image>().color = new Color32(255, 255, 255, 255);
                    if(firstMatch == -1)
                    {
                        firstMatch = i;

                        //Modify PosY of container
                        container.transform.localPosition = mainPostion;
                        container.transform.localPosition = new Vector2(container.transform.localPosition.x, -buttons[i].transform.localPosition.y - 50);
                    }
                }
                i++;
            } 
        }
        else
        {
            buttons.ForEach(refe => refe.GetComponent<Image>().color = new Color32(255, 255, 255, 255));
        }
        
    }
    public void UpdateBufferPosition()
    {
        firstMatch = -1;
    }
}
