using UnityEngine;

public class AmmoTrace : MonoBehaviour
{
    [SerializeField] private GameObject tracePrefab;
    
    public void CreateTrace(RaycastHit hit)
    {
        GameObject trace = Instantiate(tracePrefab, hit.point + (hit.normal * 0.02f), Quaternion.LookRotation(hit.normal), hit.transform);
        Destroy(trace, 10f);
    }
}
