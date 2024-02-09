using UnityEngine;
using System.Collections;

public class FixtureStick : Gadget
{
    private void OnDestroy()
    {
        Debug.Log("Förstörd");
    }
    public void AddSticks(Transform parent)
    {
        FixtureStick _fixtureStick = Instantiate(this, parent);
        _fixtureStick.Bottom = parent.GetComponent<Gadget>().Top;

        if (FixtureHandler.Current.FixtureStick == null)
        {
            FixtureHandler.Current.FixtureStick = _fixtureStick;
        }

        FixtureHandler.Current.CountWeight(FixtureHandler.Current.FixtureStick);
    }
}
