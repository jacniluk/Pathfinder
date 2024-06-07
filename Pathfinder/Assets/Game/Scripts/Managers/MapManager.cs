using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform mapRoot;
    [SerializeField] private Transform ground;
    [SerializeField] private SpriteRenderer grid;

    [Header("Prefabs")]
    [SerializeField] private GameObject obstaclePrefab;

    public static MapManager Instance;

    private const int PLACING_OBSTACLES_MAX_TRIES = 100;

    private Vector3 obstacleSize;
    private float obstacleLongestSide;

    private List<GameObject> obstacles;
    private int randomObstaclesCount;

    public Transform Ground => ground;
    public GameObject ObstaclePrefab => obstaclePrefab;

    private void Awake()
	{
		Instance = this;

        GameObject obstacle = Instantiate(obstaclePrefab);
        obstacleSize = obstacle.transform.GetChild(0).localScale;
        Destroy(obstacle);

        obstacleLongestSide = obstacleSize.x > obstacleSize.z ? obstacleSize.x : obstacleSize.z;

        obstacles = new List<GameObject>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            ToggleGrid();
        }
    }

    private void ToggleGrid()
    {
        grid.gameObject.SetActive(grid.gameObject.activeSelf == false);
    }

    public void SetMapSizeWidth(float mapSizeWidth)
	{
        ground.localScale = new Vector3(mapSizeWidth / 10.0f, ground.localScale.y, ground.localScale.z);
        grid.size = new Vector2(mapSizeWidth, grid.size.y);
	}

    public void SetMapSizeLength(float mapSizeLength)
    {
        ground.localScale = new Vector3(ground.localScale.x, ground.localScale.y, mapSizeLength / 10.0f);
        grid.size = new Vector2(grid.size.x, mapSizeLength);
    }

    public void SetRandomObstaclesCount(int _randomObstaclesCount)
    {
        randomObstaclesCount = _randomObstaclesCount;
    }

    public void PlaceRandomObstacles()
    {
        StartCoroutine(PlaceRandomObstaclesCoroutine());
    }

    private IEnumerator PlaceRandomObstaclesCoroutine()
    {
        ClearObstacles();

        yield return null;

        Vector3 randomPosition = Vector3.zero;
        float maxRandomPlacingRangeX = (ground.localScale.x * 10.0f - obstacleLongestSide) / 2.0f;
        float maxRandomPlacingRangeZ = (ground.localScale.z * 10.0f - obstacleLongestSide) / 2.0f;
        Quaternion randomRotation = Quaternion.identity;
        for (int i = 0; i < randomObstaclesCount; i++)
        {
            bool foundRandomPosition = false;
            int tries = 0;
            while (foundRandomPosition == false)
            {
                randomPosition = new Vector3(
                    Utilities.RandomValue(0.0f, maxRandomPlacingRangeX) * Utilities.RandomDirection(),
                    obstaclePrefab.transform.position.y,
                    Utilities.RandomValue(0.0f, maxRandomPlacingRangeZ) * Utilities.RandomDirection()
                );

                randomRotation = Utilities.RandomRotationY();

                if (ValidateObstacle(randomPosition, randomRotation))
                {
                    foundRandomPosition = true;
                }
                else
                {
                    tries++;
                    if (tries >= PLACING_OBSTACLES_MAX_TRIES)
                    {
#if UNITY_EDITOR
                        Debug.Log("Cannot create more obstacles. Stuck after " + obstacles.Count + ".");
#endif

                        break;
                    }
                }
            }

            if (foundRandomPosition)
            {
                PlaceObstacle(randomPosition, randomRotation);
            }
            else
            {
                break;
            }
        }
    }

    public void ClearObstacles()
    {
        for (int i = 0; i < obstacles.Count; i++)
        {
            Destroy(obstacles[i]);
        }
        obstacles.Clear();
    }

    public bool ValidateObstacle(Vector3 position, Quaternion rotation)
    {
        return Physics.CheckBox(position, obstacleSize / 2.0f, rotation, ReferencesManager.Instance.ObstacleLayerMask) == false;
    }

    public void PlaceObstacle(Vector3 position, Quaternion rotation)
    {
        GameObject obstacle = Instantiate(obstaclePrefab, position, rotation, mapRoot);
        obstacles.Add(obstacle);
    }
}
