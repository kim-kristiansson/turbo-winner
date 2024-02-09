using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InformationManager : MonoBehaviour
{
    public Text weightInfo;
    public Text amountInfo;
    private void Update()
    {
        if(weightInfo.text != FixtureHandler.Current.ToString())
        {
            weightInfo.text = FixtureHandler.Current.weight.ToString();
        }

        if(amountInfo.text != FixtureHandler.Current.amount.ToString())
        {
            amountInfo.text = FixtureHandler.Current.amount.ToString();
        }
    }
}
