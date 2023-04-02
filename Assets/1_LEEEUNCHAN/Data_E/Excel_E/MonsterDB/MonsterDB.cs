using System.Collections.Generic;
using UnityEngine;

[ExcelAsset]
public class MonsterDB : ScriptableObject
{
    public List<MonsterDB_Entity> Monsters; // Replace 'EntityType' to an actual type that is serializable.
}
