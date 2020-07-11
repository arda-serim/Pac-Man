using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pinky : Ghost
{
    private void Update()
    {

        SpriteChecker();
        if (isSpriteChanged && !tempBool)
        {
            StartCoroutine(IsSpriteChangedChanger());
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
        return pacman.transform.position + pacman.transform.right;
    }
}
