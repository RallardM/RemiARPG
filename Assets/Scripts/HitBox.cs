using UnityEngine;

public class HitBox : MonoBehaviour
{
    LayerMask m_selfLayerMask;
    Transform m_characterTransform;
    Rigidbody2D m_hitBoxRigidbody;

    private void Awake()
    {
        m_selfLayerMask = gameObject.layer;
        m_characterTransform = transform.parent;
        m_hitBoxRigidbody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        float horizontal = m_characterTransform.localPosition.x;
        float vertical = m_characterTransform.localPosition.y;
        Vector2 movement = new(horizontal, vertical);
        m_hitBoxRigidbody.MovePosition(movement);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter name : " + other.name);
    }

    private void OnTriggerStay(Collider other)
    {
        Debug.Log("OnTriggerStay name : " + other.name);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("OnCollisionEnter name : " + collision.gameObject.name);
    }
}
