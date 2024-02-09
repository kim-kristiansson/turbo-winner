using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastOrigin : MonoBehaviour
{
    public Vector3 Position
    {
        get
        {
            return this.transform.position;
        }
    }
    // Parameter have to be the layer that the raycast should ignore 
    public RaycastHit Raycast(LayerMask layer)
    {
        Physics.Raycast(this.transform.position, Vector3.down, out RaycastHit hit, 100, ~layer);
        return hit;
    }
}
