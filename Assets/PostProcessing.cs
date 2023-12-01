using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PostProcessing : MonoBehaviour
{
    public Camera cam;
    // Update is called once per frame
    void Start()
    {
        CheckOption();
    }

    void Update()
    {
        if(cam.backgroundColor.r < .7f && cam.backgroundColor.g < .7f && cam.backgroundColor.b < .7f)
        {
            gameObject.GetComponent<PostProcessVolume>().profile.GetSetting<Bloom>().active = true;

        }
        else
        {
            //gameObject.GetComponent<PostProcessVolume>().isGlobal = false;
            gameObject.GetComponent<PostProcessVolume>().profile.GetSetting<Bloom>().active = false;

        }
    }
    public void CheckOption()
    {
        if (PlayerPrefs.GetInt("postProcessing") == 1) gameObject.GetComponent<PostProcessVolume>().isGlobal = true;
        else gameObject.GetComponent<PostProcessVolume>().isGlobal = false;
        if (PlayerPrefs.GetInt("antiAliasing") == 1) cam.GetComponent<PostProcessLayer>().antialiasingMode = PostProcessLayer.Antialiasing.SubpixelMorphologicalAntialiasing;
        else cam.GetComponent<PostProcessLayer>().antialiasingMode = PostProcessLayer.Antialiasing.None;
    }
}
