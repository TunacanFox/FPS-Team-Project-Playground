using UnityEngine;
using FPS.WeaponSO;
using FPS.WeaponHandler;
using FPS.Lee.WeaponDetail;
using Photon.Pun;
using Unity.VisualScripting;
using TMPro;

namespace FPS.WeaponHandler
{
    //총기의 Vector값과 Quaternion 기반으로 총기를 만들어줌, 만들어서 1인칭 화면으로 띄워주는 클래스
    //총기의 세부 스탯은 WeaponStat으로 이관, 이 클래스로 불러온다
    public static class WeaponFactory
    {
        public static Weapon CreateWeapon(string weaponName) //이 메서드는 WeaponSwap.cs에서 호출된다.
        {
             //if (PlayerSetup.spawned.TryGetValue(PhotonNetwork.LocalPlayer.UserId, out PlayerSetup player))
              {
                GameObject Test = GameObject.Find("Player");
                /*
                  만약 테스트로 무기 Swap이 되는지 확인하고자 한다면
                    0. Player라는 이름을 가진 오브젝트에 WeaponSwap 스크립트 추가해야함. (단, Player는 누군가의 자식이면 안된다)
                    1. 위의 조건문 부분 주석처리하기
                    2. Test가 있는 부분의 주석 풀기
                    3. 아래의 weaponPoint 초기화 하는 부분에 player대신 Test를 넣는다.
                    4. 확인하고 나서 원상복구 하기 (조립은 분해의 역순)
                */
                WeaponDataSO weaponData = WeaponSettingAssets.instance[weaponName]; //이 데이터로 weaponName에 해당하는 무기 가져온다
                Transform weaponPoint = Test.transform.Find("WeaponPoint");

                if (weaponPoint == null)
                    Debug.LogError("WeaponPoint is null. Please add Create Empty as 'Player' child and rename it 'WeaponPoint'.");


                //이거 이미 WeaponSwap에 있어서 불필요하다
                if (weaponPoint.childCount > 0) //Destroy가 아니라 대상 총기를 SetActive를 false로 해야한다.
                {
                    //GameObject.Destroy(weaponPoint.GetChild(0).gameObject);
                }

                Weapon weapon = GameObject.Instantiate(weaponData.weaponPrefab, weaponPoint).AddComponent<Weapon>(); //이건 되는거 까진 좋은데,
                                                                                                                     //AddComponent<Weapon>이 안된다. 싱글로 하면 붙여진다

                weapon.AddComponent<WeaponStat>(); //이건 되고
                weapon.AddComponent<Animation>(); //Weapon의 변수 중 Animation weaponAnimation을 넣어주기 위해 추가.
                                                  //Animation의 Element에 총기에 맞는 애니메이션들 넣어주면 된다. (현재는 Reload Animation)
                weapon.AddComponent<Sway>(); //왜 넣는건진 모르겠는데 추가
                weapon.WeaponUISetup(weaponData); //Weapon의 변수들을 SO의 데이터를 이용해서 초기화
                return weapon;
            }

            return null;
           
        }
    }
}