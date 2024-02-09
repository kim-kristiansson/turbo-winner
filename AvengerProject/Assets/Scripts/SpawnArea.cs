using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;

public class SpawnArea : BaseClass
{
    public bool isDefault = false;
    new public Collider collider;
    private void Awake()
    {
        collider = this.GetComponent<Collider>();
    }
}