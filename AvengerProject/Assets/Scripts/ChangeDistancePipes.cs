using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

public static class ChangeDistancePipes
{
    public static List<List<DistancePipe>> ChangeAllLayerDistancePipes(this List<List<DistancePipe>> list, float newSize)
    {
        List<List<DistancePipe>> newDistancePipeList = new List<List<DistancePipe>>();
        FixtureLayer fixtureLayer = null;
        DistancePipe oldDistancePipe;
        List<DistancePipe> oldDistancePipeList = new List<DistancePipe>();

        for (int i = 0; i < list.Count; i++)
        {
            oldDistancePipeList = list[i];

            for (int j = 0; j < oldDistancePipeList.Count; j++)
            {
                oldDistancePipe = oldDistancePipeList[j];

                fixtureLayer = GetFixtureLayer(oldDistancePipe);

               

                if (oldDistancePipe.transform.childCount > 0)
                {
                    newDistancePipeList.Add(FixtureHandler.Current.CreateDistancePipes(false, newSize, "LayerDistancePipe", oldDistancePipe.transform.parent.transform));
                    PassingOverChild(newDistancePipeList[i][0], oldDistancePipe);
                }

                oldDistancePipe.transform.DetachChildren();
                MonoBehaviour.Destroy(oldDistancePipe.gameObject);
            }

            if (fixtureLayer != null && fixtureLayer)
            {
                fixtureLayer.LayerDistancePipes = newDistancePipeList[i];
            }
        }

        //newDistancePipeList = ChargeHandler.Instance.IsTooHighLayer();

        ChargeSettings.layerDistanceHeight = newDistancePipeList[0][0].Height;
        return newDistancePipeList;
    }


    static FixtureLayer GetFixtureLayer(DistancePipe distancePipe)
    {
        foreach (FixtureLayer fixtureLayer in ChargeHandler.Instance.FixtureLayers)
        {
            if (fixtureLayer.LayerDistancePipes.Contains(distancePipe))
            {
                return fixtureLayer;
            }
            else if(fixtureLayer.FirstSectionDistancePipe.Contains(distancePipe))
            {
                return fixtureLayer;
            }
        }

        return null;
    }
    static void PassingOverChild(DistancePipe newDistancePipe, DistancePipe oldDistancePipe)
    {
        List<Transform> childList = new List<Transform>();

        for (int h = 0; h < oldDistancePipe.transform.childCount; h++)
        {
            Transform child = oldDistancePipe.transform.GetChild(h).transform;
            childList.Add(child);
            child.GetComponent<Equipment>().Bottom = newDistancePipe.Top;
        }
        foreach (Transform child in childList)
        {
            child.SetParent(newDistancePipe.transform);
        }
    }

    // First Section Distance Pipes
    public static List<List<DistancePipe>> ChangeFirstSectionDistancePipes(this List<List<DistancePipe>> list, float newSize)
    {
        List<List<DistancePipe>> newDistancePipeList = new List<List<DistancePipe>>();
        FixtureLayer fixtureLayer = null;
        DistancePipe oldDistancePipe;
        List<DistancePipe> oldDistancePipeList = new List<DistancePipe>();

        for (int i = 0; i < list.Count; i++)
        {
            oldDistancePipeList = list[i];

            for (int j = 0; j < oldDistancePipeList.Count; j++)
            {
                oldDistancePipe = oldDistancePipeList[j];

                fixtureLayer = GetFixtureLayer(oldDistancePipe);

                if (oldDistancePipe.transform.childCount > 0)
                {
                    newDistancePipeList.Add(FixtureHandler.Current.CreateDistancePipes(false, newSize, "FirstSectionDistancePipe", oldDistancePipe.transform.parent.transform));
                    PassingOverChild(newDistancePipeList[i][0], oldDistancePipe);
                }

                oldDistancePipe.transform.DetachChildren();
                MonoBehaviour.Destroy(oldDistancePipe.gameObject);
            }

            if (fixtureLayer != null && fixtureLayer)
            {
                fixtureLayer.FirstSectionDistancePipe = newDistancePipeList[i];
            }
        }

        //newDistancePipeList = ChargeHandler.Instance.IsTooHighLayer();

        ChargeSettings.firstSectionDistancePipe = newDistancePipeList[0][0].Height;
        return newDistancePipeList;
    }

    // Contains Extentions
    public static FixtureLayer GetFixtureLayer(this List<FixtureLayer> list, DistancePipe distancePipe)
    {
        foreach(FixtureLayer fixtureLayer in list)
        {
            if(fixtureLayer.FirstSectionDistancePipe.Contains(distancePipe))
            {
                return fixtureLayer;
            }
        }

        return null;
    }
    public static FixtureLayer GetFixtureLayer(this List<FixtureLayer> list, List<DistancePipe> distancePipeList)
    {
        foreach (FixtureLayer fixtureLayer in list)
        {
            if (fixtureLayer.FirstSectionDistancePipe.Contains(distancePipeList[0]))
            {
                return fixtureLayer;
            }
            else if (fixtureLayer.LayerDistancePipes.Contains(distancePipeList[0]))
            {
                return fixtureLayer;
            }
        }

        return null;
    }
}
