using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class ChargeHandler : MonoBehaviour
{
    public FixtureLayer fixtureLayer;
    private List<FixtureLayer> fixtureLayers = new List<FixtureLayer>();
    private List<List<DistancePipe>> layerDistancePipes = new List<List<DistancePipe>>();
    private List<List<DistancePipe>> firstSectionDistancePipes = new List<List<DistancePipe>>();
    private List<List<DistancePipe>> sectionDistancePipes = new List<List<DistancePipe>>();
    public List<FixtureLayer> FixtureLayers { get { return fixtureLayers; } set { fixtureLayers = value; } }
    public List<List<DistancePipe>> LayerDistancePipes  
    { 
        get 
        {
            layerDistancePipes.Clear();

            foreach (FixtureLayer fixtureLayer in fixtureLayers)
            {
                if(fixtureLayer.LayerDistancePipes.Count > 0)
                {
                    layerDistancePipes.Add(fixtureLayer.LayerDistancePipes);
                }
            }

            return layerDistancePipes;
        }
    }
    public List<List<DistancePipe>> FirstSectionDistancePipes
    {
        get 
        {
            firstSectionDistancePipes.Clear();

            foreach(FixtureLayer fixtureLayer in fixtureLayers)
            {
                if(fixtureLayer.DistancePipes.Count > 0)
                {
                    foreach(List<DistancePipe> distancePipeList in fixtureLayer.DistancePipes)
                    {
                        foreach(DistancePipe distancePipe in distancePipeList)
                        {
                            if (distancePipe.tag == "FirstSectionDistancePipe")
                            {
                                firstSectionDistancePipes.Add(distancePipeList);

                                break;
                            }
                        }
                    }
                }
            }
        
            return firstSectionDistancePipes;
        }
    }
    public List<List<DistancePipe>> SectionDistancePipes {
        get {
            sectionDistancePipes.Clear();

            foreach (FixtureLayer fixtureLayer in fixtureLayers)
            {
                if (fixtureLayer.DistancePipes.Count > 0)
                {
                    foreach (List<DistancePipe> distancePipeList in fixtureLayer.DistancePipes)
                    {
                        foreach (DistancePipe distancePipe in distancePipeList)
                        {
                            if (distancePipe.tag == "DistancePipe")
                            {
                                sectionDistancePipes.Add(distancePipeList);

                                break;
                            }
                        }
                    }
                }
            }

            return sectionDistancePipes;
        }
    }
    public static ChargeHandler Instance { get; set; }
    private void Awake()
    {
        Instance = this;
    }
    public FixtureLayer CreateNewLayerObject()
    {
        ChargeHandler.Instance.FixtureLayers.Add(Instantiate(fixtureLayer, -Vector3.zero, transform.rotation));

        return ChargeHandler.Instance.FixtureLayers[ChargeHandler.Instance.FixtureLayers.Count - 1];
    }
    public void RecreateFirstLayer(int layerPipeIndex)
    {
        FixtureLayer newLayer = ChargeHandler.Instance.CreateNewLayerObject();
        newLayer.LayerDistancePipes = FixtureHandler.Current.CreateDistancePipes(false, FixtureHandler.Current.PrefabDistancePipes[layerPipeIndex].size, "LayerDistancePipe");
        newLayer.BottomPlates = FixtureHandler.Current.CreateBottomPlates();
        newLayer.Net = FixtureHandler.Current.CreateNet(FixtureHandler.Current.Net.id);
        newLayer.DistancePipes.Add(FixtureHandler.Current.CreateDistancePipes(false, ChargeSettings.firstSectionDistancePipe, "FirstSectionDistancePipe"));
        newLayer.SpawnPlates = FixtureHandler.Current.CreateSpawnPlates(FixtureHandler.Current.SpawnPlate.id);
        newLayer.Cylinders = newLayer.SpawnPlates.InsertCylinders(FixtureHandler.Current.SpawnPlate.id);

        if (ChargeHandler.Instance.FixtureLayers.FirstLayer().DistancePipes.Count > 1)
        {
            for (int i = 1; i < ChargeHandler.Instance.FixtureLayers.FirstLayer().DistancePipes.Count; i++)
            {
                newLayer.DistancePipes.Add(FixtureHandler.Current.CreateDistancePipes(false, ChargeHandler.Instance.FixtureLayers.FirstLayer().DistancePipes[1][0].size, "DistancePipe"));
                newLayer.SupportPlates.Add(FixtureHandler.Current.SpawnPlates.GetSupportPlates());
                
                foreach(SupportPlate supportPlate in newLayer.SupportPlates.Last())
                {
                    supportPlate.DistancePipes = FixtureHandler.Current.DistancePipes;
                }
            }
        }

        ChargeSettings.layerDistanceHeight = newLayer.LayerDistancePipes[0].size;
    }
    public void ChangePlates(string testPlateID)
    {
        TestPlate testPlate = TestPlateHandler.instance.GetTestPlate(testPlateID);
        float diameter = FixtureHandler.Current.Cylinder.Diameter;
        float height = FixtureHandler.Current.Cylinder.Height;

        GeneratorManager.Instance.GenerateCharge(diameter, height, testPlate);
    }
    public List<DistancePipe> CangeLayerDistancePipes(float newSize, List<DistancePipe> distancePipeList)
    {
        List<Transform> childList = new List<Transform>();
        List<DistancePipe> newDistancePipeList = new List<DistancePipe>();
        FixtureLayer _fixtureLayer = null;

        foreach (DistancePipe distancePipe in distancePipeList)
        {
            if(_fixtureLayer == null)
            {
                foreach (FixtureLayer fixtureLayer in ChargeHandler.Instance.FixtureLayers)
                {
                    if (fixtureLayer.LayerDistancePipes.Contains(distancePipe))
                    {
                        _fixtureLayer = fixtureLayer;
                    }
                }
            }

            if (distancePipe.transform.childCount > 0)
            {
                newDistancePipeList = FixtureHandler.Current.CreateDistancePipes(false, newSize, "LayerDistancePipe", distancePipe.transform.parent.transform);
                DistancePipe _newDistancePipe = newDistancePipeList[0];

                for (int i = 0; i < distancePipe.transform.childCount; i++)
                {
                    Transform child = distancePipe.transform.GetChild(i).transform;
                    childList.Add(child);
                    child.GetComponent<Equipment>().Bottom = _newDistancePipe.Top;
                }
                foreach (Transform child in childList)
                {
                    child.SetParent(_newDistancePipe.transform);
                }
            }

            distancePipe.transform.DetachChildren();
            Destroy(distancePipe.gameObject);
        }

        _fixtureLayer.LayerDistancePipes = newDistancePipeList;
        ChargeSettings.layerDistanceHeight = newDistancePipeList[0].Height;
        return newDistancePipeList;
    }
    public void ClearCharge()
    {
        foreach(FixtureLayer fixtureLayer in FixtureLayers)
        {
            Destroy(fixtureLayer.gameObject);
        }

        FixtureLayers.Clear();
    }
    public FixtureLayer CurrentLayer()
    {
        return fixtureLayers[fixtureLayers.Count - 1];
    }
    public bool IsTooHighLayer()
    {
        List<FixtureLayer> removedLayers = new List<FixtureLayer>();
        bool isTooHigh = false;

        if(FixtureHandler.Current.FixtureStick != null)
        {
            foreach (FixtureLayer fixtureLayer in FixtureLayers)
            {
                if (fixtureLayer.Top > FixtureHandler.Current.FixtureStick.Top)
                {
                    isTooHigh = true;
                    RemoveLayer(fixtureLayer);
                    removedLayers.Add(fixtureLayer);
                }
            }
            foreach (FixtureLayer fixtureLayer in removedLayers)
            {
                FixtureLayers.Remove(fixtureLayer);
                Destroy(fixtureLayer.gameObject);
            }
        }

        FixtureHandler.Current.AttatchToNewObjects(FixtureLayers[FixtureLayers.Count - 1]);

        return isTooHigh;
    }
    public void RemoveLayer(FixtureLayer fixtureLayer)
    {
        FixtureHandler.Current.weight -= fixtureLayer.Net.weight;
        Destroy(fixtureLayer.Net.gameObject);

        foreach(List<DistancePipe> distancePipeList in fixtureLayer.DistancePipes)
        {
            foreach(DistancePipe distancePipe in distancePipeList)
            {
                FixtureHandler.Current.weight -= distancePipe.weight;
                Destroy(distancePipe.gameObject);
            }
        }

        foreach(SpawnPlate spawnPlate in fixtureLayer.SpawnPlates)
        {
            FixtureHandler.Current.weight -= spawnPlate.weight;
            Destroy(spawnPlate.gameObject);
        }

        foreach(List<SupportPlate> supportPlateList in fixtureLayer.SupportPlates)
        {
            foreach(SupportPlate supportPlate in supportPlateList)
            {
                FixtureHandler.Current.weight -= supportPlate.weight;
                Destroy(supportPlate.gameObject);
            }
        }

        foreach(BottomPlate bottomPlate in fixtureLayer.BottomPlates)
        {
            FixtureHandler.Current.weight -= bottomPlate.weight;
            Destroy(bottomPlate.gameObject);
        }

        foreach(Cylinder cylinder in fixtureLayer.Cylinders)
        {
            FixtureHandler.Current.weight -= cylinder.weight;
            FixtureHandler.Current.amount -= 1;
            Destroy(cylinder.gameObject);
        }

        foreach(DistancePipe layerDistancePipe in fixtureLayer.LayerDistancePipes)
        {
            FixtureHandler.Current.weight -= layerDistancePipe.weight;
            Destroy(layerDistancePipe.gameObject);
        }
    }
    public void CheckForMoreLayerSpace()
    {
        while (FixtureLayers.Last().Height + ChargeSettings.layerDistanceHeight + FixtureHandler.Current.BuildTop.Top < FixtureHandler.Current.FixtureStick.Top)
        {
            int layerDistanceIndex = 0;

            for(int i = 0; i < FixtureHandler.Current.PrefabDistancePipes.Count; i++)
            {
                if(FixtureHandler.Current.PrefabDistancePipes[i].Height == FixtureHandler.Current.LayerDistancePipe.Height)
                {
                    layerDistanceIndex = i;
                }
            }

            RecreateFirstLayer(layerDistanceIndex);
        }
    }
}
