using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SingletonBase<T> : MonoBehaviour where T : SingletonBase<T>
{
    protected static T instence;
    public static T Instence
    {
        get
        {
            if (instence == null)
            {
                instence = FindObjectOfType<T>();
                if (instence == null)
                {
                    instence = new GameObject().AddComponent<T>();
                }
            }
            return instence;
        }
    }


    private void Awake()
    {
        if (instence != null && instence != this)
        {
            Destroy(this.gameObject);
            return;
        }
        awakeChild();
    }
    protected abstract void awakeChild();


    public void OnDestroy()
    {
        resetInstence();
    }
    private void OnApplicationQuit()
    {
        resetInstence();
    }
    public void resetInstence()
    {
        if (instence == this)
        {
            instence = null;
        }
    }
}
