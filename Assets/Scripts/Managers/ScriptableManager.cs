using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class ScriptableManager : MonoBehaviour
{
    [SerializeField] GameData gameData;
    [SerializeField] PlayerMovementSettings PlayerMovementSettings;
    [SerializeField] LevelElementData LevelElementData;


    //-------------------------------------------------------------------
    void Awake()
    {
        SaveManager.LoadGameData(gameData);

        Scriptable.GameData = GetGameData;
        Scriptable.PlayerSettings = GetPlayerMovementSettings;
        Scriptable.LevelElementData = GetLevelElementData;
    }


    //-------------------------------------------------------------------
    GameData GetGameData() => gameData;
    LevelElementData GetLevelElementData() => LevelElementData;


    //-------------------------------------------------------------------
    PlayerMovementSettings GetPlayerMovementSettings() => PlayerMovementSettings;

}



public static class Scriptable
{
    public static Func<GameData> GameData;
    public static Func<PlayerMovementSettings> PlayerSettings;
    public static Func<LevelElementData> LevelElementData;

}