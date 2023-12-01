using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class Hotkeys : MonoBehaviour
{
    //Key delay
    float timer = 0f;

    public bool block = false;

    //Cursor (unused)
    public GameObject pointer;
    private Vector3 pointerCoords;
    private void Start()
    {
        //pointerCoords = new Vector3(Screen.width / 2, Screen.height / 2, -100);
    }
    // Update is called once per frame
    void Update()
    {
        GameObject canvas = gameObject;

        /*
        var gamepad = Gamepad.current;
        try
        {
            Pointer Suprimed
            if (gameObject.GetComponent<UI>().blockOpened == false && gameObject.GetComponent<UI>().eraseUsing == false && gameObject.GetComponent<SaveLoad>().loadOpened == false && gameObject.GetComponent<UI>().blockView == false && gameObject.GetComponent<Command>().loadOpened == false && gameObject.GetComponent<Settings>().blockSettings == false && gameObject.GetComponent<Online>().blockOnline == false && gameObject.GetComponent<Computers>().blockComputer == false && canvas.GetComponent<Trophies>().blockTrophies == false && canvas.GetComponent<Message>().blockAlert == false && canvas.GetComponent<Profile>().blockProfile == false && canvas.GetComponent<Piano>().blockOpened == false)
            {
                if (gamepad.leftStick.left.isPressed)
                {
                    if (pointerCoords.x > 0)
                    {
                        pointerCoords = new Vector3(pointerCoords.x - 4.5f, pointerCoords.y, -100);

                    }
                    if (SystemInfo.operatingSystemFamily == OperatingSystemFamily.Windows && Cursor.visible == true)
                    {
                        Cursor.lockState = CursorLockMode.Locked;
                        Cursor.visible = false;
                    }
                }
                if (gamepad.leftStick.right.isPressed)
                {
                    if (pointerCoords.x < Screen.width)
                    {
                        pointerCoords = new Vector3(pointerCoords.x + 4.5f, pointerCoords.y, -100);
                    }
                    if (SystemInfo.operatingSystemFamily == OperatingSystemFamily.Windows && Cursor.visible == true)
                    {
                        Cursor.lockState = CursorLockMode.Locked;
                        Cursor.visible = false;
                    }
                }
                if (gamepad.leftStick.down.isPressed)
                {
                    if (pointerCoords.y > 0)
                    {
                        pointerCoords = new Vector3(pointerCoords.x, pointerCoords.y - 4.5f, -100);
                    }
                    if (SystemInfo.operatingSystemFamily == OperatingSystemFamily.Windows && Cursor.visible == true)
                    {
                        Cursor.lockState = CursorLockMode.Locked;
                        Cursor.visible = false;
                    }
                }
                if (gamepad.leftStick.up.isPressed)
                {
                    if (pointerCoords.y < Screen.height)
                    {
                        pointerCoords = new Vector3(pointerCoords.x, pointerCoords.y + 4.5f, -100);
                    }
                    if (SystemInfo.operatingSystemFamily == OperatingSystemFamily.Windows && Cursor.visible == true)
                    {
                        Cursor.lockState = CursorLockMode.Locked;
                        Cursor.visible = false;
                    }
                }
            }
                if (timer > 0.2f)
                {

                    if (gamepad == null || gamepad.enabled == false)
                    {
                        pointer.transform.position = new Vector3(-1000, -1000, -100);
                        if (SystemInfo.operatingSystemFamily == OperatingSystemFamily.Windows)
                        {
                            Cursor.lockState = CursorLockMode.None;
                            // Show the cursor
                            Cursor.visible = true;
                        }
                    }
                    else
                    {
                        pointer.transform.position = pointerCoords;
                    }
                
                    IEnumerator asyncDetect()
                    {
                    try
                    {
                        if (gamepad.buttonNorth.isPressed)
                        {
                            gameObject.GetComponent<UI>().BlockUIClick();
                            timer = 0f;
                        }
                        if (gamepad.buttonEast.isPressed)
                        {
                            gameObject.GetComponent<UI>().View();
                            timer = 0f;
                        }
                        if (gamepad.buttonWest.isPressed)
                        {
                            gameObject.GetComponent<Dock>().DockUI();
                            timer = 0f;
                        }
                    }
                    catch
                    {

                    }
                        yield return new WaitForEndOfFrame();
                    }
                    StartCoroutine(asyncDetect());
                }
        }
        catch
        {

        }*/


        if (timer < 1)
        {
            timer += Time.deltaTime;
        }
        if (SystemInfo.operatingSystemFamily == OperatingSystemFamily.Windows && block == false) //Only pc
        {
            if (timer > 0.2f)
            {
                //Menu
                if (gameObject.GetComponent<UI>().blockOpened == false && gameObject.GetComponent<SaveLoad>().loadOpened == false && gameObject.GetComponent<Command>().loadOpened == false && gameObject.GetComponent<Settings>().blockSettings == false && gameObject.GetComponent<Online>().blockOnline == false && gameObject.GetComponent<Computers>().blockComputer == false && canvas.GetComponent<Trophies>().blockTrophies == false && canvas.GetComponent<Message>().blockAlert == false && canvas.GetComponent<Profile>().blockProfile == false && canvas.GetComponent<Piano>().blockOpened == false)
                {
                    if (Input.GetKey(KeyCode.E))
                    {
                        gameObject.GetComponent<UI>().BlockUIClick();
                        timer = 0f;
                    }
                    if (Input.GetKey(KeyCode.D))
                    {
                        gameObject.GetComponent<UI>().EraseBlock();
                        timer = 0f;
                    }
                    if (Input.GetKey(KeyCode.R))
                    {
                        gameObject.GetComponent<UI>().musicControler.GetComponent<Sounds>().PlaySound(3);
                        if (PlayerPrefs.GetInt("fastDialogs") == 1)
                        {
                            gameObject.GetComponent<Message>().ShowAlert("Warning", "Do you want to delete\nall the blocks?", "Yes", "No", new System.Action(() => { gameObject.GetComponent<UI>().musicControler.GetComponent<Sounds>().PlaySound(3); gameObject.GetComponent<UI>().EraseAll(); }), new System.Action(() => { gameObject.GetComponent<UI>().musicControler.GetComponent<Sounds>().PlaySound(3); }));
                        }
                        else
                        {
                            gameObject.GetComponent<UI>().EraseAll();
                        }

                        timer = 0f;
                    }
                    if (Input.GetKey(KeyCode.S))
                    {
                        gameObject.GetComponent<SaveLoad>().SaveChangesBtn();
                        timer = 0f;
                    }
                    if (Input.GetKey(KeyCode.L))
                    {
                        gameObject.GetComponent<SaveLoad>().LoadUIClick();
                        timer = 0f;
                    }
                    if (Input.GetKey(KeyCode.O))
                    {
                        gameObject.GetComponent<Online>().BlockUIClick();
                        timer = 0f;
                    }
                    if (Input.GetKey(KeyCode.C))
                    {
                        gameObject.GetComponent<Settings>().BlockUIClick();
                        timer = 0f;
                    }
                    if (Input.GetKey(KeyCode.T))
                    {
                        gameObject.GetComponent<Trophies>().BlockUIClick();
                        timer = 0f;
                    }
                }
                //Other
                if (Input.GetKey(KeyCode.X))
                {
                    gameObject.GetComponent<Dock>().DockUI();
                    timer = 0f;
                }
                if (Input.GetKey(KeyCode.Z))
                {
                    gameObject.GetComponent<UI>().View();
                    timer = 0f;
                }
                if (Input.GetKey(KeyCode.Q))
                {
                    gameObject.GetComponent<UI>().ExitBtn();
                    timer = 0f;
                }

                //Camera movement (Not needed timeout)
                if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    gameObject.GetComponent<UI>().HoldMoveCameraLeft();
                    
                }
                if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    gameObject.GetComponent<UI>().HoldMoveCameraRight();
                    
                }
                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    gameObject.GetComponent<UI>().HoldMoveCameraUp();
                    
                }
                if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    gameObject.GetComponent<UI>().HoldMoveCameraDown();
                    
                }
                if(Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.DownArrow) || Input.GetKeyUp(KeyCode.RightArrow))
                {
                    gameObject.GetComponent<UI>().ReleaseButton();
                    
                }


                //Inventory
                if (gameObject.GetComponent<UI>().blockOpened == true)
                {
                    if (Input.GetKey(KeyCode.Escape))
                    {
                        gameObject.GetComponent<UI>().BlockUIClick();
                        timer = 0f;
                    }
                    if (Input.GetKey(KeyCode.Alpha1))
                    {
                        gameObject.GetComponent<UI>().ClickedTag(0);
                        timer = 0f;
                    }
                    if (Input.GetKey(KeyCode.Alpha2))
                    {
                        gameObject.GetComponent<UI>().ClickedTag(1);

                        timer = 0f;
                    }
                    if (Input.GetKey(KeyCode.Alpha3))
                    {
                        gameObject.GetComponent<UI>().ClickedTag(2);
                        timer = 0f;
                    }
                    if (Input.GetKey(KeyCode.Alpha4))
                    {
                        gameObject.GetComponent<UI>().ClickedTag(3);
                        timer = 0f;
                    }

                }

                //Online
                if (gameObject.GetComponent<Online>().blockOnline == true)
                {
                    if (Input.GetKey(KeyCode.S))
                    {
                        gameObject.GetComponent<Online>().SearchFriend();
                        timer = 0f;
                    }
                    if (Input.GetKey(KeyCode.Escape))
                    {
                        gameObject.GetComponent<Online>().BlockUIClick();
                        timer = 0f;
                    }
                }

                //Message
                if (gameObject.GetComponent<Message>().blockAlert == true)
                {
                    if (Input.GetKey(KeyCode.Y))
                    {
                        gameObject.GetComponent<Message>().OptionAClicked();
                        timer = 0f;
                    }
                    if (Input.GetKey(KeyCode.N))
                    {
                        gameObject.GetComponent<Message>().OptionBClicked();
                        timer = 0f;
                    }
                    if (Input.GetKey(KeyCode.Escape))
                    {
                        gameObject.GetComponent<Message>().BlockUIClick();
                        timer = 0f;
                    }


                }

                //Save and load
                if (gameObject.GetComponent<SaveLoad>().loadOpened == true)
                {
                    if (Input.GetKey(KeyCode.Escape))
                    {
                        gameObject.GetComponent<SaveLoad>().LoadUIClick();
                        timer = 0f;
                    }
                    if (Input.GetKey(KeyCode.R))
                    {
                        gameObject.GetComponent<Online>().RefreshSaveLoadMenu();
                        timer = 0f;
                    }
                    if (Input.GetKey(KeyCode.C))
                    {
                        gameObject.GetComponent<SaveLoad>().CustomBgBtn();
                        timer = 0f;
                    }
                    if (Input.GetKey(KeyCode.N))
                    {
                        gameObject.GetComponent<SaveLoad>().CreateWorld();
                        timer = 0f;
                    }
                    if (Input.GetKey(KeyCode.B))
                    {
                        gameObject.GetComponent<Gamebox>().GameboxOptionsClick();
                        timer = 0f;
                    }
                }
                //Settings
                if (gameObject.GetComponent<Settings>().blockSettings == true && Input.GetKey(KeyCode.Escape))
                {
                    gameObject.GetComponent<Settings>().BlockUIClick();
                    timer = 0f;
                }
                //Trophies
                if (gameObject.GetComponent<Trophies>().blockTrophies == true && Input.GetKey(KeyCode.Escape))
                {
                    gameObject.GetComponent<Trophies>().BlockUIClick();
                    timer = 0f;
                }

                /*Note Menu
                if (gameObject.GetComponent<Piano>().blockOpened == true && Input.GetKey(KeyCode.Escape))
                {
                    gameObject.GetComponent<Piano>().BlockUIClick(null);
                    timer = 0f;
                }

                //Command
                if (gameObject.GetComponent<Command>().loadOpened == true && Input.GetKey(KeyCode.Escape))
                {
                    gameObject.GetComponent<Command>().LoadUIClick(null);
                    timer = 0f;
                }*/
            }
        }
    }
    public void LockHkey()
    {
        gameObject.GetComponent<Hotkeys>().block = true;
    }

    public void UnlockHkey()
    {
        gameObject.GetComponent<Hotkeys>().block = false;
    }
        
}
