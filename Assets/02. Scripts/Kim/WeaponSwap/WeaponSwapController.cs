using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FPS.WeaponHandler;
using FPS.Lee.WeaponDetail; //using ���ӽ����̽� WeaponFactory�� ����ϱ� ����

namespace FPS.WeaponSwap
{
    public class WeaponSwapController : MonoBehaviour
    {
        [SerializeField] private List<string> weaponNameList = new List<string>(); //�ѱ� �̸��� ���� ��
        
        [SerializeField] private Weapon[] testArray = new Weapon[7]; //���⿡ �ѱ⸦ ���� 0�� �ֹ��� 1�� �������� ���. 1��~6��


        [SerializeField] private Weapon previousWeapon; // ���� Ȱ��ȭ�� �ѱ�

        public GameObject weaponPoint; //�ش� ������Ʈ�� Vector3�� ���� �޾Ƽ� ���ο� �ѱ⸦ �����.
        public Vector3 currentVector;
        public Quaternion currentQuaternion;

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
                    break;
                }
            }

            //if (Input.GetKeyCode("E"))
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

            if (testArray[index] == null)
            {
                testArray[index] = WeaponFactory.CreateWeapon(weaponNameList[index]); //���� ����
                if(previousWeapon != null)
                    previousWeapon.gameObject.SetActive(false);
            }
            else if (testArray[index] != null)
            {
                previousWeapon.gameObject.SetActive(false);
                testArray[index].gameObject.SetActive(true); //�ѱ⸦ �ٲ����� �ش� �ѱ⸦ ���� ó���Ѵ�.

            }
            previousWeapon = testArray[index]; //WeaponName���� �ѱ� ����
        }
    } //class
} //namespace
