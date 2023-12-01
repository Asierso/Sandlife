using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public partial class Controler : MonoBehaviour
{
    //Too important global vars (NOT MODIFY OR DELETE ANYTHING)
    public GameObject Cube; //GameObject to clone Cubes in specified pos
    public Camera cam;
    public Canvas canvas;
    public Image selection; //Selector
    public Sprite[] BlockSprites; //Resources of blocks skins
    public List<GameObject> cubeObjects = new List<GameObject>(); //Global list of block objects to save
    public List<GameObject> particlesObjects = new List<GameObject>(); //Global list of particles summoned
    public List<GameObject> temporalObjects = new List<GameObject>(); //Global list of tmp objects
    public List<GameObject> fluidsObjects = new List<GameObject>(); //Global list of tmp FLUIDS
    public GameObject musicControler;
    public Dictionary<string,string> var = new Dictionary<string, string>(); //All command vars
    public GameObject focusObject;
    public GameObject destroyParticles;
    public List<GameObject> vortexA = new List<GameObject>(); //Recieve vortex
    public List<GameObject> vortexB = new List<GameObject>(); //Sender vortex
    public int lastVortexValue = 0; //Cte of vortex to instance
    int blockID = 0; 

    //Called from grid to put block in x,y position
    #region BlockPlacement
    void Update()
    {
        //Camera adjustment to screen size
        cam.GetComponent<Camera>().orthographicSize = Screen.height  / 2.0f;
    }

    public void Click(GameObject self)
    {
        if (gameObject.GetComponent<UI>().blockOpened == false && gameObject.GetComponent<UI>().eraseUsing == false && gameObject.GetComponent<SaveLoad>().loadOpened == false && gameObject.GetComponent<UI>().blockView == false && gameObject.GetComponent<Command>().loadOpened == false && gameObject.GetComponent<Settings>().blockSettings == false && gameObject.GetComponent<Online>().blockOnline == false && gameObject.GetComponent<Computers>().blockComputer == false && canvas.GetComponent<Trophies>().blockTrophies == false && canvas.GetComponent<Message>().blockAlert == false && canvas.GetComponent<Profile>().blockProfile == false && canvas.GetComponent<Piano>().blockOpened == false)
        {
            //Achieves modifier
            PlayerPrefs.SetInt("blocksPutted", PlayerPrefs.GetInt("blocksPutted") + 1);
            PlayerPrefs.SetInt("blocksCount", PlayerPrefs.GetInt("blocksCount") + 1);

            //Achievements of special blocks
            if (blockID == 35) PlayerPrefs.SetInt("spongeCount", PlayerPrefs.GetInt("spongeCount") + 1);
            if (blockID == 44) PlayerPrefs.SetInt("uraniumCount", PlayerPrefs.GetInt("uraniumCount") + 1);

            //Blockput
            GameObject cube = Instantiate(Cube, new Vector3(self.transform.position.x, self.transform.position.y, Cube.transform.position.z), Cube.transform.rotation);
            cube.GetComponent<SpriteRenderer>().sprite = BlockSprites[blockID];
            cube.GetComponent<Block>().blockID = blockID;
            cubeObjects.Add(cube);
        }
    }
    //Setter of current blockID spawn id
    public void SelectionControl(Button sender)
    {
        selection.GetComponent<Image>().sprite = sender.image.sprite;
        musicControler.GetComponent<Sounds>().PlaySound(3);
    }
    public void SelectionSetID(int blockID)
    {
        this.blockID = blockID;
        musicControler.GetComponent<Sounds>().PlaySound(3);
        PlayerPrefs.SetInt("blocksChanged",PlayerPrefs.GetInt("blocksChanged") + 1);
    }
    public void ChangeSelectionIcon(int blockID)
    {
        selection.GetComponent<Image>().sprite = gameObject.GetComponent<MenuSearch>().buttons[blockID].GetComponent<Image>().sprite;
    }
    #endregion
}
