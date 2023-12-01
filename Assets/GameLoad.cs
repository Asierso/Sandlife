using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLoad : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        PlayerPrefs.SetFloat("stmusic", 0);
        StartCoroutine(StartLoadingGame());
    }

    IEnumerator StartLoadingGame()
    {
        
        yield return new WaitForSeconds(2f);
        AsyncOperation Operation = SceneManager.LoadSceneAsync("Game", LoadSceneMode.Single);

        while (Operation.isDone == false)
        {
            yield return null;
        }
        SceneManager.UnloadSceneAsync("Load");
    }
}
