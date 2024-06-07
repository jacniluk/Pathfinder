using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RunnerController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotateTime;

    private IEnumerator runCoroutine;

    public void Run(Vector3 start, List<Vector3> _path, UnityAction afterRunAction)
    {
        List<Vector3> path = new List<Vector3>(_path);

        transform.position = start;
        transform.LookAt(path[0]);
        gameObject.SetActive(true);

        runCoroutine = RunCoroutine(path, afterRunAction);
        StartCoroutine(runCoroutine);
    }

    private IEnumerator RunCoroutine(List<Vector3> path, UnityAction afterRunAction)
    {
        Vector3 distance;
        Vector3 offset;
        Quaternion startRotation;
        Quaternion targetRotation;
        while (path.Count > 0)
        {
            distance = path[0] - transform.position;
            startRotation = transform.rotation;
            targetRotation = Quaternion.Euler(0, Mathf.Atan2(distance.x, distance.z) * Mathf.Rad2Deg, 0);
            float startTime = Time.realtimeSinceStartup;
            float endTime = startTime + rotateTime;

            while (transform.position != path[0])
            {
                distance = path[0] - transform.position;
                offset = moveSpeed * Time.deltaTime * distance.normalized;
                if (offset.magnitude > distance.magnitude)
                {
                    offset = distance;
                }
                transform.position += offset;

                transform.rotation = Quaternion.Lerp(startRotation, targetRotation, Utilities.CalculateProgress01(Time.realtimeSinceStartup, startTime, endTime));

                yield return null;
            }

            path.RemoveAt(0);
        }

        afterRunAction.Invoke();

        runCoroutine = null;
        gameObject.SetActive(false);
    }

    public void Hide()
    {
        if (runCoroutine != null)
        {
            StopCoroutine(runCoroutine);
            runCoroutine = null;
            gameObject.SetActive(false);
        }
    }
}
