using UnityEngine;
using UnityEngine.EventSystems;

public class CameraMovement : MonoBehaviour
{
    [Header("Panning")]
    [SerializeField] private float panSpeed = 0.5f; // kecepatan geser kamera
    [SerializeField] private Vector2 limitX = new Vector2(-10f, 10f);
    [SerializeField] private Vector2 limitZ = new Vector2(-10f, 10f);

    [Header("Zooming (Perspective)")]
    [SerializeField] private float zoomSpeed = 0.2f;  // kecepatan zoom
    [SerializeField] private float minZoom = 25f;     // field of view terdekat
    [SerializeField] private float maxZoom = 60f;     // field of view terjauh

    private Vector3 lastPanPosition;
    private int panFingerId;
    private bool isPanning;

    private Camera cam;

    private void Awake()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        // Cegah kamera gerak kalau sentuhan di atas UI
        if (IsPointerOverUI()) return;

#if UNITY_EDITOR || UNITY_STANDALONE
        HandleMouse();
#else
        HandleTouch();
#endif
    }

    private bool IsPointerOverUI()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        return EventSystem.current != null && EventSystem.current.IsPointerOverGameObject();
#else
        if (Input.touchCount > 0 && EventSystem.current != null)
        {
            return EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId);
        }
        return false;
#endif
    }

    private void HandleMouse()
    {
        if (Input.GetMouseButtonDown(0))
        {
            lastPanPosition = Input.mousePosition;
            isPanning = true;
        }
        else if (Input.GetMouseButton(0) && isPanning)
        {
            PanCamera(Input.mousePosition);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isPanning = false;
        }

        // Zoom pakai scroll wheel (Editor saja)
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scroll) > 0.01f)
        {
            ZoomCamera(scroll * 1000f);
        }
    }

    private void HandleTouch()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                lastPanPosition = touch.position;
                panFingerId = touch.fingerId;
                isPanning = true;
            }
            else if (touch.fingerId == panFingerId && touch.phase == TouchPhase.Moved)
            {
                PanCamera(touch.position);
            }
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                isPanning = false;
            }
        }
        else if (Input.touchCount == 2)
        {
            // Pinch to zoom
            Touch t0 = Input.GetTouch(0);
            Touch t1 = Input.GetTouch(1);

            Vector2 prevPos0 = t0.position - t0.deltaPosition;
            Vector2 prevPos1 = t1.position - t1.deltaPosition;

            float prevMagnitude = (prevPos0 - prevPos1).magnitude;
            float currentMagnitude = (t0.position - t1.position).magnitude;

            float difference = currentMagnitude - prevMagnitude;

            ZoomCamera(difference);
        }
    }

    private void PanCamera(Vector3 newPanPosition)
    {
        Vector3 offset = cam.ScreenToViewportPoint(lastPanPosition - newPanPosition);

        Vector3 move = new Vector3(offset.x * panSpeed, 0, offset.y * panSpeed);

        transform.Translate(move, Space.World);

        // Clamp posisi
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, limitX.x, limitX.y);
        pos.z = Mathf.Clamp(pos.z, limitZ.x, limitZ.y);
        transform.position = pos;

        lastPanPosition = newPanPosition;
    }

    private void ZoomCamera(float deltaMagnitudeDiff)
    {
        cam.fieldOfView -= deltaMagnitudeDiff * zoomSpeed * Time.deltaTime;
        cam.fieldOfView = Mathf.Clamp(cam.fieldOfView, minZoom, maxZoom);
    }
}