using UnityEngine;
using FPS.WeaponSO;
using FPS.WeaponHandler;
using FPS.Lee.WeaponDetail;
using Photon.Pun;
using Unity.VisualScripting;
using TMPro;

namespace FPS.WeaponHandler
{
    //�ѱ��� Vector���� Quaternion ������� �ѱ⸦ �������, ���� 1��Ī ȭ������ ����ִ� Ŭ����
    //�ѱ��� ���� ������ WeaponStat���� �̰�, �� Ŭ������ �ҷ��´�
    public static class WeaponFactory
    {
        public static Weapon CreateWeapon(string weaponName) //�� �޼���� WeaponSwap.cs���� ȣ��ȴ�.
        {
             //if (PlayerSetup.spawned.TryGetValue(PhotonNetwork.LocalPlayer.UserId, out PlayerSetup player))
              {
                GameObject Test = GameObject.Find("Player");
                /*
                  ���� �׽�Ʈ�� ���� Swap�� �Ǵ��� Ȯ���ϰ��� �Ѵٸ�
                    0. Player��� �̸��� ���� ������Ʈ�� WeaponSwap ��ũ��Ʈ �߰��ؾ���. (��, Player�� �������� �ڽ��̸� �ȵȴ�)
                    1. ���� ���ǹ� �κ� �ּ�ó���ϱ�
                    2. Test�� �ִ� �κ��� �ּ� Ǯ��
                    3. �Ʒ��� weaponPoint �ʱ�ȭ �ϴ� �κп� player��� Test�� �ִ´�.
                    4. Ȯ���ϰ� ���� ���󺹱� �ϱ� (������ ������ ����)
                */
                WeaponDataSO weaponData = WeaponSettingAssets.instance[weaponName]; //�� �����ͷ� weaponName�� �ش��ϴ� ���� �����´�
                Transform weaponPoint = Test.transform.Find("WeaponPoint");

                if (weaponPoint == null)
                    Debug.LogError("WeaponPoint is null. Please add Create Empty as 'Player' child and rename it 'WeaponPoint'.");


                //�̰� �̹� WeaponSwap�� �־ ���ʿ��ϴ�
                if (weaponPoint.childCount > 0) //Destroy�� �ƴ϶� ��� �ѱ⸦ SetActive�� false�� �ؾ��Ѵ�.
                {
                    //GameObject.Destroy(weaponPoint.GetChild(0).gameObject);
                }

                Weapon weapon = GameObject.Instantiate(weaponData.weaponPrefab, weaponPoint).AddComponent<Weapon>(); //�̰� �Ǵ°� ���� ������,
                                                                                                                     //AddComponent<Weapon>�� �ȵȴ�. �̱۷� �ϸ� �ٿ�����

                weapon.AddComponent<WeaponStat>(); //�̰� �ǰ�
                weapon.AddComponent<Animation>(); //Weapon�� ���� �� Animation weaponAnimation�� �־��ֱ� ���� �߰�.
                                                  //Animation�� Element�� �ѱ⿡ �´� �ִϸ��̼ǵ� �־��ָ� �ȴ�. (����� Reload Animation)
                weapon.AddComponent<Sway>(); //�� �ִ°��� �𸣰ڴµ� �߰�
                weapon.WeaponUISetup(weaponData); //Weapon�� �������� SO�� �����͸� �̿��ؼ� �ʱ�ȭ
                return weapon;
            }

            return null;
           
        }
    }
}