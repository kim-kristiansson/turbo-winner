using System.Collections;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using UnityEditor.SceneManagement;
using UnityEngine;

public class Fixture : Gadget
{
    public RaycastOrigin raycasterLeft;
    public RaycastOrigin raycasterRight;
    public List<FixturePoint> fixturePoints = new List<FixturePoint>();
    public FixtureStick fixtureStick;

    public FixturePoint leftCenterPoint;
    public FixturePoint rightCenterPoint;

    private void Awake()
    {
        FixtureHandler.Current.Fixture = this;

        FixtureHandler.Current.BuildTop = this;

        FixtureHandler.Current.Bottom = this.Top;
    }
    public Vector3 LeftCenterPoint
    {
        get
        {
            return leftCenterPoint.transform.position;
        }
    }
    public Vector3 RightCenterPoint 
    {
        get 
        {
            return rightCenterPoint.transform.position;
        }
    }
}