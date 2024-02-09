using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorManager : MonoBehaviour
{
    int layerPipeIndex = 0;
    public static GeneratorManager Instance { get; set; }

    private void Awake()
    {
        Instance = this;
    }

    public void GenerateCharge(float diameter, float height, TestPlate _testPlate)
    {
        if(FixtureHandler.Current.Fixture != null)
        {
            ChargeHandler.Instance.ClearCharge();
            FixtureHandler.Current.Clear();
        }

        FixtureHandler.Current.CreateFixture(false);
        FixtureHandler.Current.CreateCylinder(diameter, height);

        ChargeHandler.Instance.CreateNewLayerObject();

        if (diameter < 6)
        {
            ChargeHandler.Instance.FixtureLayers.Last().Net = FixtureHandler.Current.CreateNet("thick");
            ChargeHandler.Instance.FixtureLayers.Last().BottomPlates = FixtureHandler.Current.CreatePlaceholderBottomPlates();
        }
        else
        {
            ChargeHandler.Instance.FixtureLayers.Last().BottomPlates = FixtureHandler.Current.CreateBottomPlates();
            ChargeHandler.Instance.FixtureLayers.Last().Net = FixtureHandler.Current.CreateNet("placeholder");
        }

        if (FixtureHandler.Current.Cylinder.Height < FixtureHandler.Current.FixtureStick.Height * 0.1)
        {
            OneLayerPlates(_testPlate);
        }
        else
        {
            MultipleLayerPlates(_testPlate);
        }
        while (IsRoomForLayer())
        {
            ChargeHandler.Instance.RecreateFirstLayer(layerPipeIndex);
        }
    }
    public void OneLayerPlates(TestPlate _testPlate)
    {
        FixtureLayer fixtureLayer = ChargeHandler.Instance.FixtureLayers.Last();
        bool isFirstSectionDistancePipe = false;

        for (int i = FixtureHandler.Current.PrefabDistancePipes.Count - 1; i >= 0; i--)
        {
            if (!isFirstSectionDistancePipe && _testPlate.Height + FixtureHandler.Current.PrefabDistancePipes[i].Height < FixtureHandler.Current.Cylinder.Height * 0.51f)
            {
                fixtureLayer.DistancePipes.Add(FixtureHandler.Current.CreateDistancePipes(false, FixtureHandler.Current.PrefabDistancePipes[i].size, "FirstSectionDistancePipe"));
                fixtureLayer.SpawnPlates = FixtureHandler.Current.CreateSpawnPlates(_testPlate.id);
                fixtureLayer.Cylinders = FixtureHandler.Current.SpawnPlates.InsertCylinders(FixtureHandler.Current.SpawnPlate.id);
                ChargeSettings.firstSectionDistancePipe = FixtureHandler.Current.DistancePipe.size;

                isFirstSectionDistancePipe = true;
            }
            if(_testPlate.Height + FixtureHandler.Current.PrefabDistancePipes[i].Height < FixtureHandler.Current.Cylinder.Height)
            {
                SelectionManager.Instance.PossibleFirstSectionDistancePipes.Add(FixtureHandler.Current.PrefabDistancePipes[i]);
            }
        }
    }
    public void MultipleLayerPlates(TestPlate _testPlate)
    {
        FixtureLayer fixtureLayer = ChargeHandler.Instance.FixtureLayers.Last();
        bool isFirstSectionDistancePipe = false;

        foreach (DistancePipe distancePipe in FixtureHandler.Current.PrefabDistancePipes)
        {
                Debug.Log(FixtureHandler.Current.Cylinder);
            if (!isFirstSectionDistancePipe && _testPlate.Height + distancePipe.Height > FixtureHandler.Current.Cylinder.Height * 0.15f)
            {
                fixtureLayer.DistancePipes.Add(FixtureHandler.Current.CreateDistancePipes(false, distancePipe.Height, "FirstSectionDistancePipe"));
                fixtureLayer.SpawnPlates = FixtureHandler.Current.CreateSpawnPlates(_testPlate.id);
                fixtureLayer.Cylinders = FixtureHandler.Current.SpawnPlates.InsertCylinders(FixtureHandler.Current.SpawnPlate.id);
                ChargeSettings.firstSectionDistancePipe = FixtureHandler.Current.DistancePipe.size;

                isFirstSectionDistancePipe = true;
            }
            if (_testPlate.Height + distancePipe.Height < FixtureHandler.Current.Cylinder.Height)
            {
                SelectionManager.Instance.PossibleFirstSectionDistancePipes.Add(distancePipe);
            }
        }

        float multiplyCounter = 1;

        for (int i = 0; i < 10; i++)
        {
            foreach (DistancePipe distancePipe_2 in FixtureHandler.Current.PrefabDistancePipes)
            {
                if (multiplyCounter * (_testPlate.Height + distancePipe_2.Height) + FixtureHandler.Current.layerHeight > FixtureHandler.Current.Cylinder.Height * 0.65f &&
                    multiplyCounter * (_testPlate.Height + distancePipe_2.Height) + FixtureHandler.Current.layerHeight < FixtureHandler.Current.Cylinder.Height * 0.95f)
                {
                    for (int j = 0; j < multiplyCounter; j++)
                    {
                        fixtureLayer.DistancePipes.Add(FixtureHandler.Current.CreateDistancePipes(false, distancePipe_2.size, "DistancePipe"));
                        fixtureLayer.SupportPlates.Add(FixtureHandler.Current.SpawnPlates.GetSupportPlates());

                        foreach (SupportPlate supportPlate in fixtureLayer.SupportPlates.Last())
                        {
                            supportPlate.DistancePipes = FixtureHandler.Current.DistancePipes;
                        }
                    }
                    ChargeSettings.sectionDistancePipe = FixtureHandler.Current.DistancePipe.size;

                    goto End;
                }
            }
            multiplyCounter++;
        }
        End:
        return;
    }
    public int PotentialLayerPipe()
    {
        int layerPipeIndex = -1;

        for (int i = 0; i < FixtureHandler.Current.PrefabDistancePipes.Count; i++)
        {
            if (layerPipeIndex == -1 && FixtureHandler.Current.BuildTop.Top + FixtureHandler.Current.PrefabDistancePipes[i].Height > FixtureHandler.Current.Cylinder.Top + 2)
            {
                layerPipeIndex = i;
            }

            if(FixtureHandler.Current.BuildTop.Top + FixtureHandler.Current.PrefabDistancePipes[i].Height > FixtureHandler.Current.Cylinder.Top)
            {
                SelectionManager.Instance.PossibleLayerDistancePlate.Add(FixtureHandler.Current.PrefabDistancePipes[i]);
            }
        }

        return layerPipeIndex;
    }
    public float PotentialLayerHeight()
    {
        layerPipeIndex = PotentialLayerPipe();

        if(layerPipeIndex > -1)
        {
            float layerPipeHeight = FixtureHandler.Current.PrefabDistancePipes[layerPipeIndex].Height;

            float layerHeight = layerPipeHeight
                              + FixtureHandler.Current.BottomPlate.Height
                              + ChargeHandler.Instance.FixtureLayers.FirstLayer().NetPlusCylinderHeight();

            return layerHeight;
        }

        return -1;
    }
    public void ButtonClear()
    {
        if (FixtureHandler.Current.Fixture != null)
        {
            Destroy(FixtureHandler.Current.Fixture.gameObject);
        }
    }
    public bool IsRoomForLayer()
    {
        float _potentialHeight = PotentialLayerHeight();
        float _potentialChargeHeight = _potentialHeight + FixtureHandler.Current.BuildTop.Top;

        if (_potentialHeight > -1 && _potentialChargeHeight < FixtureHandler.Current.FixtureStick.Top)
        {
            return true;
        }

        return false;
    }
}
