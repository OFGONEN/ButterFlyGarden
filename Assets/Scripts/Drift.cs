using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;

public class Drift : MonoBehaviour
{
    public Transform targetTransform;

    [Range(0.0f, 0.5f)]
    public float maxCoordValue = 0.2f;

    [Range(3, 15)]
    public int numberOfWaypoints;

    [NaughtyAttributes.ReadOnly]
    public List<Vector3> waypoints;

    private Vector3 initialPosition;

    private float X_Min { get { return initialPosition.x - maxCoordValue; } }
    private float X_Max { get { return initialPosition.x + maxCoordValue; } }
    private float Z_Min { get { return initialPosition.z - maxCoordValue; } }
    private float Z_Max { get { return initialPosition.z + maxCoordValue; } }

    void Start()
    {
        initialPosition = targetTransform.position;

        waypoints = new List<Vector3>(numberOfWaypoints);
        for (int i = 0; i < numberOfWaypoints; i++)
            waypoints.Add(new Vector3(Random.Range(X_Min, X_Max), 0.0f, Random.Range(Z_Min, Z_Max)));

        DoDrift();
    }

    public void DoDrift()
    {
        targetTransform.DOPath(waypoints.ToArray(), 20, PathType.CatmullRom)
                       .SetOptions( /* Close the path */ true,
                                    /* Allow position to change */ AxisConstraint.None,
                                    /* No Rotation! */ AxisConstraint.X | AxisConstraint.Y | AxisConstraint.Z)
                       .SetEase(Ease.Linear)
                       .SetLoops( /* Loop indefinetely */ -1);

    }
}
