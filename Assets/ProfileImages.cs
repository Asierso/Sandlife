using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProfileImages : MonoBehaviour
{
    public Sprite[] ProfileImagesList;
    public string[] ProfileName;
    public string[] ProfileUnlockCondition;
    public enum TrophieTypes {Bronze,Silver,Gold}
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Sprite GetProfileImage(int id)
    {
        return ProfileImagesList[id];
    }
    public string GetProfileName(int id)
    {
        return ProfileName[id];
    }
    public bool GetIsProfileUnlocked(int id)
    {
        bool returned = false;
        string condition = ProfileUnlockCondition[id];
        string[] paramsf = condition.Split('-');
        switch(paramsf[1])
        {
            case "g":
                if(PlayerPrefs.GetInt("gold") >= Convert.ToInt32(paramsf[0]))
                {
                    returned = true;
                }
                break;
            case "s":
                if ((PlayerPrefs.GetInt("silverTotal") + PlayerPrefs.GetInt("silver")) >= Convert.ToInt32(paramsf[0]))
                {
                    returned = true;
                }
                break;
            case "b":
                if ((PlayerPrefs.GetInt("bronzeTotal") + PlayerPrefs.GetInt("bronze")) >= Convert.ToInt32(paramsf[0]))
                {
                    returned = true;
                }
                break;
        }
        return returned;
    }
    public (TrophieTypes trophieType,int amount) GetCondition(int id)
    {
        (TrophieTypes trophieType, int amount) tuple = (TrophieTypes.Bronze,0);
        string condition = ProfileUnlockCondition[id];
        string[] paramsf = condition.Split('-');
        tuple.amount = Convert.ToInt32(paramsf[0]);

        switch (paramsf[1])
        {
            case "g":
                tuple.trophieType = TrophieTypes.Gold;
                break;
            case "s":
                tuple.trophieType = TrophieTypes.Silver;
                break;
            case "b":
                tuple.trophieType = TrophieTypes.Bronze;
                break;
        }

        return tuple;
    }
}
