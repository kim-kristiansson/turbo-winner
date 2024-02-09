using UnityEngine;
using System.Collections;
using System.Transactions;

public class Gadget : BaseClass
{
    private Vector3 v3;

    public float weight;
    public bool isLeft;
    public bool isRight;

    public float Height 
    {
        get { return MathUtils.GetHeight(this.gameObject); }
    }
    public float Top 
    {
        get 
        {
            v3 = this.transform.position;
            return this.GetComponent<Renderer>().bounds.max.y;
        }
        set 
        {
            v3 = this.transform.position;
            v3.y = value - (MathUtils.GetHeight(this.gameObject) / 2);
            this.transform.position = v3;
        }
    }
    public float Bottom 
    {
        get 
        {
            v3 = this.transform.position;
            return this.GetComponent<Renderer>().bounds.min.y;
        }
        set {
            v3 = this.transform.position;
            v3.y = value + (MathUtils.GetHeight(this.gameObject) / 2);
            this.transform.position = v3;
        }
    }
}