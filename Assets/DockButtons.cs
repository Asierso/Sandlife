using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DockButtons : MonoBehaviour
{
    public GameObject canvas;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(canvas.GetComponent<Controler>().focusObject.GetComponent<Focus>().show == true) gameObject.GetComponent<Button>().interactable = false; else gameObject.GetComponent<Button>().interactable = true;
    }
}
