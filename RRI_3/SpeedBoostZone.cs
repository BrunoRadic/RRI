using System.Collections;
using UnityEngine;

public class SpeedBoostZone : MonoBehaviour
{
    [Header("Settings")]
    public float boostMultiplier = 2f;
    public float boostDuration   = 5f;
    public Color zoneColor       = new Color(0f, 1f, 0.5f, 0.3f);

    private Coroutine activeCoroutine;

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

        if (activeCoroutine != null) StopCoroutine(activeCoroutine);
        activeCoroutine = StartCoroutine(BoostRoutine(pc));
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        PlayerController pc = other.GetComponent<PlayerController>();
        if (pc == null) return;

        if (activeCoroutine != null) StopCoroutine(activeCoroutine);
        activeCoroutine = null;
        pc.speedMultiplier = 1f;
        Debug.Log("[SpeedZone] Igrac izasao — boost uklonjen");
    }

    IEnumerator BoostRoutine(PlayerController pc)
    {
        pc.speedMultiplier = boostMultiplier;
        Debug.Log("[SpeedZone] Boost aktiviran: " + boostMultiplier + "x brzina");

        yield return new WaitForSeconds(boostDuration);

        pc.speedMultiplier = 1f;
        Debug.Log("[SpeedZone] Boost istekao");
    }
}
