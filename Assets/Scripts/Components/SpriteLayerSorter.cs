using UnityEngine;

public class SpriteLayerSorter : MonoBehaviour
{
    Vector3 SavePos;

    void OnEnable() => UpdateSortingLayer();

    void UpdateSortingLayer()
    {
        Vector3 position = transform.position;
        position.z = position.y / 100f;
        transform.position = position;
    }

    void Update()
    {
        if(transform.position != SavePos) 
        {
            UpdateSortingLayer();
            SavePos = transform.position;
        }
    }
}