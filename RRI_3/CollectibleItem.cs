using UnityEngine;

public class CollectibleItem : MonoBehaviour
{
    [Header("Settings")]
    public int points = 10;
    public float rotateSpeed = 90f;
    public float bobAmplitude = 0.3f;
    public float bobFrequency = 1.5f;

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime);
        float newY = startPos.y + Mathf.Sin(Time.time * bobFrequency) * bobAmplitude;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (ScoreManager.Instance != null)
            ScoreManager.Instance.AddPoints(points);

        Debug.Log("[Collectible] Skupljeno! +" + points + " bodova");
        Destroy(gameObject);
    }
}
