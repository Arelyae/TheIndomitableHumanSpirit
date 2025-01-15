using UnityEngine;

[CreateAssetMenu(fileName = "ZTargetingData", menuName = "Game/Z-Targeting Data", order = 0)]
public class ZTargetingData : ScriptableObject
{
    [Header("Z-Targeting Settings")]
    public float maxTargetingDistance = 20f;  
    public float radiusOffset = 2f;  
}
