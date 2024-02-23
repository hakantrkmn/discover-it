using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu]
public class LevelElementData : ScriptableObject
{
    public List<Element> startElements;
    public List<Element> craftedElements;
    public List<Element> allElements;

    public Element GetUndiscoveredElement()
    {
        foreach (var element in startElements)
        {
            allElements.Remove(element);
        }

        foreach (var element in craftedElements)
        {
            allElements.Remove(element);
        }

        return allElements[Random.Range(0, allElements.Count)];
    }
}
