using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Element : ScriptableObject
{
    public string name;
    public Sprite image;
    public List<ElementMerge> mergeTable;

   
}
