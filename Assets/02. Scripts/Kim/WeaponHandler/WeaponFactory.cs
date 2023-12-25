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
        //★★★★
        //1. 만약 네트워크 환경에서 다른 녀석에게도 CreateWeapon이 된다면 아래의 메서드에 매개변수로 GameObject player를 받게 한다.
        //2. 그리고 GaemObject Test = Player를 Find하는게 아니라 매개변수로 초기화한다.
        //★★★★
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

                //weapon.AddComponent<WeaponStat>(); //이건 테스트용으로 넣었으나, 나중에 WeaponUISetup함수로 대체함. 필요하다면 사용
                weapon.AddComponent<Animation>(); //Weapon의 변수 중 Animation weaponAnimation을 넣어주기 위해 추가.
                                                  //Animation의 Element에 총기에 맞는 애니메이션들 넣어주면 된다. (현재는 Reload Animation)
                weapon.AddComponent<Sway>(); //왜 넣는건진 모르겠는데 추가
                weapon.WeaponUISetup(weaponData); //Weapon의 변수들을 SO의 데이터를 이용해서 초기화
                return weapon;
            }
            return null;
        }

        //별 차이는 없다. 그저 WeaponPoint가 아닌 그 부모 데이터에 만드는 것일 뿐.
        public static Weapon CreateDropedWeapon(string weaponName, GameObject dropedWeapon) //이 메서드는 WeaponSwap.cs에서 호출된다.
        {
            //if (PlayerSetup.spawned.TryGetValue(PhotonNetwork.LocalPlayer.UserId, out PlayerSetup player))
            {
                GameObject Test = dropedWeapon;
                /*
                  만약 테스트로 무기 Swap이 되는지 확인하고자 한다면
                    0. Player라는 이름을 가진 오브젝트에 WeaponSwap 스크립트 추가해야함. (단, Player는 누군가의 자식이면 안된다)
                    1. 위의 조건문 부분 주석처리하기
                    2. Test가 있는 부분의 주석 풀기
                    3. 아래의 weaponPoint 초기화 하는 부분에 player대신 Test를 넣는다.
                    4. 확인하고 나서 원상복구 하기 (조립은 분해의 역순)
                */
                WeaponDataSO weaponData = WeaponSettingAssets.instance[weaponName]; //이 데이터로 weaponName에 해당하는 무기 가져온다
                //Transform weaponPoint = Test.transform.Find("WeaponPoint"); //weaponPoint의 자식으로 주라고 하는 것은 Ray로 검색하기 너무 복잡해서 수정.

                Weapon weapon = GameObject.Instantiate(weaponData.weaponPrefab, Test.transform).AddComponent<Weapon>(); //이건 되는거 까진 좋은데,
                                                                                                                     //AddComponent<Weapon>이 안된다. 싱글로 하면 붙여진다

                //컴포넌트 추가하는 부분
                //weapon.AddComponent<WeaponStat>(); //이건 테스트용으로 넣었으나, 나중에 WeaponUISetup함수로 대체함. 필요하다면 사용
                Animation animationComponent = weapon.AddComponent<Animation>(); //Weapon의 변수 중 Animation weaponAnimation을 넣어주기 위해 추가.
                                                                                 //Animation의 Element에 총기에 맞는 애니메이션들 넣어주면 된다. (현재는 Reload Animation)

                Sway swayComponent = weapon.AddComponent<Sway>();
                //weapon.AddComponent<Sway>(); //Sway는 왜 넣는건진 모르겠는데 추가
                weapon.WeaponUISetup(weaponData); //Weapon의 변수들을 SO의 데이터를 이용해서 초기화

                //이 2개의 컴포넌트가 활성화 되어 있으니까 플레이어가 화면 돌리면 같이 흔들림 -> 그래서 비활성화함.
                weapon.enabled = false; //Weapon 컴포넌트 비활성화
                swayComponent.enabled = false; //Sway 컴포넌트 비활성화
                animationComponent.enabled = false; //나중에 버그될 수 있으니 Animation 컴포넌트도 비활성화


                return weapon;
            }
            return null;
        }
    } //class
} //namespace