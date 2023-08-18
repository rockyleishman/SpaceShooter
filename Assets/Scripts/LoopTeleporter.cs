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

    private void LateUpdate()
    {
        //lock rotation //NEEDS WORK
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        BaseObject baseObject = other?.GetComponent<BaseObject>();

        if (baseObject != null)
        {
            if (baseObject.transform.position.y - transform.position.y > _levelDataObject.LevelSize.y / 2.0f)
            {
                //disable trails
                TrailRenderer[] trails = baseObject?.GetComponentsInChildren<TrailRenderer>();
                DisableTrails(trails);

                //teleport north to south
                baseObject.transform.position = new Vector3(baseObject.transform.position.x, baseObject.transform.position.y - _levelDataObject.LevelSize.y, baseObject.transform.position.z);

                //enable trails
                EnableTrails(trails);
            }
            else if (transform.position.y - baseObject.transform.position.y > _levelDataObject.LevelSize.y / 2.0f)
            {
                //disable trails
                TrailRenderer[] trails = baseObject?.GetComponentsInChildren<TrailRenderer>();
                DisableTrails(trails);

                //teleport south to north
                baseObject.transform.position = new Vector3(baseObject.transform.position.x, baseObject.transform.position.y + _levelDataObject.LevelSize.y, baseObject.transform.position.z);

                //enable trails
                EnableTrails(trails);
            }

            if (baseObject.transform.position.x - transform.position.x > _levelDataObject.LevelSize.x / 2.0f)
            {
                //disable trails
                TrailRenderer[] trails = baseObject?.GetComponentsInChildren<TrailRenderer>();
                DisableTrails(trails);

                //teleport east to west
                baseObject.transform.position = new Vector3(baseObject.transform.position.x - _levelDataObject.LevelSize.x, baseObject.transform.position.y, baseObject.transform.position.z);

                //enable trails
                EnableTrails(trails);
            }
            else if (transform.position.x - baseObject.transform.position.x > _levelDataObject.LevelSize.x / 2.0f)
            {
                //disable trails
                TrailRenderer[] trails = baseObject?.GetComponentsInChildren<TrailRenderer>();
                DisableTrails(trails);

                //teleport west to east
                baseObject.transform.position = new Vector3(baseObject.transform.position.x + _levelDataObject.LevelSize.x, baseObject.transform.position.y, baseObject.transform.position.z);

                //enable trails
                EnableTrails(trails);
            }
        }
    }

    private void DisableTrails(TrailRenderer[] trails)
    {
        if (trails.Length > 0)
        {
            foreach (TrailRenderer trail in trails)
            {
                trail.emitting = false;
            }
        }
    }

    private void EnableTrails(TrailRenderer[] trails)
    {
        if (trails.Length > 0)
        {
            foreach (TrailRenderer trail in trails)
            {
                trail.emitting = true;
                trail.Clear();
            }
        }
    }
}
