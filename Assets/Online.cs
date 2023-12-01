using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Proyecto26;
using System.IO;
using System.Threading;
using System.Linq;
using Newtonsoft.Json;
public class Online : MonoBehaviour
{
    public GameObject[] online;
    private List<Vector2> onlineVct = new List<Vector2>();
    public bool blockOnline = false;
    public GameObject musicControler;
    public GameObject searchBox;
    public GameObject[] friendTag;
    public Sprite[] playerStatusIcons;
    public GameObject PlayerIdText;
    public GameObject[] skills;
    public Sprite[] medals;
    public Sprite[] onlineStatus;
    public Image onlineDock;
    bool onlineOn = false;
    bool haveInternetConection = false;

    public List<GameObject> KeyCups;

    //Test DB: /Players/InternalTest
    //Online DB: /Players
    //Work in progress
    void Start()
    {
        StartCoroutine(GetWorlds());
        friendTag[1].GetComponent<TMPro.TextMeshProUGUI>().text = "";
        friendTag[2].GetComponent<TMPro.TextMeshProUGUI>().text = "";
        friendTag[3].GetComponent<Image>().sprite = null;
        friendTag[4].GetComponent<Image>().sprite = null;
        foreach (GameObject inventoryComponent in online)
        {
            onlineVct.Add(inventoryComponent.transform.position);
            inventoryComponent.transform.position = new Vector2(-1000, -1000);
        }
        blockOnline = false;

        IFireSerializable lastConnectionTime = new LastConexion();
        lastConnectionTime.Set();

        var player = new Player();
        player.id = "@0";
        player.name = "Guest";
        player.blocksPutted = 0;
        player.worlds = 0;
        player.bronze = 0;
        player.silver = 0;
        player.gold = 0;
        if (Application.internetReachability != NetworkReachability.NotReachable)
        {
            RestClient.Put<LastConexion>(Credentials.databaseURL + "/Players/LastConection.json", lastConnectionTime);
            RestClient.Put<Player>(Credentials.databaseURL + "/Players/Collection/@0.json", player);


            if (!PlayerPrefs.GetString("id").Contains("@"))
            {
                player.Set();
                MyData.playerId = player.id;
                MyData.playerName = player.name;
                Debug.Log("Creating player");
                RestClient.Get<Player>(Credentials.databaseURL + "/Players/Collection/" + player.id + ".json").Catch(request =>
                {
                    RestClient.Put<Player>(Credentials.databaseURL + "/Players/Collection/" + player.id + ".json", player);
                });
                PlayerPrefs.SetString("id", player.id);
                PlayerIdText.GetComponent<TMPro.TextMeshProUGUI>().text = "Player: " + player.id;
            }
            else
            {
                Debug.Log("Loading player");
                PlayerIdText.GetComponent<TMPro.TextMeshProUGUI>().text = "Player: " + PlayerPrefs.GetString("id");
                MyData.playerId = PlayerPrefs.GetString("id");
                MyData.playerName = PlayerPrefs.GetString("name");
            }
            player.Set();
            MyData.playerActivity = player.activity;
            Debug.Log(MyData.playerId);
            onlineOn = true;
            haveInternetConection = true;
            onlineDock.sprite = onlineStatus[0];
        }
        else
        {
            player.id = "@0 (Offline)";
            haveInternetConection = false;
            onlineDock.sprite = onlineStatus[1];
        }
        StartCoroutine(PingConection());

    }

    private void Update()
    {
        if (onlineOn == true && PlayerPrefs.GetInt("onlineMode") == 1)
        {
            StartCoroutine(RenewPlayer());
        }
    }

    private IEnumerator PingConection()
    {
        yield return new WaitForSeconds(10f);
        if(Application.internetReachability != NetworkReachability.NotReachable)
        {
            haveInternetConection = true;
            onlineDock.sprite = onlineStatus[0];
        }
        else
        {
            haveInternetConection = false;
            onlineDock.sprite = onlineStatus[1];
        }
        StartCoroutine(PingConection());
    }


