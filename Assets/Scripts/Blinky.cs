using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blinky : Ghost
{
    
    private void Update()
    {
        SpriteChanged();
        if (isSpriteChanged && !tempBool)
        {
            StartCoroutine(isSpriteChanger());
        }
        if (!isSpriteChanged)
        {
            Turn(SetWaypoint());
        }
        transform.rotation = Quaternion.identity;
        MoveForward();
        SendRays();
    }
    public override Vector3 SetWaypoint()
    {
        return new Vector3 (1.369f, 2.276f);
    }
}
