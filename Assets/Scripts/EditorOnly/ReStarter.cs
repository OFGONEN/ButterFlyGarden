using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReStarter : MonoBehaviour
{
    public GameEvent reStart;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
            reStart.Raise();
    }
}
