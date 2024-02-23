using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

public class ReadFile : MonoBehaviour
{
    public List<Element> createdElements;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    [Button]
    void setMerge()
    {
        
        for (int i = 1; i < createdElements.Count; i +=3)
        {
            var element = AssetDatabase.LoadAssetAtPath("Assets/TestElement/" + createdElements[i].name + ".asset",typeof(ScriptableObject)) as Element  ;
            var nextElement = AssetDatabase.LoadAssetAtPath("Assets/TestElement/" + createdElements[i+1].name + ".asset",typeof(ScriptableObject)) as Element  ;
            var previousElement = AssetDatabase.LoadAssetAtPath("Assets/TestElement/" + createdElements[i-1].name + ".asset",typeof(ScriptableObject)) as Element  ;
            element.mergeTable = new List<ElementMerge>();
            var mergeTable = new ElementMerge();
            mergeTable.mergeWith = nextElement;
            mergeTable.outcome =  previousElement;
            element.mergeTable.Add(mergeTable);
            AssetDatabase.SaveAssets();
        }
    }

    [Button]
    void ReadString()
    {
        string[] lines = File.ReadAllLines("Assets/merge.txt");
        Debug.Log(lines.Length);
        for (int i = 0; i < lines.Length; i = i + 3)
        {
            if (!createdElements.Any(x=> x.name == lines[i].Trim()))
            {
                Element asset = ScriptableObject.CreateInstance<Element>();
                asset.name = lines[i].Trim();
                AssetDatabase.CreateAsset(asset, "Assets/TestElement/" + lines[i].Trim() + ".asset");
                createdElements.Add(asset);
                AssetDatabase.SaveAssets();
                if (!createdElements.Any(x=> x.name == lines[i+1].Trim()))
                {
                    Element asset2 = ScriptableObject.CreateInstance<Element>();
                    asset2.name = lines[i+1].Trim();
                    AssetDatabase.CreateAsset(asset2, "Assets/TestElement/" + lines[i+1].Trim() + ".asset");
                    createdElements.Add(asset2);
                    AssetDatabase.SaveAssets();
                }
                if (!createdElements.Any(x=> x.name == lines[i+2].Trim()))
                {
                    Element asset2 = ScriptableObject.CreateInstance<Element>();
                    asset2.name = lines[i+2].Trim();
                    AssetDatabase.CreateAsset(asset2, "Assets/TestElement/" + lines[i+2].Trim() + ".asset");
                    createdElements.Add(asset2);
                    AssetDatabase.SaveAssets();
                }
            }
            


            //Debug.Log(lines[i].Trim() + " = " + lines[i + 1].Trim() + " + " + lines[i + 2].Trim());
        }



        EditorUtility.FocusProjectWindow();
    }
}