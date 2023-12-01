using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    public bool blockSettings = true;
    public GameObject musicControler;
    public GameObject[] settings;
    private List<float> settingsX = new List<float>();
    private List<float> settingsY = new List<float>();
    public GameObject[] gridBtn;
    public GameObject[] musicBtn;
    public GameObject[] soundBtn;
    public GameObject[] playerIdBtn;
    public GameObject[] focusEnabledBtn;
    public GameObject playerName;
    public GameObject[] shareSkillsBtn;
    public GameObject[] onlineModeBtn; 
    public GameObject[] destroyParticlesBtn;
    public GameObject[] realisticLightsBtn;
    public GameObject[] fastDialogsBtn;
    public GameObject[] postProcessingBtn;
    public GameObject[] antialiasingBtn;
    public GameObject[] migrateProfileBtn;
    public GameObject target;
    public Image playerImage;
    public GameObject postProcessUnit;
    // Start is called before the first frame update
    void Start()
    {
        //Default config
        if(!File.Exists(Application.persistentDataPath + "/config.dat"))
        {
            File.WriteAllText(Application.persistentDataPath + "/config.dat", Application.version);

            //Configs
            PlayerPrefs.SetInt("grid", 0);
            PlayerPrefs.SetInt("music", 1);
            PlayerPrefs.SetInt("sounds", 1);
            PlayerPrefs.SetInt("showPlayerId", 1);
            PlayerPrefs.SetInt("focusEnabled", 1);
            PlayerPrefs.SetInt("shareSkills", 1);
            PlayerPrefs.SetInt("onlineMode", 1);
            PlayerPrefs.SetInt("destroyParticles", 1);
            PlayerPrefs.SetInt("realisticLights", 1);
            PlayerPrefs.SetInt("migrateProfile", 0);
            PlayerPrefs.SetInt("profileImage", 0);
            PlayerPrefs.SetInt("fastDialogs", 1);
            PlayerPrefs.SetInt("postProcessing", 0);
            PlayerPrefs.SetInt("antialiasing", 1); 
            PlayerPrefs.SetString("name","Guest");
            PlayerPrefs.Save();
        }
        else if(File.ReadAllText(Application.persistentDataPath + "/config.dat") != Application.version)
        {
            //Configs
            PlayerPrefs.SetInt("grid", 0);
            PlayerPrefs.SetInt("music", 1);
            PlayerPrefs.SetInt("sounds", 1);
            PlayerPrefs.SetInt("showPlayerId", 1);
            PlayerPrefs.SetInt("focusEnabled", 1);
            PlayerPrefs.SetString("name", "Guest");
            PlayerPrefs.SetInt("shareSkills", 1);
            PlayerPrefs.SetInt("onlineMode", 1);
            PlayerPrefs.SetInt("destroyParticles", 1);
            PlayerPrefs.SetInt("realisticLights", 1);
            PlayerPrefs.SetInt("migrateProfile", 0);
            PlayerPrefs.SetInt("profileImage", 0);
            PlayerPrefs.SetInt("fastDialogs", 1);
            PlayerPrefs.SetInt("postProcessing", 0);
            PlayerPrefs.SetInt("antialiasing", 1);
            PlayerPrefs.Save();
            Debug.Log("New version charged");
            File.WriteAllText(Application.persistentDataPath + "/config.dat", Application.version);
        }
        foreach (GameObject inventoryComponent in settings)
        {
            settingsX.Add(inventoryComponent.transform.position.x);
            settingsY.Add(inventoryComponent.transform.position.y);
            inventoryComponent.transform.position = new Vector2(-1000, -1000);
        }

        playerImage.sprite = gameObject.GetComponent<ProfileImages>().GetProfileImage(PlayerPrefs.GetInt("profileImage"));

        blockSettings = false;
        //version.text = string.Format("Sandlife v{0} beta (experimental)",Application.version);
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerPrefs.GetInt("grid") == 0)
        {
            gridBtn[1].GetComponent<Image>().color = new Color32(255, 255, 255, 255);
            gridBtn[0].GetComponent<Image>().color = new Color32(100, 100, 100, 255);
            gameObject.GetComponent<GridModifier>().Grid(false);
        }
        else
        {
            gridBtn[1].GetComponent<Image>().color = new Color32(100, 100, 100, 255);
            gridBtn[0].GetComponent<Image>().color = new Color32(255, 255, 255, 255);
            gameObject.GetComponent<GridModifier>().Grid(true);
        }

        if (PlayerPrefs.GetInt("music") == 0)
        {
            musicBtn[1].GetComponent<Image>().color = new Color32(255, 255, 255, 255);
            musicBtn[0].GetComponent<Image>().color = new Color32(100, 100, 100, 255);
        }
        else
        {
            musicBtn[1].GetComponent<Image>().color = new Color32(100, 100, 100, 255);
            musicBtn[0].GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        }

        if (PlayerPrefs.GetInt("sounds") == 0)
        {
            soundBtn[1].GetComponent<Image>().color = new Color32(255, 255, 255, 255);
            soundBtn[0].GetComponent<Image>().color = new Color32(100, 100, 100, 255);
        }
        else
        {
            soundBtn[1].GetComponent<Image>().color = new Color32(100, 100, 100, 255);
            soundBtn[0].GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        }

        if (PlayerPrefs.GetInt("showPlayerId") == 0)
        {
            playerIdBtn[1].GetComponent<Image>().color = new Color32(255, 255, 255, 255);
            playerIdBtn[0].GetComponent<Image>().color = new Color32(100, 100, 100, 255);
        }
        else
        {
            playerIdBtn[1].GetComponent<Image>().color = new Color32(100, 100, 100, 255);
            playerIdBtn[0].GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        }

        if (PlayerPrefs.GetInt("focusEnabled") == 0)
        {
            focusEnabledBtn[1].GetComponent<Image>().color = new Color32(255, 255, 255, 255);
            focusEnabledBtn[0].GetComponent<Image>().color = new Color32(100, 100, 100, 255);
        }
        else
        {
            focusEnabledBtn[1].GetComponent<Image>().color = new Color32(100, 100, 100, 255);
            focusEnabledBtn[0].GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        }
        if (PlayerPrefs.GetInt("shareSkills") == 0)
        {
            shareSkillsBtn[1].GetComponent<Image>().color = new Color32(255, 255, 255, 255);
            shareSkillsBtn[0].GetComponent<Image>().color = new Color32(100, 100, 100, 255);
        }
        else
        {
            shareSkillsBtn[1].GetComponent<Image>().color = new Color32(100, 100, 100, 255);
            shareSkillsBtn[0].GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        }

        if (PlayerPrefs.GetInt("onlineMode") == 0)
        {
            onlineModeBtn[1].GetComponent<Image>().color = new Color32(255, 255, 255, 255);
            onlineModeBtn[0].GetComponent<Image>().color = new Color32(100, 100, 100, 255);
        }
        else
        {
            onlineModeBtn[1].GetComponent<Image>().color = new Color32(100, 100, 100, 255);
            onlineModeBtn[0].GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        }

        if (PlayerPrefs.GetInt("destroyParticles") == 0)
        {
            destroyParticlesBtn[1].GetComponent<Image>().color = new Color32(255, 255, 255, 255);
            destroyParticlesBtn[0].GetComponent<Image>().color = new Color32(100, 100, 100, 255);
        }
        else
        {
            destroyParticlesBtn[1].GetComponent<Image>().color = new Color32(100, 100, 100, 255);
            destroyParticlesBtn[0].GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        }

        if (PlayerPrefs.GetInt("realisticLights") == 0)
        {
            realisticLightsBtn[1].GetComponent<Image>().color = new Color32(255, 255, 255, 255);
            realisticLightsBtn[0].GetComponent<Image>().color = new Color32(100, 100, 100, 255);
        }
        else
        {
            realisticLightsBtn[1].GetComponent<Image>().color = new Color32(100, 100, 100, 255);
            realisticLightsBtn[0].GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        }
        if (PlayerPrefs.GetInt("fastDialogs") == 0)
        {
            fastDialogsBtn[1].GetComponent<Image>().color = new Color32(255, 255, 255, 255);
            fastDialogsBtn[0].GetComponent<Image>().color = new Color32(100, 100, 100, 255);
        }
        else
        {
            fastDialogsBtn[1].GetComponent<Image>().color = new Color32(100, 100, 100, 255);
            fastDialogsBtn[0].GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        }
        if (PlayerPrefs.GetInt("postProcessing") == 0)
        {
            postProcessingBtn[1].GetComponent<Image>().color = new Color32(255, 255, 255, 255);
            postProcessingBtn[0].GetComponent<Image>().color = new Color32(100, 100, 100, 255);
        }
        else
        {
            postProcessingBtn[1].GetComponent<Image>().color = new Color32(100, 100, 100, 255);
            postProcessingBtn[0].GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        }
        if (PlayerPrefs.GetInt("antialiasing") == 0)
        {
            antialiasingBtn[1].GetComponent<Image>().color = new Color32(255, 255, 255, 255);
            antialiasingBtn[0].GetComponent<Image>().color = new Color32(100, 100, 100, 255);
        }
        else
        {
            antialiasingBtn[1].GetComponent<Image>().color = new Color32(100, 100, 100, 255);
            antialiasingBtn[0].GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        }

        if (blockSettings == true)
        {
            if(playerName.GetComponent<InputField>().text != PlayerPrefs.GetString("name"))
            {
                PlayerPrefs.SetString("name", playerName.GetComponent<InputField>().text);
            }
        }
        else
        {
            playerName.GetComponent<InputField>().text = PlayerPrefs.GetString("name");
        }

        //Config applyments
        if (PlayerPrefs.GetInt("showPlayerId") == 0)
        {
            gameObject.GetComponent<Online>().PlayerIdText.GetComponent<TMPro.TextMeshProUGUI>().color = new Color32(255,255,255, 0);
        }
        else
        {
            gameObject.GetComponent<Online>().PlayerIdText.GetComponent<TMPro.TextMeshProUGUI>().color = new Color32(223,100,255, 255);
        }
    }

    public void BlockUIClick()
    {
        musicControler.GetComponent<Sounds>().PlaySound(3);
        if (blockSettings == true)
        {
            foreach (GameObject inventoryComponent in settings)
            {
                inventoryComponent.transform.position = new Vector2(-1000, -1000);
            }
            blockSettings = false;
            //gameObject.GetComponent<Controler>().focusObject.GetComponent<Focus>().show = false;
        }
        else
        {
            int i = 0;
            foreach (GameObject inventoryComponent in settings)
            {
                inventoryComponent.transform.position = new Vector2(settingsX[i], settingsY[i]);
                i++;
            }
            blockSettings = true;
            //gameObject.GetComponent<Controler>().focusObject.GetComponent<Focus>().show = true;
        }
    }

    public void GridYes()
    {
        PlayerPrefs.SetInt("grid", 1);
        PlayerPrefs.Save();
        musicControler.GetComponent<Sounds>().PlaySound(3);
    }

    public void GridNo()
    {
        PlayerPrefs.SetInt("grid", 0);
        PlayerPrefs.Save();
        musicControler.GetComponent<Sounds>().PlaySound(3);
    }
    public void MusicYes()
    {
        PlayerPrefs.SetInt("music", 1);
        PlayerPrefs.Save();
        musicControler.GetComponent<Sounds>().PlaySound(3);
    }

    public void MusicNo()
    {
        PlayerPrefs.SetInt("music", 0);
        PlayerPrefs.Save();
        musicControler.GetComponent<Sounds>().PlaySound(3);
    }
    public void SoundYes()
    {
        PlayerPrefs.SetInt("sounds", 1);
        PlayerPrefs.Save();
        musicControler.GetComponent<Sounds>().PlaySound(3);
    }

    public void SoundNo()
    {
        PlayerPrefs.SetInt("sounds", 0);
        PlayerPrefs.Save();
        musicControler.GetComponent<Sounds>().PlaySound(3);
    }

    public void PlayerIdYes()
    {
        PlayerPrefs.SetInt("showPlayerId", 1);
        PlayerPrefs.Save();
        musicControler.GetComponent<Sounds>().PlaySound(3);
    }

    public void PlayerIdNo()
    {
        PlayerPrefs.SetInt("showPlayerId", 0);
        PlayerPrefs.Save();
        musicControler.GetComponent<Sounds>().PlaySound(3);
    }

    public void FocusEnabledYes()
    {
        PlayerPrefs.SetInt("focusEnabled", 1);
        PlayerPrefs.Save();
        musicControler.GetComponent<Sounds>().PlaySound(3);
    }

    public void FocusEnabledNo()
    {
        PlayerPrefs.SetInt("focusEnabled", 0);
        PlayerPrefs.Save();
        musicControler.GetComponent<Sounds>().PlaySound(3);
    }

    public void ShareSkillsYes()
    {
        PlayerPrefs.SetInt("shareSkills", 1);
        PlayerPrefs.Save();
        musicControler.GetComponent<Sounds>().PlaySound(3);
    }

    public void ShareSkillsNo()
    {
        PlayerPrefs.SetInt("shareSkills", 0);
        PlayerPrefs.Save();
        musicControler.GetComponent<Sounds>().PlaySound(3);
    }

    public void OnlineModeYes()
    {
        PlayerPrefs.SetInt("onlineMode", 1);
        PlayerPrefs.Save();
        musicControler.GetComponent<Sounds>().PlaySound(3);
    }

    public void OnlineModeNo()
    {
        PlayerPrefs.SetInt("onlineMode", 0);
        PlayerPrefs.Save();
        musicControler.GetComponent<Sounds>().PlaySound(3);
    }

    public void DestroyParticlesYes()
    {
        PlayerPrefs.SetInt("destroyParticles", 1);
        PlayerPrefs.Save();
        musicControler.GetComponent<Sounds>().PlaySound(3);
    }

    public void DestroyParticlesNo()
    {
        PlayerPrefs.SetInt("destroyParticles", 0);
        PlayerPrefs.Save();
        musicControler.GetComponent<Sounds>().PlaySound(3);
    }

    public void RealisticLightsYes()
    {
        PlayerPrefs.SetInt("realisticLights", 1);
        PlayerPrefs.Save();
        musicControler.GetComponent<Sounds>().PlaySound(3);
    }

    public void RealisticLightsNo()
    {
        PlayerPrefs.SetInt("realisticLights", 0);
        PlayerPrefs.Save();
        musicControler.GetComponent<Sounds>().PlaySound(3);
    }

    public void MigrateProfile()
    {
        blockSettings = true;
        BlockUIClick();
        gameObject.GetComponent<Message>().ShowAlert("Warning", "Are you sure you want to migrate\nyour profile with your progress\nto another device", "Yes", "No", new System.Action(() => { PlayerPrefs.SetInt("migrateProfile", 1); gameObject.GetComponent<Settings>().BlockUIClick(); }), new System.Action(() => { gameObject.GetComponent<Settings>().BlockUIClick(); }));
    }

    public void ChangeProfileImage(GameObject handle)
    {
        /*
        int image = PlayerPrefs.GetInt("profileImage");
        
        image++;
        if(image >= gameObject.GetComponent<ProfileImages>().ProfileImagesList.Length)
        {
            image = 0;
        }
        */
        PlayerPrefs.SetInt("profileImage",Convert.ToInt32(handle.name));
        playerImage.sprite = gameObject.GetComponent<ProfileImages>().GetProfileImage(Convert.ToInt32(handle.name));
        gameObject.GetComponent<Profile>().selectedImage.GetComponent<Image>().sprite = gameObject.GetComponent<ProfileImages>().GetProfileImage(Convert.ToInt32(handle.name));
        musicControler.GetComponent<Sounds>().PlaySound(3);
    }

    public void LoadProfile()
    {
        blockSettings = true;
        BlockUIClick();
        gameObject.GetComponent<Message>().ShowAlert("Warning", "Are you sure you want to use\nthis profile (" + gameObject.GetComponent<Settings>().target.GetComponent<InputField>().text + ") on your device \n(You will lose your previous progress)", "Yes", "No", new System.Action(() => { gameObject.GetComponent<Online>().TryMigrate(gameObject.GetComponent<Settings>().target.GetComponent<InputField>().text); }), new System.Action(() => { gameObject.GetComponent<Settings>().BlockUIClick(); }));
        
    }

    public void DialogsYes()
    {
        PlayerPrefs.SetInt("fastDialogs", 1);
        PlayerPrefs.Save();
        musicControler.GetComponent<Sounds>().PlaySound(3);
    }

    public void DialogsNo()
    {
        PlayerPrefs.SetInt("fastDialogs", 0);
        PlayerPrefs.Save();
        musicControler.GetComponent<Sounds>().PlaySound(3);
    }

    public void PostProcessingYes()
    {
        PlayerPrefs.SetInt("postProcessing", 1);
        PlayerPrefs.Save();
        postProcessUnit.GetComponent<PostProcessing>().CheckOption();
        musicControler.GetComponent<Sounds>().PlaySound(3);
    }

    public void PostProcessingNo()
    {
        PlayerPrefs.SetInt("postProcessing", 0);
        PlayerPrefs.Save();
        postProcessUnit.GetComponent<PostProcessing>().CheckOption();
        musicControler.GetComponent<Sounds>().PlaySound(3);
    }

    public void AntialiasingYes()
    {
        PlayerPrefs.SetInt("antialiasing", 1);
        PlayerPrefs.Save();
        postProcessUnit.GetComponent<PostProcessing>().CheckOption();
        musicControler.GetComponent<Sounds>().PlaySound(3);
    }

    public void AntialiasingNo()
    {
        PlayerPrefs.SetInt("antialiasing", 0);
        PlayerPrefs.Save();
        postProcessUnit.GetComponent<PostProcessing>().CheckOption();
        musicControler.GetComponent<Sounds>().PlaySound(3);
    }
}
