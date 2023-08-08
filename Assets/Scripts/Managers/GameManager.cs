using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    private LevelData LevelDataObject;

    private void Start()
    {
        //get data objects
        LevelDataObject = DataManager.Instance.LevelDataObject;
    }
}
