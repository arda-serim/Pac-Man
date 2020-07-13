using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inky : Ghost
{
    [Space]
    [SerializeField]GameObject blinky;


    public override Vector3 SetWaypoint()
    {
        if (phase == Phase.Dead)
        {
            return new Vector3(0, 0.35f);
        }

        if (phase == Phase.Scatter)
        {
            return new Vector3(4.059f, -4.466f);
        }

        return pacman.transform.position + pacman.transform.right * 0.5f - blinky.transform.position;
    }


}
