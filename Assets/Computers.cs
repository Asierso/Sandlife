using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using Proyecto26;
using System.Net;
using System;
using UnityEditor;

public class Computers : MonoBehaviour
{
    public GameObject[] computer;
    private List<float> computerX = new List<float>();
    private List<float> computerY = new List<float>();
    public bool blockComputer = false;
    public GameObject musicControler;
    public GameObject onStatus;
    public Sprite[] onStatusSprites;
    GameObject handle;
    public GameObject background;
    public TMPro.TextMeshProUGUI terminal;
    public InputField commandTerminal;
    public TMPro.TextMeshProUGUI commandTerminalIndicator;
    public GameObject content;
    public Vector2 defaultSizeDelta;
    private string directory;
    // Start is called before the first frame update
    void Start()
    {
        defaultSizeDelta = content.GetComponent<RectTransform>().sizeDelta;
        foreach (GameObject inventoryComponent in computer)
        {
            computerX.Add(inventoryComponent.transform.position.x);
            computerY.Add(inventoryComponent.transform.position.y);
            inventoryComponent.transform.position = new Vector2(-1000, -1000);
        }
        blockComputer = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Detect if is on
        try
        {
            if (handle.GetComponent<Block>().on >= 1 && blockComputer == true)
            {
                //Starting
                Debug.Log("Computer booting");
                onStatus.GetComponent<Image>().sprite = onStatusSprites[1];
                background.GetComponent<Image>().color = new Color32(50, 50, 50, 255);
                commandTerminal.GetComponent<Image>().color = new Color32(50, 50, 50, 255);
                //commandTerminalIndicator.GetComponent<Image>().color = new Color32(50, 50, 50, 255);
                commandTerminal.readOnly = false;
                commandTerminalIndicator.text = ">";
                commandTerminal.caretColor = new Color32(0, 255, 0, 255);
                terminal.text = handle.GetComponent<Block>().internalInfo;
                if (handle.GetComponent<Block>().blockStatus == 0)
                {
                    //musicControler.GetComponent<Sounds>().PlaySound(14);
                    PlayerPrefs.SetInt("computerCount", PlayerPrefs.GetInt("computerCount") + 1);
                    handle.GetComponent<Block>().blockStatus = 1;
                    if (handle.GetComponent<Block>().internalInfo == "")
                    {
                        terminal.text = "SandlifeOS - Console\n(c) Asierso Studio | Welcome " + PlayerPrefs.GetString("name") + "\n";
                    }
                }
            }
            else if (handle.GetComponent<Block>().on == 0)
            {
                //Need energy
                onStatus.GetComponent<Image>().sprite = onStatusSprites[0];
                handle.GetComponent<Block>().blockStatus = 0;
                background.GetComponent<Image>().color = new Color32(10, 10, 10, 255);
                commandTerminal.GetComponent<Image>().color = new Color32(10, 10, 10, 255);
                //commandTerminalIndicator.GetComponent<Image>().color = new Color32(10, 10, 10, 255);
                commandTerminal.readOnly = true;
                terminal.text = "";
                commandTerminalIndicator.text = "";
                commandTerminal.caretColor = new Color32(0, 0, 0, 0);
            }
        }
        catch
        {

        }
    }

    public void LoadUIClick(GameObject handle)
    {
        this.handle = handle;
        musicControler.GetComponent<Sounds>().PlaySound(3);
        if (blockComputer == true)
        {
            foreach (GameObject inventoryComponent in computer)
            {
                inventoryComponent.transform.position = new Vector2(-1000, -1000);
            }
            blockComputer = false;
            content.GetComponent<RectTransform>().sizeDelta = defaultSizeDelta;
            //gameObject.GetComponent<Controler>().focusObject.GetComponent<Focus>().show = false;
        }
        else
        {
            int i = 0;
            foreach (GameObject inventoryComponent in computer)
            {
                inventoryComponent.transform.position = new Vector2(computerX[i], computerY[i]);
                i++;
            }
            blockComputer = true;
            ResizeBuffer(handle);
            //gameObject.GetComponent<Controler>().focusObject.GetComponent<Focus>().show = true;
            terminal.text = handle.GetComponent<Block>().internalInfo;
        }
    }

    public void ButtonClose()
    {
        LoadUIClick(null);
    }
    public void OnSubmit()
    {
        StartCoroutine(execute(false));
    }

    #region Buffer redimension
    private void ResizeBuffer(GameObject handle)
    {
        Debug.Log("Resizing");
        foreach (var lines in handle.GetComponent<Block>().internalInfo.Split('\n'))
        {
            var contentRt = content.GetComponent<RectTransform>().sizeDelta;
            Debug.Log("ResizingA");
            content.GetComponent<RectTransform>().sizeDelta = new Vector2(contentRt.x, contentRt.y + 100);
        }
    }
    private void IncreaseSpace()
    {
        var contentRt = content.GetComponent<RectTransform>().sizeDelta;
        content.GetComponent<RectTransform>().sizeDelta = new Vector2(contentRt.x, contentRt.y + 100);
    }
    #endregion

