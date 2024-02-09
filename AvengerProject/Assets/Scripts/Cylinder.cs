using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cylinder : Gadget
{
    private void Awake()
    {
        FixtureHandler.Current.Cylinder = this;

        this.Bottom = FixtureHandler.Current.Bottom;
    }
    public float Diameter 
        {
        get { return this.transform.localScale.x; }
    }
}