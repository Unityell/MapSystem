using System.Collections.Generic;
using UnityEngine;

public class Pool
{
    GameObject PoolFolder;

    private Dictionary<string, GameObject> ParentFolders = new Dictionary<string, GameObject>();

    public Pool()
    {
        PoolFolder = new GameObject("Pool");
        PoolFolder.transform.SetParent(GameObject.FindGameObjectWithTag("Systems").transform);
    }

    public void Refresh()
    {
        for (int i = 0; i < PoolFolder.transform.childCount; i++)
        {
            MonoBehaviour.Destroy(PoolFolder.transform.GetChild(i).gameObject);
        }

        ParentFolders.Clear();
    }

    public T TryGetDeactiveObjByType<T>(string Name) where T : Component
    {
        return GetAllOfType<T>().Find(x => !x.gameObject.activeSelf && x.gameObject.name == Name);
    }

    public T TryGetDeactiveObjByType<T>() where T : Component
    {
        return GetAllOfType<T>().Find(x => !x.gameObject.activeSelf);
    }

    public void Add(Component obj)
    {
        string TypeName = obj.GetType().Name;

        if (!ParentFolders.ContainsKey(TypeName))
        {
            GameObject TypeParent = new GameObject($"{TypeName} [1]");
            TypeParent.transform.SetParent(PoolFolder.transform);
            ParentFolders[TypeName] = TypeParent;
        }
        else
        {
            GameObject parentFolder = ParentFolders[TypeName];
            int currentCount = parentFolder.transform.childCount + 1;
            parentFolder.name = $"{TypeName} [{currentCount}]";
        }

        obj.transform.SetParent(ParentFolders[TypeName].transform);
    }

    public List<T> GetAllOfType<T>() where T : Component
    {
        string TypeName = typeof(T).Name;

        if (ParentFolders.TryGetValue(TypeName, out GameObject ParentFolder))
        {
            T[] Components = ParentFolder.GetComponentsInChildren<T>(true);
            return new List<T>(Components);
        }
        else
        {
            return new List<T>();
        }
    }
}