using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

public static class ListExtentions
{
    public static void SetSelection(this List<List<DistancePipe>> list, bool selection)
    {
        foreach(List<DistancePipe> distancePipeList in list)
        {
            foreach(DistancePipe distancePipe in distancePipeList)
            {
                distancePipe.isSelected = selection;
            }
        }
    }
    public static void SetSelection(this List<DistancePipe> list, bool selection)
    {
        foreach(DistancePipe distancePipe in list)
        {
            distancePipe.isSelected = selection;
        }
    }
    public static void SetSelection(this List<BottomPlate> list, bool selection)
    {
        foreach(BottomPlate bottomPlate in list)
        {
            bottomPlate.isSelected = selection;
        }
    }
    public static void SetSelection(this List<SupportPlate> list, bool selection)
    {
        foreach(SupportPlate supportPlate in list)
        {
            supportPlate.isSelected = selection;
        }
    }
    public static void SetSelection(this List<SpawnPlate> list, bool selection)
    {
        foreach (SpawnPlate spawnPlate in list)
        {
            spawnPlate.isSelected = selection;
        }
    }
    public static bool IsSelected(this List<List<DistancePipe>> list)
    {
        bool isSelected = false;

        if (list.Count > 0)
        {
            if (list[0].Count > 0)
            {
                isSelected = list[0][0].isSelected;
            }
            else
            {
                Debug.Log("IsSelectedError");
            }
        }
        else
        {
            Debug.Log("IsSelectedError");
        }

        return isSelected;
    }    
    public static bool IsSelected(this List<DistancePipe> list)
    {
        bool isSelected = false;

        if (list.Count > 0)
        {
            isSelected = list[0].isSelected;
        }
        else
        {
            Debug.Log("IsSelectedError");
        }

        return isSelected;
    }    
    public static bool IsSelected(this List<BottomPlate> list)
    {
        bool isSelected = false;

        if (list.Count > 0)
        {
            isSelected = list[0].isSelected;
        }
        else
        {
            Debug.Log("IsSelectedError");
        }

        return isSelected;
    }    
    public static bool IsSelected(this List<SupportPlate> list)
    {
        bool isSelected = false;

        if (list.Count > 0)
        {
            isSelected = list[0].isSelected;
        }
        else
        {
            Debug.Log("IsSelectedError");
        }

        return isSelected;
    }
    public static bool IsSelected(this List<SpawnPlate> list)
    {
        bool isSelected = false;

        if (list.Count > 0)
        {
            isSelected = list[0].isSelected;
        }
        else
        {
            Debug.Log("IsSelectedError");
        }

        return isSelected;
    }
    public static List<Cylinder> InsertCylinders(this List<SpawnPlate> list, string spawnPlateID)
    {
        List<Cylinder> _cylinderList = new List<Cylinder>();

        if (LoadSaveManager.current.ProfileExists(spawnPlateID, FixtureHandler.Current.Cylinder.Diameter))
        {
            foreach(PositionProfile positionProfile in LoadSaveManager.current.LocalPositionProfiles)
            {
                foreach(SpawnPlate spawnPlate in list)
                {
                    foreach (SpawnArea spawnArea in spawnPlate.spawnAreaList)
                    {
                        if (positionProfile.spawnAreaID == spawnArea.id)
                        {
                            foreach (Vector3 v3 in positionProfile.positions)
                            {
                                _cylinderList.Add(FixtureHandler.Current.CreateCylinder(v3, spawnArea.transform));

                                FixtureHandler.Current.Cylinder.transform.localPosition = v3;

                                FixtureHandler.Current.Cylinder.Bottom = FixtureHandler.Current.Bottom;

                                FixtureHandler.Current.amount++;
                            }
                        }
                    }
                }
            }
        }

        return _cylinderList;
    }
    public static List<SupportPlate> GetSupportPlates(this List<SpawnPlate> list)
    {
        FixtureHandler.Current.SupportPlates = new List<SupportPlate>();

        foreach(SpawnPlate spawnPlate in list)
        {
            FixtureHandler.Current.SupportPlate = MonoBehaviour.Instantiate(spawnPlate.supportPlate, spawnPlate.transform);

            FixtureHandler.Current.SupportPlate.transform.SetParent(FixtureHandler.Current.DistancePipe.transform);

            FixtureHandler.Current.SupportPlates.Add(FixtureHandler.Current.SupportPlate);

            FixtureHandler.Current.SupportPlate.Bottom = FixtureHandler.Current.BuildTop.Top;
        }

        FixtureHandler.Current.BuildTop = FixtureHandler.Current.SupportPlate;

        FixtureHandler.Current.layerHeight += FixtureHandler.Current.SupportPlate.Height;

        return FixtureHandler.Current.SupportPlates;
    }
    public static FixtureLayer Last(this List<FixtureLayer> list)
    {
        return list[list.Count - 1];
    }
    public static List<DistancePipe> Last(this List<List<DistancePipe>> list)
    {
        if(list.Count > 0)
        {
            return list[list.Count - 1];
        }

        return new List<DistancePipe>();
    }
    public static FixtureLayer FirstLayer(this List<FixtureLayer> list)
    {
        return list[0];
    }
    public static float Height(this List<List<DistancePipe>> list)
    {
        float sum = 0;

        foreach(List<DistancePipe> distancePipeList in list)
        {
            sum += distancePipeList[0].Height;
        }

        return sum;
    }
    public static float Height(this List<List<SupportPlate>> list)
    {
        float sum = 0;

        foreach (List<SupportPlate> supportPlateList in list)
        {
            sum += supportPlateList[0].Height;
        }

        return sum;
    }
    public static void ChangeMaterial(this List<List<SupportPlate>> list, Material newMaterial)
    {
        foreach (List<SupportPlate> supportPlateList in list)
        {
            foreach(SupportPlate supportPlate in supportPlateList)
            {
                supportPlate.GetComponent<Renderer>().material = newMaterial;
            }
        }
    }
    public static void ChangeMaterial(this List<SpawnPlate> list, Material newMaterial)
    {
        foreach (SpawnPlate spawnPlate in list)
        {
            foreach(Mesh meshHole in spawnPlate.meshHoleList)
            {
                meshHole.GetComponent<Renderer>().material = newMaterial;
            }

            spawnPlate.GetComponent<Renderer>().material = newMaterial;
        }
        
    }
    public static void ChangeMaterial(this List<BottomPlate> list, Material newMaterial)
    {
        foreach (BottomPlate bottomPlate in list)
        {
            bottomPlate.GetComponent<Renderer>().material = newMaterial;
        }

    }
    public static List<List<DistancePipe>> ChangeMaterial(this List<List<DistancePipe>> list, Material newMaterial)
    {
        List<List<DistancePipe>> selectedLayerDistancePipes = new List<List<DistancePipe>>();

        foreach (List<DistancePipe> distancePipeList in list)
        {
            foreach (DistancePipe distancePipe in distancePipeList)
            {
                distancePipe.GetComponent<Renderer>().material = newMaterial;
            }

            selectedLayerDistancePipes.Add(distancePipeList);
        }

        return selectedLayerDistancePipes;
    }
    public static List<SupportPlate> Last(this List<List<SupportPlate>> list)
    {
        return list[list.Count - 1];
    }
    public static SupportPlate Last(this List<SupportPlate> list)
    {
        return list[list.Count - 1];
    }
    public static SpawnPlate Last(this List<SpawnPlate> list)
    {
        return list[list.Count - 1];
    }
    public static void SetParent(this List<DistancePipe> list, Transform parent)
    {
        foreach(DistancePipe distancePipe in list)
        {
            distancePipe.transform.SetParent(parent);
        }
    }
    public static void SetParent(this List<SupportPlate> list, Transform parent)
    {
        foreach (SupportPlate supportPlate in list)
        {
            supportPlate.transform.SetParent(parent);
        }
    }
}
