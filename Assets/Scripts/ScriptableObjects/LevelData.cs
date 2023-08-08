using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelDataObject", menuName = "LevelDataObject", order = 50)]
public class LevelData : ScriptableObject
{
    [SerializeField] public Vector2 LevelSize = new Vector2(10000.0f, 10000.0f);
}
