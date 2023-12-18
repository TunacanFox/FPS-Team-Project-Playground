using UnityEngine;
using UnityEngine.UI;

namespace FPSProto.Controller
{
    public class Aim : MonoBehaviour
    {
        public Image[] CrossHairs;        
        Camera cam;
        int enemy;

        private Quaternion originalGunRotation;

        void Start()
        {
            cam = Camera.main;
            enemy = LayerMask.GetMask("Enemy");            

            for (int i = 0; i < CrossHairs.Length; i++)
            {
                if (CrossHairs[i] != null)
                {
                    CrossHairs[i].color = Color.white;
                }
            }
        }

        void Update()
        {            
            DetectCollider();                        
        }

        public void DetectCollider()
        {
            Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2);
            Ray ray = cam.ScreenPointToRay(screenCenter);
            RaycastHit hit;

            for (int i = 0; i < CrossHairs.Length; i++)
            {
                if (CrossHairs[i] == null)
                {
                    return;
                }
            }

            if (Physics.Raycast(ray, out hit, float.PositiveInfinity, enemy))
            {
                Debug.DrawLine(ray.origin, hit.point, Color.red);
                Debug.Log($"{hit.collider.name}를 감지했습니다 !");
                for (int i = 0; i < CrossHairs.Length; i++)
                {
                    CrossHairs[i].color = Color.red;
                }
            }
            else
            {
                Debug.DrawLine(ray.origin, ray.origin + ray.direction * 100f, Color.blue);
                for (int i = 0; i < CrossHairs.Length; i++)
                {
                    CrossHairs[i].color = Color.white;
                }
            }
        }
                

    }
}
