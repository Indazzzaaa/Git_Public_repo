using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnumFeatureTesting : MonoBehaviour
{
    [System.Flags]
    private enum FeaturingMe : byte
    {
        NONE = 0b_0000_0000,
        TWO = 0b_0000_0001,
        THREE = 0b_0000_0010,
        FOUR = 0b_0000_0100
    }

    [SerializeField] private FeaturingMe _myFeatures;

    // Start is called before the first frame update
    private void Start()
    {
        _myFeatures = FeaturingMe.THREE | FeaturingMe.TWO;
        print(_myFeatures);
    }
}