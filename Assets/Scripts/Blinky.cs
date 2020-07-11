using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blinky : Ghost
{
<<<<<<< HEAD
     private void Update()
    {

        SpriteChecker();
        if (isSpriteChanged && !tempBool)
        {
            StartCoroutine(IsSpriteChangedChanger());
        }
        if (!isSpriteChanged)
=======
    public override Vector3 SetWaypoint()
    {
        if (scatterMode)
>>>>>>> 9ba065742e12aae9432788834b755b56ee0fcac9
        {
            return new Vector3(4.064f, 4.584f);
        }

        return pacman.transform.position;
    }
}
