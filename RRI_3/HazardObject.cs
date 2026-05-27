using System.Collections;
using UnityEngine;


public class HazardObject : MonoBehaviour
{
    [Header("Settings")]
    public float slowMultiplier = 0.4f;
    public float slowDuration   = 2f;
    public Color normalColor    = new Color(0.8f, 0.2f, 0.2f);
    public Color hitColor       = Color.yellow;

    private Renderer rend;
    [HideInInspector] public bool isActive = true;

    void Awake()
    {
        rend = GetComponent<Renderer>();
        if (rend != null) rend.material.color = normalColor;
    }

    public void TriggerSlow(PlayerController pc)
    {
        if (!isActive) return;
        StartCoroutine(ApplySlow(pc));
    }

    IEnumerator ApplySlow(PlayerController pc)
    {
        isActive = false;
        pc.speedMultiplier = slowMultiplier;
        if (rend != null) rend.material.color = hitColor;

        Debug.Log("[Hazard] Igrac usporen na " + (slowMultiplier * 100f) + "% brzine");

        yield return new WaitForSeconds(slowDuration);

        pc.speedMultiplier = 1f;
        if (rend != null) rend.material.color = normalColor;
        isActive = true;

        Debug.Log("[Hazard] Brzina vracena na normalu");
    }
}
