using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Profile : MonoBehaviour
{
    public bool blockProfile = true;
    public GameObject[] profiles;
    private List<Vector2> profilePosition = new List<Vector2>();
    public GameObject musicControler;
    public GameObject button;
    public GameObject selectedImage;
    public List<GameObject> buttons = new List<GameObject>();
    public Sprite[] trophies;
    private int ProfileAmount = 0;
    // Start is called before the first frame update
    void Start()
    {
        int x = 0;
        int y = 0;
        selectedImage.GetComponent<Image>().sprite = gameObject.GetComponent<ProfileImages>().GetProfileImage(PlayerPrefs.GetInt("profileImage"));
        ProfileAmount = gameObject.GetComponent<ProfileImages>().ProfileImagesList.Length;
        foreach (GameObject handle in profiles)
        {
            profilePosition.Add(handle.transform.position);
            handle.transform.position = new Vector2(-1000, -1000);
        }

        for (int i = 0; i < ProfileAmount; i++)
        {
            buttons.Add(Instantiate(button, button.transform));
            //buttons[i].transform.position = new Vector2(button.transform.position.x + (75 * x), button.transform.position.y - (75 * y));
            buttons[i].transform.localPosition = new Vector2(2 / button.transform.localPosition.x + (65 * x), 2 / button.transform.localPosition.y - (75 * y));
            buttons[i].transform.name = i.ToString();
            buttons[i].transform.SetParent ( button.transform.parent);
            buttons[i].GetComponent<Image>().sprite = gameObject.GetComponent<ProfileImages>().GetProfileImage(i);
            x++;
            if (x == 6)
            {
                y++;
                x = 0;
            }
        }

        LoadIcons();
        Destroy(button);

        

        blockProfile = false;
    }

    void LoadIcons()
    {
        ProfileAmount = gameObject.GetComponent<ProfileImages>().ProfileImagesList.Length;
        for (int i = 0; i < ProfileAmount; i++)
        {
            if (gameObject.GetComponent<ProfileImages>().GetIsProfileUnlocked(i) == true)
            {
                Transform[] contents = buttons[i].GetComponentsInChildren<Transform>();
                contents.ToList().ForEach((obj) => { try { if (obj.GetComponent<TMPro.TextMeshProUGUI>().enabled == true) { obj.GetComponent<TMPro.TextMeshProUGUI>().text = gameObject.GetComponent<ProfileImages>().GetProfileName(i); } if (obj.GetComponent<Image>().enabled == true) { obj.GetComponent<Image>().color = new Color32(255, 255, 255, 0); } } catch { } });
                contents.ToList().ForEach((obj) => { try { if (obj.GetComponent<Image>().enabled == true && obj.name == "Image") { obj.GetComponent<Image>().color = new Color32(255, 255, 255, 0); } } catch { } });
                buttons[i].GetComponent<Button>().interactable = true;
            }
            else
            {
                var condition = gameObject.GetComponent<ProfileImages>().GetCondition(i);
                Transform[] contents = buttons[i].GetComponentsInChildren<Transform>();
                buttons[i].GetComponent<Button>().interactable = false;
                int needed = 0;
                switch(condition.trophieType)
                {
                    case ProfileImages.TrophieTypes.Bronze:needed = 0;break;
                    case ProfileImages.TrophieTypes.Silver:needed = 1;break;
                    case ProfileImages.TrophieTypes.Gold:needed = 2;break;
                }
                contents.ToList().ForEach((obj) => { try { if (obj.GetComponent<TMPro.TextMeshProUGUI>().enabled == true) { obj.GetComponent<TMPro.TextMeshProUGUI>().text = condition.amount.ToString(); } if (obj.GetComponent<Image>().enabled == true) { obj.GetComponent<Image>().sprite = trophies[needed]; obj.GetComponent<Image>().color = new Color32(255, 255, 255, 255); } } catch { } });
                contents.ToList().ForEach((obj) => { try { if (obj.GetComponent<Image>().enabled == true && obj.name == "Image") { obj.GetComponent<Image>().sprite = trophies[needed]; obj.GetComponent<Image>().color = new Color32(255, 255, 255, 255); } } catch { } });
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BlockUIClick()
    {
        musicControler.GetComponent<Sounds>().PlaySound(3);
        if (blockProfile == true)
        {
            foreach (GameObject handle in profiles)
            {
                handle.transform.position = new Vector2(-1000, -1000);
            }
            gameObject.GetComponent<Settings>().BlockUIClick();
            blockProfile = false;
            //gameObject.GetComponent<Controler>().focusObject.GetComponent<Focus>().show = false;
        }
        else
        {
            int i = 0;
            foreach (GameObject handle in profiles)
            {
                handle.transform.position = profilePosition[i];
                i++;
            }
            gameObject.GetComponent<Settings>().BlockUIClick();

            LoadIcons();
            blockProfile = true;
        }
    }
}
