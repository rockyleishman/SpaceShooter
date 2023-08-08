using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private LevelData LevelDataObject;
    
    private Camera _mainCamera;
    [Header("Overlay Cameras")]
    [SerializeField] public Camera Camera_N;
    [SerializeField] public Camera Camera_NE;
    [SerializeField] public Camera Camera_E;
    [SerializeField] public Camera Camera_SE;
    [SerializeField] public Camera Camera_S;
    [SerializeField] public Camera Camera_SW;
    [SerializeField] public Camera Camera_W;
    [SerializeField] public Camera Camera_NW;

    [Header("Focus Target")]
    [SerializeField] public Transform FocusTarget;

    [Header("Background")]
    [SerializeField] public ParallaxTexture[] ParallaxTextures;
    
    private Vector3 _currentVelocity; //used for Vector3.SmoothDamp

    private void Start()
    {
        //get data objects
        LevelDataObject = DataManager.Instance.LevelDataObject;

        //get main camera
        _mainCamera = Camera.main;

        //position overlay cameras
        Camera_N.transform.localPosition = new Vector3(0.0f, LevelDataObject.LevelSize.y, _mainCamera.transform.position.z);
        Camera_NE.transform.localPosition = new Vector3(LevelDataObject.LevelSize.x, LevelDataObject.LevelSize.y, _mainCamera.transform.position.z);
        Camera_E.transform.localPosition = new Vector3(LevelDataObject.LevelSize.x, 0.0f, _mainCamera.transform.position.z);
        Camera_SE.transform.localPosition = new Vector3(LevelDataObject.LevelSize.x, -LevelDataObject.LevelSize.y, _mainCamera.transform.position.z);
        Camera_S.transform.localPosition = new Vector3(0.0f, -LevelDataObject.LevelSize.y, _mainCamera.transform.position.z);
        Camera_SW.transform.localPosition = new Vector3(-LevelDataObject.LevelSize.x, -LevelDataObject.LevelSize.y, _mainCamera.transform.position.z);
        Camera_W.transform.localPosition = new Vector3(-LevelDataObject.LevelSize.x, 0.0f, _mainCamera.transform.position.z);
        Camera_NW.transform.localPosition = new Vector3(-LevelDataObject.LevelSize.x, LevelDataObject.LevelSize.y, _mainCamera.transform.position.z);

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