    public IEnumerator execute(bool exclude,string command = "")
    {
        yield return new WaitForSeconds(0.5f);
        string[] args;

        if (exclude == false)
        {
            if (commandTerminal.text.Contains(' '))
            {
                args = commandTerminal.text.Split(' ');
            }
            else
            {
                args = new string[] { commandTerminal.text };
            }
        }
        else
        {
            args = command.Split(' ');
        }

        switch(args[0])
        {
            case "clear":
                terminal.text = "";break;
            case "version":
                terminal.text += "Sandlife v" + Application.version + "\n"; IncreaseSpace(); break;
            case "ls":
                terminal.text += "Files in: /" + directory + ":\n";
                IncreaseSpace();
                foreach (string files in Directory.EnumerateDirectories(Application.persistentDataPath + "/" + directory))
                {
                    terminal.text += Path.GetFileName(files) + "/  ";
                }
                foreach (string files in Directory.EnumerateFiles(Application.persistentDataPath + "/" + directory))
                {
                    terminal.text += Path.GetFileName(files) + "  ";
                }
                terminal.text += "\n";
                IncreaseSpace();
                break;
            case "directory":
                if(Directory.Exists(Application.persistentDataPath + "/" + directory))
                {
                    if(!args[1].Contains("/"))
                    {
                        directory = args[1] + "/";
                    }  
                    else
                    {
                        directory = args[1];
                    }
                }
                else if(args[1] == "/")
                {
                    directory = "";
                }
                break;
            case "removedir":
                if (Directory.Exists(Application.persistentDataPath + "/" + directory + args[1]))
                {
                    Directory.Delete(Application.persistentDataPath + "/" + directory + args[1]);
                    terminal.text += "Directory " + args[1] + " removed sucess\n";
                    IncreaseSpace();
                }
                else
                {
                    terminal.text += "Directory " + args[1] + " not exists\n";
                    IncreaseSpace();
                }
                break;
            case "remove":
                if (File.Exists(Application.persistentDataPath + "/" + directory + args[1]))
                {
                    File.Delete(Application.persistentDataPath + "/" + directory + args[1]);
                    terminal.text += "File " + args[1] + " removed sucess\n";
                    IncreaseSpace();
                }
                else
                {
                    terminal.text += "File " + args[1] + " not exists\n";
                    IncreaseSpace();
                }
                break;
            case "create":
                string fileText = "";
                for (int i = 0;i<args.Length;i++)
                {
                    if(i >= 2)
                    {
                        fileText += args[i];
                    }
                }
                File.WriteAllText(Application.persistentDataPath + "/" + directory + args[1], fileText);
                terminal.text += "File " + args[1] + " created sucess\n";
                IncreaseSpace();
                break;
            case "createdir":
                if(args[1] == "/")
                {
                    terminal.text += "Directory name violate rules\n";
                    IncreaseSpace();
                }
                else
                {
                    Directory.CreateDirectory(Application.persistentDataPath + "/" + directory + args[1]);
                    terminal.text += "Directory " + args[1] + " created sucess\n";
                    IncreaseSpace();
                }
                break;
            case "read":
                if (File.Exists(Application.persistentDataPath + "/" + directory + args[1]))
                {
                    terminal.text += File.ReadAllText(Application.persistentDataPath + "/" + directory + args[1]) + "\n";
                    IncreaseSpace();
                }
                else
                {
                    terminal.text += "File " + args[1] + " not founded\n";
                    IncreaseSpace();
                }
                break;
            case "bot":
                string botInfo = "";
                if (File.Exists(Application.persistentDataPath + "/" + directory + args[1]))
                {
                    try
                    {
                        botInfo = File.ReadAllText(Application.persistentDataPath + "/" + directory + args[1]);
                        var bot = JsonConvert.DeserializeObject<BotConfig>(botInfo);
                        bot.Set();
                        RestClient.Put<BotConfig>(Online.Credentials.databaseURL + "/Players/Collection/" + bot.id + ".json", bot);
                        terminal.text += "Bot with id " + bot.id + " created sucess\n";
                        IncreaseSpace();
                    }
                    catch
                    {
                        terminal.text += "Syntax error in bot file (Should be a .json file)\n";
                        IncreaseSpace();
                    }
                }
                else
                {
                    terminal.text += "Bot file " + args[1] + " not founded\n";
                    IncreaseSpace();
                }
                break;
            case "download":
                try
                {
                    WebClient client = new WebClient();
                    client.DownloadFile(args[1], Application.persistentDataPath + "/" + directory + args[2]);
                    terminal.text += "Download file " + args[1] + " sucess\n";
                    IncreaseSpace();
                }
                catch
                {
                    terminal.text += "Download error\n";
                    IncreaseSpace();
                }
                break;
            case "run":
                terminal.text += "Programs .sliferun are not further avaiable\n";
                IncreaseSpace();
                //StartCoroutine(Program(args[1]));
                break;
            case "":break;
            case " ": break;
            default: terminal.text += "Syntax Error (command " + args[0] + " not exists)\n"; IncreaseSpace(); break;
        }
        try { handle.GetComponent<Block>().internalInfo = terminal.text; } catch { }
        commandTerminal.text = "";


    }
    public IEnumerator Program(string args)
    {
        Debug.Log("Running " + args);
        yield return new WaitForEndOfFrame();
        if (File.Exists(Application.persistentDataPath + "/" + directory + args))
        {
            string[] lines = File.ReadAllText(Application.persistentDataPath + "/" + directory + args ).Split('|');
            lines.ToList().ForEach((Obj) =>
            {
                Debug.Log(Obj);
                StartCoroutine(execute(true, Obj));
            });
        }
        else
        {
            terminal.text += "File " + args + " not exists\n";
            IncreaseSpace();
        }
    }
}
[System.Serializable]
public class BotConfig : Online.Player { }