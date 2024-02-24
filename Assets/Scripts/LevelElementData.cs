using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu]
public class LevelElementData : ScriptableObject
{
    public List<Element> startElements;
    public List<Element> craftedElements;
    //public List<Element> allElements;


    [Button]
    public void ClearSave()
    {
        craftedElements.Clear();
        ES3.Save("data",this);
    }
    
    /*public Element GetUndiscoveredElement()
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
    }*/
    
    public int GetCraftedElementAmount()
    {
        int temp = 0;

        temp += startElements.Count + craftedElements.Count;

        return temp;
    }
}
