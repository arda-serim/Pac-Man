using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pinky : Ghost
{
    public override Vector3 SetWaypoint()
    {
        if (phase == Phase.Dead)
        {
            return new Vector3(0, 1.3f);
        }

        if (phase == Phase.Scatter)
        {
            return new Vector3(-4.019f, 4.592f);
        }

        if (phase == Phase.Frightened)
        {
            return transform.position;
        }

        return pacman.transform.position + pacman.transform.right;
    }
}
