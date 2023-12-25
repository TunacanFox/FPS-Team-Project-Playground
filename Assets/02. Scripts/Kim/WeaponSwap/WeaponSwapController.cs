using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FPS.WeaponHandler;
using FPS.Lee.WeaponDetail; //using ���ӽ����̽� WeaponFactory�� ����ϱ� ����, WeaponŬ������ ����ϱ� ����
using FPS.WeaponSO;

namespace FPS.WeaponSwap
{
    public class WeaponSwapController : MonoBehaviour
    {
        [SerializeField] private List<string> weaponNameList = new List<string>(); //�ѱ� �̸��� ���� �� -> ���� �������� �����ϴ� ���̴�.
        
        [SerializeField] private Weapon[] weaponSlotArray = new Weapon[7]; //�ѱ⸦ ���� 0�� �ֹ��� 1�� �������� ���. 1��~6��. ���⿡ ���� WeaponŬ������ ����� ���� ��


        [SerializeField] private Weapon previousWeapon; // ������ Ȱ��ȭ�� �ѱ� -> SetActive(false)�� ���� weaponSlotArray�� ���� �������� Ȱ��ȴ�.
        public WeaponSlot currentWeaponSlot; //���� ��ü ���� switch�� �� SetActive�� false�� ���� �ٷ� �ѱ� ��� true�� ���� �κ�

        public GameObject weaponPoint; //�ش� ������Ʈ�� Vector3�� ���� �޾Ƽ� ���ο� �ѱ⸦ �����.
        public Vector3 currentVector;
        public Quaternion currentQuaternion;

        //DroppedWeapon�� ���� ���� ��ü�� ����.
        public Weapon droppedWeapon;
        public WeaponPickup weaponPickup;
        public WeaponSlot droppedWeapon_weaponSlot;
        public LayerMask dropedWeaponLayer;

        //public WeaponFactory weaponFactory; //�ѱ� ������ ���� Ŭ������ ���� �����ϰ� �������.
                                            //Start�κп� FindObjectType���� �ʱ�ȭ�ϰ� ����. ���� �۵� ���� ��� ������ �󿡼� �����ϱ�.
        
        
        private void Start()
        {
            //�⺻ �ѱ� �κ��丮 ����. ���� ���� ������Ʈ�� ��ȣ�ۿ��ϸ� �ش��ϴ� ���Կ� ���⸦ �����־ ���� �����ϰ� �ϸ� �ȴ�.
            weaponNameList.Clear(); //�����ߴµ� ���� �̻��� ������ ���� ���� �ϴ� �ʱ�ȭ �ϰ� �ִ´�.
            weaponNameList.Add("(AR) AK74");
            weaponNameList.Add("(HG) M1911");
            weaponNameList.Add("(SG) Bennelli_M4");
            weaponNameList.Add("(EX) RPG7");
            weaponNameList.Add("(SP) FlashGrenade");
            weaponNameList.Add("(MG) M249");
            weaponNameList.Add("(SMG) Uzi"); //weaponNameList.Count = 7;
            //weaponList�� Add�� ������ ���� Ű �е忡 �ϳ��� ���Ⱑ ����, �ҷ��� �� �ִ�.


            weaponPoint = GameObject.Find("WeaponPoint");
            if(weaponPoint == null)
            {
                Debug.LogError("Create the Object in Player Object's Child and Name it 'WeaponPoint'");
            }


            /*
            weaponFactory = FindObjectOfType<WeaponFactory>(); //WeaponFactory ��ũ��Ʈ�� ����� ������Ʈ�� ������ ã�Ƽ� ����

            if (weaponFactory == null || weaponList == null)
            {
                Debug.LogError("WeaponFactory or weaponList is not assigned in WeaponSwap");
            }
            */
        }

