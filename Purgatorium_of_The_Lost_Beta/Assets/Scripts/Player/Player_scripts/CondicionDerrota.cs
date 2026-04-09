using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CondicionDerrota : MonoBehaviour
{
    public string playerTag = "Player";
    public string sceneName = "GameOver" ;

    void Update()
    {
        if (GameObject.FindGameObjectWithTag(playerTag) == null)
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}
