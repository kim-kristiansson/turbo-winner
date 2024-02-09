using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class ButtonGenerate :MonoBehaviour
{
    public InputField inputFieldDiameter;
    public InputField inputFieldHeight;
    
    public void GenerateCharge()
    {
        float diameter = float.Parse(inputFieldDiameter.text);
        float height = float.Parse(inputFieldHeight.text);
        TestPlate testPlate = FixtureHandler.Current.DecidePlate(diameter);

        GeneratorManager.Instance.GenerateCharge(diameter, height, testPlate);
    }
}