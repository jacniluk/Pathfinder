using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PathManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private RunnerController runner;

    [Header("Prefabs")]
    [SerializeField] private GameObject pathmarkerPrefab;

    public static PathManager Instance;

    private List<Vector3> path;
    private List<GameObject> pathmarkers;
    private UnityAction afterSelectPointsAction;
    private IEnumerator selectingPointsCoroutine;
    private GameObject pointMarker;
    private Node start;
    private Node target;

    private void Awake()
	{
		Instance = this;

        path = new List<Vector3>();
        pathmarkers = new List<GameObject>();
    }

    public void EnableSelectingPointsMode(UnityAction _afterSelectPointsAction)
    {
        afterSelectPointsAction = _afterSelectPointsAction;

        selectingPointsCoroutine = SelectingPointsCoroutine();
        StartCoroutine(selectingPointsCoroutine);
    }

    private IEnumerator SelectingPointsCoroutine()
    {
        Ray ray;
        RaycastHit raycastHit;
        Node node;
        pointMarker = Instantiate(pathmarkerPrefab);
        MeshRenderer pointMarkerMeshRenderer = pointMarker.GetComponentInChildren<MeshRenderer>();

        while (target == null)
        {
            if (Utilities.IsCursorOnUi() == false)
            {
                bool isSelectingTarget = start != null;

                ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out raycastHit, Mathf.Infinity, ReferencesManager.Instance.GroundLayerMask))
                {
                    node = NavigationManager.Instance.FindNodeWithPoint(raycastHit.point);

                    bool validation = node.traversable && (isSelectingTarget == false || node != start);

                    pointMarkerMeshRenderer.material = validation ? ReferencesManager.Instance.GreenGhostMaterial : ReferencesManager.Instance.RedGhostMaterial;

                    pointMarker.transform.position = node.position;
                    pointMarker.SetActive(true);

                    if (Input.GetMouseButtonDown(0) && validation)
                    {
                        if (isSelectingTarget == false)
                        {
                            start = node;
                        }
                        else
                        {
                            target = node;
                        }
                        GameObject pathmarker = Instantiate(pathmarkerPrefab, node.position, Quaternion.identity);
                        pathmarkers.Add(pathmarker);
                    }
                }
                else
                {
                    pointMarker.SetActive(false);
                }
            }
            else
            {
                pointMarker.SetActive(false);
            }

            yield return null;
        }

        Destroy(pointMarker);

        afterSelectPointsAction.Invoke();

        selectingPointsCoroutine = null;
    }

    public void DisableSelectingPointsMode()
    {
        if (selectingPointsCoroutine != null)
        {
            StopCoroutine(selectingPointsCoroutine);
            selectingPointsCoroutine = null;
            Destroy(pointMarker);
        }
    }

    public bool BuildPath()
    {
        path = NavigationManager.Instance.FindPath(start, target);
        for (int i = 0; i < path.Count - 1; i++)
        {
            GameObject pathmarker = Instantiate(pathmarkerPrefab, path[i], Quaternion.identity);
            pathmarkers.Add(pathmarker);
        }

        return path.Count > 0;
    }

    public void ClearPath(bool duringSelectingPoints = false)
    {
        start = null;
        target = null;
        for (int i = 0; i < pathmarkers.Count; i++)
        {
            Destroy(pathmarkers[i]);
        }
        pathmarkers.Clear();

        if (duringSelectingPoints)
        {
            if (selectingPointsCoroutine == null)
            {
                selectingPointsCoroutine = SelectingPointsCoroutine();
                StartCoroutine(selectingPointsCoroutine);
            }
        }
    }

    public void ShowRunner(UnityAction afterRunAction)
    {
        runner.Run(start.position, path, afterRunAction);
    }

    public void HideRunner()
    {
        runner.Hide();
    }
}
