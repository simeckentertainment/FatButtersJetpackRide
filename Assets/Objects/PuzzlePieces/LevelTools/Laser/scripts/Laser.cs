using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;
using System.Collections.Generic;

public class Laser : MonoBehaviour
{
    [SerializeField] private bool enableLaser;
    [SerializeField] private List<LaserPoint> laserPoints = new List<LaserPoint>();
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private UnityEvent onPlayerHit;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private bool debugHitGizmos;
    [SerializeField] private bool connectStartToEndOnly = true; // draw only one beam from first to last
    private void Awake()
    {
        if (lineRenderer == null)
        {
            lineRenderer = GetComponent<LineRenderer>();
        }

        GetPoints();
    }

    private void Update()
    {
        if (!enableLaser)
        {
            if (lineRenderer != null)
            {
                lineRenderer.enabled = false; // hide laser when disabled
            }
            return;
        }

        VisualLaser();
        CheckPlayerHit();
    }

    [Button("Visualize Laser")]
    public void VisualLaser()
    {
        if (lineRenderer == null)
        {
            Debug.LogWarning("LineRenderer missing on Laser", gameObject);
            return;
        }

        if (laserPoints == null || laserPoints.Count < 2)
        {
            Debug.LogWarning("Laser needs at least two points to visualize", gameObject);
            lineRenderer.enabled = false;
            return;
        }

        lineRenderer.enabled = true;
        lineRenderer.useWorldSpace = true;

        if (connectStartToEndOnly)
        {
            // Single straight beam from first point to last point
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, laserPoints[0].transform.position);
            lineRenderer.SetPosition(1, laserPoints[laserPoints.Count - 1].transform.position);
        }
        else
        {
            // Multi-point path following all points
            lineRenderer.positionCount = laserPoints.Count;
            for (int i = 0; i < laserPoints.Count; i++)
            {
                if (laserPoints[i] != null)
                {
                    lineRenderer.SetPosition(i, laserPoints[i].transform.position);
                }
            }
        }
    }

    private void CheckPlayerHit()
    {
        if (laserPoints == null || laserPoints.Count < 2)
        {
            return;
        }

        for (int i = 0; i < laserPoints.Count - 1; i++)
        {
            if (laserPoints[i] == null || laserPoints[i + 1] == null)
            {
                continue;
            }

            Vector3 start = laserPoints[i].transform.position;
            Vector3 end = laserPoints[i + 1].transform.position;
            Vector3 dir = end - start;
            float distance = dir.magnitude;
            if (distance <= Mathf.Epsilon)
            {
                continue;
            }

            if (Physics.Raycast(start, dir.normalized, out RaycastHit hit, distance, playerLayer, QueryTriggerInteraction.Ignore))
            {
                onPlayerHit?.Invoke();

                if (debugHitGizmos)
                {
                    Debug.DrawLine(start, hit.point, Color.red, 0.05f);
                }
                return; // already hit player this frame
            }
        }
    }

    [Button("Get Points")]
    public void GetPoints()
    {
        if (laserPoints == null)
        {
            laserPoints = new List<LaserPoint>();
        }

        laserPoints.Clear();
        foreach (Transform child in transform)
        {
            LaserPoint lp = child.GetComponent<LaserPoint>();
            if (lp != null)
            {
                laserPoints.Add(lp);
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (laserPoints == null || laserPoints.Count < 2)
        {
            return;
        }

        Gizmos.color = Color.blue;

        for (int i = 0; i < laserPoints.Count - 1; i++)
        {
            if (laserPoints[i] != null && laserPoints[i + 1] != null)
            {
                Gizmos.DrawLine(laserPoints[i].transform.position, laserPoints[i + 1].transform.position);
            }
        }
    }

    public void Hit()
    {
       print("Laser Hit");  
    }
}