    public void TryMigrate(string id)
    {
        gameObject.GetComponent<SaveLoad>().musicControler.GetComponent<Sounds>().PlaySound(3);
        //gameObject.GetComponent<Message>().BlockUIClick();
        RestClient.Get<Player>(Credentials.databaseURL + "/Players/Collection/" + id + ".json").Done(req =>
        {
            if (req.migration == 1)
            {
                PlayerPrefs.SetString("id", id);
                MyData.playerId = id;
                PlayerPrefs.SetInt("migrateProfile", 0);
                PlayerPrefs.SetInt("worlds", req.worlds);
                PlayerPrefs.SetInt("bronze", req.bronze);
                PlayerPrefs.SetInt("bronzeTotal", req.bronzeT);
                PlayerPrefs.SetInt("silver", req.silver);
                PlayerPrefs.SetInt("silverTotal", req.silverT);
                PlayerPrefs.SetInt("gold", req.gold);
                PlayerPrefs.SetInt("blocksPutted", req.blocksPutted);
                PlayerPrefs.SetInt("blocksChanged", req.blocksChanged);
                PlayerPrefs.SetInt("profileImage", req.image);
                PlayerPrefs.SetString("name", req.name);
                gameObject.GetComponent<Message>().ShowAlert("Warning", "Your profile has been uploaded correctly", "Ok", "Close", new System.Action(() => { gameObject.GetComponent<Settings>().BlockUIClick(); }), new System.Action(() => { gameObject.GetComponent<Settings>().BlockUIClick(); }));
            }
            else
            {
                gameObject.GetComponent<Message>().ShowAlert("Error", "Your profile could not be loaded.\nCheck if the profile has the migration\noption active and try again", "Try again", "Cancel", new System.Action(() => { gameObject.GetComponent<Settings>().LoadProfile(); }), new System.Action(() => { gameObject.GetComponent<Settings>().BlockUIClick(); }));
            }
        });
        RestClient.Get<Player>(Credentials.databaseURL + "/Players/Collection/" + id + ".json").Catch(req =>
        {
            gameObject.GetComponent<Message>().ShowAlert("Error", "Your profile could not be loaded.\nCheck if the profile exists and try again", "Try again", "Cancel", new System.Action(() => { gameObject.GetComponent<Settings>().LoadProfile(); }), new System.Action(() => { gameObject.GetComponent<Settings>().BlockUIClick(); }));
        });
    }

    public void BlockUIClick()
    {
        GetWorld(0);
        musicControler.GetComponent<Sounds>().PlaySound(3);
        if (blockOnline == true)
        {
            foreach (GameObject inventoryComponent in online)
            {
                inventoryComponent.transform.position = new Vector2(-1000, -1000);
            }
            blockOnline = false;
            //gameObject.GetComponent<Controler>().focusObject.GetComponent<Focus>().show = false;
        }
        else
        {
            int i = 0;
            foreach (GameObject inventoryComponent in online)
            {
                inventoryComponent.transform.position = onlineVct[i];
                i++;
            }
            if (SystemInfo.operatingSystemFamily != OperatingSystemFamily.Windows)
            {
                foreach (GameObject key in KeyCups)
                {
                    key.transform.position = new Vector2(-1000, -1000);
                }
            }
            blockOnline = true;
            if (searchBox.GetComponent<InputField>().text == "")
            {
                searchBox.GetComponent<InputField>().text = MyData.playerId;
                SearchFriend();
                searchBox.GetComponent<InputField>().text = "";
            }
            //gameObject.GetComponent<Controler>().focusObject.GetComponent<Focus>().show = true;
        }
    }

    public void SearchFriend()
    {
        if (searchBox.GetComponent<InputField>().text == "")
        {
            searchBox.GetComponent<InputField>().text = PlayerPrefs.GetString("id");
            musicControler.GetComponent<Sounds>().PlaySound(3);
            FriendsSearcher fs = new FriendsSearcher();
            fs.playerSatusIcons = playerStatusIcons;
            fs.friendTag = friendTag;
            fs.skills = skills;
            fs.medals = medals;
            fs.Set(searchBox.GetComponent<InputField>().text);
            fs.canvas = gameObject;
            StartCoroutine(fs.Search());
            searchBox.GetComponent<InputField>().text = "";
        }
        else
        {
            musicControler.GetComponent<Sounds>().PlaySound(3);
            FriendsSearcher fs = new FriendsSearcher();
            fs.playerSatusIcons = playerStatusIcons;
            fs.friendTag = friendTag;
            fs.skills = skills;
            fs.medals = medals;
            fs.Set(searchBox.GetComponent<InputField>().text);
            fs.canvas = gameObject;
            StartCoroutine(fs.Search());
        }

    }

