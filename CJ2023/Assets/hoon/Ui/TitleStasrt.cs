using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleStasrt : MonoBehaviour
{
    [SerializeField]
    private string[] sceneNames;
    public void Next(string index)
    {
        index = sceneNames[Random.Range(0, sceneNames.Length)];
        DataManager.Instence.sceneChange();
        SceneManager.LoadScene(index);
    }
}
