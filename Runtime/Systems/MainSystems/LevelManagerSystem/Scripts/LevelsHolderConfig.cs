using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelsHolderConfig", menuName = "Level System/LevelsHolderConfig")]
public class LevelsHolderConfig : ScriptableObject
{
    [SerializeField] List<LevelConfig> LevelList = new();
    public List<LevelConfig> GetLevelList() => LevelList;
}
