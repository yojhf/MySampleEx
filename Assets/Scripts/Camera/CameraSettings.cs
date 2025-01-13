using Unity.Cinemachine;
using UnityEngine;

namespace MySampleEx
{
    public class CameraSettings : MonoBehaviour
    {
        public CinemachineCamera freeLookCam;
        public Transform follow;
        public Transform lookAt;


        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            UpdateCameraSettings();
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        void UpdateCameraSettings()
        {
            freeLookCam.Follow = follow;
            freeLookCam.LookAt = lookAt;
        }
    }
}