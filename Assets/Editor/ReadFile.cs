using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

public class ReadFile : EditorWindow
{
    public static List<Element> createdElements;





    [MenuItem("Element/Create New Elements",false,10)]
    public static void ReadString()
    {
        createdElements = new List<Element>();

        var info = new DirectoryInfo("Assets/TestElement");
        var fileInfo = info.GetFiles();
        foreach (var file in fileInfo)
        {
            file.Delete();
        }
        AssetDatabase.Refresh();
        string[] lines = File.ReadAllLines("Assets/merge.txt");
        Debug.Log(lines.Length);
        for (int i = 0; i < lines.Length; i = i + 3)
        {
            if (!createdElements.Any(x=> x.name == lines[i]))
            {
                Element asset = ScriptableObject.CreateInstance<Element>();
                asset.name = lines[i];
                asset.mergeTable = new List<ElementMerge>();
                AssetDatabase.CreateAsset(asset, "Assets/TestElement/" + lines[i] + ".asset");
                createdElements.Add(asset);
                AssetDatabase.SaveAssets();
                if (!createdElements.Any(x=> x.name == lines[i+1]))
                {
                    Element asset2 = ScriptableObject.CreateInstance<Element>();
                    asset2.name = lines[i+1];
                    asset2.mergeTable = new List<ElementMerge>();
                    AssetDatabase.CreateAsset(asset2, "Assets/TestElement/" + lines[i+1] + ".asset");
                    createdElements.Add(asset2);
                    AssetDatabase.SaveAssets();
                }
                else
                {
                    createdElements.Add(createdElements.First(x=> x.name == lines[i+1]));
                }
                if (!createdElements.Any(x=> x.name == lines[i+2]))
                {
                    Element asset2 = ScriptableObject.CreateInstance<Element>();
                    asset2.name = lines[i+2];
                    asset2.mergeTable = new List<ElementMerge>();
                    AssetDatabase.CreateAsset(asset2, "Assets/TestElement/" + lines[i+2] + ".asset");
                    createdElements.Add(asset2);
                    AssetDatabase.SaveAssets();
                }
                else
                {
                    createdElements.Add(createdElements.First(x=> x.name == lines[i+2]));
                }
            }
            else
            {
                createdElements.Add(createdElements.First(x=> x.name == lines[i]));
            }
            


            //Debug.Log(lines[i] + " = " + lines[i + 1] + " + " + lines[i + 2]);
        }
        for (int i = 1; i < lines.Length; i +=3)
        {
            
            var element = AssetDatabase.LoadAssetAtPath("Assets/TestElement/" + lines[i] + ".asset",typeof(ScriptableObject)) as Element  ;
            Debug.Log(lines[i]);
            Debug.Log(element);
            var nextElement = AssetDatabase.LoadAssetAtPath("Assets/TestElement/" + lines[i+1] + ".asset",typeof(ScriptableObject)) as Element  ;
            var previousElement = AssetDatabase.LoadAssetAtPath("Assets/TestElement/" + lines[i-1] + ".asset",typeof(ScriptableObject)) as Element  ;
            //element.mergeTable = new List<ElementMerge>();
            var mergeTable = new ElementMerge();
            mergeTable.mergeWith = nextElement;
            mergeTable.outcome =  previousElement;
            element.mergeTable.Add(mergeTable);
            AssetDatabase.SaveAssets();
        }


        EditorUtility.FocusProjectWindow();
    }
}