using UnityEngine;

public class ReferencesManager : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private LayerMask groundLayerMask;
    [SerializeField] private LayerMask obstacleLayerMask;
    [SerializeField] private Material greenGhostMaterial;
    [SerializeField] private Material redGhostMaterial;

    public LayerMask GroundLayerMask => groundLayerMask;
    public LayerMask ObstacleLayerMask => obstacleLayerMask;
    public Material GreenGhostMaterial => greenGhostMaterial;
    public Material RedGhostMaterial => redGhostMaterial;

    public static ReferencesManager Instance;

	private void Awake()
    {
        Instance = this;
    }
}
