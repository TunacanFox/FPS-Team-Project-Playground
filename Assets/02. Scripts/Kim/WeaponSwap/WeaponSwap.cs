using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FPS.WeaponHandler;
using FPS.Lee.WeaponDetail; //using ���ӽ����̽� WeaponFactory�� ����ϱ� ����

namespace FPS.WeaponSwap
{
    public class WeaponSwap : MonoBehaviour
    {
        private List<string> weaponList = new List<string>(); //�ѱ� �̸��� ������ �˻� ������ �ѱ� ������ �Ŵ�
        [SerializeField] private Weapon currentWeapon; // ���� Ȱ��ȭ�� �ѱ�

        public GameObject weaponPoint; //�ش� ������Ʈ�� Vector3�� ���� �޾Ƽ� ���ο� �ѱ⸦ �����.
        public Vector3 currentVector;
        public Quaternion currentQuaternion;

        //public WeaponFactory weaponFactory; //�ѱ� ������ ���� Ŭ������ ���� �����ϰ� �������.
                                            //Start�κп� FindObjectType���� �ʱ�ȭ�ϰ� ����. ���� �۵� ���� ��� ������ �󿡼� �����ϱ�.
        

        private void Start()
        {
            //�⺻ �ѱ� �κ��丮 ����. ���� ���� ������Ʈ�� ��ȣ�ۿ��ϸ� �ش��ϴ� ���Կ� ���⸦ �����־ ���� �����ϰ� �ϸ� �ȴ�.
            weaponList.Clear(); //�����ߴµ� ���� �̻��� ������ ���� ���� �ϴ� �ʱ�ȭ �ϰ� �ִ´�.
            weaponList.Add("(AR) AK74");
            weaponList.Add("(HG) M1911");
            weaponList.Add("(SG) Bennelli_M4");
            weaponList.Add("(EX) RPG7");
            weaponList.Add("(SP) FlashGrenade");
            weaponList.Add("(MG) M249");
            weaponList.Add("(SMG) Uzi");
            //weaponList�� Add�� ������ ���� Ű �е忡 �ϳ��� ���Ⱑ ����, �ҷ��� �� �ִ�.

            weaponPoint = GameObject.Find("WeaponPoint");

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
            for (int i = 0; i < weaponList.Count; i++)
            {
                if (Input.GetKeyDown((KeyCode)((int)KeyCode.Alpha1 + i)))
                {
                    SwitchWeapon(i);
                    break;
                }
            }
        }

        void SwitchWeapon(int index)
        {
            if (index < 0 || index >= weaponList.Count) //�ε��� ������ ������ �׳� ��ȯ (���� �ȵ�)
                return;

            if (currentWeapon != null)
            {
                Destroy(currentWeapon); //�ѱ⸦ �ٲ����� ������ �ѱ⸦ ����
            }

            //Debug.Log($"weaponList: {weaponList[index]}, index: {index}"); //������ �ѱ��� �̸��� �ε����� ����غ���.

            currentWeapon = WeaponFactory.CreateWeapon(weaponList[index]); //�ѱ� ����
        }

    }


}
