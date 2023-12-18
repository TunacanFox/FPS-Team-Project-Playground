using UnityEngine;
using FPS.WeaponSO;
using FPS.WeaponHandler;
using FPS.Lee.WeaponDetail;

namespace FPS.WeaponHandler
{
    //�ѱ��� Vector���� Quaternion ������� �ѱ⸦ �������, ���� 1��Ī ȭ������ ����ִ� Ŭ����
    //�ѱ��� ���� ������ WeaponStat���� �̰�, �� Ŭ������ �ҷ��´�
    public class WeaponFactory : MonoBehaviour
    {
        //Instantiate�� ���� ����
        //Canvas�� �ڽ��� Image�� �ڽ����� �����ϱ� ���� ����2���� ��.
        public GameObject foundationPrefab; //�θ�1
        public Transform foundationTransform; //�ڽ�1 �ٸ��� �߰����� �ʴ� �̻� �� �༮�� �ڽ����� Instantiate�� ���� ���̴�.
        
        //Instantiate�� ���� Vector, Rotation
        public Vector3 weaponVector; //���� �������� ��ġ
        public Quaternion weaponRotation; //���� �������� ����
        public Transform trackingPositionSettingPrefab; //���� ������ ��Ȯ�� Find�ؼ� Vector, Rotation ������ �� �ְ� ����.

        //Instantiate�� ���� ������
        public GameObject weaponPrefab; //������ ������


        private void Start()
        {
            //foundationPrefab = GameObject.Find("Player"); //���� ���� �θ� 
            //������ ���̾��Ű â�� �� ������Ʈ�� ����ִ� �ִ°� WeaponFactory��, ������ �̰Ŵ� �ʱ�ȭ�� Start�� �ϴµ�, �÷��̾�� ���߿� ���´�
            //�׷��� ���� �ȵ��� �ٲ��ֱⰡ ���� ����. -> ���� �÷��̾� �ȿ� �׳� �־��� �׷��� �÷��̾��� �ڽ����� ����.
            //�θ� ã�� �ڵ�� �ƴϴ� �۵� ���� �׷��� ������

            //foundationTransform = foundationPrefab.transform.Find("Main Camera"); //���ڱ� ����
            //trackingPositionSettingPrefab = foundationTransform.transform.Find("PositionSettingPrefab"); //�굵 �������� ���� �ִ°ɷ� ����

        }

        private void Update()
        {
            /*
            if (Input.GetKeyUp(KeyCode.Space))
            {
                
                CreateWeapon("(SG) Bennelli_M4"); //�׽�Ʈ ���̴� �׳� �ּ� ó��
            }
             */
    }

        public GameObject CreateWeapon(string weaponName)
        {
            //Instantiate�� ���� Vector, Rotation �ʱ�ȭ. �� �༮�� �÷��̾��� ��ġ�� ���� ��� ���ϴ� ȣ��Ǹ� �ʱ�ȭ �ؾ��Ѵ�.
            //float distanceFromCamera = 1.0f; // �� ���� ���ӿ� ���� �����ؾ� �մϴ�. �ӽ÷� �� �־��µ�, ��Ȯ�� ���� �𸣰ڴ�.
            weaponVector = new Vector3(trackingPositionSettingPrefab.transform.position.x, trackingPositionSettingPrefab.transform.position.y, trackingPositionSettingPrefab.transform.position.z);
            weaponRotation = trackingPositionSettingPrefab.transform.rotation;

            WeaponDataSO weaponData = WeaponSettingAssets.instance[weaponName];
            //SO�� �������� �ϴµ�, ��� SO �ϳ��ϳ� �� �ϱ�� �׷��ϱ� ��ųʸ��� �� ������ �������°ǰ�?


            //�׽�Ʈ
            Debug.Log("CreateWeapon weaponName: " + weaponName); //�̰� ������ ���� �ȳ�
                                                                 //�׽�Ʈ��Ʈ
            Debug.Log("WeaponData.magazineSize: " + weaponData.magazineSize); //�� ������
            Debug.Log("WeaponData.ammo: " + weaponData.ammo); //�� ������
            Debug.Log("WeaponUISetup ����Ǳ� ����");

            

            if (weaponData != null)
            {
                this.weaponPrefab = weaponData.weaponPrefab; //�� �� Ȯ��

                GameObject weaponInstance = Instantiate(weaponPrefab, weaponVector, weaponRotation, foundationTransform);
                //WeaponStat weaponStat = weaponInstance.GetComponent<WeaponStat>();
                WeaponStat weaponStat = weaponInstance.AddComponent<WeaponStat>();
                Weapon forWeaponScriptAdd = weaponInstance.AddComponent<Weapon>(); //Weapon ��ũ��Ʈ�� �ٿ��ֱ� ���� �����ϴ� �༮

                forWeaponScriptAdd.WeaponUISetup(weaponData); //�׽�Ʈ ���� UI�� TextMeshPro�� �ٲ� �� �ֳ� �׽�Ʈ 
                                                       //ȣ�� ���� �ȵȴ�.



                //���� ���� weaponInstance�� weaponStat ��ũ��Ʈ ���̱�
                if (weaponStat != null)
                {
                    Debug.Log("weaponStat.Setup(weaponData) ����");
                    weaponStat.Setup(weaponData); //�� �Լ��� SO�̿��� ������ �ڵ����� �ʱ�ȭ���ش�.
                                                  //weaponStat.TestPrint(); //�׽�Ʈ �� �Լ�


                }
                else
                {
                    Debug.LogError("WeaponStat ������Ʈ�� ã�� �� �����ϴ�.");
                }
                return weaponInstance;
            }
            else
            {
                Debug.LogError("Weapon data not found for: " + weaponName);
                return null;
            }
        }
    }
}