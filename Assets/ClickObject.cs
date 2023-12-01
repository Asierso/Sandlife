using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ClickObject : MonoBehaviour
{
    /*These GameObject is for control each grid trigger element to
     * spawn a block in each position
     * Named in game "Space"
     */

    //Canvas gameObject var
    public Canvas canvas;

    //Detect click and call method Click of Controler
    #region ClickTriggerDetection
    private void OnMouseDown()
    {
        if (Input.touchCount == 0)
        {
            canvas.GetComponent<Controler>().Click(gameObject);
        }    
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        /*
        try
        {
            if (collision.transform.tag == "Pointer")
            {
                var gamepad = Gamepad.current;
                if (gamepad.rightTrigger.isPressed)
                {
                    canvas.GetComponent<Controler>().Click(gameObject);
                }
            }
        }
        catch
        {

        }*/
    }
    private void OnMouseEnter()
    {
        if (SystemInfo.operatingSystemFamily == OperatingSystemFamily.Windows) //Windows
        {
            if (Input.GetMouseButton(0))
            {
                if (canvas.GetComponent<Grid>().gridObject.Contains(gameObject))
                {
                    canvas.GetComponent<Controler>().Click(gameObject);
                }
            }
            
        }
        else //Android
        {
            if (Input.touchCount == 1)
            {
                Input.GetTouch(0);
                if (canvas.GetComponent<Grid>().gridObject.Contains(gameObject))
                {
                    canvas.GetComponent<Controler>().Click(gameObject);
                }

            }
            else if (Input.GetMouseButtonDown(0))
            {
                if (canvas.GetComponent<Grid>().gridObject.Contains(gameObject))
                {
                    canvas.GetComponent<Controler>().Click(gameObject);
                }
            }
        }

        
    }
        
    #endregion
}
