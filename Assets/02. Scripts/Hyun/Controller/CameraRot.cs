using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

namespace FPSProto.Controller
{
    public class CameraRot : MonoBehaviour
    {
        [SerializeField] private float mouseSpeed = 4f;
        
        public Transform player; // �÷��̾� Transform�� �ν����Ϳ��� �Ҵ�
                                 // -> �̰Ÿ� ���߿� ���� ��Ʈ��ũ���� ������� �÷��̾�� �ڵ����� �ʱ�ȭ�ϰ� �ϸ� �ɰ� ����
        private Vector3 offset; // �÷��̾�� ī�޶� ������ �Ÿ� �� ����
        private float mouseX = 0f;

        void Start()
        {
            // ���� �� �÷��̾�� ī�޶� ������ �ʱ� �Ÿ� �� ������ ���
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

            // ī�޶��� ��ġ�� �÷��̾��� ��ġ�� offset�� �̿��Ͽ� ������Ʈ
            transform.position = player.position + offset;
            transform.rotation = Quaternion.Euler(transform.eulerAngles.x, player.eulerAngles.y, 0);
        }
    }
}