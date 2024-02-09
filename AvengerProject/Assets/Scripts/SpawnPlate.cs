using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class SpawnPlate : Equipment
{
    public List<SpawnArea> spawnAreaList = new List<SpawnArea>();
    public List<Mesh> meshHoleList = new List<Mesh>();
    public SupportPlate supportPlate;
    private void Awake()
    {
        FixtureHandler.Current.SpawnPlate = this;

        if(FixtureHandler.Current.Bottom > FixtureHandler.Current.BuildTop.Top)
        {
            this.Bottom = FixtureHandler.Current.Bottom;
        }
        else
        {
            this.Bottom = FixtureHandler.Current.BuildTop.Top;
        }

        FixtureHandler.Current.SpawnPlates.Add(this);
    }
}