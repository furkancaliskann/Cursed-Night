using System.Collections.Generic;
using UnityEngine;

public class BuildingPreview : MonoBehaviour
{
    public List<GameObject> number = new List<GameObject>();

    void Start()
    {
        ChangeMeshColor(gameObject, Color.green);
    }
    private void ChangeMeshColor(GameObject meshObject, Color color)
    {
        if (meshObject == null) return;

        MeshRenderer meshRenderer = meshObject.GetComponent<MeshRenderer>();

        if (meshRenderer != null)
        {
            for (int i = 0; i < meshRenderer.materials.Length; i++)
            {
                meshRenderer.materials[i].color = color;
            }
        }

        for (int i = 0; i < meshObject.transform.childCount; i++)
        {
            if (meshObject.transform.GetChild(i).gameObject != gameObject)
            {
                ChangeMeshColor(meshObject.transform.GetChild(i).gameObject, color);
            }
        }
    }
    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Terrain")) return;

        ChangeMeshColor(gameObject, Color.red);
        if (!number.Contains(other.gameObject))
            number.Add(other.gameObject);
    }
    void OnTriggerExit(Collider other)
    {
        if (number.Contains(other.gameObject))
            number.Remove(other.gameObject);
        if(number.Count == 0)
            ChangeMeshColor(gameObject, Color.green);
    }
}
