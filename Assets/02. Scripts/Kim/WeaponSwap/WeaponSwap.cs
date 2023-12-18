using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FPS.WeaponHandler; //using ���ӽ����̽� WeaponFactory�� ����ϱ� ����

namespace FPS.WeaponSwap
{
    public class WeaponSwap : MonoBehaviour
    {
        private List<string> weaponList = new List<string>(); //�ѱ� �̸��� ������ �˻� ������ �ѱ� ������ �Ŵ�
        [SerializeField] private GameObject currentWeapon; // ���� Ȱ��ȭ�� �ѱ�

        public GameObject positionSettingPrefab; //���� ���·� Vector3�� Rotation�� ������ �Ǵ� �ѱ� ������Ʈ. ���̾��Ű���� positionSettingPrefab�̸��� ���� ������Ʈ�� �߰��ؾ� �Ѵ�.
        //�� ������Ʈ�� Vector3�� ���� �޾Ƽ� ���ο� �ѱ⸦ �����.
        public Vector3 currentVector;
        public Quaternion currentQuaternion;

        public WeaponFactory weaponFactory; //�ѱ� ������ ���� Ŭ������ ���� �������.

        private void Start()
        {
            //�⺻ �ѱ� �κ��丮 ����. ���� ���� ������Ʈ�� ��ȣ�ۿ��ϸ� �ش��ϴ� ���Կ� ���⸦ �����־ ���� �����ϰ� �ϸ� �ȴ�.
            weaponList.Clear(); //�����ߴµ� ���� �̻��� ������ ���� ���� �ϴ� �ʱ�ȭ �ϰ� �ִ´�.
            weaponList.Add("(AR) AK74");
            weaponList.Add("(HG) M1911");
            weaponList.Add("(SG) Bennelli_M4");
            weaponList.Add("(EX) RPG7");
            weaponList.Add("(SP) FlashGrenade");


            if (weaponFactory == null || weaponList == null)
            {
                Debug.LogError("WeaponFactory or weaponList is not assigned in WeaponSwap");
            }
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

            currentWeapon = weaponFactory.CreateWeapon(weaponList[index]); //�ѱ� ����
        }

    }


}
