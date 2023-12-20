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

                Weapon weapon = GameObject.Instantiate(weaponData.weaponPrefab, weaponPoint).AddComponent<Weapon>(); //이건 되는거 까진 좋은데,
                                                                                                                     //AddComponent<Weapon>이 안된다. 싱글로 하면 붙여진다

                weapon.AddComponent<WeaponStat>(); //이건 되고
                weapon.AddComponent<Animation>(); //Weapon의 변수 중 Animation weaponAnimation을 넣어주기 위해 추가.
                                                  //Animation의 Element에 총기에 맞는 애니메이션들 넣어주면 된다. (현재는 Reload Animation)
                weapon.WeaponUISetup(weaponData); //Weapon 스크립트의 
                return weapon;
            }

            return null;
           
        }
    }
}