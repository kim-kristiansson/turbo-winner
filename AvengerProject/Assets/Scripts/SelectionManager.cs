using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectionManager : MonoBehaviour
{

    private GameObject panel;
    RaycastHit hit;
    Ray ray;
    public Material highlightMaterial;
    public Material selectedMaterial;
    public Material defaultPlateMaterial;
    public Material defaultDistancePipeMaterial;
    public bool inventoryEnabled;
    public GameObject equipmentPanelSpawnPlate;
    private List<Slot> slots = new List<Slot>();
    public GameObject slotHolderSpawnPlate;
    private Transform _selection;


    List<BottomPlate> possibleBottomPlates = new List<BottomPlate>();
    List<TestPlate> possibleSpawnPlates = new List<TestPlate>();
    List<DistancePipe> possibleDistancePlate = new List<DistancePipe>();
    List<DistancePipe> possibleLayerDistancePlate = new List<DistancePipe>();
    List<DistancePipe> possibleFirstSectionDistancePipes = new List<DistancePipe>();
    List<List<DistancePipe>> selectedLayerDistancePipes = new List<List<DistancePipe>>();
    List<List<DistancePipe>> selectedFirstDistancePipes = new List<List<DistancePipe>>();
    public List<BottomPlate> PossibleBottomPlates { get { return possibleBottomPlates; } set { possibleBottomPlates = value; } }
    public List<TestPlate> PossibleSpawnPlates { get { return possibleSpawnPlates; } set { possibleSpawnPlates = value; } }
    public List<DistancePipe> PossibleDistancePlate { get { return possibleDistancePlate; } set { possibleDistancePlate = value; } }
    public List<DistancePipe> PossibleLayerDistancePlate { get { return possibleLayerDistancePlate; } set { possibleLayerDistancePlate = value; } }
    public List<DistancePipe> PossibleFirstSectionDistancePipes { get { return possibleFirstSectionDistancePipes; } set { possibleFirstSectionDistancePipes = value; } }
    public List<List<DistancePipe>> SelectedLayerDistancePipes { get { return selectedLayerDistancePipes; } set { selectedLayerDistancePipes = value; } }
    public List<List<DistancePipe>> SelectedFirstSectionDistancePipes { get { return selectedFirstDistancePipes; } set { selectedFirstDistancePipes = value; } }
    public static SelectionManager Instance { get; set; }

    private void Start()
    {
        Instance = this;

        for(int i = 0; i < slotHolderSpawnPlate.transform.childCount; i++)
        {
            slots.Add(slotHolderSpawnPlate.transform.GetChild(i).GetComponent<Slot>());
        }
    }
    void Update()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (_selection != null)
        {
            switch (_selection.tag)
            {
                case "BottomPlate":
                    ChangeMaterial(defaultPlateMaterial, _selection.GetComponent<BottomPlate>());
                    break;

                case "SpawnPlate":
                    ChangeMaterial(defaultPlateMaterial, _selection.GetComponent<SpawnPlate>());
                    break;

                case "SupportPlate":
                    ChangeMaterial(defaultPlateMaterial, _selection.GetComponent<SupportPlate>());
                    break;

                case "DistancePipe":
                    break;

                case "FirstSectionDistancePipe":
                    if(!SelectedFirstSectionDistancePipes.IsSelected())
                    {
                        SelectAllSimilar(defaultDistancePipeMaterial, _selection.GetComponent<DistancePipe>());
                    }
                    break;

                case "LayerDistancePipe":
                    if(!SelectedLayerDistancePipes.IsSelected())
                    {
                        SelectAllSimilar(defaultDistancePipeMaterial, _selection.GetComponent<DistancePipe>());
                    }
                    break;

                default:
                    break;
            }

            _selection = null;
        }
        if(!EventSystem.current.IsPointerOverGameObject())
        {
            if (Physics.Raycast(ray, out hit))
            {
                Transform selection = hit.transform;
                if (Input.GetMouseButtonDown(0))
                {

                }
                switch (selection.tag)
                {
                    case "BottomPlate":
                        ChangeMaterial(highlightMaterial, selection.GetComponent<BottomPlate>());
                        _selection = selection;

                        if (Input.GetMouseButtonDown(0))
                        {
                            ClearSelectedLists();
                            ClearItems();
                            ChangeMaterial(selectedMaterial, selection.GetComponent<BottomPlate>());
                            AddItems(SelectionManager.Instance.possibleBottomPlates);
                        }
                        break;

                    case "SpawnPlate":
                        ChangeMaterial(highlightMaterial, selection.GetComponent<SpawnPlate>());
                        _selection = selection;

                        if (Input.GetMouseButtonDown(0))
                        {
                            ClearSelectedLists();
                            ClearItems();
                            ChangeMaterial(selectedMaterial, selection.GetComponent<SpawnPlate>());
                            AddItems(SelectionManager.Instance.PossibleSpawnPlates);
                        }
                        break;

                    case "SupportPlate":
                        ChangeMaterial(highlightMaterial, selection.GetComponent<SupportPlate>());
                        _selection = selection;

                        if (Input.GetMouseButtonDown(0))
                        {
                            ClearSelectedLists();
                            ClearItems();
                            ChangeMaterial(selectedMaterial, selection.GetComponent<SupportPlate>());
                            AddItems(SelectionManager.Instance.PossibleSpawnPlates);
                        }
                        break;

                    case "DistancePipe":
                        _selection = selection;

                        if (Input.GetMouseButtonDown(0))
                        {
                            ClearSelectedLists();
                            ClearItems();
                            AddItems(SelectionManager.Instance.possibleDistancePlate, selection.tag);
                        }
                        break;

                    case "FirstSectionDistancePipe":
                        if(!selection.GetComponent<DistancePipe>().isSelected)
                        {
                            SelectAllSimilar(highlightMaterial, selection.GetComponent<DistancePipe>());
                            _selection = selection;

                            if (Input.GetMouseButtonDown(0))
                            {
                                ClearSelectedLists();
                                ClearItems();
                                SelectedFirstSectionDistancePipes = SelectAllSimilar(selectedMaterial, selection.GetComponent<DistancePipe>());

                                if(SelectedFirstSectionDistancePipes.Count > 0)
                                {
                                    AddItems(SelectionManager.Instance.PossibleFirstSectionDistancePipes, selection.tag);
                                }

                                SelectedFirstSectionDistancePipes.SetSelection(true);
                            }
                        }
                        break;

                    case "LayerDistancePipe":
                        if(!selection.GetComponent<DistancePipe>().isSelected)
                        {
                            SelectAllSimilar(highlightMaterial, selection.GetComponent<DistancePipe>());
                            _selection = selection;

                            if (Input.GetMouseButtonDown(0))
                            {
                                ClearSelectedLists();
                                ClearItems();
                                SelectedLayerDistancePipes = SelectAllSimilar(selectedMaterial, selection.GetComponent<DistancePipe>());

                                if (SelectedLayerDistancePipes.Count > 0)
                                {
                                    AddItems(SelectionManager.Instance.possibleLayerDistancePlate, selection.tag);
                                }

                                SelectedLayerDistancePipes.SetSelection(true);
                            }
                        }
                        break;

                    default:
                        break;
                }
            }
        }
    }
    public void ChangeMaterial(Material newMaterial, SpawnPlate spawnPlate)
    {
        foreach (FixtureLayer fixtureLayer in ChargeHandler.Instance.FixtureLayers)
        {
            if (fixtureLayer.SpawnPlates.Contains(spawnPlate))
            {
                fixtureLayer.SpawnPlates.ChangeMaterial(newMaterial);
                fixtureLayer.SupportPlates.ChangeMaterial(newMaterial);
            }
        }
    }
    public void ChangeMaterial(Material newMaterial, SupportPlate supportPlate)
    {
        foreach (FixtureLayer fixtureLayer in ChargeHandler.Instance.FixtureLayers)
        {
            foreach (List<SupportPlate> supportPlateList in fixtureLayer.SupportPlates)
            {
                if (supportPlateList.Contains(supportPlate))
                {
                    fixtureLayer.SpawnPlates.ChangeMaterial(newMaterial);
                    fixtureLayer.SupportPlates.ChangeMaterial(newMaterial);
                }
            }
        }
    }
    public void ChangeMaterial(Material newMaterial, BottomPlate bottomPlate)
    {
        foreach (FixtureLayer fixtureLayer in ChargeHandler.Instance.FixtureLayers)
        {
            if (fixtureLayer.BottomPlates.Contains(bottomPlate))
            {
                fixtureLayer.BottomPlates.ChangeMaterial(newMaterial);
            }
        }
    }
    public List<List<DistancePipe>> SelectAllSimilar(Material newMaterial, DistancePipe distancePipe)
    {
        if(distancePipe.tag == "LayerDistancePipe")
        {
            return ChargeHandler.Instance.LayerDistancePipes.ChangeMaterial(newMaterial);
        }
        else if(distancePipe.tag == "FirstSectionDistancePipe")
        {
            return ChargeHandler.Instance.FirstSectionDistancePipes.ChangeMaterial(newMaterial);
        }

        return null;
    }
    public void AddItems(List<TestPlate> possiblePlates)
    {
        Debug.Log(possiblePlates.Count);
        foreach (TestPlate testPlate in possiblePlates)
        {
            foreach (Slot slot in slots)
            {
                if (slot.isEmpty)
                {
                    slot.GetComponent<Image>().sprite = testPlate.icon;
                    slot.isEmpty = false;
                    slot.id = testPlate.id;
                    slot.tag = testPlate.tag;
                    break;
                }
            }
        }
    }
    public void AddItems(List<BottomPlate> possibleBottomPlates)
    {
        foreach (BottomPlate bottomPlate in possibleBottomPlates)
        {
            foreach (Slot slot in slots)
            {
                if (slot.isEmpty)
                {
                    slot.GetComponent<Image>().sprite = bottomPlate.icon;
                    slot.isEmpty = false;
                    slot.id = bottomPlate.id;
                    slot.tag = bottomPlate.tag;
                    break;
                }
            }
        }
    }
    public void AddItems(List<DistancePipe> possibleDistancePipes, string tag)
    {
        foreach (DistancePipe distancePipe in possibleDistancePipes)
        {
            foreach (Slot slot in slots)
            {
                if (slot.isEmpty)
                {
                    slot.GetComponent<Image>().sprite = distancePipe.icon;
                    slot.isEmpty = false;
                    slot.id = distancePipe.id;
                    slot.tag = tag;
                    slot.size = distancePipe.Height;
                    break;
                }
            }
        }
    }
    public void ClearItems()
    {
        foreach (Slot slot in slots)
        {
            if (!slot.isEmpty)
            {
                slot.GetComponent<Image>().sprite = null;
                slot.isEmpty = true;
                slot.id = null;
                slot.tag = "Untagged";
            }
        }
    }
    public void DefaultMaterialSelectedList(List<List<DistancePipe>> distancePipeMatrix)
    {
        foreach(List<DistancePipe> distancePipeList in distancePipeMatrix)
        {
            foreach (DistancePipe distancePipe in distancePipeList)
            {
                distancePipe.GetComponent<Renderer>().material = defaultDistancePipeMaterial;
            }
        }
    }
    public void ClearSelectedLists()
    {
        // Layer Distance Pipes
        DefaultMaterialSelectedList(SelectedLayerDistancePipes);
        SelectedLayerDistancePipes.SetSelection(false);
        SelectedLayerDistancePipes.Clear();

        // First Section Distance Pipes
        DefaultMaterialSelectedList(SelectedFirstSectionDistancePipes);
        SelectedFirstSectionDistancePipes.SetSelection(false);
        SelectedFirstSectionDistancePipes.Clear();
    }
}
