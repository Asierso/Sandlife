using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    //Bools of show and hide ui parts
    public bool blockOpened = true;
    public bool eraseUsing = false;
    public bool blockView = false;
    private int openedId = 3;

    //Resources for buttons
    public Sprite[] aminationEraser = new Sprite[2];
    public Sprite[] aminationView = new Sprite[2];
    public Image eraser;
    public Image viewer;
    public GameObject musicControler;
    public GameObject[] inventory;
    public GameObject[] viewButtons;

    //Position buffer list and vars
    private List<Vector2> inventoryPosition = new List<Vector2>();
    private List<Vector2> viewButtonsPosition = new List<Vector2>();
    public Vector2 decorationVct;
    public Vector2 energyVct;
    public Vector2 randomVct;
    public Vector2 allVct;

    //Gameobjects of all categories of inventory
    public GameObject decorationScroll;
    public GameObject decorationButton;
    public GameObject energyScroll;
    public GameObject energyButton;
    public GameObject randomScroll;
    public GameObject randomButton;
    public GameObject allScroll;
    public GameObject allButton;
    public GameObject inventoryBuffer;
    public Sprite[] inventoryBufferImage;

    //Keycups gameobject
    public List<GameObject> KeyCups;

    //Textures for movement buttons
    public Sprite[] releasedMoveButtonUDLR;
    public Sprite[] holdMoveButtonUDLR;
    public Image[] buttonsUDLR;

    //Enums
    public enum CameraDirections { Up, Down, Left, Right, None };
    private CameraDirections current = CameraDirections.None;

    //Inventory controler
    #region Inventory
    void Start()
    {
        //Add mainpos to list Vt2
        foreach (GameObject inventoryComponent in inventory)
        {
            inventoryPosition.Add(inventoryComponent.transform.position);
            inventoryComponent.transform.position = new Vector2(-1000, -1000);
        }

        //Add mainpos to list Vt2 of viewButtons
        foreach (GameObject handle in viewButtons)
        {
            viewButtonsPosition.Add(handle.transform.position);
            handle.transform.position = new Vector2(-1000, -1000);
        }


        //Inventory tabs load
        decorationVct = decorationScroll.transform.position;
        decorationScroll.transform.position = new Vector2(-1000, -1000);
        energyVct = energyScroll.transform.position;
        energyScroll.transform.position = new Vector2(-1000, -1000);
        randomVct = randomScroll.transform.position;
        randomScroll.transform.position = new Vector2(-1000, -1000);
        allVct = allScroll.transform.position;
        allScroll.transform.position = new Vector2(-1000, -1000);

        //Hide menu
        blockOpened = false;
    }

    //Get a spec clicked
    public void ClickedTag(int id)
    {
        musicControler.GetComponent<Sounds>().PlaySound(3);
        openedId = id;
        ChangePanel();
    }

    //Change panel of inventory to another one
    void ChangePanel()
    {
        inventoryBuffer.GetComponent<Image>().sprite = inventoryBufferImage[openedId];
        switch (openedId)
        {
            case 0:
                decorationScroll.transform.position = decorationVct;
                energyScroll.transform.position = new Vector2(-1000, -1000);
                randomScroll.transform.position = new Vector2(-1000, -1000);
                allScroll.transform.position = new Vector2(-1000, -1000);
                break;
            case 1:
                decorationScroll.transform.position = new Vector2(-1000, -1000);
                energyScroll.transform.position = energyVct;
                randomScroll.transform.position = new Vector2(-1000, -1000);
                allScroll.transform.position = new Vector2(-1000, -1000);
                break;
            case 2:
                decorationScroll.transform.position = new Vector2(-1000, -1000);
                energyScroll.transform.position = new Vector2(-1000, -1000);
                randomScroll.transform.position = randomVct;
                allScroll.transform.position = new Vector2(-1000, -1000);
                break;
            case 3:
                decorationScroll.transform.position = new Vector2(-1000, -1000);
                energyScroll.transform.position = new Vector2(-1000, -1000);
                randomScroll.transform.position = new Vector2(-1000, -1000);
                allScroll.transform.position = allVct;
                break;
        }
    }

    public void Close()
    {
        blockOpened = true;
        BlockUIClick();
    }
    //Show and hide Selector
    public void BlockUIClick()
    {
        musicControler.GetComponent<Sounds>().PlaySound(3);
        if (blockOpened == true)
        {
            foreach (GameObject inventoryComponent in inventory)
            {
                inventoryComponent.transform.position = new Vector2(-1000, -1000);
            }
            decorationScroll.transform.position = new Vector2(-1000, -1000);
            energyScroll.transform.position = new Vector2(-1000, -1000);
            randomScroll.transform.position = new Vector2(-1000, -1000);
            allScroll.transform.position = new Vector2(-1000, -1000);
            blockOpened = false;
            //gameObject.GetComponent<Controler>().focusObject.GetComponent<Focus>().show = false;
        }
        else
        {
            int i = 0;
            foreach (GameObject inventoryComponent in inventory)
            {
                inventoryComponent.transform.position = inventoryPosition[i];
                i++;
            }
            if (SystemInfo.operatingSystemFamily != OperatingSystemFamily.Windows)
            {
                foreach (GameObject key in KeyCups)
                {
                    key.transform.position = new Vector2(-1000, -1000);
                }
            }

            ChangePanel();
            blockOpened = true;
        }
    }
    #endregion

    private void Update()
    {
        MoveCamera(current);
    }


    //Buttons action of dock
    #region DockButtons
    public void EraseBlock()
    {
        musicControler.GetComponent<Sounds>().PlaySound(3);
        if (eraseUsing == true)
        {
            eraseUsing = false;
            eraser.sprite = aminationEraser[0];
        }
        else
        {
            eraseUsing = true;
            eraser.sprite = aminationEraser[1];
        }
    }
    public void EraseBtn()
    {

        musicControler.GetComponent<Sounds>().PlaySound(3);
        if (PlayerPrefs.GetInt("fastDialogs") == 1)
        {
            gameObject.GetComponent<Message>().ShowAlert("Warning", "Do you want to delete\nall the blocks?", "Yes", "No", new System.Action(() => { musicControler.GetComponent<Sounds>().PlaySound(3); EraseAll(); }), new System.Action(() => { musicControler.GetComponent<Sounds>().PlaySound(3); }));
        }
        else
        {
            EraseAll();
        }
    }
    public void EraseAll()
    {

        foreach (GameObject toDelete in gameObject.GetComponent<Controler>().cubeObjects.ToArray())
        {
            Destroy(toDelete);
        }
        foreach (GameObject toDelete in gameObject.GetComponent<Controler>().particlesObjects.ToArray())
        {
            Destroy(toDelete);
        }
        foreach (GameObject toDelete in gameObject.GetComponent<Controler>().temporalObjects.ToArray())
        {
            try { Destroy(toDelete); } catch { }
        }
        gameObject.GetComponent<Controler>().cubeObjects.Clear();
        gameObject.GetComponent<Controler>().particlesObjects.Clear();
        gameObject.GetComponent<Controler>().temporalObjects.Clear();
        gameObject.GetComponent<Controler>().vortexA.Clear();
        gameObject.GetComponent<Controler>().vortexB.Clear();
        gameObject.GetComponent<Controler>().lastVortexValue = 0;
    }

    public void View()
    {
        musicControler.GetComponent<Sounds>().PlaySound(3);
        if (blockView == true)
        {
            blockView = false;
            viewer.sprite = aminationView[1];
            foreach (GameObject handle in viewButtons)
            {
                handle.transform.position = new Vector2(-1000, -1000);
            }

        }
        else
        {
            blockView = true;
            eraseUsing = false;
            eraser.sprite = aminationEraser[0];
            viewer.sprite = aminationView[0];
            int i = 0;
            foreach (GameObject handle in viewButtons)
            {
                handle.transform.position = viewButtonsPosition[i];
                i++;
            }
        }
    }
    public void ExitBtn()
    {
        musicControler.GetComponent<Sounds>().PlaySound(3);
        if (PlayerPrefs.GetInt("fastDialogs") == 1)
        {
            gameObject.GetComponent<Message>().ShowAlert("Warning", "Are you sure\nyou want to quit the game?", "Yes", "No", new System.Action(() => { musicControler.GetComponent<Sounds>().PlaySound(3); Application.Quit(); }), new System.Action(() => { musicControler.GetComponent<Sounds>().PlaySound(3); }));

        }
        else
        {
            Application.Quit();
        }
    }
    public void MoveCamera(CameraDirections camdir)
    {
        var cam = gameObject.GetComponent<Controler>().cam;
        switch (camdir)
        {
            case CameraDirections.Up:
                buttonsUDLR[0].sprite = holdMoveButtonUDLR[0];
                buttonsUDLR[1].sprite = releasedMoveButtonUDLR[1];
                buttonsUDLR[2].sprite = releasedMoveButtonUDLR[2];
                buttonsUDLR[3].sprite = releasedMoveButtonUDLR[3];
                if (cam.transform.localPosition.y < 0)
                {
                    cam.transform.position = new Vector3(cam.transform.position.x, cam.transform.position.y + 5, cam.transform.position.z);
                }
                break;
            case CameraDirections.Left:
                buttonsUDLR[0].sprite = releasedMoveButtonUDLR[0];
                buttonsUDLR[1].sprite = releasedMoveButtonUDLR[1];
                buttonsUDLR[2].sprite = holdMoveButtonUDLR[2];
                buttonsUDLR[3].sprite = releasedMoveButtonUDLR[3];
                if (cam.transform.localPosition.x > 0)
                {
                    cam.transform.position = new Vector3(cam.transform.position.x - 5, cam.transform.position.y, cam.transform.position.z);
                }
                break;
            case CameraDirections.Right:
                buttonsUDLR[0].sprite = releasedMoveButtonUDLR[0];
                buttonsUDLR[1].sprite = releasedMoveButtonUDLR[1];
                buttonsUDLR[2].sprite = releasedMoveButtonUDLR[2];
                buttonsUDLR[3].sprite = holdMoveButtonUDLR[3];
                if (cam.transform.localPosition.x < 500)
                {
                    cam.transform.position = new Vector3(cam.transform.position.x + 5, cam.transform.position.y, cam.transform.position.z);
                }
                break;
            case CameraDirections.Down:
                buttonsUDLR[0].sprite = releasedMoveButtonUDLR[0];
                buttonsUDLR[1].sprite = holdMoveButtonUDLR[1];
                buttonsUDLR[2].sprite = releasedMoveButtonUDLR[2];
                buttonsUDLR[3].sprite = releasedMoveButtonUDLR[3];
                if (cam.transform.localPosition.y > -500)
                {
                    cam.transform.position = new Vector3(cam.transform.position.x, cam.transform.position.y - 5, cam.transform.position.z);
                }
                break;
            case CameraDirections.None:
                buttonsUDLR[0].sprite = releasedMoveButtonUDLR[0];
                buttonsUDLR[1].sprite = releasedMoveButtonUDLR[1];
                buttonsUDLR[2].sprite = releasedMoveButtonUDLR[2];
                buttonsUDLR[3].sprite = releasedMoveButtonUDLR[3];
                break;
        }
    }



    #endregion

    //All voids to move camera with buttons
    #region CameraButtonsAction
    public void HoldMoveCameraUp() { current = CameraDirections.Up; musicControler.GetComponent<Sounds>().PlaySound(3); }
    public void HoldMoveCameraLeft() { current = CameraDirections.Left; musicControler.GetComponent<Sounds>().PlaySound(3); }
    public void HoldMoveCameraRight() { current = CameraDirections.Right; musicControler.GetComponent<Sounds>().PlaySound(3); }
    public void HoldMoveCameraDown() { current = CameraDirections.Down; musicControler.GetComponent<Sounds>().PlaySound(3); }
    public void ReleaseButton() => current = CameraDirections.None;
    #endregion
}
