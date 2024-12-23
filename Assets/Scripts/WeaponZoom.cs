using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using StarterAssets;
using UnityEngine;

public class WeaponZoom : MonoBehaviour
{
    [SerializeField] private Transform followCamera;
    [SerializeField] private Camera weaponCamera;
    [SerializeField] private float defaultFov = 80f;
    [SerializeField] private float defaultWeaponFov = 40f;

    [SerializeField] private float zoomedFov = 40f;
    [SerializeField] private float weaponZoomedFov = 20f;

    private CinemachineVirtualCamera virtualCamera;

    

    private FirstPersonController fpsController;

    
    private float targetFov;
    private float weaponTargetFov;
    
    
    private float zoomSpeed = 10f;
    private float defaultRotationSpd;


    private void Start() {
        virtualCamera = followCamera.GetComponent<CinemachineVirtualCamera>();
        fpsController = transform.parent.parent.parent.GetComponent<FirstPersonController>();

        defaultFov = virtualCamera.m_Lens.FieldOfView;
        defaultRotationSpd = fpsController.RotationSpeed;
        targetFov = defaultFov;
        weaponTargetFov = defaultWeaponFov;
        
    }

    private void OnDisable() {
        targetFov = defaultFov;
        weaponTargetFov = defaultWeaponFov;
        fpsController.RotationSpeed = defaultRotationSpd;

        virtualCamera.m_Lens.FieldOfView = Mathf.Lerp(
            virtualCamera.m_Lens.FieldOfView, 
            targetFov, 
            Time.deltaTime * zoomSpeed
        );

        weaponCamera.fieldOfView = Mathf.Lerp(
            weaponCamera.fieldOfView, 
            weaponTargetFov, 
            Time.deltaTime * zoomSpeed
        );
    }

    private void Update() {
        if (Input.GetMouseButton(1)) {
            
            
                
                targetFov = zoomedFov;
                weaponTargetFov = weaponZoomedFov;
                fpsController.RotationSpeed = 1f;
            

        } else {
            
            targetFov = defaultFov;
            weaponTargetFov = defaultWeaponFov;
            fpsController.RotationSpeed = defaultRotationSpd;
        }

        virtualCamera.m_Lens.FieldOfView = Mathf.Lerp(
            virtualCamera.m_Lens.FieldOfView, 
            targetFov, 
            Time.deltaTime * zoomSpeed
        );

        weaponCamera.fieldOfView = Mathf.Lerp(
            weaponCamera.fieldOfView, 
            weaponTargetFov, 
            Time.deltaTime * zoomSpeed
        );

        
    }


}