    IEnumerator RenewPlayer()
    {
        onlineOn = false;

        var renewPlayer = new Player();

        //Online
        renewPlayer.Set();
        renewPlayer.id = PlayerPrefs.GetString("id");
        renewPlayer.name = PlayerPrefs.GetString("name");
        renewPlayer.migration = PlayerPrefs.GetInt("migrateProfile");
        renewPlayer.image = PlayerPrefs.GetInt("profileImage");
        //Skills
        if (PlayerPrefs.GetInt("shareSkills") == 1)
        {
            renewPlayer.blocksPutted = PlayerPrefs.GetInt("blocksPutted");
            renewPlayer.worlds = PlayerPrefs.GetInt("worlds");
            renewPlayer.bronze = PlayerPrefs.GetInt("bronze");
            renewPlayer.silver = PlayerPrefs.GetInt("silver");
            renewPlayer.gold = PlayerPrefs.GetInt("gold");
            renewPlayer.bronzeT = PlayerPrefs.GetInt("bronzeTotal");
            renewPlayer.silverT = PlayerPrefs.GetInt("silverTotal");
            renewPlayer.blocksChanged = PlayerPrefs.GetInt("blocksChanged");
            renewPlayer.level = (int)((renewPlayer.blocksPutted + Math.Pow(renewPlayer.blocksChanged, 1.2) + Math.Pow(renewPlayer.gold, 3) + Math.Pow(renewPlayer.silver + PlayerPrefs.GetInt("silverTotal"), 2) + renewPlayer.bronze + PlayerPrefs.GetInt("bronzeTotal")) / 100);
        }

        RestClient.Put<Player>(Credentials.databaseURL + "/Players/Collection/" + PlayerPrefs.GetString("id") + ".json", renewPlayer);

        if (renewPlayer.migration == 1)
        {
            renewPlayer.Set();
            string newId = renewPlayer.id;
            PlayerPrefs.SetString("id", newId);
            MyData.playerId = newId;
            PlayerPrefs.SetInt("worlds", 0);
            PlayerPrefs.SetInt("bronze", 0);
            PlayerPrefs.SetInt("silver", 0);
            PlayerPrefs.SetInt("gold", 0);
            PlayerPrefs.SetInt("blocksPutted", 0);
            PlayerPrefs.SetInt("blocksChanged", 0);
            PlayerPrefs.SetInt("migrateProfile", 0);
            PlayerPrefs.SetInt("profileImage", 0);
            PlayerPrefs.SetInt("bronzeTotal", 0);
            PlayerPrefs.SetInt("silverTotal", 0);
            PlayerPrefs.SetInt("profileImage", 0);
            PlayerPrefs.SetString("name", "Guest");
        }


        MyData.playerActivity = renewPlayer.activity;
        yield return new WaitForSeconds(1f);
        onlineOn = true;
        //Thread.Sleep(1000);

    }

