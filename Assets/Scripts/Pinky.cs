using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pinky : Ghost
{
    public override Vector3 SetWaypoint()
    {
        if (phase == Phase.Dead)
        {
            return new Vector3(0, 0.35f);
        }

        if (phase == Phase.Scatter)
        {
            return new Vector3(-4.019f, 4.592f);
        }
        return pacman.transform.position + pacman.transform.right;
    }
}
