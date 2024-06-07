using System.Collections;
using UnityEngine;

public class ObstaclePlacingManager : MonoBehaviour
{
	[Header("Settings")]
	[SerializeField] private float rotateAngle;

	public static ObstaclePlacingManager Instance;

	private IEnumerator placingCoroutine;
	private GameObject obstacleGhost;

	private void Awake()
	{
		Instance = this;
	}

	public void EnablePlacingMode()
	{
        placingCoroutine = PlacingCoroutine();
		StartCoroutine(placingCoroutine);
	}

	private IEnumerator PlacingCoroutine()
	{
		Ray ray;
        RaycastHit raycastHit;
        obstacleGhost = Instantiate(MapManager.Instance.ObstaclePrefab);
        obstacleGhost.GetComponent<Collider>().enabled = false;
		MeshRenderer obstacleGhostMeshRenderer = obstacleGhost.GetComponentInChildren<MeshRenderer>();
        while (true)
		{
			bool isCursorOnUi = Utilities.IsCursorOnUi();

			if (isCursorOnUi == false)
			{
                ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out raycastHit, Mathf.Infinity, ReferencesManager.Instance.GroundLayerMask))
                {
                    obstacleGhost.transform.position = raycastHit.point;
                    obstacleGhost.SetActive(true);
                }
                else
                {
                    obstacleGhost.SetActive(false);
                }
            }
            else
			{
                obstacleGhost.SetActive(false);
            }

            if (Input.mouseScrollDelta.y != 0.0f)
			{
				obstacleGhost.transform.RotateAround(obstacleGhost.transform.position, obstacleGhost.transform.up, -Input.mouseScrollDelta.y * rotateAngle);
            }

            bool validation = MapManager.Instance.ValidateObstacle(obstacleGhost.transform.position, obstacleGhost.transform.rotation);

            obstacleGhostMeshRenderer.material = validation ? ReferencesManager.Instance.GreenGhostMaterial : ReferencesManager.Instance.RedGhostMaterial;

			if (isCursorOnUi == false && Input.GetMouseButtonDown(0) && validation)
			{
				MapManager.Instance.PlaceObstacle(obstacleGhost.transform.position, obstacleGhost.transform.rotation);
			}

            yield return null;
		}
	}

	public void DisablePlacingMode()
	{
		StopCoroutine(placingCoroutine);
        Destroy(obstacleGhost);
	}
}
