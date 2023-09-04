 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Test : MonoBehaviour
{
    public int n;
    void Start()
    {
        SceneManager.LoadScene(n);
    }

}
