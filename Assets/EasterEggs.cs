using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EasterEggs : MonoBehaviour
{
    public enum RewardType { Bronze,Silver,Gold}
    public GameObject musicControler;
    public void EasterEggFounded(string name,RewardType reward)
    {
        if(PlayerPrefs.GetInt("easter" + name) == 0)
        {
            switch (reward)
            {
                case RewardType.Bronze: PlayerPrefs.SetInt("bronze", PlayerPrefs.GetInt("bronze") + 1);break;
                case RewardType.Silver: PlayerPrefs.SetInt("silver", PlayerPrefs.GetInt("silver") + 1);break;
                case RewardType.Gold: PlayerPrefs.SetInt("gold", PlayerPrefs.GetInt("gold") + 1);break;
            }
            PlayerPrefs.SetInt("easter" + name, 1);
            musicControler.GetComponent<Sounds>().PlaySound(15);
        }
    }
}
