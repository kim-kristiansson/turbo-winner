using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : Gadget
{
	public Material onEnterMaterial;
    public bool isSelected = false;
    [HideInInspector]
    public Material defaultMaterial;
    List<Equipment> children = new List<Equipment>();
    public List<Equipment> Children { get { return children; } set { children = value; } }
    private void Awake()
    {
        //if(ChargeHandler.Instance.CurrentLayer().BuildBottom == null)
        //{
        //    ChargeHandler.Instance.CurrentLayer().BuildBottom = this;
        //}
        //
        //ChargeHandler.Instance.CurrentLayer().BuildTop = this;
    }
    private void Start()
    {
        defaultMaterial = this.GetComponent<Renderer>().material;
    }
}
