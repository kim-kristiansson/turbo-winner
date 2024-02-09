using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class BottonGenerateNewCharge : MonoBehaviour
{
    Slot slot;
    private void Start()
    {
        slot = this.GetComponent<Slot>();
    }
    public void GenerateNewCharge()
    {
        switch (slot.tag)
        {
            case "SpawnPlate":
                SelectionManager.Instance.ClearSelectedLists();
                ChargeHandler.Instance.ChangePlates(slot.id);
                break;
            case "LayerDistancePipe":
                SelectionManager.Instance.SelectedLayerDistancePipes = ChargeHandler.Instance.LayerDistancePipes.ChangeAllLayerDistancePipes(slot.size);
                ChargeHandler.Instance.IsTooHighLayer();
                ChargeHandler.Instance.CheckForMoreLayerSpace();

                foreach(List<DistancePipe> distancePipeList in ChargeHandler.Instance.LayerDistancePipes)
                {
                    if(!SelectionManager.Instance.SelectedLayerDistancePipes.Contains(distancePipeList))
                    {
                        SelectionManager.Instance.SelectedLayerDistancePipes.Add(distancePipeList);
                    }
                }

                SelectionManager.Instance.SelectedLayerDistancePipes.SetSelection(true);
                SelectionManager.Instance.SelectedLayerDistancePipes.ChangeMaterial(SelectionManager.Instance.selectedMaterial);
                break;
            case "FirstSectionDistancePipe":
                foreach(FixtureLayer fixtureLayer in ChargeHandler.Instance.FixtureLayers)
                {
                    SelectionManager.Instance.SelectedFirstSectionDistancePipes.Add(fixtureLayer.ChangeFirstSectionDistancePipes(slot.size));

                    if(!fixtureLayer.IsTooHighSections())
                    {
                        fixtureLayer.RecreateSections();
                    }
                }

                ChargeHandler.Instance.IsTooHighLayer();
                ChargeHandler.Instance.CheckForMoreLayerSpace();
                
                SelectionManager.Instance.SelectedFirstSectionDistancePipes.Clear();
                
                foreach (FixtureLayer fixtureLayer in ChargeHandler.Instance.FixtureLayers)
                {
                    SelectionManager.Instance.SelectedFirstSectionDistancePipes.Add(fixtureLayer.FirstSectionDistancePipe);
                }
                
                SelectionManager.Instance.SelectedFirstSectionDistancePipes.SetSelection(true);
                SelectionManager.Instance.SelectedFirstSectionDistancePipes.ChangeMaterial(SelectionManager.Instance.selectedMaterial);

                break;
            default:
                Debug.Log("Nyfiken");
                break;
        }
    }
}
