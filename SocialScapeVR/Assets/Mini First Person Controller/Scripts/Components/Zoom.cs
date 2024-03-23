using UnityEngine;

[ExecuteInEditMode]
public class Zoom : MonoBehaviour
{
  Camera camera;
  public float defaultFOV = 60;
  public float maxZoomFOV = 15;
  public float zoomSpeed = 5f; // Adjust zoom speed as needed

  bool isZooming = false;

  void Awake()
  {
    camera = GetComponent<Camera>();
    if (camera)
    {
      defaultFOV = camera.fieldOfView;
    }
  }

  void Update()
  {
    isZooming = Input.GetMouseButton(1); // Check if right mouse button is held

    if (isZooming)
    {
      float zoomAmount = Input.GetAxis("Mouse Y") * zoomSpeed; // Zoom based on mouse Y movement

      // Clamp zoomAmount to avoid going beyond min/max FOV
      zoomAmount = Mathf.Clamp(zoomAmount, -Mathf.Abs(maxZoomFOV - defaultFOV), Mathf.Abs(defaultFOV - maxZoomFOV));

      camera.fieldOfView = Mathf.Clamp(camera.fieldOfView + zoomAmount, maxZoomFOV, defaultFOV);
    }
  }
}
