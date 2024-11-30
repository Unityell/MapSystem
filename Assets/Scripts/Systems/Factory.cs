using UnityEngine;

public class Factory
{
    private Pool Pool;

    public Factory(Pool Pool)
    {
        this.Pool = Pool;
    }

    public T CreateByType<T>() where T : Component
    {
        var Object = Pool.TryGetDeactiveObjByType<T>();

        if(Object)
        {
            Object.gameObject.SetActive(true);
            return Object;
        }
        else
        {
            GameObject NewObject = new GameObject();
            var Component = NewObject.AddComponent<T>();
            Pool.Add(Component);
            return Component;
        }
    }

    public T Create<T>(GameObject Prefab, Vector3 Position, Quaternion Rotation) where T : Component
    {
        var Object = Pool.TryGetDeactiveObjByType<T>(Prefab.name);

        if(Object)
        {
            Object.transform.position = Position;
            Object.transform.rotation = Rotation;
            Object.gameObject.SetActive(true);
            return Object;
        }
        else
        {
            GameObject Instance = MonoBehaviour.Instantiate(Prefab, Position, Rotation);
            Instance.name = Prefab.name;
            var Component = Instance.GetComponent<T>();
            Pool.Add(Component);
            return Component;
        }
    }

    public T Create<T>(GameObject Prefab, Vector3 Position) where T : Component
    {
        return Create<T>(Prefab, Position, Quaternion.identity);
    }

    public T Create<T>(GameObject Prefab) where T : Component
    {
        return Create<T>(Prefab, Vector3.zero, Quaternion.identity);
    }
}