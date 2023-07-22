using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSorter : IComparer
{
    //enables the sorting by GameObject name; used for sorting the stages into stageArray in the GameManager
    int IComparer.Compare(object x, object y)
    {
        return ((new CaseInsensitiveComparer()).Compare(((GameObject)x).name, ((GameObject)y).name));
    }
}