        void Update()
        {
            // Ű�е� �Է� ����. Ű�е� 1���� �� i = 0
            for (int i = 0; i < weaponNameList.Count; i++)
            {
                if (Input.GetKeyDown((KeyCode)((int)KeyCode.Alpha1 + i))) //KeyCode,Alpha1 Ű�е� 1���� �������� Ȯ��. 
                {
                    SwitchWeapon(i);
                    currentWeaponSlot = previousWeapon.weaponSlot; //switch������ ���� weaponslot�� ���� SetActive�� �������� ����.
                    break;
                }
            }

            //EŰ �Է� ���� -> ��Ʈ��ũ �ƴ� �� �����̴�.
            if (Input.GetKeyDown(KeyCode.E))
            {
                RaycastHit hit;
                // ī�޶��� ��ġ���� �������� ���� �߻�
                Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
                
                //�׽�Ʈ DrawRay
                float maxRayDistance = 100f;
                Debug.DrawRay(ray.origin, ray.direction * maxRayDistance, Color.magenta, 1f);


                if (Physics.Raycast(ray, out hit, Mathf.Infinity, dropedWeaponLayer))
                {
                    Debug.Log("���̾�� ã�µ� ������");

                    WeaponPickup weaponPickupComponent = hit.transform.GetComponent<WeaponPickup>();
                    if (weaponPickupComponent != null)
                    {
                        weaponPickup = weaponPickupComponent; //ã�� ������Ʈ�� ������ �ʱ�ȭ�Ѵ�.
                        Debug.Log("WeaponPickup ������Ʈ ã��");
                    }
                    else
                    {
                        Debug.Log("WeaponPickup ������Ʈ ����");
                    }


                    // ���̾ �ε��� ������Ʈ�� �ٷ� ���� �ڽ� ������Ʈ Ž��
                    foreach (Transform child in hit.transform)
                    {

                        // �ش� ������Ʈ�� �پ� �ִ� Ư�� Ŭ���� ã��
                        Weapon droppedWeapon = child.GetComponent<Weapon>();
                        if (droppedWeapon != null)
                        {
                            droppedWeapon_weaponSlot = droppedWeapon.weaponSlot;
                            this.droppedWeapon = droppedWeapon;

                            Debug.Log("����� Weapon Ŭ���� ã�µ� ��������. ���� ����");
                            // Ŭ������ ã������, ���⼭ ó��
                            // ��: yourClassComponent.YourMethod();


                            PickupDroppedWeapon(); //���� ��ü ����
                        }
                        else
                        {
                            Debug.Log("WeaponPickup �˻� ����");
                        }
                    }
                }
            }
            /*
            // Sodo Code
            //if ��ư�� �����µ�, ����� Ʈ����/���̾ �����̶�� WeaponPickup�� ChangeWeapon �޼��带 ����
            //�Ű������� ������ �����ʹ� weaponHolderList�̰�, �� ����Ʈ�� �ϴ� ���� ������ �� Ŭ�������� ���ο� ����Ʈ�� �����,
            //�ش��ϴ� �ε����� ������ �����Ѵ�.
            //�׸��� ��ȯ���� ������ ���� �ִ� weaponHolderList�� �ٲٸ� ���� ��ü �Ϸ�
            */
        }

        void SwitchWeapon(int index) //1, 2, 3�� �ε����ϱ� �ű⿡ �´�
        {
            if (index < 0 || index >= weaponNameList.Count) //�ε��� ������ ������ �׳� ��ȯ (���� �ȵ�)
                return;

            //Weapon �κ��� ����ִٸ� -> Weapon ����
            if (weaponSlotArray[index] == null)
            {
                weaponSlotArray[index] = WeaponFactory.CreateWeapon(weaponNameList[index]); //���� ����
                if(previousWeapon != null)
                    previousWeapon.gameObject.SetActive(false);
            }
            //Weapon �κ��� ������� �ʴٸ� ���ⱳü -> ��Ȱ��ȭ��(false) Weapon Ȱ��ȭ(true)
            else if (weaponSlotArray[index] != null)
            {
                previousWeapon.gameObject.SetActive(false);
                weaponSlotArray[index].gameObject.SetActive(true); //�ѱ⸦ �ٲ����� �ش� �ѱ⸦ ���� ó���Ѵ�.

            }
            previousWeapon = weaponSlotArray[index]; //SetActive�� false�� ������ֱ� ���ؼ� ���� ���� ����
        }

