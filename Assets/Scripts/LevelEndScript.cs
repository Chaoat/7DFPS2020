using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEndScript : MonoBehaviour
{
    public string nextScene;

    private void OnTrigger(Collider other) {
        SceneManager.LoadScene(nextScene, LoadSceneMode.Single);
    }
}