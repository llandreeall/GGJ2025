using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : MonoBehaviour
{
    private T prefab;
    private List<T> objectsList;
    private List<T> activeObjectsList;
    private int initialSize;
    private Transform parentTransform;

    public ObjectPool(T prefab, int initialSize = 1)
    {
        this.prefab = prefab;
        this.initialSize = initialSize;
        parentTransform = null;
        objectsList = new List<T>();
        activeObjectsList = new List<T>();
        InstantiateObjects();
    }

    public ObjectPool(List<T> prefabList, Transform parentTransform, int initialSize = 1)
    {
        if (prefabList.Count == 0)
        {
            Debug.LogError("Prefab list you want to pass to pool is empty!");
            return;
        }

        this.initialSize = initialSize;
        this.parentTransform = parentTransform;

        objectsList = new List<T>();
        activeObjectsList = new List<T>();

        int amountOfEach = initialSize / prefabList.Count;
        int spawnedItems = 0;
        int index = 0;
        while (spawnedItems < initialSize)
        {
            prefab = prefabList[index];
            T obj = InstantiateObject();
            objectsList.Add(obj);
            spawnedItems++;
            if (spawnedItems % amountOfEach == 0)
                index++;
        }
    }

    public ObjectPool(T prefab, Transform parentTransform, int initalSize = 1)
    {
        this.prefab = prefab;
        initialSize = initalSize;
        this.parentTransform = parentTransform;
        objectsList = new List<T>();
        activeObjectsList = new List<T>();
        InstantiateObjects();
    }

    public ObjectPool(ObjectPool<T> other)
    {
        prefab = other.prefab;
        objectsList = other.objectsList;
        activeObjectsList = other.activeObjectsList;
        initialSize = other.initialSize;
        parentTransform = other.parentTransform;
    }

    private void InstantiateObjects()
    {
        for (int i = 0; i < initialSize; i++)
        {
            if (prefab == null)
            {
                Debug.LogError("Prefab is null");
                return;
            }
            T obj = InstantiateObject();
            objectsList.Add(obj);
            obj.name += "_" + i;
        }
    }

    private T InstantiateObject()
    {
        T obj = null;
        if (parentTransform != null)
        {
            obj = Object.Instantiate(prefab, parentTransform);
        }
        else
        {
            obj = Object.Instantiate(prefab);
        }
        obj.gameObject.SetActive(false);
        return obj;
    }

    public T GetFirstAvailable()
    {
        for (int i = 0; i < objectsList.Count; i++)
        {
            if (!activeObjectsList.Contains(objectsList[i]))
            {
                if (!objectsList[i].gameObject.activeSelf)
                {
                    objectsList[i].gameObject.SetActive(true);
                }
                activeObjectsList.Add(objectsList[i]);
                return objectsList[i];
            }
        }

        T obj = InstantiateObject();
        obj.gameObject.SetActive(true);
        objectsList.Add(obj);
        activeObjectsList.Add(obj);
        return obj;
    }

    public T GetRandomAvailable()
    {
        List<T> eligibleObjects = new List<T>();
        for (int i = 0; i < objectsList.Count; i++)
        {
            if (!activeObjectsList.Contains(objectsList[i]))
            {
                eligibleObjects.Add(objectsList[i]);
            }
        }

        if (eligibleObjects.Count > 0)
        {
            T randomObj = eligibleObjects[Random.Range(0, eligibleObjects.Count)];
            if (!randomObj.gameObject.activeSelf)
            {
                randomObj.gameObject.SetActive(true);
            }
            activeObjectsList.Add(randomObj);
            return randomObj;

        }

        T obj = InstantiateObject();
        if (!obj.gameObject.activeSelf)
        {
            obj.gameObject.SetActive(true);
        }
        objectsList.Add(obj);
        activeObjectsList.Add(obj);
        return obj;
    }

    public void RemoveObject(T obj)
    {
        if (obj.gameObject.activeSelf)
        {
            obj.gameObject.SetActive(false);
        }
    }

    public void DeleteFromPool(T obj)
    {
        if (objectsList.Contains(obj))
        {
            activeObjectsList.Remove(obj);
            objectsList.Remove(obj);
        }
    }

    public void ReturnToPool(T obj, bool disableObject = true)
    {
        if (objectsList.Contains(obj))
        {
            if (parentTransform != null)
            {
                obj.transform.SetParent(parentTransform, false);
                obj.transform.localPosition = Vector3.zero;
            }
            if (disableObject)
            {
                RemoveObject(obj);
            }
            else
            {
                obj.transform.position = Vector3.one * -10000;
            }
            activeObjectsList.Remove(obj);
        }
    }

    public List<T> GetObjectsList()
    {
        return objectsList;
    }

    public List<T> GetUseObjects()
    {
        return activeObjectsList;
    }

    public bool PoolIsEmpty()
    {
        return objectsList.Count == 0;
    }
}