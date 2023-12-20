using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

namespace FPSProto.Controller
{
    public class CameraRot : MonoBehaviour
    {
        [SerializeField] private float mouseSpeed = 4f;
        
        public Transform player; // 플레이어 Transform을 인스펙터에서 할당
                                 // -> 이거를 나중에 포톤 네트워크에서 만들어준 플레이어로 자동으로 초기화하게 하면 될거 같다
        private Vector3 offset; // 플레이어와 카메라 사이의 거리 및 방향
        private float mouseX = 0f;

        void Start()
        {
            // 시작 시 플레이어와 카메라 사이의 초기 거리 및 방향을 계산
            offset = transform.position - player.position;
        }

        void Update()
        {
            mouseX += Input.GetAxis("Mouse Y") * mouseSpeed;

            mouseX = Mathf.Clamp(mouseX, -50f, 30f);

            //transform.rotation = Quaternion.Euler(new Vector3(-mouseY, 0, 0));
            transform.localRotation = Quaternion.Euler(new Vector3(-mouseX, 0, 0));
            //transform.localRotation = Quaternion.Euler(new Vector3(-mouseX, transform.localEulerAngles.y, 0));
        }

        void LateUpdate()
        {
            //Vector3 playerRotation = player.eulerAngles;
            //playerRotation.x = 0;
            //transform.rotation = Quaternion.Euler(playerRotation);

            // 카메라의 위치를 플레이어의 위치와 offset을 이용하여 업데이트
            transform.position = player.position + offset;
            transform.rotation = Quaternion.Euler(transform.eulerAngles.x, player.eulerAngles.y, 0);
        }
    }
}