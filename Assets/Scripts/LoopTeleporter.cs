using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopTeleporter : MonoBehaviour
{
    private LevelData _levelDataObject;

    private void Start()
    {
        //get data objects
        _levelDataObject = DataManager.Instance.LevelDataObject;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        BaseObject baseObject = other?.GetComponent<BaseObject>();

        if (baseObject != null)
        {
            if (baseObject.transform.position.y - transform.position.y > _levelDataObject.LevelSize.y / 2.0f)
            {
                //teleport north to south
                baseObject.transform.position = new Vector3(baseObject.transform.position.x, baseObject.transform.position.y - _levelDataObject.LevelSize.y, baseObject.transform.position.z);
            }
            else if (transform.position.y - baseObject.transform.position.y > _levelDataObject.LevelSize.y / 2.0f)
            {
                //teleport south to north
                baseObject.transform.position = new Vector3(baseObject.transform.position.x, baseObject.transform.position.y + _levelDataObject.LevelSize.y, baseObject.transform.position.z);
            }

            if (baseObject.transform.position.x - transform.position.x > _levelDataObject.LevelSize.x / 2.0f)
            {
                //teleport east to west
                baseObject.transform.position = new Vector3(baseObject.transform.position.x - _levelDataObject.LevelSize.x, baseObject.transform.position.y, baseObject.transform.position.z);
            }
            else if (transform.position.x - baseObject.transform.position.x > _levelDataObject.LevelSize.x / 2.0f)
            {
                //teleport west to east
                baseObject.transform.position = new Vector3(baseObject.transform.position.x + _levelDataObject.LevelSize.x, baseObject.transform.position.y, baseObject.transform.position.z);
            }
        }
    }
}
