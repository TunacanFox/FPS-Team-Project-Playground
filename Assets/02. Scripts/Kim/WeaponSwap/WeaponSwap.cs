using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FPS.WeaponHandler;
using FPS.Lee.WeaponDetail; //using ���ӽ����̽� WeaponFactory�� ����ϱ� ����

namespace FPS.WeaponSwap
{
    public class WeaponSwap : MonoBehaviour
    {
        [SerializeField] private List<string> weaponHolderList = new List<string>(); //�ѱ� �̸��� ������ �˻� ������ �ѱ� ������ �Ŵ�.
                                                                                     //�̰� 1~6�� �ѱ� �κ��丮��.
        [SerializeField] private Weapon currentWeapon; // ���� Ȱ��ȭ�� �ѱ�

        public GameObject weaponPoint; //�ش� ������Ʈ�� Vector3�� ���� �޾Ƽ� ���ο� �ѱ⸦ �����.
        public Vector3 currentVector;
        public Quaternion currentQuaternion;

        //public WeaponFactory weaponFactory; //�ѱ� ������ ���� Ŭ������ ���� �����ϰ� �������.
                                            //Start�κп� FindObjectType���� �ʱ�ȭ�ϰ� ����. ���� �۵� ���� ��� ������ �󿡼� �����ϱ�.
        

        private void Start()
        {
            //�⺻ �ѱ� �κ��丮 ����. ���� ���� ������Ʈ�� ��ȣ�ۿ��ϸ� �ش��ϴ� ���Կ� ���⸦ �����־ ���� �����ϰ� �ϸ� �ȴ�.
            weaponHolderList.Clear(); //�����ߴµ� ���� �̻��� ������ ���� ���� �ϴ� �ʱ�ȭ �ϰ� �ִ´�.
            weaponHolderList.Add("(AR) AK74");
            weaponHolderList.Add("(HG) M1911");
            weaponHolderList.Add("(SG) Bennelli_M4");
            weaponHolderList.Add("(EX) RPG7");
            weaponHolderList.Add("(SP) FlashGrenade");
            weaponHolderList.Add("(MG) M249");
            weaponHolderList.Add("(SMG) Uzi");
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
            // Ű�е� �Է� ����
            for (int i = 0; i < weaponHolderList.Count; i++)
            {
                if (Input.GetKeyDown((KeyCode)((int)KeyCode.Alpha1 + i)))
                {
                    SwitchWeapon(i);
                    break;
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

        void SwitchWeapon(int index)
        {
            if (index < 0 || index >= weaponHolderList.Count) //�ε��� ������ ������ �׳� ��ȯ (���� �ȵ�)
                return;

            if (currentWeapon != null)
            {
                Destroy(currentWeapon); //�ѱ⸦ �ٲ����� ������ �ѱ⸦ ����
            }

            //Debug.Log($"weaponList: {weaponList[index]}, index: {index}"); //������ �ѱ��� �̸��� �ε����� ����غ���.

            currentWeapon = WeaponFactory.CreateWeapon(weaponHolderList[index]); //�ѱ� ����
        }

    } //class
} //namespace
