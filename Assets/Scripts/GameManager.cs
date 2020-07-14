using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class GameManager : MonoSingleton<GameManager>
{
    public Action frightened;

    void OnTriggerEnter2D(Collider2D collider)
    {
        collider.gameObject.transform.position = new Vector3(-collider.gameObject.transform.position.x, collider.transform.position.y);
    }
}
