using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager1 : TutorialBase
{
    protected override IEnumerator TutorialRoutine()
    {
        while (!Input.GetKeyDown(KeyCode.W) || !Input.GetKeyDown(KeyCode.A) || !Input.GetKeyDown(KeyCode.S) || !Input.GetKeyDown(KeyCode.D))
        {
            yield return null;
        }
    }
}
