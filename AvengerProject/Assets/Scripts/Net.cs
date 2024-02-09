using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Net : Gadget
{
    private void Awake()
    {
        this.Bottom = FixtureHandler.Current.BuildTop.Top;
        FixtureHandler.Current.Bottom = this.Top;
    }
}
