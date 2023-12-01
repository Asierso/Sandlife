using Newtonsoft.Json;
using Proyecto26;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Gamebox : MonoBehaviour
{
    private string gameboxId = "";
    private float updateDelta = 0f;
    // Start is called before the first frame update
    void Start()
    {
        if (Application.internetReachability != NetworkReachability.NotReachable)
        {
            int total = 0;
            int totalAfterMod = 0;

            RestClient.Get<GameboxTotal>(Credentials.databaseURL + "/Gamebox/Amount.json").Done(req =>
            {
                total = req.amount;
                totalAfterMod = req.amount;
                for (int i = 0; i < total + 1; i++)
                {
                    RestClient.Get<GameboxExpirationContext>(Credentials.databaseURL + "/Gamebox/Expires/" + i + ".json").Done(req2 =>
                    {
                        if (req2.expireDay != ExpireDateGeneration())
                        {
                            RestClient.Delete(Credentials.databaseURL + "/Gamebox/Instances/" + req2.referenceName + ".json");
                            totalAfterMod--;
                        }
                    });
                }
                RestClient.Put<GameboxTotal>(Credentials.databaseURL + "/Gamebox/Amount.json", new GameboxTotal() { amount = totalAfterMod });
            });

        }
    }

    public void GameboxOptionsClick()
    {
        gameObject.GetComponent<SaveLoad>().LoadUIClick();
        if (Application.internetReachability != NetworkReachability.NotReachable)
        {
            gameObject.GetComponent<Message>().ShowAlert("Gamebox", "Share and see worlds globally", "Share", "Load", new System.Action(() => { gameObject.GetComponent<UI>().musicControler.GetComponent<Sounds>().PlaySound(3); gameboxId = gameObject.GetComponent<Message>().input.text; CreateGamebox(); StartCoroutine(PostBlocks()); }), new System.Action(() => { gameObject.GetComponent<UI>().musicControler.GetComponent<Sounds>().PlaySound(3); gameboxId = gameObject.GetComponent<Message>().input.text; StartCoroutine(GetBlocks()); }), true, "Enter gamebox code");
        }
        else
        {
            gameObject.GetComponent<Message>().ShowAlert("Error", "Could not connect to the server", "Try", "Close", new System.Action(() => { gameObject.GetComponent<UI>().musicControler.GetComponent<Sounds>().PlaySound(3); GameboxOptionsClick(); }), new System.Action(() => { gameObject.GetComponent<SaveLoad>().LoadUIClick(); }));
        }
    }

    void CreateGamebox()
    {
        if (gameboxId != "")
        {
            int expireDay = ExpireDateGeneration();
            RestClient.Get<GameboxRoom>(Credentials.databaseURL + "/Gamebox/Instances/" + gameboxId + ".json").Done(req =>
            {
                gameObject.GetComponent<Message>().ShowAlert("Error", "Could not create your gamebox", "Ok", "Close", new Action(() => { gameObject.GetComponent<UI>().musicControler.GetComponent<Sounds>().PlaySound(3); gameObject.GetComponent<SaveLoad>().LoadUIClick(); }), new System.Action(() => { gameObject.GetComponent<UI>().musicControler.GetComponent<Sounds>().PlaySound(3); gameObject.GetComponent<SaveLoad>().LoadUIClick(); }));
            });
            RestClient.Get<GameboxRoom>(Credentials.databaseURL + "/Gamebox/Instances/" + gameboxId + ".json").Catch(req =>
            {
                var gamebox = new GameboxRoom()
                {
                    author = PlayerPrefs.GetString("id"),
                    expireDay = expireDay
                };
                RestClient.Put<GameboxRoom>(Credentials.databaseURL + "/Gamebox/Instances/" + gameboxId + ".json", gamebox);
                StartCoroutine(PostBlocks());
            });
            int total = 0;
            RestClient.Get<GameboxTotal>(Credentials.databaseURL + "/Gamebox/Amount.json").Done(req =>
            {
                total = req.amount + 1;
                var expirationContext = new GameboxExpirationContext()
                {
                    expireDay = expireDay,
                    referenceName = gameboxId
                };
                RestClient.Put<GameboxExpirationContext>(Credentials.databaseURL + "/Gamebox/Expires/" + total + ".json", expirationContext);
                RestClient.Put<GameboxTotal>(Credentials.databaseURL + "/Gamebox/Amount.json", new GameboxTotal() { amount = total });
            });
            RestClient.Get<GameboxTotal>(Credentials.databaseURL + "/Gamebox/Amount.json").Catch(req =>
            {
                var expirationContext = new GameboxExpirationContext()
                {
                    expireDay = expireDay,
                    referenceName = gameboxId
                };
                RestClient.Put<GameboxExpirationContext>(Credentials.databaseURL + "/Gamebox/Expires/" + total + ".json", expirationContext);
                RestClient.Put<GameboxTotal>(Credentials.databaseURL + "/Gamebox/Amount.json", new GameboxTotal() { amount = total });
            });
        }
        else
        {
            gameObject.GetComponent<Message>().ShowAlert("Error", "Could not create your gamebox", "Ok", "Close", new Action(() => { gameObject.GetComponent<UI>().musicControler.GetComponent<Sounds>().PlaySound(3); gameObject.GetComponent<SaveLoad>().LoadUIClick(); }), new System.Action(() => { gameObject.GetComponent<UI>().musicControler.GetComponent<Sounds>().PlaySound(3); gameObject.GetComponent<SaveLoad>().LoadUIClick(); }));
        }
        /*
        var expirationContext = new GameboxExpirationContext()
        {
            expireDay = expireDay,
            referenceName = gameboxId
        };
        RestClient.Put<GameboxExpirationContext>(Credentials.databaseURL + "/Gamebox/Expires/0.json", expirationContext);*/
    }

    private int ExpireDateGeneration()
    {
        int yy = DateTime.Now.Year;
        int dd = DateTime.Now.DayOfYear;

        dd++;
        if(dd > 365 && !DateTime.IsLeapYear(yy))
        {
            dd = 0;
        }
        else if (dd > 366 && DateTime.IsLeapYear(yy))
        {
            dd = 0;
        }
        return dd;
    }

    IEnumerator GetBlocks()
    {
        if (gameboxId != "")
        {
            var cubes = new List<Cube>();
            Debug.Log("Reading");
            RestClient.Get<GameboxRoom>(Credentials.databaseURL + "/Gamebox/Instances/" + gameboxId + ".json").Done(req =>
            {
            //RestClient.Put<GameboxRoom>(Credentials.databaseURL + "/Gamebox/Instances/" + gameboxId + ".json", req);
            RestClient.GetArray<Cube>(Credentials.databaseURL + "/Gamebox/Instances/" + gameboxId + "/Blocks.json").Done(req =>
            {
                        gameObject.GetComponent<UI>().EraseAll();
                        IEnumerator Loader()
                        {
                            Debug.Log("Reading done");

                            cubes = req.ToList();
                            int i = 0;
                            gameObject.GetComponent<Controler>().cubeObjects.Clear();
                            foreach (var cube in cubes)
                            {
                                Debug.Log("Block " + i + "with id: " + cube.blockID);
                                GameObject cubeInstancied = Instantiate(gameObject.GetComponent<Controler>().Cube, new Vector3(cube.posX, cube.posY, cube.posZ), gameObject.GetComponent<Controler>().Cube.transform.rotation);
                                cubeInstancied.GetComponent<Block>().blockID = cube.blockID;
                                cubeInstancied.GetComponent<Block>().blockStatus = cube.blockStatus;
                                cubeInstancied.GetComponent<Block>().internalInfo = cube.internalInfo;
                                if (cubeInstancied.GetComponent<Block>().blockID >= 0)
                                {
                                    cubeInstancied.GetComponent<SpriteRenderer>().sprite = gameObject.GetComponent<Controler>().BlockSprites[cube.blockID];
                                }
                                else if (cubeInstancied.GetComponent<Block>().blockID == -2)
                                {
                                    gameObject.GetComponent<SaveLoad>().CustomBgColorLoad(cubeInstancied);
                                }
                                gameObject.GetComponent<Controler>().cubeObjects.Add(cubeInstancied);
                                i++;
                                yield return new WaitForSeconds(0.02f);
                            }
                        }
                        StartCoroutine(Loader());
                    });
                RestClient.GetArray<Cube>(Credentials.databaseURL + "/Gamebox/Instances/" + gameboxId + "/Blocks.json").Catch(req =>
            {
                        RestClient.Delete(Credentials.databaseURL + "/Gamebox/Instances/" + gameboxId + ".json");
                    });
            });
            RestClient.Get<GameboxRoom>(Credentials.databaseURL + "/Gamebox/Instances/" + gameboxId + ".json").Catch(req =>
            {
                gameObject.GetComponent<Message>().ShowAlert("Error", "Could not find your gamebox", "Ok", "Close", new Action(() => { gameObject.GetComponent<UI>().musicControler.GetComponent<Sounds>().PlaySound(3); gameObject.GetComponent<SaveLoad>().LoadUIClick(); }), new System.Action(() => { gameObject.GetComponent<UI>().musicControler.GetComponent<Sounds>().PlaySound(3); gameObject.GetComponent<SaveLoad>().LoadUIClick(); }));

            });
            yield return new WaitForEndOfFrame();
        }
        else
        {
            gameObject.GetComponent<Message>().ShowAlert("Error", "Could not find your gamebox", "Ok", "Close", new Action(() => { gameObject.GetComponent<UI>().musicControler.GetComponent<Sounds>().PlaySound(3); gameObject.GetComponent<SaveLoad>().LoadUIClick(); }), new System.Action(() => { gameObject.GetComponent<UI>().musicControler.GetComponent<Sounds>().PlaySound(3); gameObject.GetComponent<SaveLoad>().LoadUIClick(); }));
        }
    }

    IEnumerator PostBlocks()
    {
        int i = 0;
        foreach (var cube in gameObject.GetComponent<Controler>().cubeObjects)
        {
            yield return new WaitForSeconds(0.1f);
            var parsedCube = new Cube(cube.GetComponent<Block>().blockID, cube.transform.position.x, cube.transform.position.y, cube.transform.position.z, cube.GetComponent<Block>().blockStatus, cube.GetComponent<Block>().internalInfo, cube.transform.rotation.eulerAngles.z);
            RestClient.Put<Cube>(Credentials.databaseURL + "/Gamebox/Instances/" + gameboxId + "/Blocks/" + i + ".json", parsedCube);
            i++;
        }

        yield return new WaitForEndOfFrame();
    }

    private class GameboxRoom
    {
        public string author = "";
        public int expireDay = 0;
    }
    private class GameboxExpirationContext
    {
        public string referenceName = "";
        public int expireDay = 0;
    }
    public class GameboxTotal
    {
        public int amount = 0;
    }
    public static class Credentials
    {
        public static readonly string projectId = "sandlife-3ab0f-default-rtdb"; // You can find this in your Firebase project settings
        public static readonly string databaseURL = "https://sandlife-3ab0f-default-rtdb.europe-west1.firebasedatabase.app/";
    }

}