    public void UploadWorld(string file, string fid)
    {
        if (fid == "" || fid == PlayerPrefs.GetString("id"))
        {
            gameObject.GetComponent<Message>().ShowAlert("Error", "You cannot send worlds to yourself", "Ok", "Close", new System.Action(() => { gameObject.GetComponent<SaveLoad>().LoadUIClick(); }), new System.Action(() => { gameObject.GetComponent<SaveLoad>().LoadUIClick(); }));

        }
        else
        {
            RestClient.Get<Player>(Credentials.databaseURL + "/Players/Collection/" + fid + ".json").Done(req =>
            {
                gameObject.GetComponent<Message>().ShowAlert("Warning", "World was send sucess", "Ok", "Close", new System.Action(() => { gameObject.GetComponent<SaveLoad>().LoadUIClick(); }), new System.Action(() => { gameObject.GetComponent<SaveLoad>().LoadUIClick(); }));
                string text = File.ReadAllText(Application.persistentDataPath + "/Saves/" + file);
                List<Cube> newObjects = JsonConvert.DeserializeObject<List<Cube>>(text);

                var wcount = new WorldCount();
                var winfo = new WorldInfo();

                winfo.name = Path.GetFileNameWithoutExtension(file);
                winfo.creator = PlayerPrefs.GetString("id");
                wcount.worldNumber = 0;

                var uploadWorldBlocks = new Action(() =>
                {
                    int i = 0;
                    newObjects.ForEach((obj) =>
                    {
                        RestClient.Put<Cube>(Credentials.databaseURL + "/Inbox/" + fid + "/" + wcount.worldNumber.ToString() + "/" + i + ".json", obj);
                        i++;
                    });
                });

                RestClient.Get<WorldCount>(Credentials.databaseURL + "/Inbox/" + fid + "/WorldCount.json").Done(req =>
                {
                    wcount.worldNumber = req.worldNumber;
                    wcount.worldNumber++;
                    winfo.worldNumber = wcount.worldNumber;
                    RestClient.Put<WorldCount>(Credentials.databaseURL + "/Inbox/" + fid + "/WorldCount.json", wcount);
                    RestClient.Put<WorldInfo>(Credentials.databaseURL + "/Inbox/" + fid + "/Manifest_" + winfo.worldNumber + ".json", winfo);
                    uploadWorldBlocks.Invoke();

                });
                RestClient.Get<WorldCount>(Credentials.databaseURL + "/Inbox/" + fid + "/WorldCount.json").Catch(req =>
                {
                    RestClient.Put<WorldCount>(Credentials.databaseURL + "/Inbox/" + fid + "/WorldCount.json", wcount);
                    winfo.worldNumber = wcount.worldNumber;
                    RestClient.Put<WorldInfo>(Credentials.databaseURL + "/Inbox/" + fid + "/Manifest_" + winfo.worldNumber + ".json", winfo);
                    uploadWorldBlocks.Invoke();
                });
                


                
            });
            RestClient.Get<Player>(Credentials.databaseURL + "/Players/Collection/" + fid + ".json").Catch(req =>
            {
                gameObject.GetComponent<Message>().ShowAlert("Error", "Player profile not founded", "Try", "Close", new System.Action(() => { gameObject.GetComponent<Online>().UploadWorld(file, fid); }), new System.Action(() => { gameObject.GetComponent<SaveLoad>().LoadUIClick(); }));
            });
        }
    }

    public void GetWorld(int worldNum)
    {
        int worldNumber = 0;
        var winfo = new WorldInfo();
        var cubes = new List<Cube>();

        RestClient.Get<WorldCount>(Credentials.databaseURL + "/Inbox/" + PlayerPrefs.GetString("id") + "/WorldCount.json").Done(req =>
        {
            worldNumber = req.worldNumber;
            RestClient.Get<WorldInfo>(Credentials.databaseURL + "/Inbox/" + PlayerPrefs.GetString("id") + "/Manifest_" + worldNum + ".json").Done(req2 =>
            {
                winfo = req2;
                RestClient.GetArray<Cube>(Credentials.databaseURL + "/Inbox/" + PlayerPrefs.GetString("id") + "/" + worldNum + ".json").Done(req3 =>
                {
                    cubes = req3.ToList();
                    RestClient.Delete(Credentials.databaseURL + "/Inbox/" + PlayerPrefs.GetString("id") + "/" + worldNum + ".json");
                    RestClient.Delete(Credentials.databaseURL + "/Inbox/" + PlayerPrefs.GetString("id") + "/Manifest_" + worldNum + ".json");
                    RestClient.Put<WorldCount>(Credentials.databaseURL + "/Inbox/" + PlayerPrefs.GetString("id") + "/WorldCount.json", worldNumber - 1);
                    File.WriteAllText(Application.persistentDataPath + "/Saves/" + winfo.creator + ";" + winfo.name + ".slife", JsonConvert.SerializeObject(cubes));
                });
            });
        });
    }
    public IEnumerator GetWorlds()
    {
        yield return new WaitForEndOfFrame();
        int worldNumber = 0;
        RestClient.Get<WorldCount>(Credentials.databaseURL + "/Inbox/" + PlayerPrefs.GetString("id") + "/WorldCount.json").Done(req =>
        {
            worldNumber = req.worldNumber;
            for(int i = 0;i<=worldNumber;i++)
            {
                GetWorld(i);
            }
        });
        yield return gameObject.GetComponent<SaveLoad>().UpdateLists();
    }

