using UnityEngine;

public class DebugAI : MonoBehaviour
{
    private Animator m_animator;

    private void Awake()
    {
        m_animator = GetComponent<Animator>();
    }


    public void OnEndOfDamageAnim()
    {
        m_animator.SetBool("IsReceivingDamage", false);
    }
}
