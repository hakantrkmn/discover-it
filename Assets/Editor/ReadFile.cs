using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class ReadFile : EditorWindow
{
    public static string path = "Assets/Elements";

    [MenuItem("Element/Create New Elements", false, 10)]
    public static void ReadString()
    {
        var createdElements = new List<string>();

        var info = new DirectoryInfo("Assets/Elements");
        var fileInfo = info.GetFiles();

        foreach (var file in fileInfo)
        {
            file.Delete();
        }

        AssetDatabase.Refresh();

        string[] lines = File.ReadAllLines("Assets/merge.txt");

        foreach (var line in lines)
        {
            if (!createdElements.Contains(line))
            {
                Element asset = ScriptableObject.CreateInstance<Element>();
                asset.name = line;
                asset.mergeTable = new List<ElementMerge>();
                AssetDatabase.CreateAsset(asset, path + "/" + line + ".asset");
                createdElements.Add(asset.name);
                AssetDatabase.SaveAssets();
            }
            else
            {
                createdElements.Add(line);
            }
        }

        for (int i = 1; i < lines.Length; i += 3)
        {
            var element =
                AssetDatabase.LoadAssetAtPath(path + "/" + lines[i] + ".asset", typeof(ScriptableObject)) as
                    Element;
            var nextElement =
                AssetDatabase.LoadAssetAtPath(path + "/" + lines[i + 1] + ".asset",
                    typeof(ScriptableObject)) as Element;
            var previousElement =
                AssetDatabase.LoadAssetAtPath(path + "/" + lines[i - 1] + ".asset",
                    typeof(ScriptableObject)) as Element;
            var mergeTable = new ElementMerge
            {
                mergeWith = nextElement,
                outcome = previousElement
            };
            element.mergeTable.Add(mergeTable);
            EditorUtility.SetDirty(element);
            AssetDatabase.SaveAssets();
        }


        EditorUtility.FocusProjectWindow();
    }
}