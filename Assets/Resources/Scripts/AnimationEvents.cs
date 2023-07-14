using UnityEngine;

public class AnimationEvents : MonoBehaviour
{
    private Animator m_animator;

    private void Awake()
    {
        m_animator = GetComponent<Animator>();
    }

    public void OnAttackEnd()
    {
        m_animator.SetBool("IsAttacking", false);
    }

    public void OnEndOfDamageAnim()
    {
        m_animator.SetBool("IsReceivingDamage", false);
    }

    public void OnEndOfDeathAnim()
    {
        Destroy(transform.parent.gameObject);
    }
}
