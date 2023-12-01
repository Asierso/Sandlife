using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;

public class Ads : MonoBehaviour,IUnityAdsListener
{
    public GameObject adButton;
    public Sprite adButtonWindowsIcon;
    public GameObject adText;
    // Start is called before the first frame update
    void Start()
    {
        Advertisement.Initialize("4005201",false); //TestMode (true) (false)
        Advertisement.AddListener(this);
        if (SystemInfo.operatingSystemFamily == OperatingSystemFamily.Windows)
        {
            adButton.GetComponent<Button>().enabled = false;
            adButton.GetComponent<Image>().sprite = adButtonWindowsIcon;
            //adButton.GetComponent<Image>().color = new Color32(255,255,255,0);
            adText.GetComponent<TMPro.TextMeshProUGUI>().text = "Watch ads to earn 3 trophies of gold (only available in android)";
        }
    }

    // Update is called once per frame
    public void ShowAd()
    {
        if (Advertisement.IsReady("rewardedVideo"))
        {
            Advertisement.Show("rewardedVideo");
        }
    }

    public void OnUnityAdsReady(string placementId)
    {

    }

    public void OnUnityAdsDidError(string message)
    {

    }

    public void OnUnityAdsDidStart(string placementId)
    {

    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        switch(showResult)
        {
            case ShowResult.Finished:
                PlayerPrefs.SetInt("gold", PlayerPrefs.GetInt("gold") + 3);
                gameObject.GetComponent<UI>().musicControler.GetComponent<Sounds>().PlaySound(15);
                gameObject.GetComponent<Trophies>().BlockUIClick();
                gameObject.GetComponent<Message>().ShowAlert("Congratulations", "You earn 3 golden trophies", "Ok", "Close", new System.Action(() => { gameObject.GetComponent<Trophies>().BlockUIClick(); }), new System.Action(() => { gameObject.GetComponent<Trophies>().BlockUIClick(); }));
                break;
        }
    }
}
