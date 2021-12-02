using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameInfo", menuName = "CreateInfoHolder")]
public class InfoScriptable : ScriptableObject
{
    [TextArea(10, 100)]
    public string textAreaString;

    [Multiline(40)]
    public string multilineString;
}