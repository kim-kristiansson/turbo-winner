using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupportPlate : Equipment
{
    List<DistancePipe> distancePipes = new List<DistancePipe>();

    public List<DistancePipe> DistancePipes { get { return distancePipes; } set { distancePipes = value; } }
}
