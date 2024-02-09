using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlateHandler : MonoBehaviour
{
    public List<TestPlate> testPlates = new List<TestPlate>();

    public static TestPlateHandler instance
    {
        get;
        set;
    }
    private void Awake()
    {
        instance = this;
        testPlates = CreateTestPlates();
    }
    public List<TestPlate> CreateTestPlates()
    {
        List<TestPlate> testPlateList = new List<TestPlate>();

        Vector3 v3 = new Vector3(-200, 0, 0);

        foreach(TestPlate testPlate in testPlates)
        {
            testPlateList.Add(Instantiate(testPlate, v3, testPlate.transform.rotation));

            v3.x -= 50;
        }

        return testPlateList;
    }
    public TestPlate GetTestPlate(string testPlateID)
    {
        foreach(TestPlate testPlate in testPlates)
        {
            if(testPlate.id ==  testPlateID)
            {
                return testPlate;
            }
        }

        return null;
    }
}