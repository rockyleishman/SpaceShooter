using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] public Transform FocusTarget;
    private Camera _mainCamera;

    [SerializeField] public ParallaxTexture[] ParallaxTextures;
    
    private Vector3 _currentVelocity; //used for Vector3.SmoothDamp

    private void Start()
    {
        //get camera
        _mainCamera = GetComponentInChildren<Camera>();

        //start at focus target (player)
        transform.position = FocusTarget.position;
    }

    private void Update()
    {
        FollowTarget();
        ScrollBackground();
    }

    private void FollowTarget()
    {
        transform.position = Vector3.SmoothDamp(transform.position, FocusTarget.position, ref _currentVelocity, Mathf.Pow(1.0f / Vector3.Distance(transform.position, FocusTarget.position), 2.0f), Mathf.Infinity, Time.deltaTime);
    }

    private void ScrollBackground()
    {
        if (ParallaxTextures.Length > 0)
        {
            Vector2 offset = new Vector2(transform.position.x, transform.position.y);

            foreach (ParallaxTexture texture in ParallaxTextures)
            {
                texture.SetOffset(offset);
            }
        }
    }
}
