using System.Collections;
using UnityEngine;

public class InteractableGate : MonoBehaviour
{
    [Header("Settings")]
    public float interactRange  = 3f;
    public float openHeight     = 3f;
    public float animDuration   = 1.2f;
    public float stayOpenTime   = 3f;

    [Header("UI Hint")]
    public GameObject interactHint;

    private Vector3 closedPos;
    private Vector3 openPos;
    private bool isOpen   = false;
    private bool isMoving = false;
    private Transform player;

    void Start()
    {
        closedPos = transform.position;
        openPos   = closedPos + Vector3.up * openHeight;

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null) player = playerObj.transform;

        if (interactHint != null) interactHint.SetActive(false);
    }

    void Update()
    {
        if (player == null) return;

        float dist = Vector3.Distance(transform.position, player.position);
        bool inRange = dist <= interactRange;

        if (interactHint != null)
            interactHint.SetActive(inRange && !isOpen && !isMoving);

        if (inRange && !isMoving && !isOpen && Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(OpenAndClose());
        }
    }

    IEnumerator OpenAndClose()
    {
        isMoving = true;
        if (interactHint != null) interactHint.SetActive(false);

        yield return StartCoroutine(MoveTo(openPos));
        isOpen   = true;
        isMoving = false;
        Debug.Log("[Gate] Vrata otvorena");

        yield return new WaitForSeconds(stayOpenTime);

        isMoving = true;
        isOpen   = false;
        yield return StartCoroutine(MoveTo(closedPos));
        isMoving = false;
        Debug.Log("[Gate] Vrata zatvorena");
    }

    IEnumerator MoveTo(Vector3 target)
    {
        Vector3 start   = transform.position;
        float elapsed   = 0f;

        while (elapsed < animDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.SmoothStep(0f, 1f, elapsed / animDuration);
            transform.position = Vector3.Lerp(start, target, t);
            yield return null;
        }

        transform.position = target;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, interactRange);
    }
}
