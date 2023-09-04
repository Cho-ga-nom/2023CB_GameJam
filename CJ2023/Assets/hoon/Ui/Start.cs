using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Start : MonoBehaviour
{
    public void Next(string index)
    {
        DataManager.Instence.sceneChange();
        SceneManager.LoadScene(index);
    }
}
