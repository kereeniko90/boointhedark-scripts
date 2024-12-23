using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class Waypoints : MonoBehaviour
{   
    [SerializeField] private RectTransform prefab;
    //[SerializeField] private float edgeBuffer = 20f;

    private RectTransform waypoint;

    private Transform player;
    private TextMeshProUGUI distanceText;
    private Camera mainCamera;
    private Vector3 targetPosition;

    private Canvas canvas;

    
    
    void Start()
    {
        var canvasObj = GameObject.Find("Waypoints");

        canvas = canvasObj.GetComponent<Canvas>();

        waypoint = Instantiate(prefab, canvasObj.transform);

        distanceText = waypoint.GetComponentInChildren<TextMeshProUGUI>();

        player = GameObject.Find("pfPlayer").transform;

        targetPosition = transform.position;

        mainCamera = Camera.main;
    }

    
    void Update()
    {
        Vector3 worldPos = transform.position;
        float distance = Vector3.Distance(player.position, worldPos);


        if (distance < 40f) {
            waypoint.gameObject.SetActive(false);
            return;
        }

        waypoint.gameObject.SetActive(true);
        
        
        Vector3 viewportPoint = mainCamera.WorldToViewportPoint(worldPos);
        
        
        if (canvas.renderMode == RenderMode.ScreenSpaceOverlay)
        {
            Vector2 screenPoint = new Vector2(
                viewportPoint.x * Screen.width,
                viewportPoint.y * Screen.height
            );
            
            waypoint.position = screenPoint;
        }
        else
        {
            
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvas.GetComponent<RectTransform>(),
                new Vector2(viewportPoint.x * Screen.width, viewportPoint.y * Screen.height),
                canvas.renderMode == RenderMode.ScreenSpaceCamera ? mainCamera : null,
                out Vector2 localPoint
            );
            
            waypoint.localPosition = localPoint;
        }
        
        
        distanceText.text = distance.ToString("0") + "m";
        
        
        bool isInFrontOfCamera = viewportPoint.z > 0;
        bool isInViewport = viewportPoint.x >= 0 && viewportPoint.x <= 1 &&
                           viewportPoint.y >= 0 && viewportPoint.y <= 1;
                           
        waypoint.gameObject.SetActive(isInFrontOfCamera);
        
        
    }
    
}
