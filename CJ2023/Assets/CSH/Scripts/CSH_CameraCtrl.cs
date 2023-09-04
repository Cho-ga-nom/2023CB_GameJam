using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSH_CameraCtrl : MonoBehaviour
{
    public Transform Player;

    public void Start()
    {
        Player = GameManager.Instence.playerCtrl.transform;
    }

    void Update()
    {
        transform.position = new Vector3(Player.position.x, Player.position.y, -15);
    }
}
