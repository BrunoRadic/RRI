using UnityEngine;

public class GravityZone : MonoBehaviour
{
    [Header("Settings")]
    public float gravityMultiplier = 0.2f;
    public Color zoneColor = new Color(0.5f, 0f, 1f, 0.3f);

    void Start()
    {
        Renderer rend = GetComponent<Renderer>();
        if (rend != null) rend.material.color = zoneColor;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        PlayerController pc = other.GetComponent<PlayerController>();
        if (pc == null) return;

        pc.gravityMultiplier = gravityMultiplier;
        Debug.Log("[GravityZone] Smanjena gravitacija aktivna");
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        PlayerController pc = other.GetComponent<PlayerController>();
        if (pc == null) return;

        pc.gravityMultiplier = 1f;
        Debug.Log("[GravityZone] Gravitacija vracena na normalu");
    }
}
