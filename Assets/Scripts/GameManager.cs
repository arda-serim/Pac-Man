using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class GameManager : MonoSingleton<GameManager>
{
    public Action frightened;
    List<GameObject> bigPoints;

    void Start()
    {
        bigPoints = GameObject.FindGameObjectsWithTag("BigPoint").ToList<GameObject>();
    }

    void Update()
    {
        foreach (var bigPoint in bigPoints)
        {
            if (bigPoint == null)
            {
                bigPoints.Remove(bigPoint);
                frightened();
            }
        }
    }
}
