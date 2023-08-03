using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : Singleton<PoolManager>
{
    private Dictionary<string, Stack<PoolObject>> _stackDictionary = new Dictionary<string, Stack<PoolObject>>();

    private void Start()
    {
        PoolManager.Instance.Load();
    }

    private void Load()
    {
        //load pool object prefabs from resources
        PoolObject[] poolObjects = Resources.LoadAll<PoolObject>("PoolObjects");

        //create stack in dictionary for each prefab
        foreach (PoolObject poolObject in poolObjects)
        {
            //create stack
            Stack<PoolObject> poolObjectStack = new Stack<PoolObject>();
            //populate stack
            poolObjectStack.Push(poolObject);
            //add stack to dictionary
            _stackDictionary.Add(poolObject.name, poolObjectStack);
        }
    }

    public PoolObject Spawn(string name)
    {
        Stack<PoolObject> poolObjectStack = _stackDictionary[name];

        if (poolObjectStack.Count == 1)
        {
            //return clone if only 1 left
            PoolObject clone = Instantiate(poolObjectStack.Peek());
            clone.name = name;
            return clone;
        }
        else
        {
            //pop, activate, and return
            PoolObject poolObject = poolObjectStack.Pop();
            poolObject.gameObject.SetActive(true);
            return poolObject;
        }
    }

    public void Despawn(PoolObject poolObject)
    {
        //deactivate
        poolObject.gameObject.SetActive(false);

        //push to stack
        Stack<PoolObject> poolObjectStack = _stackDictionary[poolObject.name];
        poolObjectStack.Push(poolObject);
    }
}
