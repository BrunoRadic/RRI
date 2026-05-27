using UnityEngine;

public class PlayerAnimatorController : MonoBehaviour
{
    private Animator animator;
    public float rotationSpeed = 10f;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        float speed = new Vector2(h, v).magnitude;

        animator.SetFloat("Speed", speed);

        if (speed > 0.1f)
        {
            Vector3 direction = new Vector3(h, 0f, v);
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            animator.ResetTrigger("Attack");
            animator.SetTrigger("Jump");
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            animator.ResetTrigger("Jump");
            animator.SetTrigger("Attack");
        }
    }
}