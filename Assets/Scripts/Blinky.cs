using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blinky : Ghost
{
    public override Vector3 SetWaypoint()
    {
        if (scatterMode)
        {
            return new Vector3(4.064f, 4.584f);
        }

        return pacman.transform.position;
    }
}
