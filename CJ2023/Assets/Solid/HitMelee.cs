using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitMelee : HitChecker
{
    public void Awake()
    {
        collider = GetComponent<Collider2D>();

    }
    void OnEnable()
    {
        collider.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        checkTarget(collision.transform, this.transform.position);
    }
}
