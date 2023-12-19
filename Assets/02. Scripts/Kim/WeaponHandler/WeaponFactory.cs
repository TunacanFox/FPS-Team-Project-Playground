using UnityEngine;
using FPS.WeaponSO;
using FPS.WeaponHandler;
using FPS.Lee.WeaponDetail;
using Photon.Pun;
using Unity.VisualScripting;

namespace FPS.WeaponHandler
{
    //�ѱ��� Vector���� Quaternion ������� �ѱ⸦ �������, ���� 1��Ī ȭ������ ����ִ� Ŭ����
    //�ѱ��� ���� ������ WeaponStat���� �̰�, �� Ŭ������ �ҷ��´�
    public static class WeaponFactory
    {
        public static Weapon CreateWeapon(string weaponName) //�� �޼���� WeaponSwap.cs���� ȣ��ȴ�.
        {
            if (PlayerSetup.spawned.TryGetValue(PhotonNetwork.LocalPlayer.UserId, out PlayerSetup player))
            {
                WeaponDataSO weaponData = WeaponSettingAssets.instance[weaponName]; //�� �����ͷ� weaponName�� �ش��ϴ� ���� �����´�
                Transform weaponPoint = player.transform.Find("WeaponPoint");

                if (weaponPoint.childCount > 0)
                    GameObject.Destroy(weaponPoint.GetChild(0).gameObject);

                Weapon weapon = GameObject.Instantiate(weaponData.weaponPrefab, weaponPoint).AddComponent<Weapon>();
                weapon.AddComponent<WeaponStat>();
                weapon.WeaponUISetup(weaponData);
                return weapon;
            }

            return null;
           
        }
    }
}