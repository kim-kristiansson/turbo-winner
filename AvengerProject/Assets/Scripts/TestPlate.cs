using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TestPlate : BaseClass
{
    public List<SpawnArea> spawnAreas = new List<SpawnArea>();
    public Sprite icon;
    public float Height {
        get { return MathUtils.GetHeight(this.gameObject); }
    }
}