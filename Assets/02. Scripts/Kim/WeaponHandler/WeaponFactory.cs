using UnityEngine;
using FPS.WeaponSO;
using FPS.WeaponHandler;
using FPS.Lee.WeaponDetail;
using Photon.Pun;
using Unity.VisualScripting;

namespace FPS.WeaponHandler
{
    //총기의 Vector값과 Quaternion 기반으로 총기를 만들어줌, 만들어서 1인칭 화면으로 띄워주는 클래스
    //총기의 세부 스탯은 WeaponStat으로 이관, 이 클래스로 불러온다
    public static class WeaponFactory
    {
        public static Weapon CreateWeapon(string weaponName) //이 메서드는 WeaponSwap.cs에서 호출된다.
        {
            if (PlayerSetup.spawned.TryGetValue(PhotonNetwork.LocalPlayer.UserId, out PlayerSetup player))
            {
                WeaponDataSO weaponData = WeaponSettingAssets.instance[weaponName]; //이 데이터로 weaponName에 해당하는 무기 가져온다
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