    public void RefreshSaveLoadMenu()
    {
        StartCoroutine(GetWorlds());
    }


    public void Copy(GameObject handle)
    {
        GUIUtility.systemCopyBuffer = MyData.playerId;
        musicControler.GetComponent<Sounds>().PlaySound(3);
    }

    public void Report()
    {
        BlockUIClick();
        string report = searchBox.GetComponent<InputField>().text;
        if (report == "" || report == PlayerPrefs.GetString("id"))
        {
            report = "yourself";
        }
        gameObject.GetComponent<Message>().ShowAlert("Warning", "Do you want to report this player\n(" + report + ")\nfor cheating?", "Yes", "No", new System.Action(() => { gameObject.GetComponent<Online>().ReportProcess(); }), new System.Action(() => { gameObject.GetComponent<Online>().BlockUIClick(); }));
    }

    public void ReportProcess()
    {
        musicControler.GetComponent<Sounds>().PlaySound(3);
        string report = searchBox.GetComponent<InputField>().text;
        if (report == "" || report == PlayerPrefs.GetString("id"))
        {
            gameObject.GetComponent<Message>().ShowAlert("Error", "You cannot report yourself", "Ok", "Close", new System.Action(() => { gameObject.GetComponent<Online>().BlockUIClick(); }), new System.Action(() => { gameObject.GetComponent<Online>().BlockUIClick(); }));

        }
        else
        {
            RestClient.Get<Player>(Credentials.databaseURL + "/Players/Collection/" + report + ".json").Done(req =>
            {
                gameObject.GetComponent<Message>().ShowAlert("Warning", "Player reported correctly.\nYour request will be reviewed as soon as possible", "Ok", "Close", new System.Action(() => { gameObject.GetComponent<Online>().BlockUIClick(); }), new System.Action(() => { gameObject.GetComponent<Online>().BlockUIClick(); }));
                var reporter = new PlayerReported();
                reporter.id = req.id;
                reporter.reporter = PlayerPrefs.GetString("id");
                RestClient.Put<Player>(Credentials.databaseURL + "/Players/Reported/" + report + ".json", reporter);
            });
            RestClient.Get<Player>(Credentials.databaseURL + "/Players/Collection/" + report + ".json").Catch(req =>
            {
                gameObject.GetComponent<Message>().ShowAlert("Error", "Player profile not founded", "Try", "Close", new System.Action(() => { gameObject.GetComponent<Online>().ReportProcess(); }), new System.Action(() => { gameObject.GetComponent<Online>().BlockUIClick(); }));
            });
        }
    }

    public void VerifyPlayerOnline(string id,Action online = null, Action offline = null)
    {
        RestClient.Get<Player>(Credentials.databaseURL + "/Players/Collection/" + id + ".json").Done(req =>
        {
            Thread.Sleep(2000);
            Debug.Log("Try 2");
            RestClient.Get<Player>(Credentials.databaseURL + "/Players/Collection/" + id + ".json").Done(req2 =>
            {
                if (req.activity != req2.activity) online?.Invoke();
                if (req.activity == req2.activity) offline?.Invoke();
            });
        });
    }

    [Serializable]
    public class PlayerReported
    {
        public string id = "";
        public string reporter = "";
    }
    [Serializable] // This makes the class able to be serialized into a JSON
    public class Player : IFireSerializable
    {
        public string id = "";
        public string activity = "";
        public string name = "";
        public int blocksPutted = 0;
        public int blocksChanged = 0;
        public string version = "";
        public string platform = "";
        public int worlds = 0;
        public int bronze = 0;
        public int bronzeT = 0;
        public int silver = 0;
        public int silverT = 0;
        public int gold = 0;
        public int level = 0;
        public int migration = 0;
        public int image = 0;

        public Player()
        {
            version = Application.version;
            platform = SystemInfo.operatingSystemFamily.ToString();
        }

