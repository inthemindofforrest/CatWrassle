using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisibleCharacterController : MonoBehaviour
{
    float DefaultHeight;
    public float FallSpeed;


    void Update()
    {
        if(!Grounded())
        {
            Gravity();
        }
    }

    bool Grounded()
    {
        bool Returning = false;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, .1f, 1 << 0))
        {
            if (hit.transform.gameObject != gameObject)
            {
                Returning = true;
            }
        }

        return Returning;
    }
    void Gravity()
    {
        transform.position += Physics.gravity * Time.deltaTime / FallSpeed;
    }
}
