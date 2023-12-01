using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class Capture : MonoBehaviour
{
    //Unused code about screenshots
    public GameObject dock;
    public void TakePhoto()
    {
        if(!Directory.Exists(Application.persistentDataPath + "Photos"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/Photos");
        }
        StartCoroutine(takeCaputure());
    }
    public void DeletePhoto(string file)
    {
        try
        {
            File.Delete(Application.persistentDataPath + "/Photos/" + file);
        }
        catch
        {

        }
    }
    public string GetPhoto(string worldname)
    {
        try
        {
            return Path.GetFileNameWithoutExtension(worldname) + ".png";
        }
        catch
        {
            return string.Empty;
        }
    }
    public Sprite LoadPhoto(string file)
    {
        byte[] bytes = File.ReadAllBytes(Application.persistentDataPath + "/Photos/" + file);
        Texture2D tex = new Texture2D(2, 2);
        tex.LoadImage(bytes);
        return Sprite.Create(tex, new Rect(0, 0, Screen.width, Screen.height), new Vector2(0f, 5f));
    }
    IEnumerator takeCaputure()
    {
        gameObject.GetComponent<Dock>().DockUI();
        gameObject.GetComponent<Dock>().DockBtn.GetComponent<Image>().color = new Color32(255, 255, 255, 0);
        yield return new WaitForSeconds(0.1f);
        try
        {
            if(SystemInfo.operatingSystemFamily == OperatingSystemFamily.Windows)
            {
                ScreenCapture.CaptureScreenshot(Application.persistentDataPath + "/Photos/" + Path.GetFileNameWithoutExtension(gameObject.GetComponent<SaveLoad>().currentFile) + ".png");
            }
            else
            {
                ScreenCapture.CaptureScreenshot("/Photos/" + Path.GetFileNameWithoutExtension(gameObject.GetComponent<SaveLoad>().currentFile) + ".png");
            }
        }
        catch
        {

        }
        yield return new WaitForSeconds(0.1f);
        gameObject.GetComponent<Dock>().DockUI();
        gameObject.GetComponent<Dock>().DockBtn.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
    }
}
