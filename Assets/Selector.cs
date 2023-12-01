using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Selector : MonoBehaviour
{
    public int x3 = 0;
    public int y3 = 0;
    public int numb3 = 0;
    public int x2 = 0;
    public int y2 = 0;
    public int numb2 = 0;
    public int x1 = 0;
    public int y1 = 0;
    public int numb1 = 0;
    public int x0 = 0;
    public int y0 = 0;
    public int numb0 = 0;
    public GameObject allSelector;
    public GameObject rdnSelector;
    public GameObject engSelector;
    public GameObject decSelector;
    public GameObject btnBase;

    public GameObject allContainer;
    public GameObject rdnContainer;
    public GameObject engContainer;
    public GameObject decContainer;
    public List<GameObject> CatAll;
    public List<GameObject> CatRdn;
    public List<GameObject> CatEng;
    public List<GameObject> CatDec;
    private float cooldown = 0f;
    public int menuOpened = 3;
    private float desp = 75f; 
    // Start is called before the first frame update
    void Start()
    {
        
        try
        {
            Transform[] c3 = allContainer.GetComponentsInChildren<Transform>();
            Transform[] c2 = rdnContainer.GetComponentsInChildren<Transform>();
            Transform[] c1 = engContainer.GetComponentsInChildren<Transform>();
            Transform[] c0 = decContainer.GetComponentsInChildren<Transform>();
            c3.ToList().ForEach((obj) => { try { if (obj.GetComponent<Button>().enabled == true) { CatAll.Add(obj.gameObject); } } catch { } });
            c2.ToList().ForEach((obj) => { try { if (obj.GetComponent<Button>().enabled == true) { CatRdn.Add(obj.gameObject); } } catch { } });
            c1.ToList().ForEach((obj) => { try { if (obj.GetComponent<Button>().enabled == true) { CatEng.Add(obj.gameObject); } } catch { } });
            c0.ToList().ForEach((obj) => { try { if (obj.GetComponent<Button>().enabled == true) { CatDec.Add(obj.gameObject); } } catch { } });
        }
        catch
        {
            /*
            var c3 = allContainer.GetComponentsInParent(typeof(Button), true);
            var c2 = rdnContainer.GetComponentsInParent(typeof(Button), true);
            var c1 = engContainer.GetComponentsInParent(typeof(Button), true);
            var c0 = decContainer.GetComponentsInParent(typeof(Button), true);*/

            
        }
    }

    // Update is called once per frame
    void UpdateUnused()
    {
        var gamepad = Gamepad.current;
        try
        {
            if (cooldown <= -0.1f && gameObject.GetComponent<UI>().blockOpened == true)
        {
            if (gamepad.dpad.up.isPressed && menuOpened > 0)
            {
                menuOpened--;

                gameObject.GetComponent<UI>().ClickedTag(menuOpened);
                cooldown = 0.1f;
            }
            if (gamepad.dpad.down.isPressed && menuOpened < 3)
            {
                menuOpened++;

                gameObject.GetComponent<UI>().ClickedTag(menuOpened);
                cooldown = 0.1f;
            }
            if (gamepad.buttonSouth.isPressed)
            {
                switch(menuOpened)
                {
                    case 3: var objs3 = gameObject.GetComponent<MenuSearch>().objects; List<string> corrected3 = new List<string>(); objs3.ToList().ForEach((obj) => { corrected3.Add(obj.Trim(new char[] { ' ' })); }); CatAll[numb3].gameObject.GetComponent<Button>().onClick.Invoke(); /*gameObject.GetComponent<Controler>().SelectionSetID(corrected3.IndexOf(CatAll[numb3].GetComponent<Button>().name.ToLower()));*/break;
                    case 2: var objs2 = gameObject.GetComponent<MenuSearch>().objects; List<string> corrected2 = new List<string>(); objs2.ToList().ForEach((obj) => { corrected2.Add(obj.Trim(new char[] { ' ' })); });  CatRdn[numb2].gameObject.GetComponent<Button>().onClick.Invoke(); /*gameObject.GetComponent<Controler>().SelectionSetID(corrected2.IndexOf(CatAll[numb2].GetComponent<Button>().name.ToLower()));*/break;
                    case 1: var objs1 = gameObject.GetComponent<MenuSearch>().objects; List<string> corrected1 = new List<string>(); objs1.ToList().ForEach((obj) => { corrected1.Add(obj.Trim(new char[] { ' ' })); });  CatEng[numb1].gameObject.GetComponent<Button>().onClick.Invoke(); /*gameObject.GetComponent<Controler>().SelectionSetID(corrected1.IndexOf(CatAll[numb1].GetComponent<Button>().name.ToLower()));*/break;
                    case 0: var objs0 = gameObject.GetComponent<MenuSearch>().objects; List<string> corrected0 = new List<string>(); objs0.ToList().ForEach((obj) => { corrected0.Add(obj.Trim(new char[] { ' ' })); }); CatDec[numb0].gameObject.GetComponent<Button>().onClick.Invoke(); /*gameObject.GetComponent<Controler>().SelectionSetID(corrected0.IndexOf(CatAll[numb0].GetComponent<Button>().name.ToLower()));*/break;
                }
            }
        }

        
            switch (menuOpened)
            {
                case 3://Menu all
                    rdnSelector.transform.position = new Vector3(-1000, -1000, -100);
                    decSelector.transform.position = new Vector3(-1000, -1000, -100);
                    engSelector.transform.position = new Vector3(-1000, -1000, -100);

                    if (gamepad == null || gamepad.enabled == false)
                    {
                        allSelector.transform.position = new Vector3(-1000, -1000, -100);
                    }
                    else
                    {
                        try { allSelector.transform.position = new Vector2(CatAll[numb3].transform.position.x - 0.7f, CatAll[numb3].transform.position.y + 0.9f); } catch { }
                    }
                    if (cooldown <= 0f)
                    {
                        if (gamepad.leftStick.left.isPressed && numb3 > 0)
                        {
                            x3--;
                            numb3--;

                            if (x3 == -1)
                            {
                                x3 = 4;
                                y3--;
                                allContainer.transform.localPosition = new Vector2(allContainer.transform.localPosition.x, allContainer.transform.localPosition.y - desp);

                            }
                            cooldown = 0.1f;

                        }
                        if (gamepad.leftStick.right.isPressed && numb3 < CatAll.ToArray().Length - 1)
                        {
                            x3++;
                            numb3++;

                            if (x3 == 5)
                            {
                                x3 = 0;
                                y3++;
                                allContainer.transform.localPosition = new Vector2(allContainer.transform.localPosition.x, allContainer.transform.localPosition.y + desp);

                            }
                            cooldown = 0.1f;

                        }
                        /*
                        if(gamepad.leftStick.up.isPressed && numb3 > 4)
                        {
                            y3--;
                            allContainer.transform.localPosition = new Vector2(allContainer.transform.localPosition.x, allContainer.transform.localPosition.y - desp);
                            numb3 = numb3 - 5;
                            cooldown = 0.1f;
                        }

                        if (gamepad.leftStick.down.isPressed && numb3 < (int)CatAll.Count - 4)
                        {
                            y3++;
                            allContainer.transform.localPosition = new Vector2(allContainer.transform.localPosition.x, allContainer.transform.localPosition.y + desp);
                            numb3 = numb3 + 5;
                            cooldown = 0.1f;

                        }*/

                    }
                    break;
                case 2://Random all
                    allSelector.transform.position = new Vector3(-1000, -1000, -100);
                    decSelector.transform.position = new Vector3(-1000, -1000, -100);
                    engSelector.transform.position = new Vector3(-1000, -1000, -100);
                    if (gamepad == null || gamepad.enabled == false)
                    {
                        rdnSelector.transform.position = new Vector3(-1000, -1000, -100);
                    }
                    else
                    {
                        try { rdnSelector.transform.position = new Vector2(CatRdn[numb2].transform.position.x - 0.7f, CatRdn[numb2].transform.position.y + 0.9f); } catch { }
                    }
                    if (cooldown <= 0f)
                    {
                        if (gamepad.leftStick.left.isPressed && numb2 > 0)
                        {
                            x2--;
                            numb2--;
                            if (x2 == -1)
                            {
                                x2 = 4;
                                y2--;
                                rdnContainer.transform.localPosition = new Vector2(rdnContainer.transform.localPosition.x, rdnContainer.transform.localPosition.y - desp);
                            }
                            cooldown = 0.1f;

                        }
                        if (gamepad.leftStick.right.isPressed && numb2 < CatRdn.ToArray().Length - 1)
                        {
                            x2++;
                            numb2++;
                            if (x2 == 5)
                            {
                                x2 = 0;
                                y2++;
                                rdnContainer.transform.localPosition = new Vector2(rdnContainer.transform.localPosition.x, rdnContainer.transform.localPosition.y + desp);
                            }
                            cooldown = 0.1f;

                        }

                        /*
                        if (gamepad.leftStick.up.isPressed && numb2 > 4)
                        {
                            y2--;
                            rdnContainer.transform.localPosition = new Vector2(rdnContainer.transform.localPosition.x, rdnContainer.transform.localPosition.y - desp);
                            numb2 = numb2 - 5;
                            cooldown = 0.1f;
                        }

                        if (gamepad.leftStick.down.isPressed && numb2 < (int)CatRdn.Count - 4)
                        {
                            y2++;
                            rdnContainer.transform.localPosition = new Vector2(rdnContainer.transform.localPosition.x, rdnContainer.transform.localPosition.y + desp);
                            numb2 = numb2 + 5;
                            cooldown = 0.1f;

                        }*/
                    }
                    break;
                case 1://Random all
                    allSelector.transform.position = new Vector3(-1000, -1000, -100);
                    decSelector.transform.position = new Vector3(-1000, -1000, -100);
                    rdnSelector.transform.position = new Vector3(-1000, -1000, -100);
                    if (gamepad == null || gamepad.enabled == false)
                    {
                        engSelector.transform.position = new Vector3(-1000, -1000, -100);
                    }
                    else
                    {
                        try { engSelector.transform.position = new Vector2(CatEng[numb1].transform.position.x - 0.7f, CatEng[numb1].transform.position.y + 0.9f); } catch { }
                    }
                    if (cooldown <= 0f)
                    {
                        if (gamepad.leftStick.left.isPressed && numb1 > 0)
                        {
                            x1--;
                            numb1--;
                            if (x1 == -1)
                            {
                                x1 = 4;
                                y1--;
                                engContainer.transform.localPosition = new Vector2(engContainer.transform.localPosition.x, engContainer.transform.localPosition.y - desp);
                            }
                            cooldown = 0.1f;

                        }
                        if (gamepad.leftStick.right.isPressed && numb1 < CatEng.ToArray().Length - 1)
                        {
                            x1++;
                            numb1++;
                            if (x1 == 5)
                            {
                                x1 = 0;
                                y1++;
                                engContainer.transform.localPosition = new Vector2(engContainer.transform.localPosition.x, engContainer.transform.localPosition.y + desp);
                            }
                            cooldown = 0.1f;

                        }
                        /*
                        if (gamepad.leftStick.up.isPressed && numb1 > 4)
                        {
                            y1--;
                            engContainer.transform.localPosition = new Vector2(engContainer.transform.localPosition.x, engContainer.transform.localPosition.y - desp);
                            numb1 = numb1 - 5;
                            cooldown = 0.1f;
                        }

                        if (gamepad.leftStick.down.isPressed && numb1 < (int)CatEng.Count - 4)
                        {
                            y1++;
                            engContainer.transform.localPosition = new Vector2(engContainer.transform.localPosition.x, engContainer.transform.localPosition.y + desp);
                            numb1 = numb1 + 5;
                            cooldown = 0.1f;

                        }*/
                    }
                    break;
                case 0://Decoration
                    allSelector.transform.position = new Vector3(-1000, -1000, -100);
                    engSelector.transform.position = new Vector3(-1000, -1000, -100);
                    rdnSelector.transform.position = new Vector3(-1000, -1000, -100);
                    if (gamepad == null || gamepad.enabled == false)
                    {
                        decSelector.transform.position = new Vector3(-1000, -1000, -100);
                    }
                    else
                    {
                        try { decSelector.transform.position = new Vector2(CatDec[numb0].transform.position.x - 0.7f, CatDec[numb0].transform.position.y + 0.9f); } catch { }
                    }
                    if (cooldown <= 0f)
                    {
                        if (gamepad.leftStick.left.isPressed && numb0 > 0)
                        {
                            x0--;
                            numb0--;
                            if (x0 == -1)
                            {
                                x0 = 4;
                                y0--;
                                decContainer.transform.localPosition = new Vector2(decContainer.transform.localPosition.x, decContainer.transform.localPosition.y - desp);
                            }
                            cooldown = 0.1f;

                        }
                        if (gamepad.leftStick.right.isPressed && numb0 < CatDec.ToArray().Length - 1)
                        {
                            x0++;
                            numb0++;
                            if (x0 == 5)
                            {
                                x0 = 0;
                                y0++;
                                decContainer.transform.localPosition = new Vector2(decContainer.transform.localPosition.x, decContainer.transform.localPosition.y + desp);
                            }
                            cooldown = 0.1f;

                        }
                        /*
                        if (gamepad.leftStick.up.isPressed && numb0 > 4)
                        {
                            y0--;
                            decContainer.transform.localPosition = new Vector2(decContainer.transform.localPosition.x, decContainer.transform.localPosition.y - desp);
                            numb0 = numb0 - 5;
                            cooldown = 0.1f;
                        }

                        if (gamepad.leftStick.down.isPressed && numb0 < (int)CatDec.Count - 4)
                        {
                            y0++;
                            decContainer.transform.localPosition = new Vector2(decContainer.transform.localPosition.x, decContainer.transform.localPosition.y + desp);
                            numb0 = numb0 + 5;
                            cooldown = 0.1f;

                        }*/
                    }
                    break;
            }
       



        if (cooldown > -0.1f)
        { 
            cooldown -= Time.deltaTime;
        }
        }
        catch
        {

        }
    }
}