        void PickupDroppedWeapon()
        {
            droppedWeapon_weaponSlot = weaponPickup.GetWeaponSlotData();
            //�ڡڡڡڱ� ���� weaponPickup�� Ray�� ã�� weaponPickup���� �ʱ�ȭ �ؾ� ��Ȯ�� �����Ͱ� �� ����.�ڡڡڡ�

            //droppedWeapon;
            Weapon Temp;
            Temp = droppedWeapon;

            // ���� ���Կ� �ִ� ���⸦ �ӽ� ������ ���� ----> �̷��� �ؾ� �ֹ��⸦ �ٲٷ��� �ϸ� �ֹ��Ⱑ ��ü�ȴ�.
            Weapon currentWeaponInSlot = weaponSlotArray[(int)droppedWeapon_weaponSlot];

            // ������ ���⸦ WeaponPoint�� �ڽ����� ����� ��ġ�� ȸ���� �����մϴ�.
            GameObject droppedWeaponObject = droppedWeapon.gameObject; //�׳� ���� ������Ʈ�� ��ü�ؾ� �ʱ�ȭ�� �ϴ� ���� �� �� �ִ�.
            droppedWeaponObject.transform.SetParent(weaponPoint.transform); //weaponPoint�� �ڽ����� �����.
            droppedWeaponObject.transform.localPosition = Vector3.zero;
            droppedWeaponObject.transform.localRotation = Quaternion.identity;

            // ������ ���⸦ ���� ���Կ� �����մϴ�.
            weaponSlotArray[(int)droppedWeapon_weaponSlot] = droppedWeapon;

            // ���� ���Կ� �ִ� ���⸦ ������ ������ ��ġ�� �̵���ŵ�ϴ�.
            if (currentWeaponInSlot != null)
            {
                GameObject currentWeaponObject = currentWeaponInSlot.gameObject;
                Transform originalParent = weaponPickup.transform; // ������ ������ ���� �θ�
                currentWeaponObject.transform.SetParent(originalParent); // �θ� ����
                currentWeaponObject.transform.localPosition = Vector3.zero; // ���� ��ġ ����
                currentWeaponObject.transform.localRotation = Quaternion.identity; // ���� ȸ�� ����
                currentWeaponObject.SetActive(false); // �ϴ� ��Ȱ��ȭ
                //�ڡڡڡڡڡڱ׸��� ������Ʈ ������.
            }

            // ���ο� ���� Ȱ��ȭ -> �̰Ŵ� currentWeaponSlot�� ���� SetActive ���θ� ���Ѵ�. ������ true �ٸ��� false���·� �д�.
            droppedWeaponObject.SetActive(true);

            // ������ Ȱ��ȭ�� ���� ������Ʈ
            previousWeapon = droppedWeapon;
            //��������

            //�ڡڡڡڡڡڡ�������Ʈ �Ѵ°� ����. 

            //���Ŀ� �̰Ÿ� ���Կ� ���� ����ǰ� �Ѵ�.
            /*
            switch (droppedWeapon_weaponSlot)
            {
                case WeaponSlot.MainArm: //1�� ����, 0�� ����Ʈ
                    Temp = weaponSlotArray[0];
                    weaponSlotArray[0] = weaponPickup.ChangeWeapon(Temp);
                    break;

                case WeaponSlot.SubArm: //2�� ����, 1�� ����Ʈ
                    Temp = weaponSlotArray[1];
                    weaponSlotArray[1] = weaponPickup.ChangeWeapon(Temp);
                    break;

                case WeaponSlot.Melee: //3�� ����, 2�� ����Ʈ
                    Temp = weaponSlotArray[2];
                    weaponSlotArray[2] = weaponPickup.ChangeWeapon(Temp);
                    break;

                case WeaponSlot.Explosive: //4�� ����, 3�� ����Ʈ
                    Temp = weaponSlotArray[3];
                    weaponSlotArray[3] = weaponPickup.ChangeWeapon(Temp);
                    break;

                case WeaponSlot.SpecialWeapon: //5�� ����, 4�� ����Ʈ
                    Temp = weaponSlotArray[4];
                    weaponSlotArray[4] = weaponPickup.ChangeWeapon(Temp);
                    break;

                case WeaponSlot.Grappling: //6�� ����, 5�� ����Ʈ
                    Temp = weaponSlotArray[5];
                    weaponSlotArray[5] = weaponPickup.ChangeWeapon(Temp);
                    break;


            } //switch  */
        } //PickupDroppedWeapon




    } //class
} //namespace
