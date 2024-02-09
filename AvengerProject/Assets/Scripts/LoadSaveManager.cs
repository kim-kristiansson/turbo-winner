using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UIElements;
using UnityEngine.Video;

public class LoadSaveManager : MonoBehaviour
{
    public static LoadSaveManager current;
    public List<PositionProfile> localPositionProfiles = new List<PositionProfile>();


    public List<PositionProfile> LocalPositionProfiles {
        get { return localPositionProfiles; }
        set { localPositionProfiles = value; }
    }

    private void Awake()
    {
        current = this;

        XMLManager.instance.LoadPositions();
    }
    public bool ProfileExists(string plateID, float diameter)
    {
        foreach(PositionProfile positionProfile in XMLManager.instance.positionDatabase.list)
        {
            if(positionProfile.testPlateID == plateID && positionProfile.itemDiameter == diameter)
            {
                GetPositions(plateID, diameter);

                return true;
            }
        }
        return false;
    }
    public void GetPositions(string plateID, float cylinderDiameter)
    {
        LocalPositionProfiles.Clear();

        foreach (PositionProfile positionProfile in XMLManager.instance.positionDatabase.list)
        {
            if(plateID == positionProfile.testPlateID && cylinderDiameter == positionProfile.itemDiameter)
            {
                LoadSaveManager.current.LocalPositionProfiles.Add(positionProfile);
            }
        }
    }
}
