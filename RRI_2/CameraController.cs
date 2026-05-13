// CameraController.cs
// Assets/Scripts/CameraController.cs
// Attach to: Main Camera

using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("References")]
    public Transform playerTransform;
    public GameObject weaponObject;

    [Header("Mouse Look")]
    public float sensitivityY = 2f;
    public float pitchMin = -60f;
    public float pitchMax = 60f;

    [Header("Eye Height (fraction of capsule height)")]
    [Tooltip("0.85 = 85% up the capsule. Stays correct when crouching.")]
    public float eyeHeightFraction = 0.85f;

    [Header("TPS")]
    public float tpsDistance = 3.5f;
    public float tpsShoulderOffset = 0.6f;
    public float tpsVerticalOffset = 1.2f;
    public float tpsSmoothTime = 0.1f;

    [Header("FPS Eye Heights")]
    public float standingEyeHeight = 1.4f;
    public float crouchEyeHeight = 0.3f;

    [Header("Mode")]
    public bool startInFPS = false;

    bool _isFPS;
    float _pitch;
    Vector3 _tpsVelocity;

    Transform _weaponOriginalParent;
    Vector3 _weaponOriginalLocalPos;
    Quaternion _weaponOriginalLocalRot;

    PlayerController _pc;
    Collider _weaponCollider;
    float _fpsEyeHeight;

    void Start()
    {
        _isFPS = startInFPS;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (playerTransform != null)
            _pc = playerTransform.GetComponent<PlayerController>();

        _fpsEyeHeight = standingEyeHeight;

        if (weaponObject != null)
        {
            _weaponCollider = weaponObject.GetComponent<Collider>();
            _weaponOriginalParent = weaponObject.transform.parent;
            _weaponOriginalLocalPos = weaponObject.transform.localPosition;
            _weaponOriginalLocalRot = weaponObject.transform.localRotation;
        }
    }

    void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.V))
            _isFPS = !_isFPS;

        float mouseY = Input.GetAxis("Mouse Y") * sensitivityY;
        _pitch = Mathf.Clamp(_pitch - mouseY, pitchMin, pitchMax);

        if (_isFPS) UpdateFPS();
        else UpdateTPS();
    }

    // Eye position tracks CharacterController height so it drops automatically when crouching.
    Vector3 GetEyePosition()
    {
        if (_pc == null)
            return playerTransform.position + Vector3.up * 1.6f;

        return playerTransform.position + Vector3.up * _pc.CurrentEyeHeight;
    }

    void UpdateFPS()
    {
        float targetEyeH = (_pc != null && _pc.IsCrouching) ? crouchEyeHeight : standingEyeHeight;
        _fpsEyeHeight = Mathf.Lerp(_fpsEyeHeight, targetEyeH, Time.deltaTime * 8f);

        Vector3 targetPos = playerTransform.position + Vector3.up * _fpsEyeHeight;
        transform.position = Vector3.Lerp(transform.position, targetPos, 20f * Time.deltaTime);
        transform.rotation = Quaternion.Euler(_pitch, playerTransform.eulerAngles.y, 0f);

        if (weaponObject != null)
        {
            weaponObject.transform.SetParent(transform, false);
            weaponObject.transform.localPosition = new Vector3(0.2f, -0.15f, 0.4f);
            weaponObject.transform.localRotation = Quaternion.identity;
            weaponObject.SetActive(true);
            
            if (_weaponCollider != null) _weaponCollider.enabled = false;
        }
    }

    void UpdateTPS()
    {
        if (weaponObject != null)
        {
            weaponObject.transform.SetParent(_weaponOriginalParent, false);
            weaponObject.transform.localPosition = _weaponOriginalLocalPos;
            weaponObject.transform.localRotation = _weaponOriginalLocalRot;
            weaponObject.SetActive(true);
            if (_weaponCollider != null) _weaponCollider.enabled = true;
        }

        // keeping the camera below low ceilings
        float vertOffset = _pc != null ? _pc.CurrentEyeHeight : tpsVerticalOffset;

        // shoulder position
        Quaternion yaw = Quaternion.Euler(0f, playerTransform.eulerAngles.y, 0f);
        Vector3 shoulder = playerTransform.position
                            + yaw * Vector3.right * tpsShoulderOffset
                            + Vector3.up * vertOffset;

        Quaternion camRot = Quaternion.Euler(_pitch, playerTransform.eulerAngles.y, 0f);
        Vector3 rawDesired = shoulder - camRot * Vector3.forward * tpsDistance;

        // pull camera in when wall sits between shoulder and desired position
        Vector3 desiredPos = rawDesired;
        if (Physics.Linecast(shoulder, rawDesired, out RaycastHit hit))
            desiredPos = hit.point + hit.normal * 0.15f;

        transform.position = Vector3.SmoothDamp(
            transform.position, desiredPos, ref _tpsVelocity, tpsSmoothTime);
        transform.LookAt(shoulder);
    }
}