        public string Get()
        {
            throw new NotImplementedException();
        }

        public void Set()
        {
            id = "@";
            id += Token.GenerateToken(Token.TokenForm.Id);
            activity = Token.GenerateToken(Token.TokenForm.Activity);
        }
    }
    [Serializable]
    public class LastConexion : IFireSerializable
    {
        public string second = "";
        public string minute = "";
        public string hour = "";
        public string day = "";
        public string month = "";
        public string year = "";

        public string Get() => year + ";" + ";" + month + ";" + day + ";" + hour + ";" + minute + ";" + second;

        public void Set()
        {
            year = DateTime.Now.Year.ToString();
            month = DateTime.Now.Month.ToString();
            day = DateTime.Now.Day.ToString();
            hour = DateTime.Now.Hour.ToString();
            minute = DateTime.Now.Minute.ToString();
            second = DateTime.Now.Second.ToString();
        }
    }

    public class FriendsSearcher : IFireSerializable
    {
        private string fid;
        public GameObject[] friendTag;
        public Sprite[] medals;
        public string Get() => fid;
        public Sprite[] playerSatusIcons;
        public GameObject[] skills;
        public GameObject canvas;
        public IEnumerator Search()
        {
            yield return new WaitForEndOfFrame();
            friendTag[1].GetComponent<TMPro.TextMeshProUGUI>().text = "Searching ...";
            friendTag[2].GetComponent<TMPro.TextMeshProUGUI>().text = "";
            skills[0].GetComponent<TMPro.TextMeshProUGUI>().text = "0 blocks";
            skills[1].GetComponent<TMPro.TextMeshProUGUI>().text = "0 last conection";
            skills[2].GetComponent<TMPro.TextMeshProUGUI>().text = "0 worlds";
            skills[3].GetComponent<TMPro.TextMeshProUGUI>().text = "0 bronze trophies";
            skills[4].GetComponent<TMPro.TextMeshProUGUI>().text = "0 silver trophies";
            skills[5].GetComponent<TMPro.TextMeshProUGUI>().text = "0 gold trophies";
            skills[6].GetComponent<TMPro.TextMeshProUGUI>().text = "0 xp levels";
            friendTag[3].GetComponent<Image>().sprite = null;
            friendTag[4].GetComponent<Image>().sprite = null;
            friendTag[5].GetComponent<Image>().sprite = null;
            Debug.Log("Try 1");
            RestClient.Get<Player>(Credentials.databaseURL + "/Players/Collection/" + fid + ".json").Done(req =>
            {
                friendTag[1].GetComponent<TMPro.TextMeshProUGUI>().text = req.name;
                friendTag[2].GetComponent<TMPro.TextMeshProUGUI>().text = req.id;
                skills[0].GetComponent<TMPro.TextMeshProUGUI>().text = req.blocksPutted.ToString() + " blocks";
                //string result;
                string[] lastTimeArray = req.activity.Split(';');
                skills[1].GetComponent<TMPro.TextMeshProUGUI>().text = lastTimeArray[2] + "/" + lastTimeArray[1] + "/" + lastTimeArray[0] + " last conection";
                skills[2].GetComponent<TMPro.TextMeshProUGUI>().text = req.worlds.ToString() + " worlds";
                skills[3].GetComponent<TMPro.TextMeshProUGUI>().text = req.bronze.ToString() + " bronze trophies";
                skills[4].GetComponent<TMPro.TextMeshProUGUI>().text = req.silver.ToString() + " silver trophies";
                skills[5].GetComponent<TMPro.TextMeshProUGUI>().text = req.gold.ToString() + " gold trophies";
                skills[6].GetComponent<TMPro.TextMeshProUGUI>().text = req.level.ToString() + " xp levels";
                try
                {
                    friendTag[5].GetComponent<Image>().sprite = canvas.GetComponent<ProfileImages>().GetProfileImage(req.image);
                }
                catch
                {
                    friendTag[5].GetComponent<Image>().sprite = canvas.GetComponent<ProfileImages>().GetProfileImage(0);
                }

                if (req.level >= 10000000)
                {
                    friendTag[4].GetComponent<Image>().sprite = medals[5];
                }
                else if (req.level >= 1000000)
                {
                    friendTag[4].GetComponent<Image>().sprite = medals[4];
                }
                else if (req.level >= 100000)
                {
                    friendTag[4].GetComponent<Image>().sprite = medals[3];
                }
                else if (req.level >= 10000)
                {
                    friendTag[4].GetComponent<Image>().sprite = medals[2];
                }
                else if (req.level >= 1000)
                {
                    friendTag[4].GetComponent<Image>().sprite = medals[1];
                }
                else if (req.level >= 100)
                {
                    friendTag[4].GetComponent<Image>().sprite = medals[0];
                }
                else if (req.level >= 75)
                {
                    friendTag[4].GetComponent<Image>().sprite = medals[6];
                }
                else if (req.level >= 50)
                {
                    friendTag[4].GetComponent<Image>().sprite = medals[7];
                }
                else if (req.level >= 25)
                {
                    friendTag[4].GetComponent<Image>().sprite = medals[8];
                }
                else if (req.level >= 10)
                {
                    friendTag[4].GetComponent<Image>().sprite = medals[9];
                }
                Debug.Log("Record " + req.blocksPutted);
                Thread.Sleep(2000);
                Debug.Log("Try 2");
                RestClient.Get<Player>(Credentials.databaseURL + "/Players/Collection/" + fid + ".json").Done(req2 =>
                {
                    if (req.activity != req2.activity) friendTag[3].GetComponent<Image>().sprite = playerSatusIcons[1]; else friendTag[3].GetComponent<Image>().sprite = playerSatusIcons[0];

                    //Detect if Self
                    if (req.id == PlayerPrefs.GetString("id"))
                    {
                        friendTag[1].GetComponent<TMPro.TextMeshProUGUI>().text = req.name + " (Self)";
                        friendTag[3].GetComponent<Image>().sprite = playerSatusIcons[1];
                    }
                });
            });
            RestClient.Get<Player>(Credentials.databaseURL + "/Players/Collection/" + fid + ".json").Catch(req =>
            {
                friendTag[5].GetComponent<Image>().sprite = canvas.GetComponent<ProfileImages>().GetProfileImage(0);
                friendTag[1].GetComponent<TMPro.TextMeshProUGUI>().text = "Player not founded";
                friendTag[2].GetComponent<TMPro.TextMeshProUGUI>().text = "Is the id correct?";

                //Easter eggs
                if (fid == "elpepe")
                {
                    friendTag[1].GetComponent<TMPro.TextMeshProUGUI>().text = "Ete setch";
                    friendTag[2].GetComponent<TMPro.TextMeshProUGUI>().text = "You found a easter egg :)";
                    canvas.GetComponent<EasterEggs>().EasterEggFounded("elpepe", EasterEggs.RewardType.Gold);
                }
                if (fid == "cubics")
                {
                    friendTag[1].GetComponent<TMPro.TextMeshProUGUI>().text = "Nice game!";
                    friendTag[2].GetComponent<TMPro.TextMeshProUGUI>().text = "You found a easter egg :)";
                    canvas.GetComponent<EasterEggs>().EasterEggFounded("cubics", EasterEggs.RewardType.Gold);

                }
                if (fid == "0/0")
                {
                    friendTag[1].GetComponent<TMPro.TextMeshProUGUI>().text = "Overloaded ALU :(";
                    friendTag[2].GetComponent<TMPro.TextMeshProUGUI>().text = "You found a easter egg :)";
                    canvas.GetComponent<EasterEggs>().EasterEggFounded("0/0", EasterEggs.RewardType.Gold);
                }
                if (fid == "Asierso Studio")
                {
                    friendTag[1].GetComponent<TMPro.TextMeshProUGUI>().text = "Daddy!";
                    friendTag[2].GetComponent<TMPro.TextMeshProUGUI>().text = "You found a easter egg :)";
                    canvas.GetComponent<EasterEggs>().EasterEggFounded("daddy", EasterEggs.RewardType.Gold);
                }
                if (fid == "etesetch")
                {
                    friendTag[1].GetComponent<TMPro.TextMeshProUGUI>().text = "Ete setch";
                    friendTag[2].GetComponent<TMPro.TextMeshProUGUI>().text = "You found a easter egg :)";
                    canvas.GetComponent<EasterEggs>().EasterEggFounded("elpepe", EasterEggs.RewardType.Gold);
                }
                if (fid == "free gold" || fid == "freegold")
                {
                    friendTag[1].GetComponent<TMPro.TextMeshProUGUI>().text = "Wish conceded";
                    friendTag[2].GetComponent<TMPro.TextMeshProUGUI>().text = "You found a easter egg :)";
                    canvas.GetComponent<EasterEggs>().EasterEggFounded("freegold", EasterEggs.RewardType.Gold);
                }
                if (fid == "Tuolio")
                {
                    friendTag[1].GetComponent<TMPro.TextMeshProUGUI>().text = "Shh, Do you know that?";
                    friendTag[2].GetComponent<TMPro.TextMeshProUGUI>().text = "You found a easter egg :)";
                    canvas.GetComponent<EasterEggs>().EasterEggFounded("tuolio", EasterEggs.RewardType.Gold);
                }
                if (fid == "0101001101100001011011100110010001101100011010010110011001100101" || fid == "0111001101100001011011100110010001101100011010010110011001100101")
                {
                    friendTag[1].GetComponent<TMPro.TextMeshProUGUI>().text = "Sandlife in binary";
                    friendTag[2].GetComponent<TMPro.TextMeshProUGUI>().text = "You found a easter egg :)";
                    canvas.GetComponent<EasterEggs>().EasterEggFounded("binarylife0", EasterEggs.RewardType.Gold);
                    canvas.GetComponent<EasterEggs>().EasterEggFounded("binarylife1", EasterEggs.RewardType.Gold);
                    canvas.GetComponent<EasterEggs>().EasterEggFounded("binarylife2", EasterEggs.RewardType.Gold);
                }
                if (fid == "Argon")
                {
                    friendTag[1].GetComponent<TMPro.TextMeshProUGUI>().text = "Shh, Do you know that?";
                    friendTag[2].GetComponent<TMPro.TextMeshProUGUI>().text = "You found a easter egg :)";
                    canvas.GetComponent<EasterEggs>().EasterEggFounded("argonProg", EasterEggs.RewardType.Gold);
                }
                if (fid == "12112020")
                {
                    friendTag[1].GetComponent<TMPro.TextMeshProUGUI>().text = "Sandlife alpha launch day";
                    friendTag[2].GetComponent<TMPro.TextMeshProUGUI>().text = "You found a easter egg :)";
                    canvas.GetComponent<EasterEggs>().EasterEggFounded("sandlifeLaunchDay", EasterEggs.RewardType.Gold);
                }
                friendTag[3].GetComponent<Image>().sprite = playerSatusIcons[0];
            });
        }

