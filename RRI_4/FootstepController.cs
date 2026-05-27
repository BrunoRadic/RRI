using UnityEngine;

public class FootstepController : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip footstepSound;
    public AudioClip jumpSound;
    public AudioClip landSound;
    public AudioClip attackSound;

    public void PlayFootstep()
    {
        if (footstepSound != null)
            audioSource.PlayOneShot(footstepSound);
    }

    public void PlayJump()
    {
        if (jumpSound != null)
            audioSource.PlayOneShot(jumpSound);
    }

    public void PlayLand()
    {
        if (landSound != null)
            audioSource.PlayOneShot(landSound);
    }

    public void PlayAttack()
    {
        if (attackSound != null)
            audioSource.PlayOneShot(attackSound);
    }
}