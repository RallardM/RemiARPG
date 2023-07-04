using UnityEngine;

public class HitBox : MonoBehaviour
{
    LayerMask m_selfLayerMask;

    private void Awake()
    {
        m_selfLayerMask = gameObject.layer;
    }

    private void OnTriggerEnter(Collider other)
    {
        //if (other.gameObject.layer == m_selfLayerMask)
        //{
        //    return;
        //}

        Debug.Log("OnTriggerEnter name : " + other.name);
    }

    private void OnTriggerStay(Collider other)
    {
        //if (other.gameObject.layer == m_selfLayerMask)
        //{
        //    return;
        //}

        Debug.Log("OnTriggerStay name : " + other.name);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //if (collision.gameObject.layer == m_selfLayerMask)
        //{
        //    return;
        //}

        Debug.Log("OnCollisionEnter name : " + collision.gameObject.name);
    }
    
}
