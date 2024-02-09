using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

public class XMLManager : MonoBehaviour
{
    public static XMLManager instance;

    private void OnDestroy()
    {
        XMLManager.instance.SavePositions();
    }
    private void Awake()
    {
        instance = this;
    }

    public PositionDatabase positionDatabase;

    public void SavePositions()
    {
        XmlSerializer serializer = new XmlSerializer(typeof(PositionDatabase));
        FileStream stream = new FileStream(Application.persistentDataPath + "/position_data.xml", FileMode.Create);
        serializer.Serialize(stream, positionDatabase);
        stream.Close();
    }
    public void LoadPositions()
    {
        XmlSerializer serializer = new XmlSerializer(typeof(PositionDatabase));
        FileStream stream = new FileStream(Application.persistentDataPath + "/position_data.xml", FileMode.Open);
        positionDatabase = (PositionDatabase)serializer.Deserialize(stream);
        Debug.Log(Application.persistentDataPath);
        stream.Close();
    }
}

[System.Serializable]
public class PositionProfile
{
    public int amount;
    public List<Vector3> positions = new List<Vector3>();
    public string spawnAreaID;
    public float itemDiameter;
    public string testPlateID;

    public PositionProfile()
    {
        
    }
    public PositionProfile(int _amount, List<Vector3> _positions, string _spawnAreaID, float _itemDiameter, string _testPlateID)
    {
        amount = _amount;
        positions = _positions;
        spawnAreaID = _spawnAreaID;
        itemDiameter = _itemDiameter;
        testPlateID = _testPlateID;
    }
}

[System.Serializable]
public class PositionDatabase
{
    public List<PositionProfile> list = new List<PositionProfile>();
}