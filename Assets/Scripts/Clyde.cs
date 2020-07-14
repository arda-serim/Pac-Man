using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clyde : Ghost
{
    void FixedUpdate()
    {
        if (  phase != Phase.Dead && phase != Phase.Frightened && Vector3.Distance(gameObject.transform.position, pacman.transform.position) < 2)
        {
            phase = Phase.Scatter;
        }
    }

    public override Vector3 SetWaypoint()
    {
        if (phase == Phase.Dead)
        {
            return new Vector3(0, 1.3f);
        }

        if (phase == Phase.Scatter)
        {
            return new Vector3(-4.024f, -4.466f);
        }

        if (phase == Phase.Frightened)
        {
            return transform.position;
        }

        return pacman.transform.position;
    }
}
