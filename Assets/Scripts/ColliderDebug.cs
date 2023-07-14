// Draws a line around the collider of the object no matter what is currently selected in the hierachy.

using UnityEngine;

public class ColliderDebug : MonoBehaviour
{
    private BoxCollider2D boxCollider2D;
    private LineRenderer lineRenderer;

    void Start()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.positionCount = 5;
        lineRenderer.loop = true;
        lineRenderer.useWorldSpace = false;
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.SetPosition(0, new Vector3(boxCollider2D.offset.x - boxCollider2D.size.x / 2, boxCollider2D.offset.y - boxCollider2D.size.y / 2, 0));
        lineRenderer.SetPosition(1, new Vector3(boxCollider2D.offset.x + boxCollider2D.size.x / 2, boxCollider2D.offset.y - boxCollider2D.size.y / 2, 0));
        lineRenderer.SetPosition(2, new Vector3(boxCollider2D.offset.x + boxCollider2D.size.x / 2, boxCollider2D.offset.y + boxCollider2D.size.y / 2, 0));
        lineRenderer.SetPosition(3, new Vector3(boxCollider2D.offset.x - boxCollider2D.size.x / 2, boxCollider2D.offset.y + boxCollider2D.size.y / 2, 0));
        lineRenderer.SetPosition(4, new Vector3(boxCollider2D.offset.x - boxCollider2D.size.x / 2, boxCollider2D.offset.y - boxCollider2D.size.y / 2, 0));
    }
}
