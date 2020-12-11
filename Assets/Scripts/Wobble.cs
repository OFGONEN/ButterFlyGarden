using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wobble : MonoBehaviour
{
    public bool    relativeToParent = false;
    public Vector3 maxSpeed;

    void Start()
    {
    }

    void Update()
    {
        Vector3 deltaPos = new Vector3( Random.Range( -maxSpeed.x, maxSpeed.x ), Random.Range( -maxSpeed.y, maxSpeed.y ), Random.Range( -maxSpeed.z, maxSpeed.z ) ) * Time.deltaTime;

        if( relativeToParent )
            transform.localPosition += deltaPos;
        else
            transform.position += deltaPos;
    }
}
