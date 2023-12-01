using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trophies : MonoBehaviour
{
    public GameObject[] trophies;
    public GameObject musicControler;
    private List<float> trophiesX = new List<float>();
    private List<float> trophiesY = new List<float>();
    public bool blockTrophies = true;
    public TMPro.TextMeshProUGUI bronze;
    public TMPro.TextMeshProUGUI silver;
    public TMPro.TextMeshProUGUI gold;
    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject inventoryComponent in trophies)
        {
            trophiesX.Add(inventoryComponent.transform.position.x);
            trophiesY.Add(inventoryComponent.transform.position.y);
            inventoryComponent.transform.position = new Vector2(-1000, -1000);
        }
        blockTrophies = false;
    }

    // Update is called once per frame
    void Update()
    {
        bronze.text = PlayerPrefs.GetInt("bronze").ToString();
        silver.text = PlayerPrefs.GetInt("silver").ToString();
        if (bronze.text.ToInt() >= 100)
        {
            PlayerPrefs.SetInt("bronze", bronze.text.ToInt() - 100);
            PlayerPrefs.SetInt("silver", PlayerPrefs.GetInt("silver") + 1);
            PlayerPrefs.SetInt("bronzeTotal", PlayerPrefs.GetInt("bronzeTotal") + 100);
            PlayerPrefs.Save();
        }
        if (silver.text.ToInt() >= 100)
        {
            PlayerPrefs.SetInt("silver", silver.text.ToInt() - 100);
            PlayerPrefs.SetInt("gold", PlayerPrefs.GetInt("gold") + 1);
            PlayerPrefs.SetInt("silverTotal", PlayerPrefs.GetInt("silverTotal") + 100);
            PlayerPrefs.Save();
        }
        if (PlayerPrefs.GetInt("gold") >= 100)
        {
            gold.text = "+99";
        }
        else
        {
            gold.text = PlayerPrefs.GetInt("gold").ToString();
        }
        if (PlayerPrefs.GetInt("blocksCount") >= 100)
        {
            PlayerPrefs.SetInt("bronze", PlayerPrefs.GetInt("bronze") + 1);
            PlayerPrefs.SetInt("blocksCount", PlayerPrefs.GetInt("blocksCount") - 100);
        }
        if (PlayerPrefs.GetInt("commandCount") >= 100)
        {
            PlayerPrefs.SetInt("bronze", PlayerPrefs.GetInt("bronze") + 10);
            PlayerPrefs.SetInt("commandCount", PlayerPrefs.GetInt("commandCount") - 100);
        }
        if (PlayerPrefs.GetInt("worldCount") >= 50)
        {
            PlayerPrefs.SetInt("silver", PlayerPrefs.GetInt("silver") + 1);
            PlayerPrefs.SetInt("worldCount", PlayerPrefs.GetInt("worldCount") - 50);
        }
        if (PlayerPrefs.GetInt("switchCount") >= 100)
        {
            PlayerPrefs.SetInt("bronze", PlayerPrefs.GetInt("bronze") + 1);
            PlayerPrefs.SetInt("switchCount", PlayerPrefs.GetInt("switchCount") - 100);
        }
        if (PlayerPrefs.GetInt("eraseCount") >= 100)
        {
            PlayerPrefs.SetInt("bronze", PlayerPrefs.GetInt("bronze") + 1);
            PlayerPrefs.SetInt("eraseCount", PlayerPrefs.GetInt("eraseCount") - 100);
        }
        if (PlayerPrefs.GetInt("pistonCount") >= 50)
        {
            PlayerPrefs.SetInt("bronze", PlayerPrefs.GetInt("bronze") + 1);
            PlayerPrefs.SetInt("pistonCount", PlayerPrefs.GetInt("pistonCount") - 50);
        }
        if (PlayerPrefs.GetInt("computerCount") >= 100)
        {
            PlayerPrefs.SetInt("gold", PlayerPrefs.GetInt("gold") + 1);
            PlayerPrefs.SetInt("computerCount", PlayerPrefs.GetInt("computerCount") - 100);
        }
        if (PlayerPrefs.GetInt("detectorCount") >= 100)
        {
            PlayerPrefs.SetInt("silver", PlayerPrefs.GetInt("silver") + 1);
            PlayerPrefs.SetInt("detectorCount", PlayerPrefs.GetInt("detectorCount") - 100);
        }
        if (PlayerPrefs.GetInt("spongeCount") >= 100)
        {
            PlayerPrefs.SetInt("bronze", PlayerPrefs.GetInt("bronze") + 10);
            PlayerPrefs.SetInt("spongeCount", PlayerPrefs.GetInt("spongeCount") - 100);
        }
        if (PlayerPrefs.GetInt("portalCount") >= 100)
        {
            PlayerPrefs.SetInt("bronze", PlayerPrefs.GetInt("bronze") + 1);
            PlayerPrefs.SetInt("portalCount", PlayerPrefs.GetInt("portalCount") - 100);
        }
        if (PlayerPrefs.GetInt("pipesCount") >= 100)
        {
            PlayerPrefs.SetInt("silver", PlayerPrefs.GetInt("silver") + 1);
            PlayerPrefs.SetInt("pipesCount", PlayerPrefs.GetInt("pipesCount") - 100);
        }
        if (PlayerPrefs.GetInt("divideCount") >= 100)
        {
            PlayerPrefs.SetInt("gold", PlayerPrefs.GetInt("gold") + 1);
            PlayerPrefs.SetInt("divideCount", PlayerPrefs.GetInt("divideCount") - 100);
        }
        if (PlayerPrefs.GetInt("flowerpotCount") >= 100)
        {
            PlayerPrefs.SetInt("silver", PlayerPrefs.GetInt("silver") + 1);
            PlayerPrefs.SetInt("flowerpotCount", PlayerPrefs.GetInt("flowerpotCount") - 100);
        }
        if (PlayerPrefs.GetInt("voidCount") >= 100)
        {
            PlayerPrefs.SetInt("bronze", PlayerPrefs.GetInt("bronze") + 10);
            PlayerPrefs.SetInt("voidCount", PlayerPrefs.GetInt("voidCount") - 100);
        }
        if (PlayerPrefs.GetInt("uraniumCount") >= 100)
        {
            PlayerPrefs.SetInt("bronze", PlayerPrefs.GetInt("bronze") + 1);
            PlayerPrefs.SetInt("uraniumCount", PlayerPrefs.GetInt("uraniumCount") - 100);
        }
        if (PlayerPrefs.GetInt("gateCount") >= 100)
        {
            PlayerPrefs.SetInt("silver", PlayerPrefs.GetInt("silver") + 1);
            PlayerPrefs.SetInt("gateCount", PlayerPrefs.GetInt("gateCount") - 100);
        }
        if (PlayerPrefs.GetInt("fillCount") >= 10)
        {
            PlayerPrefs.SetInt("bronze", PlayerPrefs.GetInt("bronze") + 1);
            PlayerPrefs.SetInt("fillCount", PlayerPrefs.GetInt("fillCount") - 10);
        }
        if (PlayerPrefs.GetInt("tntCount") >= 100)
        {
            PlayerPrefs.SetInt("silver", PlayerPrefs.GetInt("silver") + 1);
            PlayerPrefs.SetInt("tntCount", PlayerPrefs.GetInt("tntCount") - 100);
        }
        if (PlayerPrefs.GetInt("slimeCount") >= 100)
        {
            PlayerPrefs.SetInt("silver", PlayerPrefs.GetInt("silver") + 1);
            PlayerPrefs.SetInt("slimeCount", PlayerPrefs.GetInt("slimeCount") - 100);
        }
        if (PlayerPrefs.GetInt("burnedCount") >= 100)
        {
            PlayerPrefs.SetInt("silver", PlayerPrefs.GetInt("silver") + 1);
            PlayerPrefs.SetInt("burnedCount", PlayerPrefs.GetInt("burnedCount") - 100);
        }
        if (PlayerPrefs.GetInt("acidCount") >= 100)
        {
            PlayerPrefs.SetInt("bronze", PlayerPrefs.GetInt("bronze") + 1);
            PlayerPrefs.SetInt("acidCount", PlayerPrefs.GetInt("acidCount") - 100);
        }
    }

    public void BlockUIClick()
    {
        musicControler.GetComponent<Sounds>().PlaySound(3);
        if (blockTrophies == true)
        {
            foreach (GameObject inventoryComponent in trophies)
            {
                inventoryComponent.transform.position = new Vector2(-1000, -1000);
            }
            blockTrophies = false;
            //gameObject.GetComponent<Controler>().focusObject.GetComponent<Focus>().show = false;
        }
        else
        {
            int i = 0;
            foreach (GameObject inventoryComponent in trophies)
            {
                inventoryComponent.transform.position = new Vector2(trophiesX[i], trophiesY[i]);
                i++;
            }
            blockTrophies = true;
        }
    }
}
