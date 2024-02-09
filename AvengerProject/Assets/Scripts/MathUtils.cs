using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Numerics;
using System.Runtime;
using UnityEditor;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public static class MathUtils
{
    readonly static Collider[] col = new Collider[100];
    public static Vector3 MinusDeltaZX(Vector3 vec3)
    {
        vec3.z -= 0.0057734669224756626163575641298941f;
        vec3.x += 0.01f;

        return vec3;
    }
    public static Vector3 PlusDeltaZX(Vector3 vec3)
    {
        vec3.z += 0.0057734669224756626163575641298941f;
        vec3.x -= 0.01f;

        return vec3;
    }
    public static int NumberOfCollidersFound(float diameter, Vector3 spawnPosition, LayerMask spawnLayer)    // Looking for space to create object
    {
        float radius = diameter * 0.5f;
        return Physics.OverlapSphereNonAlloc(spawnPosition, radius, col, spawnLayer);
    }
    public static bool IsZeroColliders(float diameter, Vector3 spawnPosition, LayerMask cylinderLayer, LayerMask plateLayer)
    {
        if (NumberOfCollidersFound(diameter, spawnPosition, cylinderLayer) == 0 
            && NumberOfCollidersFound(diameter, spawnPosition, plateLayer) == 0)
            return true;
        else
            return false;
    }
    public static bool IsZeroColliders(float diameter, Vector3 spawnPosition, LayerMask spawnLayer)
    {
        if (NumberOfCollidersFound(diameter, spawnPosition, spawnLayer) == 0)
            return true;
        else
            return false;
    }
    public static bool CylinderPackingFixture()
    {
        int amount = 0;
        float plusValue = 1f;
        Cylinder cylinder = FixtureHandler.Current.Cylinder;
        bool isCylinder = false;

        foreach (TestPlate testPlate in TestPlateHandler.instance.testPlates)
        {
            foreach (SpawnArea spawnArea in testPlate.spawnAreas)
            {
                List<Vector3> profilePositionList = new List<Vector3>();

                Vector3 v3 = SetMinXValue(spawnArea.collider);

                Vector3 v3Copy = v3;

                while (v3.z < spawnArea.collider.bounds.max.z)
                {


                    if (Physics.Raycast(v3, Vector3.down, out RaycastHit hit, 1000, spawnArea.layer))
                    {
                        Debug.Log(cylinder);
                        if (IsZeroColliders(cylinder.Diameter, hit.point, cylinder.layer, testPlate.layer))
                        {
                            FixtureHandler.Current.CreateCylinder(hit.point, spawnArea.transform);

                            amount++;

                            profilePositionList.Add(FixtureHandler.Current.Cylinder.transform.localPosition);

                            isCylinder = true;
                        }
                    }

                    v3.x += plusValue;

                    if (v3.x >= spawnArea.collider.bounds.max.x)
                    {
                        v3 = new Vector3(v3Copy.x, v3Copy.y, v3.z + plusValue);
                    }
                }

                // -XML
                PositionProfile positionProfile = new PositionProfile(amount, profilePositionList, spawnArea.id, cylinder.Diameter, testPlate.id);

                XMLManager.instance.positionDatabase.list.Add(positionProfile);

                XMLManager.instance.SavePositions();

            }
        }

        return isCylinder;
    }
    //public static int CylinderPackingFixture(SpawnArea spawnArea, Cylinder cylinder, float diameter, LayerMask itemLayer, LayerMask testPlateLayer, LayerMask spawnLayer)
    //{
    //    Collider spawnAreaCollider = spawnArea.GetComponent<Collider>();
    //    Vector3 v3 = SetMinXValue(spawnAreaCollider);
    //    Vector3 v3Copy = v3;
    //    Cylinder itemInstantiation;
    //    int amount = 0;
    //    float plusValueX = 0.1f;
    //
    //    List<Vector3> profilePositionList = new List<Vector3>();
    //
    //    while (v3.z < spawnAreaCollider.bounds.max.z) {
    //        if (IsZeroColliders(diameter, v3, itemLayer, testPlateLayer, spawnLayer)) {
    //            itemInstantiation = MonoBehaviour.Instantiate(cylinder, v3, cylinder.transform.rotation);
    //            itemInstantiation.transform.parent = spawnAreaCollider.transform;
    //            amount++;
    //
    //            profilePositionList.Add(v3);
    //        }
    //
    //        if (v3.x >= spawnAreaCollider.bounds.max.x) 
    //        {
    //            v3 = new Vector3(v3Copy.x, v3Copy.y, v3.z + 0.1f);
    //        }
    //        v3.x += plusValueX;
    //    }
    //
    //    // -XML
    //    PositionProfile positionProfile = new PositionProfile(amount, profilePositionList, spawnArea.id, cylinder.diameter);
    //
    //    XMLManager.instance.positionDatabase.list.Add(positionProfile);
    //
    //    return amount;
    //}
    private static Vector3 SetMaxZValue(Collider c)
    {
        float centerX = c.bounds.center.x;
        float maxZ = c.bounds.max.z;
        float yPos = c.bounds.max.y;

        return new Vector3(centerX, yPos, maxZ);
    }
    public static float GetHeight(GameObject obstacle)
    {
        return obstacle.GetComponent<Renderer>().bounds.max.y - obstacle.GetComponent<Renderer>().bounds.min.y;
    }
    private static Vector3 SetMinXValue(Collider c)
    {
        Vector3 v3 = c.bounds.min;

        v3.y += 10;

        return v3;
    }
}