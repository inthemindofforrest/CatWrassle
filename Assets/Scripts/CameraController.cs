using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform[] Targets;
    public Vector3 Offset;
    public float MoveLerp;
    public float MinimumZoom;
    public float ZoomScalar;
    public Vector3 AxisScalar;


    //Add Player 1-2 and 3-4 and use those
    void LateUpdate()
    {
        Vector3 TempDistanceHolder = Targets[0].position;//Vector3.Scale((Targets[0].position - Targets[1].position), axisScalar).magnitude;
        Vector3 MidPoint = Vector3.zero;
        for (int i = 0; i < Targets.Length; i++)
        {
            if (i != 0)
                TempDistanceHolder += ((i % 2 == 0) ? -1 : 1) * Targets[i].position;

            MidPoint += Targets[i].position;
        }
        MidPoint /= Targets.Length;
        float zoom = Mathf.Max(Vector3.Scale(TempDistanceHolder, AxisScalar).magnitude * ZoomScalar, MinimumZoom);
        transform.position = Vector3.Lerp(transform.position, MidPoint + (Offset * zoom), MoveLerp);
    }
}
