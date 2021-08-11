using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipInteriorManager : MonoBehaviour {

    public GameObject _canvasTarget;
    public Vector3 _canvasCameraOffset;
    public GameObject _playerTarget;
    public Vector3 _playerCameraOffset;

    public Camera _mainCamera;
    public SmoothFollow _smoothFollow;

    float defaultCameraSize;
    public float zoomedCameraSize;
    public float zoomTransitionTime;

    public enum CameraTarget {
        Player,
        LevelSelect
    }

    public CameraTarget currentCameraTarget;

    public void Start()
    {
        defaultCameraSize = _mainCamera.orthographicSize;
        currentCameraTarget = CameraTarget.Player;
    }

    public void ZoomCameraToLevelSelect()
    {
        if (currentCameraTarget != CameraTarget.LevelSelect) {
            _smoothFollow.target = _canvasTarget.transform;
            DOTween.To(() => _mainCamera.orthographicSize, x => _mainCamera.orthographicSize = x, zoomedCameraSize, zoomTransitionTime);
            DOTween.To(() => _smoothFollow.cameraOffset, x => _smoothFollow.cameraOffset = x, _canvasCameraOffset, zoomTransitionTime);
            //_mainCamera.orthographicSize = zoomedCameraSize;
            currentCameraTarget = CameraTarget.LevelSelect;
        }
    }

    public void ZoomCameraToPlayer()
    {
        if (currentCameraTarget != CameraTarget.Player){
            _smoothFollow.target = _playerTarget.transform;
            DOTween.To(() => _mainCamera.orthographicSize, x => _mainCamera.orthographicSize = x, defaultCameraSize, zoomTransitionTime);
            DOTween.To(() => _smoothFollow.cameraOffset, x => _smoothFollow.cameraOffset = x, _playerCameraOffset, zoomTransitionTime);
            //_mainCamera.orthographicSize = defaultCameraSize;
            currentCameraTarget = CameraTarget.Player;
        }
    }

}
