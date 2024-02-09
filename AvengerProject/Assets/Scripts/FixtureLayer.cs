using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Transactions;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UIElements;

public class FixtureLayer : MonoBehaviour
{
    List<List<DistancePipe>> _distancePipeList = new List<List<DistancePipe>>();
    List<SpawnPlate> _spawnPlateList = new List<SpawnPlate>();
    List<List<SupportPlate>> _supportPlateList = new List<List<SupportPlate>>();
    List<BottomPlate> _bottomPlateList = new List<BottomPlate>();
    List<Cylinder> _cylinderList = new List<Cylinder>();
    List<DistancePipe> _layerDistancePipeList = new List<DistancePipe>();
    List<DistancePipe> _firstSectionDistancePipe = new List<DistancePipe>();
    public Net Net { get; set; }
    public Equipment BuildBottom { get; set; }
    public float Top { get { return Cylinders[0].Top; } }
    public float Bottom { get { return BottomPlates[0].Bottom; } }
    public float Height { get { return Top - Bottom; } }
    public List<List<DistancePipe>> DistancePipes { get { return _distancePipeList; } set { _distancePipeList = value; } }
    public List<SpawnPlate> SpawnPlates { get { return _spawnPlateList; } set { _spawnPlateList = value; } }
    public List<List<SupportPlate>> SupportPlates { get { return _supportPlateList; } set { _supportPlateList = value; } }
    public List<BottomPlate> BottomPlates { get { return _bottomPlateList; } set { _bottomPlateList = value; } }
    public List<Cylinder> Cylinders { get { return _cylinderList; } set { _cylinderList = value; } }
    public List<DistancePipe> LayerDistancePipes { get { return _layerDistancePipeList; } set { _layerDistancePipeList = value; } }
    public List<DistancePipe> FirstSectionDistancePipe { get { return DistancePipes[0]; } set { DistancePipes[0] = value; } }

    public float NetPlusCylinderHeight()
    {
        float height = Net.Height
                     + Cylinders[0].Height;

        return height;
    }
    public List<DistancePipe> ChangeFirstSectionDistancePipes(float newSize)
    {
        List<DistancePipe> newDistancePipes = new List<DistancePipe>();

        foreach (DistancePipe distancePipe in FirstSectionDistancePipe)
        {
            if(distancePipe.transform.childCount > 0)
            {
                newDistancePipes = FixtureHandler.Current.CreateDistancePipes(false, newSize, "FirstSectionDistancePipe", distancePipe.transform.parent);
                SwapChildren(newDistancePipes[0], distancePipe);
            }

            FixtureHandler.Current.weight -= distancePipe.weight;
            Destroy(distancePipe.gameObject);
        }

        foreach(Cylinder cylinder in Cylinders)
        {
            cylinder.Bottom = Net.Top;
        }

        FirstSectionDistancePipe = newDistancePipes;
        ChargeSettings.firstSectionDistancePipe = newSize;
        return newDistancePipes;
    }
    void SwapChildren(Equipment newItem, Equipment oldItem)
    {
        List<Transform> childList = new List<Transform>();

        for (int i = 0; i < oldItem.transform.childCount; i++)
        {
            Transform child = oldItem.transform.GetChild(i).transform;

            if(child.GetComponent<Equipment>() != null)
            {
                childList.Add(child);
                child.GetComponent<Equipment>().Bottom = newItem.Top;
            }
        }
        foreach (Transform child in childList)
        {
            child.SetParent(newItem.transform);
        }
    }
    public bool IsTooHighSections()
    {
        List<List<SupportPlate>> deleteSupportPlateList = new List<List<SupportPlate>>();
        List<DistancePipe> deleteParents = new List<DistancePipe>();
        bool isTooHigh = false;

        foreach (List<SupportPlate> supportPlateList in SupportPlates)
        {
            foreach (SupportPlate supportPlate in supportPlateList)
            {
                if (supportPlate.Top > Cylinders[0].Top)
                {
                    deleteParents = supportPlate.DistancePipes;

                    if (supportPlate.transform.childCount > 0)
                    {
                        SwapChildren(supportPlate.transform.parent.transform.parent.GetComponent<Equipment>(), supportPlate);
                    }

                    isTooHigh = true;
                }
            }

            if (isTooHigh)
            {
                if (this.DistancePipes.Contains(deleteParents))
                {
                    this.DistancePipes.Remove(deleteParents);
                }
                foreach (DistancePipe distancePipe in deleteParents)
                {
                    if (distancePipe.transform.childCount > 0)
                    {
                        distancePipe.transform.DetachChildren();
                    }

                    Destroy(distancePipe.gameObject);
                }

                deleteSupportPlateList.Add(supportPlateList);
            }
        }

        foreach (List<SupportPlate> deleteSupportPlates in deleteSupportPlateList)
        {
            this.SupportPlates.Remove(deleteSupportPlates);

            foreach (SupportPlate deleteSupportPlate in deleteSupportPlates)
            {
                Destroy(deleteSupportPlate.gameObject);
            }
        }
        if(this.SupportPlates.Count > 0)
        {
            FixtureHandler.Current.BuildTop = this.SupportPlates.Last().Last();
            FixtureHandler.Current.SupportPlates = this.SupportPlates.Last();
        }
        else if(this.SpawnPlates.Count > 0)
        {
            FixtureHandler.Current.BuildTop = this.SpawnPlates.Last();
        }

        return isTooHigh;
    }
    public void RecreateSections()
    {
        int multiplier = 1;
        Equipment currentTop = null;
        Gadget tempBuildTop = FixtureHandler.Current.BuildTop;

        if(this.SupportPlates.Count > 0)
        {
            Debug.Log("ok");
            foreach(SupportPlate supportPlate in this.SupportPlates.Last())
            {
                currentTop = supportPlate;

                if (supportPlate.transform.childCount > 0)
                {
                    break;
                }
            }
        }
        else
        {
            if(this.SpawnPlates[0].transform.childCount > this.SpawnPlates[1].transform.childCount)
            {
                currentTop = this.SpawnPlates[0];
            }
            else
            {
                currentTop = this.SpawnPlates[1];
            }
        }

        if(ChargeSettings.sectionDistancePipe + this.SpawnPlates.Last().Height + ChargeSettings.firstSectionDistancePipe + currentTop.Height < FixtureHandler.Current.Cylinder.Height * 0.95f)
        {
            List<DistancePipe> distancePipes = new List<DistancePipe>();
            currentTop.name = ChargeHandler.Instance.FixtureLayers.IndexOf(this).ToString();
            this.DistancePipes.Add(FixtureHandler.Current.CreateDistancePipes(false, ChargeSettings.sectionDistancePipe, "DistancePipe"));
            
            foreach (DistancePipe distancePipe in FixtureHandler.Current.DistancePipes)
            {
                distancePipe.Bottom = currentTop.Top;
                distancePipe.transform.parent = null;
            }

            this.SupportPlates.Add(FixtureHandler.Current.SpawnPlates.GetSupportPlates());
            
            foreach (SupportPlate supportPlate in this.SupportPlates.Last())
            {
                supportPlate.Bottom = FixtureHandler.Current.DistancePipe.Top;
            }
            
            SwapChildren(FixtureHandler.Current.DistancePipe, currentTop);
            FixtureHandler.Current.DistancePipes.SetParent(currentTop.transform);
            
            currentTop = this.SupportPlates.Last().Last();
        }

        FixtureHandler.Current.BuildTop = tempBuildTop;
    }
}