        public void Set()
        {
            throw new NotImplementedException();
        }

        public void Set(string fid) => this.fid = fid;
    }

    public static class Credentials
    {
        public static readonly string projectId = "FIREBASE_PROJECT_ID"; // You can find this in your Firebase project settings
        public static readonly string databaseURL = "FIREBASE_DATABASE_URL";
    }

    static class MyData
    {
        public static string playerId;
        public static string playerActivity;
        public static string playerName;
    }

    public interface IFireSerializable
    {
        string Get();
        void Set();
    }

    public class Token
    {
        public enum TokenForm { Id, Activity }
        public static string GenerateToken(TokenForm tokenForm = TokenForm.Id)
        {
            string returnVal = string.Empty;
            switch (tokenForm)
            {
                case TokenForm.Id:
                    returnVal = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString();
                    break;
                case TokenForm.Activity:
                    returnVal = DateTime.Now.Year.ToString() + ";" + DateTime.Now.Month.ToString() + ";" + DateTime.Now.Day.ToString() + ";" + DateTime.Now.Hour.ToString() + ";" + DateTime.Now.Minute.ToString() + ";" + DateTime.Now.Second.ToString();
                    break;
            }
            return returnVal;
        }
    }

    public class WorldInfo
    {
        public string name;
        public string creator;
        public int worldNumber;
    }
    public class WorldCount
    {
        public int worldNumber;
    }
}