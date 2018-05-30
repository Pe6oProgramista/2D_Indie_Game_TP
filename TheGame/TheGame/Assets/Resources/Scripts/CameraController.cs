using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    public Transform target;
    public float smoothSpeed = 0.05f;
    public SpriteRenderer background;

    private Vector2 halfExtents;
    private void Start()
    {
        float vertExtent = GetComponent<Camera>().orthographicSize;
        float horzExtent = vertExtent * Screen.width / Screen.height;

        float x = background.bounds.center.x + background.bounds.size.x / 2 - horzExtent;
        float y = background.bounds.center.y + background.bounds.size.y / 2 - vertExtent;
        halfExtents = new Vector2(x, y);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            GetComponent<ButtonOptions>().Back();
        }
    }

    void FixedUpdate()
    {
        Vector3 desiredPosition = new Vector3(target.position.x, target.position.y, transform.position.z);
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        for(int i = 0; i < 2; i++)
        {
            smoothedPosition[i] = Mathf.Clamp(smoothedPosition[i], -halfExtents[i], halfExtents[i]);
        }
        transform.position = smoothedPosition;
    }
}
