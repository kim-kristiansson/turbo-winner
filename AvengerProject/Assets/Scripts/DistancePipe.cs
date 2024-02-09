using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistancePipe : Equipment
{
    public float size;
    private void Awake()
    {
        FixtureHandler.Current.DistancePipe = this;
    }
    public List<DistancePipe> GetWholeLayer()
    {
        foreach(List<DistancePipe> distancePipeList in ChargeHandler.Instance.FirstSectionDistancePipes)
        {
            Debug.Log("firstSelect");

            if (distancePipeList.Contains(this))
            { 
                return distancePipeList;
            }
        }
        foreach(List<DistancePipe> distancePipeList in ChargeHandler.Instance.LayerDistancePipes)
        {
            Debug.Log("layerdist");

            if(distancePipeList.Contains(this))
            {
                return distancePipeList;
            }
        }
        foreach(List<DistancePipe> distancePipeList in ChargeHandler.Instance.SectionDistancePipes)
        {
            Debug.Log("section");

            if(distancePipeList.Contains(this))
            {
                return distancePipeList;
            }
        }
        return null;
    }
}