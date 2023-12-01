using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;
using System.Linq;
using Newtonsoft.Json;
using RSG;
using Proyecto26;
using System.Timers;
using System.Threading;

public class SaveLoad : MonoBehaviour
{
    //Gameobjects needed to work
    public AudioSource musicControler;
    public GameObject scroll;
    public GameObject[] WorldLoadTemp;
    public GameObject content;
    public GameObject cube;
    public GameObject[] loadui;

    //Hid
    public bool loadOpened = true;
    private List<Vector2> loadUIPosition = new List<Vector2>();

    //Resources
    public Image saveIcon;
    public Sprite[] saveIconStates;
    
    //Temporal state wars
    List<GameObject> temp = new List<GameObject>();
    public string currentFile = "";
    List<GameObject> worldSingle;
    bool stopruntime = false;
    bool saving = false;
    public List<GameObject> objectsSaved;
    private GameObject infoCube; //Cube with all info

    //Other
    private Vector2 contentRtOriginSizeDelta;
    public Color textColor;

    //KeyCups
    public List<GameObject> KeyCups;

    //Charge all worlds in boot
    #region Main Charge
    void Start()
    {
        contentRtOriginSizeDelta = content.GetComponent<RectTransform>().sizeDelta;
       
        gameObject.GetComponent<Controler>().cam.clearFlags = CameraClearFlags.SolidColor;
        CustomBgColorRandom();

        foreach (GameObject loadComponent in loadui)
        {
            loadUIPosition.Add(loadComponent.transform.position);
            loadComponent.transform.position = new Vector2(-1000, -1000);
        }
        //Operations
        if (!Directory.Exists(Application.persistentDataPath + "/Saves"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/Saves");
            Debug.Log(Application.persistentDataPath + "/Saves");
            File.WriteAllText(Application.persistentDataPath + "/Saves/Void world.slife", "[]");
        }
        int filesNumber = 0;
        float spaces = 0;

        PlayerPrefs.SetInt("worlds", 0);
        foreach (string files in Directory.EnumerateFiles(Application.persistentDataPath + "/Saves", "*.slife"))
        {

            worldSingle = new List<GameObject>();
            worldSingle.Add(Instantiate(WorldLoadTemp[0], new Vector3(WorldLoadTemp[0].transform.position.x, WorldLoadTemp[0].transform.position.y + spaces, WorldLoadTemp[0].transform.position.z), WorldLoadTemp[0].transform.rotation, content.transform));
            worldSingle.Add(Instantiate(WorldLoadTemp[1], new Vector3(WorldLoadTemp[1].transform.position.x, WorldLoadTemp[1].transform.position.y + spaces, WorldLoadTemp[0].transform.position.z), WorldLoadTemp[1].transform.rotation, content.transform));
            worldSingle.Add(Instantiate(WorldLoadTemp[2], new Vector3(WorldLoadTemp[2].transform.position.x, WorldLoadTemp[2].transform.position.y + spaces, WorldLoadTemp[0].transform.position.z), WorldLoadTemp[2].transform.rotation, content.transform));
            worldSingle.Add(Instantiate(WorldLoadTemp[3], new Vector3(WorldLoadTemp[3].transform.position.x, WorldLoadTemp[3].transform.position.y + spaces, WorldLoadTemp[0].transform.position.z), WorldLoadTemp[3].transform.rotation, content.transform));
            worldSingle.Add(Instantiate(WorldLoadTemp[4], new Vector3(WorldLoadTemp[4].transform.position.x, WorldLoadTemp[4].transform.position.y + spaces, WorldLoadTemp[0].transform.position.z), WorldLoadTemp[4].transform.rotation, content.transform));
            worldSingle.Add(Instantiate(WorldLoadTemp[5], new Vector3(WorldLoadTemp[5].transform.position.x, WorldLoadTemp[5].transform.position.y + spaces, WorldLoadTemp[0].transform.position.z), WorldLoadTemp[5].transform.rotation, content.transform));
            worldSingle[1].GetComponent<InputField>().text = Path.GetFileNameWithoutExtension(files);
            worldSingle[5].GetComponent<TMPro.TextMeshProUGUI>().text = "By you";
            worldSingle[1].name = files;
            //worldSingle[1].GetComponent<TMPro.TextMeshProUGUI>().text = Path.GetFileNameWithoutExtension(files);
            worldSingle[2].name = files;
            worldSingle[3].name = files;
            worldSingle[4].name = files;
            worldSingle[5].name = files;


            if (Path.GetFileNameWithoutExtension(files).Contains(";"))
            {
                string[] creatorAndName = Path.GetFileNameWithoutExtension(files).Split(';');
                worldSingle[5].GetComponent<TMPro.TextMeshProUGUI>().text = "By " + creatorAndName[0];
                string fname = "";
                var nameBuffer = creatorAndName.ToList();
                nameBuffer.Remove(nameBuffer[0]);
                nameBuffer.ForEach(obj => fname += obj);
                worldSingle[1].GetComponent<InputField>().text = fname;
            }

            temp.AddRange(worldSingle.ToArray());
            filesNumber++;
            spaces = spaces - Screen.height / 3 + WorldLoadTemp[0].GetComponent<RectTransform>().rect.height + (Screen.height / 16);
            PlayerPrefs.SetInt("worlds", PlayerPrefs.GetInt("worlds") + 1);

            RectTransform rt = content.GetComponent<RectTransform>();

            //rt.sizeDelta = new Vector2(rt.sizeDelta.x, rt.sizeDelta.y + WorldLoadTemp[0].GetComponent<RectTransform>().sizeDelta.y * 2f);
            rt.sizeDelta = new Vector2(rt.sizeDelta.x, rt.sizeDelta.y + WorldLoadTemp[0].GetComponent<RectTransform>().sizeDelta.y + ( WorldLoadTemp[0].GetComponent<RectTransform>().sizeDelta.y / 3));


            //StartCoroutine(LoadImg(worldSingle[4], files));
        }


        //UI
        LoadUIClick();
    }
    #endregion
    #region WorldListControler
    /*
    public IEnumerator UpdateLists()
    {
        yield return new WaitForEndOfFrame();
        int filesNumber = 0;
        float spaces = 0;
        foreach (GameObject single in temp)
        {
            Destroy(single);
        }
        temp.Clear();
        PlayerPrefs.SetInt("worlds", 0);
        content.GetComponent<RectTransform>().sizeDelta = contentRtOriginSizeDelta;
        foreach (string files in Directory.EnumerateFiles(Application.persistentDataPath + "/Saves", "*.slife"))
        {
            Debug.Log(files);
            worldSingle.Clear();
            worldSingle.Add(Instantiate(WorldLoadTemp[0], new Vector3(WorldLoadTemp[0].transform.position.x, WorldLoadTemp[0].transform.position.y + spaces, WorldLoadTemp[0].transform.position.z), WorldLoadTemp[0].transform.rotation, content.transform));
            worldSingle.Add(Instantiate(WorldLoadTemp[1], new Vector3(WorldLoadTemp[1].transform.position.x, WorldLoadTemp[1].transform.position.y + spaces, WorldLoadTemp[0].transform.position.z), WorldLoadTemp[1].transform.rotation, content.transform));
            worldSingle.Add(Instantiate(WorldLoadTemp[2], new Vector3(WorldLoadTemp[2].transform.position.x, WorldLoadTemp[2].transform.position.y + spaces, WorldLoadTemp[0].transform.position.z), WorldLoadTemp[2].transform.rotation, content.transform));
            worldSingle.Add(Instantiate(WorldLoadTemp[3], new Vector3(WorldLoadTemp[3].transform.position.x, WorldLoadTemp[3].transform.position.y + spaces, WorldLoadTemp[0].transform.position.z), WorldLoadTemp[3].transform.rotation, content.transform));
            worldSingle.Add(Instantiate(WorldLoadTemp[4], new Vector3(WorldLoadTemp[4].transform.position.x, WorldLoadTemp[4].transform.position.y + spaces, WorldLoadTemp[0].transform.position.z), WorldLoadTemp[4].transform.rotation, content.transform));
            worldSingle.Add(Instantiate(WorldLoadTemp[5], new Vector3(WorldLoadTemp[5].transform.position.x, WorldLoadTemp[5].transform.position.y + spaces, WorldLoadTemp[0].transform.position.z), WorldLoadTemp[5].transform.rotation, content.transform));

            worldSingle[1].GetComponent<InputField>().text = Path.GetFileNameWithoutExtension(files);
            worldSingle[5].GetComponent<TMPro.TextMeshProUGUI>().text = "By you";
            worldSingle[1].name = files;
            worldSingle[2].name = files;
            worldSingle[3].name = files;
            worldSingle[4].name = files;
            worldSingle[5].name = files;

            if (Path.GetFileNameWithoutExtension(files).Contains(";"))
            {
                string[] creatorAndName = Path.GetFileNameWithoutExtension(files).Split(';');
                worldSingle[5].GetComponent<TMPro.TextMeshProUGUI>().text = "By " + creatorAndName[0];
                string fname = "";
                var nameBuffer = creatorAndName.ToList();
                nameBuffer.Remove(nameBuffer[0]);
                nameBuffer.ForEach(obj => fname += obj);
                worldSingle[1].GetComponent<InputField>().text = fname;
            }

            filesNumber++;
            temp.AddRange(worldSingle);
            spaces = spaces - Screen.height / 3 + WorldLoadTemp[0].GetComponent<RectTransform>().rect.height + (Screen.height / 16);
            PlayerPrefs.SetInt("worlds", PlayerPrefs.GetInt("worlds") + 1);

            RectTransform rt = content.GetComponent<RectTransform>();
            rt.sizeDelta = new Vector2(rt.sizeDelta.x, rt.sizeDelta.y + WorldLoadTemp[0].GetComponent<RectTransform>().sizeDelta.y * 2f);
            //spaces = spaces - WorldLoadTemp[0].GetComponent<RectTransform>().rect.height; //Default is -200

            //StartCoroutine(LoadImg(worldSingle[4], files));
            //try { worldSingle[4].GetComponent<Image>().sprite = gameObject.GetComponent<Capture>().LoadPhoto(Path.GetFileNameWithoutExtension(files) + ".png"); } catch { }
        }

    }*/
    public IEnumerator UpdateLists()
    {
        yield return new WaitForEndOfFrame();
        int filesNumber = 0;
        float spaces = 0;
        foreach (GameObject single in temp)
        {
            Destroy(single);
        }
        temp.Clear();
        PlayerPrefs.SetInt("worlds", 0);
        content.GetComponent<RectTransform>().sizeDelta = contentRtOriginSizeDelta;
        foreach (string files in Directory.EnumerateFiles(Application.persistentDataPath + "/Saves", "*.slife"))
        {
            Debug.Log(files);
            worldSingle.Clear();
            worldSingle.Add(Instantiate(WorldLoadTemp[0], new Vector3(WorldLoadTemp[0].transform.position.x, WorldLoadTemp[0].transform.position.y + spaces, WorldLoadTemp[0].transform.position.z), WorldLoadTemp[0].transform.rotation, content.transform));
            worldSingle.Add(Instantiate(WorldLoadTemp[1], new Vector3(WorldLoadTemp[1].transform.position.x, WorldLoadTemp[1].transform.position.y + spaces, WorldLoadTemp[0].transform.position.z), WorldLoadTemp[1].transform.rotation, content.transform));
            worldSingle.Add(Instantiate(WorldLoadTemp[2], new Vector3(WorldLoadTemp[2].transform.position.x, WorldLoadTemp[2].transform.position.y + spaces, WorldLoadTemp[0].transform.position.z), WorldLoadTemp[2].transform.rotation, content.transform));
            worldSingle.Add(Instantiate(WorldLoadTemp[3], new Vector3(WorldLoadTemp[3].transform.position.x, WorldLoadTemp[3].transform.position.y + spaces, WorldLoadTemp[0].transform.position.z), WorldLoadTemp[3].transform.rotation, content.transform));
            worldSingle.Add(Instantiate(WorldLoadTemp[4], new Vector3(WorldLoadTemp[4].transform.position.x, WorldLoadTemp[4].transform.position.y + spaces, WorldLoadTemp[0].transform.position.z), WorldLoadTemp[4].transform.rotation, content.transform));
            worldSingle.Add(Instantiate(WorldLoadTemp[5], new Vector3(WorldLoadTemp[5].transform.position.x, WorldLoadTemp[5].transform.position.y + spaces, WorldLoadTemp[0].transform.position.z), WorldLoadTemp[5].transform.rotation, content.transform));

            int temple = 0;
            worldSingle.ForEach((obj) =>
            {
                obj.transform.localPosition = new Vector2(WorldLoadTemp[temple].transform.localPosition.x, WorldLoadTemp[temple].transform.localPosition.y + spaces);
                temple++;
            });

            worldSingle[1].GetComponent<InputField>().text = Path.GetFileNameWithoutExtension(files);
            worldSingle[5].GetComponent<TMPro.TextMeshProUGUI>().text = "By you";
            worldSingle[1].name = files;
            worldSingle[2].name = files;
            worldSingle[3].name = files;
            worldSingle[4].name = files;
            worldSingle[5].name = files;

            if (Path.GetFileNameWithoutExtension(files).Contains(";"))
            {
                string[] creatorAndName = Path.GetFileNameWithoutExtension(files).Split(';');
                worldSingle[5].GetComponent<TMPro.TextMeshProUGUI>().text = "By " + creatorAndName[0];
                string fname = "";
                var nameBuffer = creatorAndName.ToList();
                nameBuffer.Remove(nameBuffer[0]);
                nameBuffer.ForEach(obj => fname += obj);
                worldSingle[1].GetComponent<InputField>().text = fname;
            }

            filesNumber++;
            temp.AddRange(worldSingle);
            spaces = spaces - 75;
            PlayerPrefs.SetInt("worlds", PlayerPrefs.GetInt("worlds") + 1);

            RectTransform rt = content.GetComponent<RectTransform>();
            //rt.sizeDelta = new Vector2(rt.sizeDelta.x, rt.sizeDelta.y + (Screen.height / 3 + WorldLoadTemp[0].GetComponent<RectTransform>().rect.height + (Screen.height / 16)));
            rt.sizeDelta = new Vector2(rt.sizeDelta.x, rt.sizeDelta.y + WorldLoadTemp[0].GetComponent<RectTransform>().sizeDelta.y + (WorldLoadTemp[0].GetComponent<RectTransform>().sizeDelta.y / 3));



            //spaces = spaces - WorldLoadTemp[0].GetComponent<RectTransform>().rect.height; //Default is -200

            //StartCoroutine(LoadImg(worldSingle[4], files));
            //try { worldSingle[4].GetComponent<Image>().sprite = gameObject.GetComponent<Capture>().LoadPhoto(Path.GetFileNameWithoutExtension(files) + ".png"); } catch { }
        }

    }
    #endregion

    IEnumerator LoadImg(GameObject handle, string name)
    {
        yield return new WaitForEndOfFrame();
        try { handle.GetComponent<Image>().sprite = gameObject.GetComponent<Capture>().LoadPhoto(Path.GetFileNameWithoutExtension(name) + ".png"); } catch { }
    }

    public void RedirectToPlayer(GameObject handle)
    {
        var id = handle.GetComponent<TMPro.TextMeshProUGUI>().text.TrimStart(new char[] {'B','y',' '});
        if(id == "ou") { id = ""; }
        LoadUIClick();
        gameObject.GetComponent<Online>().BlockUIClick();
        gameObject.GetComponent<Online>().searchBox.GetComponent<InputField>().text = id;
        gameObject.GetComponent<Online>().SearchFriend();
    }


    #region ColorBg
    public void CustomBgBtn()
    {
        musicControler.GetComponent<Sounds>().PlaySound(3);
        CustomBgColorRandom();
    }

    int lastNum = -1;

    public void CustomBgColorRandom()
    {
        GameObject bg = Instantiate(gameObject.GetComponent<Controler>().Cube, new Vector2(-1000, -1000), new Quaternion(0, 0, 0, 0));
        bg.GetComponent<Block>().blockID = -2;
        System.Random rdn = new System.Random();
        int num = rdn.Next(1, 7);
        while (num == lastNum)
        {
            num = rdn.Next(1, 7);
        }
        lastNum = num;
        Color32 color;
        switch (num)
        {
            default: color = new Color32(47, 77, 121, 255); break;
            case 0: color = new Color32(232, 120, 100, 255); break; //red
            case 1: color = new Color32(139, 247, 193, 255); break; //green
            case 2: color = new Color32(115, 80, 163, 255); break; //purple
            case 3: color = new Color32(248, 255, 143, 255); break; //yellow
            case 4: color = new Color32(89, 89, 89, 255); break; //gray
            case 5: color = new Color32(182, 252, 249, 255); break; //aqua
        }
        bg.GetComponent<Block>().internalInfo = color.r + ";" + color.g + ";" + color.b;
        infoCube = bg;
        gameObject.GetComponent<Controler>().cubeObjects.Add(bg);
        gameObject.GetComponent<Controler>().cam.backgroundColor = color;
    }
    public void CustomBgColorLoad(GameObject handle)
    {
        string[] color = handle.GetComponent<Block>().internalInfo.Split(';');
        gameObject.GetComponent<Controler>().cam.backgroundColor = new Color32(color[0].ToByte(), color[1].ToByte(), color[2].ToByte(), 255);
    }
    public void ChangeBgColor(Color32 color)
    {
        infoCube.GetComponent<Block>().internalInfo = color.r.ToString() + ";" + color.g.ToString() + ";" + color.b.ToString();
        gameObject.GetComponent<Controler>().cam.backgroundColor = color;
    }
    #endregion
    // Update method
    void Update()
    {
        if(objectsSaved != gameObject.GetComponent<Controler>().cubeObjects)
        {
            //SprB
            saveIcon.sprite = saveIconStates[1];
        }
        else
        {
            //SprA
            saveIcon.sprite = saveIconStates[0];
        }
        
        Thread t = new Thread(() =>
        {
            
        });
        t.Start();
    }
    #region Reload

    public void LoadUIClick()
    {
        musicControler.GetComponent<Sounds>().PlaySound(3);
        if (loadOpened == true)
        {
            loadOpened = false;
            foreach (GameObject loadComponent in loadui)
            {
                loadComponent.transform.position = new Vector2(-1000, -1000);
            }
            //gameObject.GetComponent<Controler>().focusObject.GetComponent<Focus>().show = false;
        }
        else
        {
            loadOpened = true;
            int i = 0;
            foreach (GameObject loadComponent in loadui)
            {
                loadComponent.transform.position = loadUIPosition[i];
                i++;
            }
            if (SystemInfo.operatingSystemFamily != OperatingSystemFamily.Windows)
            {
                foreach (GameObject key in KeyCups)
                {
                    key.transform.position = new Vector2(-1000, -1000);
                }
            }
            //gameObject.GetComponent<Controler>().focusObject.GetComponent<Focus>().show = true;
        }
    }
    #endregion
    #region WorldOperators
    public void CreateWorld()
    {
        try
        {
            currentFile = "";
            SaveChanges();
            StartCoroutine(UpdateLists());
        }
        catch
        {

        }
    }

    public void Rename(GameObject gmo)
    {
        File.Move(gmo.name, Application.persistentDataPath + "/Saves/" + gmo.GetComponent<InputField>().text + ".slife");
        //try { File.Move(Application.persistentDataPath + "/Photos/" + gameObject.GetComponent<Capture>().GetPhoto(Path.GetFileNameWithoutExtension(gmo.name)), Application.persistentDataPath + "/Photos/" + gameObject.GetComponent<Capture>().GetPhoto(gmo.GetComponent<InputField>().text)); } catch { }
        if(currentFile == gmo.name)
        {
            currentFile = Application.persistentDataPath + "/Saves/" + gmo.GetComponent<InputField>().text + ".slife";
        }
        gmo.name = Application.persistentDataPath + "/Saves/" + gmo.GetComponent<InputField>().text + ".slife";
        StartCoroutine(UpdateLists());
    }

    public void SaveChangesBtn()
    {
        if(saving == false)
        {
            saving = true;
            SaveChanges();
            StartCoroutine(UpdateLists());
            saving = false;
        }
        
    }

    public void SaveChanges()
    {
        try
        {
            var camPos = Instantiate(gameObject.GetComponent<Controler>().Cube, new Vector2(-1000, -1000), new Quaternion(0, 0, 0, 0));
            camPos.GetComponent<Block>().blockID = -7;
            Vector3 pos = gameObject.GetComponent<Controler>().cam.transform.position;
            camPos.GetComponent<Block>().internalInfo = pos.x + ";" + pos.y + ";" + pos.z;
            gameObject.GetComponent<Controler>().cubeObjects.Add(camPos);

            Debug.Log("Sav_" + pos.x + ";" + pos.y + ";" + pos.z);

            musicControler.GetComponent<Sounds>().PlaySound(3);
            objectsSaved = gameObject.GetComponent<Controler>().cubeObjects;
            string filenumber = "0";
            if (currentFile == "")
            {
                filenumber = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString();
                currentFile = Application.persistentDataPath + "/Saves/World_" + filenumber.ToString() + ".slife";
            }
            List<Cube> jsonCube = new List<Cube>();
            for (int i = 0; i < gameObject.GetComponent<Controler>().cubeObjects.Count; i++)
            {
                 jsonCube.Add(new Cube(gameObject.GetComponent<Controler>().cubeObjects[i].GetComponent<Block>().blockID, gameObject.GetComponent<Controler>().cubeObjects[i].GetComponent<Block>().transform.position.x, gameObject.GetComponent<Controler>().cubeObjects[i].GetComponent<Block>().transform.position.y, gameObject.GetComponent<Controler>().cubeObjects[i].GetComponent<Block>().transform.position.z, gameObject.GetComponent<Controler>().cubeObjects[i].GetComponent<Block>().blockStatus, gameObject.GetComponent<Controler>().cubeObjects[i].GetComponent<Block>().internalInfo, gameObject.GetComponent<Controler>().cubeObjects[i].transform.rotation.eulerAngles.z));
                 //jsonCube.Add(new Cube(gameObject.GetComponent<Controler>().cubeObjects[i].GetComponent<Block>().blockID, gameObject.GetComponent<Controler>().cubeObjects[i].GetComponent<Block>().transform.localPosition.x, gameObject.GetComponent<Controler>().cubeObjects[i].GetComponent<Block>().transform.localPosition.y, gameObject.GetComponent<Controler>().cubeObjects[i].GetComponent<Block>().transform.localPosition.z, gameObject.GetComponent<Controler>().cubeObjects[i].GetComponent<Block>().blockStatus, gameObject.GetComponent<Controler>().cubeObjects[i].GetComponent<Block>().internalInfo));
            }
            File.WriteAllText(currentFile, JsonConvert.SerializeObject(jsonCube));
            PlayerPrefs.SetInt("worldCount", PlayerPrefs.GetInt("worldCount") + 1);
            Debug.Log("w:" + PlayerPrefs.GetInt("worldCount")); 
            //StartCoroutine(gameObject.GetComponent<Network>().SendWorld());
        }
        catch(Exception ex)
        {
            Debug.LogError("Overwrite error " + ex);
        }
    }

    public void LoadedWorld(GameObject gmo)
    {
        musicControler.GetComponent<Sounds>().PlaySound(3);
        Debug.Log(gmo.name);
        LoadWorld(Path.GetFileName(gmo.name));
        currentFile = gmo.name;
    }

    public void DeleteWorld(GameObject gmo)
    {
        musicControler.GetComponent<Sounds>().PlaySound(3);
        Debug.Log(gmo.name);
        Delete(Path.GetFileName(gmo.name));
        currentFile = "";
    }

    public void Delete(string file)
    {
        StartCoroutine(DeleteF(file));
    }
    IEnumerator DeleteF(string file)
    {
        File.Delete(Application.persistentDataPath + "/Saves/" + file);
        //gameObject.GetComponent<Capture>().DeletePhoto(Path.GetFileNameWithoutExtension(file) + ".png");
        yield return new WaitForEndOfFrame();
        StartCoroutine(UpdateLists());
    }
    public void LoadWorld(string file)
    {
        StartCoroutine( SaveFilesYield(file));
    }
    IEnumerator SaveFilesYield(string file)
    {
        //Print the time of when the function is first called.
        stopruntime = true;
        yield return new WaitForSeconds(0.02f);
        stopruntime = false;
        string text = File.ReadAllText(Application.persistentDataPath + "/Saves/" + file);
        List<Cube> newObjects = JsonConvert.DeserializeObject<List<Cube>>(text);
        gameObject.GetComponent<UI>().EraseAll();
        for (int i = 0; i < newObjects.Count; i++)
        {
            if (stopruntime == true)
            {
                stopruntime = false;
                break;
            }
            yield return new WaitForSeconds(0.01f);
            GameObject cubeInstancied = Instantiate(cube, new Vector3(newObjects[i].posX, newObjects[i].posY, newObjects[i].posZ), Quaternion.Euler(cube.transform.rotation.eulerAngles.x,cube.transform.rotation.eulerAngles.y,newObjects[i].rot));
            cubeInstancied.GetComponent<Block>().blockID = newObjects[i].blockID;
            cubeInstancied.GetComponent<Block>().blockStatus = newObjects[i].blockStatus;
            cubeInstancied.GetComponent<Block>().internalInfo = newObjects[i].internalInfo;

            //cubeInstancied.transform.localPosition = new Vector3(newObjects[i].posX, newObjects[i].posY, newObjects[i].posZ);

            if (cubeInstancied.GetComponent<Block>().blockID >= 0)
            {
                cube.GetComponent<SpriteRenderer>().sprite = gameObject.GetComponent<Controler>().BlockSprites[newObjects[i].blockID];
            }
            else if (cubeInstancied.GetComponent<Block>().blockID == -2)
            {
                CustomBgColorLoad(cubeInstancied);
            }
            else if (cubeInstancied.GetComponent<Block>().blockID == -7)
            {
                string internalInfo = cubeInstancied.GetComponent<Block>().internalInfo;
                gameObject.GetComponent<Controler>().cam.transform.position = new Vector3(float.Parse(internalInfo.Split(';')[0]), float.Parse(internalInfo.Split(';')[1]), float.Parse(internalInfo.Split(';')[2]));
            Debug.Log("Lod_" + internalInfo);

            }
            gameObject.GetComponent<Controler>().cubeObjects.Add(cubeInstancied);

        }  
    }

    public void Share(GameObject handle)
    {
        gameObject.GetComponent<SaveLoad>().LoadUIClick();
        gameObject.GetComponent<Message>().ShowAlert("Share World", "Introduce your friend id to send", "Send", "Exit", new System.Action(() => { gameObject.GetComponent<UI>().musicControler.GetComponent<Sounds>().PlaySound(3); gameObject.GetComponent<Online>().UploadWorld(Path.GetFileName(handle.name), gameObject.GetComponent<Message>().input.text); }), new System.Action(() => { gameObject.GetComponent<SaveLoad>().LoadUIClick(); }),true,"Friend id");

    }
    #endregion

}
//Serailizable JSON to load/save worlds
#region JsonStructure
[System.Serializable]
public class Cube
{
    public Cube(int blockID,float posX,float posY,float posZ,int blockStatus,string internalInfo,float rot)
    {
        this.blockID = blockID;
        this.posX = posX;
        this.posY = posY;
        this.posZ = posZ;
        this.blockStatus = blockStatus;
        this.internalInfo = internalInfo;
        this.rot = rot;
    }
    public int blockID;
    public float posX;
    public float posY;
    public float posZ;
    public float rot;
    public int blockStatus;
    public string internalInfo;
}
#endregion
