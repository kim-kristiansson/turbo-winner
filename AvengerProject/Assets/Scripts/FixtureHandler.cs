using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class FixtureHandler : MonoBehaviour
{
    public float weight = 0;
    public Fixture fixture;
    public List<SpawnPlate> leftSpawnPlateList = new List<SpawnPlate>();
    public List<SpawnPlate> rightSpawnPlateList = new List<SpawnPlate>();
    public List<SupportPlate> supportPlates = new List<SupportPlate>();
    public List<DistancePipe> prefabDistancePipes = new List<DistancePipe>();
    private List<DistancePipe> distancePipes = new List<DistancePipe>();
    public List<BottomPlate> bottomPlates = new List<BottomPlate>();
    public Cylinder cylinder;
    public List<Net> nets = new List<Net>();
    public float layerHeight = 0;
    public int amount = 0;
    public BottomPlate placeholderBottomPlate;
    public float Bottom { get; set; }
    public static FixtureHandler Current { get; set; }
    public Fixture Fixture { get; set; }
    public Gadget BuildTop { get; set; }
    public Gadget LeftTop { get; set; }
    public Gadget RightTop { get; set; }
    public Cylinder Cylinder { get; set; }
    public SpawnPlate SpawnPlate { get; set; }
    public DistancePipe DistancePipe { get; set; }
    public List<DistancePipe> PrefabDistancePipes { get; set; }
    public List<SpawnPlate> SpawnPlates { get; set; }
    public SupportPlate SupportPlate { get; set; }
    public FixtureStick FixtureStick { get; set; }
    public List<SupportPlate> SupportPlates { get; set; }
    public List<Net> Nets { get { return nets; } }
    public DistancePipe LayerDistancePipe { get; set; }
    public BottomPlate BottomPlate { get; set; }
    public Net Net { get; set; }
    public List<DistancePipe> DistancePipes { get { return distancePipes; } set { distancePipes = value; } }
    
    public void Awake() 
    {
        Current = this;
    }
    void Start()
    {
        Vector3 v3 = new Vector3(-200, 50, 0);

        FixtureHandler.Current.Cylinder = FixtureHandler.Current.cylinder;

        FixtureHandler.Current.PrefabDistancePipes = new List<DistancePipe>();

        foreach (DistancePipe distancePipe in prefabDistancePipes)
        {
            FixtureHandler.Current.PrefabDistancePipes.Add(Instantiate(distancePipe, v3, distancePipe.transform.rotation));

            v3.x -= 10;
        }

        v3 = new Vector3(-250, 100, 0);

        foreach (BottomPlate bottomPlate in bottomPlates)
        {
            if (FixtureHandler.Current.BottomPlate == null)
            {
                FixtureHandler.Current.BottomPlate = Instantiate(bottomPlate, v3, bottomPlate.transform.rotation);
            }
            else
            {
                Instantiate(bottomPlate, v3, bottomPlate.transform.rotation);
            }

            v3.x -= 50;
        }
    }
    public void OnDestroy()
    {
        if (Current.Fixture == null)
        {
            Destroy(this);
        }
    }

    // Fixture
    public void CreateFixture(bool isRings)
    {
        if(FixtureHandler.Current.Fixture != null)
        {
            Clear();
            InstantiateFixture(isRings);
        }
        else
        {
            InstantiateFixture(isRings);
        }
    }
    public void InstantiateFixture(bool isRings)
    {
        Instantiate(fixture, new Vector3(0, 0, 0), this.transform.rotation);

        foreach (FixturePoint _fixturePoint in FixtureHandler.Current.Fixture.fixturePoints) {
            if (isRings && _fixturePoint.isRings) {
                FixtureHandler.Current.Fixture.fixtureStick.AddSticks(_fixturePoint.transform);

                FixtureHandler.Current.CountWeight(FixtureHandler.Current.Fixture);
            }
            else if (_fixturePoint.isDefault) {
                FixtureHandler.Current.Fixture.fixtureStick.AddSticks(_fixturePoint.transform);

                FixtureHandler.Current.CountWeight(FixtureHandler.Current.Fixture);
            }
        }

        FixtureHandler.Current.BuildTop = FixtureHandler.Current.Fixture;
    }

    // Plates

    public List<SpawnPlate> CreateSpawnPlates(string id)
    {
        List<SpawnPlate> _spawnPlateList = new List<SpawnPlate>();

        FixtureHandler.Current.SpawnPlates = new List<SpawnPlate>();

        foreach (SpawnPlate leftSpawnPlate in leftSpawnPlateList)
        {
            if (id == leftSpawnPlate.id)
            {
                _spawnPlateList.Add(Instantiate(leftSpawnPlate, FixtureHandler.Current.Fixture.LeftCenterPoint, leftSpawnPlate.transform.rotation));

                FixtureHandler.Current.LeftTop = FixtureHandler.Current.SpawnPlate;

                FixtureHandler.Current.CountWeight(FixtureHandler.Current.SpawnPlate);

                FixtureHandler.Current.SpawnPlate.transform.SetParent(FixtureHandler.Current.BuildTop.transform);
            }
        }

        foreach (SpawnPlate rightSpawnPlate in rightSpawnPlateList)
        {
            if (id == rightSpawnPlate.id)
            {
                _spawnPlateList.Add(Instantiate(rightSpawnPlate, FixtureHandler.Current.Fixture.RightCenterPoint, rightSpawnPlate.transform.rotation));

                FixtureHandler.Current.RightTop = FixtureHandler.Current.SpawnPlate;

                FixtureHandler.Current.CountWeight(FixtureHandler.Current.SpawnPlate);

                FixtureHandler.Current.SpawnPlate.transform.SetParent(FixtureHandler.Current.BuildTop.transform);
            }
        }

        FixtureHandler.Current.BuildTop = FixtureHandler.Current.SpawnPlate;
        FixtureHandler.Current.layerHeight += FixtureHandler.Current.SpawnPlate.Height;

        return _spawnPlateList;
    }
    public TestPlate DecidePlate(float cylinderDiameter)
    {
        SelectionManager.Instance.PossibleSpawnPlates.Clear();

        for (int i = 0; i < TestPlateHandler.instance.testPlates.Count; i++)
        {
            foreach (PositionProfile positionProfile in XMLManager.instance.positionDatabase.list)
            {
                if (positionProfile.itemDiameter == cylinderDiameter && positionProfile.testPlateID == TestPlateHandler.instance.testPlates[i].id)
                {
                    if (positionProfile.amount > 0)
                    {
                        for (int j = i; j < TestPlateHandler.instance.testPlates.Count; j++)
                        {
                            SelectionManager.Instance.PossibleSpawnPlates.Add(TestPlateHandler.instance.testPlates[j]);
                        }

                        return TestPlateHandler.instance.testPlates[i];
                    }
                }
            }
        }

        if(MathUtils.CylinderPackingFixture())
        {
            return DecidePlate(cylinderDiameter);
        }

        return null;
    }
    public List<BottomPlate> CreateBottomPlates()
    {
        List<BottomPlate> _bottomPlateList = new List<BottomPlate>();

        FixtureHandler.Current.BottomPlate = Instantiate(bottomPlates[0], FixtureHandler.Current.Fixture.LeftCenterPoint, bottomPlates[0].transform.rotation);
        FixtureHandler.Current.BottomPlate.Bottom = FixtureHandler.Current.BuildTop.Top;
        FixtureHandler.Current.BottomPlate.transform.SetParent(FixtureHandler.Current.BuildTop.transform);
        FixtureHandler.Current.CountWeight(FixtureHandler.Current.BottomPlate);

        _bottomPlateList.Add(FixtureHandler.Current.BottomPlate);

        FixtureHandler.Current.BottomPlate = Instantiate(bottomPlates[0], FixtureHandler.Current.Fixture.RightCenterPoint, bottomPlates[0].transform.rotation);
        FixtureHandler.Current.BottomPlate.Bottom = FixtureHandler.Current.BuildTop.Top;
        FixtureHandler.Current.BottomPlate.transform.SetParent(FixtureHandler.Current.BuildTop.transform);
        FixtureHandler.Current.CountWeight(FixtureHandler.Current.BottomPlate);

        _bottomPlateList.Add(FixtureHandler.Current.BottomPlate);

        FixtureHandler.Current.BuildTop = FixtureHandler.Current.BottomPlate;
        FixtureHandler.Current.Bottom = FixtureHandler.Current.BottomPlate.Top;

        return _bottomPlateList;
    }
    public List<BottomPlate> CreatePlaceholderBottomPlates()
    {
        List<BottomPlate> _bottomPlateList = new List<BottomPlate>();
        _bottomPlateList.Add(Instantiate(placeholderBottomPlate, FixtureHandler.Current.BuildTop.transform));
        _bottomPlateList.Add(Instantiate(placeholderBottomPlate, FixtureHandler.Current.BuildTop.transform));
        return _bottomPlateList;
    }
    // Cylinders
    public void CreateCylinder(float diameter, float height)
    {
        Cylinder _cylinder = Instantiate(cylinder, new Vector3(100, 100, 100), cylinder.transform.rotation);
        _cylinder.transform.SetParent(FixtureHandler.Current.BuildTop.transform);
        _cylinder.transform.localScale = new Vector3(diameter, height * 0.5f, diameter);
    }
    public Cylinder CreateCylinder(Vector3 position, Transform parent)
    {
        Cylinder _cylinder = Instantiate(FixtureHandler.Current.Cylinder, position, FixtureHandler.Current.Cylinder.transform.rotation);

        FixtureHandler.Current.Cylinder.transform.parent = parent;
        FixtureHandler.Current.CountWeight(_cylinder);

        return _cylinder;
    }

    // Nets
    public Net CreateNet(string id)
    {
        foreach(Net net in FixtureHandler.Current.Nets)
        {
            if(net.id == id)
            {
                FixtureHandler.Current.Net = Instantiate(net, Vector3.zero, net.transform.rotation);
            }
        }

        FixtureHandler.Current.Net.transform.SetParent(FixtureHandler.Current.BuildTop.transform);
        FixtureHandler.Current.CountWeight(FixtureHandler.Current.Net);

        return FixtureHandler.Current.Net;
    }

    // Distance Pipes
    public List<DistancePipe> CreateDistancePipes(bool isRings, float size, string tag)
    {
        List<DistancePipe> _layerDistancePipes = new List<DistancePipe>();

        foreach (DistancePipe distancePipe in FixtureHandler.Current.PrefabDistancePipes)
        {
            if (distancePipe.size == size)
            {
                foreach (FixturePoint fixturePoint in FixtureHandler.Current.Fixture.fixturePoints)
                {
                    if (isRings && fixturePoint.isRings)
                    {
                        _layerDistancePipes.Add(Instantiate(distancePipe, FixtureHandler.Current.BuildTop.transform));

                        FixtureHandler.Current.DistancePipe.tag = tag;

                        FixtureHandler.Current.DistancePipe.transform.position = fixturePoint.transform.position;

                        FixtureHandler.Current.DistancePipe.Bottom = FixtureHandler.Current.BuildTop.Top;
                    }
                    else if (fixturePoint.isDefault)
                    {
                        _layerDistancePipes.Add(Instantiate(distancePipe, FixtureHandler.Current.BuildTop.transform));

                        FixtureHandler.Current.DistancePipe.tag = tag;

                        FixtureHandler.Current.DistancePipe.transform.position = fixturePoint.transform.position;

                        FixtureHandler.Current.DistancePipe.Bottom = FixtureHandler.Current.BuildTop.Top;
                    }
                }
            }
        }

        FixtureHandler.Current.CountWeight(_layerDistancePipes);
        FixtureHandler.Current.BuildTop = FixtureHandler.Current.DistancePipe;
        FixtureHandler.Current.layerHeight += FixtureHandler.Current.DistancePipe.Height;

        if(FixtureHandler.Current.DistancePipe.tag == "LayerDistancePipe")
        {
            FixtureHandler.Current.LayerDistancePipe = FixtureHandler.Current.DistancePipe;
        }

        FixtureHandler.Current.DistancePipes = _layerDistancePipes;

        return _layerDistancePipes;
    }
    public List<DistancePipe> CreateDistancePipes(bool isRings, float size, string tag, Transform parent)
    {
        List<DistancePipe> _layerDistancePipes = new List<DistancePipe>();

        foreach (DistancePipe distancePipe in FixtureHandler.Current.PrefabDistancePipes)
        {
            if (distancePipe.size == size)
            {
                foreach (FixturePoint fixturePoint in FixtureHandler.Current.Fixture.fixturePoints)
                {
                    if (isRings && fixturePoint.isRings)
                    {
                        _layerDistancePipes.Add(Instantiate(distancePipe, parent));

                        FixtureHandler.Current.DistancePipe.tag = tag;

                        FixtureHandler.Current.DistancePipe.transform.position = fixturePoint.transform.position;

                        FixtureHandler.Current.DistancePipe.Bottom = parent.GetComponent<Equipment>().Top;
                    }
                    else if (fixturePoint.isDefault)
                    {
                        _layerDistancePipes.Add(Instantiate(distancePipe, parent));

                        FixtureHandler.Current.DistancePipe.tag = tag;

                        FixtureHandler.Current.DistancePipe.transform.position = fixturePoint.transform.position;

                        FixtureHandler.Current.DistancePipe.Bottom = parent.GetComponent<Equipment>().Top;
                    }
                }
            }
        }

        FixtureHandler.Current.CountWeight(_layerDistancePipes);
        FixtureHandler.Current.layerHeight += FixtureHandler.Current.DistancePipe.Height;
        FixtureHandler.Current.DistancePipes = _layerDistancePipes;

        return _layerDistancePipes;
    }
    public void Clear()
    {
        FixtureHandler.Current.FixtureStick = null;
        FixtureHandler.Current.BottomPlate = null;
        Destroy(FixtureHandler.Current.Fixture.gameObject);
        FixtureHandler.Current.layerHeight = 0;
        FixtureHandler.Current.amount = 0;
        FixtureHandler.Current.weight = 0;
        SelectionManager.Instance.SelectedFirstSectionDistancePipes.Clear();
        SelectionManager.Instance.SelectedLayerDistancePipes.Clear();
    }
    void CountWeight(List<DistancePipe> _layerDistancePipes)
    {
        foreach(DistancePipe distancePipe in _layerDistancePipes)
        {
            FixtureHandler.Current.weight += distancePipe.weight;
        }
    }
    void CountWeight(Net _net)
    {
        FixtureHandler.Current.weight += _net.weight;
    }
    void CountWeight(Cylinder _cylinder)
    {
        FixtureHandler.Current.weight += _cylinder.weight;
    }
    void CountWeight(BottomPlate _bottomPlate)
    {
        FixtureHandler.Current.weight += _bottomPlate.weight;
    }
    void CountWeight(SpawnPlate _spawnPlate)
    {
        FixtureHandler.Current.weight += _spawnPlate.weight;
    }
    void CountWeight(Fixture _fixture)
    {
        FixtureHandler.Current.weight += _fixture.weight;
    }
    public void CountWeight(FixtureStick _fixtureStick)
    {
        FixtureHandler.Current.weight += _fixtureStick.weight;
    }
    public void AttatchToNewObjects(FixtureLayer fixtureLayer)
    {
        if(fixtureLayer.LayerDistancePipes.Count > 0)
        {
            LayerDistancePipe = fixtureLayer.LayerDistancePipes[0];
        }
        if (fixtureLayer.SpawnPlates.Count > 0)
        {
            SpawnPlate = fixtureLayer.SpawnPlates[0];
            BuildTop = fixtureLayer.SpawnPlates[0];
        }
        if (fixtureLayer.SupportPlates.Count > 0 && fixtureLayer.SupportPlates[0].Count > 0)
        {
            SupportPlate = fixtureLayer.SupportPlates.Last().Last();
            BuildTop = fixtureLayer.SupportPlates.Last().Last();
        }
        if (fixtureLayer.Cylinders.Count > 0)
        {
            Cylinder = fixtureLayer.Cylinders[0];
        }
    }
}