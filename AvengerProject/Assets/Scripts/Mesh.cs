using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mesh : MonoBehaviour
{
    public SpawnArea spawnArea;
    bool isRendering;
    private void Awake()
    {
        if(this.transform.childCount == 0)
        {
            this.transform.parent.GetComponent<SpawnPlate>().spawnAreaList.Add(Instantiate(spawnArea, this.transform));
        }
        else
        {
            this.transform.parent.GetComponent<SpawnPlate>().spawnAreaList.Add(this.transform.GetChild(0).GetComponent<SpawnArea>());
        }
    }
}
