using UnityEngine;

public class ProximityChecker : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private string targetTag = "Player";
    [SerializeField] private ZTargetingData zTargetingData;  // ScriptableObject reference for Z-targeting settings

    private GameObject targetObject;
    private MeshRenderer sphereMesh;

    private void Start()
    {
        sphereMesh = GetComponent<MeshRenderer>();
        if (sphereMesh == null)
        {
            Debug.LogWarning($"No MeshRenderer found on {gameObject.name}. The mesh won't be deactivated.");
        }else
        {
            // Deactivate the MeshRenderer to hide the object
            if (sphereMesh != null)
            {
                sphereMesh.enabled = false;  // Disables the visual mesh
            }
        }
    }

    private void Update()
    {
        if (targetObject == null)
        {
            targetObject = GameObject.FindGameObjectWithTag(targetTag);
            if (targetObject == null) return;  // No player found
        }

        float distanceToTarget = Vector3.Distance(transform.position, targetObject.transform.position);

        // Use maxTargetingDistance from the ScriptableObject
        if (distanceToTarget <= zTargetingData.maxTargetingDistance)
        {
            PerformAction();
        }
    }

    private void PerformAction()
    {
        Debug.Log($"{gameObject.name} detected {targetObject.name} within {zTargetingData.maxTargetingDistance} units.");

      
    }
}
