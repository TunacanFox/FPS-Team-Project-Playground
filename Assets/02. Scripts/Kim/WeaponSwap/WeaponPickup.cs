using UnityEngine;
using FPS.WeaponSO;
using System.Collections.Generic;
using Unity.VisualScripting;

//Todo
//스크립터블 오브젝트에서 enum WeaoinSlot과 string weaponName을 받아서 저장하는 오브젝트. 그리고 어떻게 생겨먹었는지 알리기 위한 프리팹만 저장
//상호작용 하면 WeaponSlot을 이용해서 WeaponSwap의 WeaponList의 몇 번 인덱스에 넣을지 지정하고, 그 지정한 곳에 weaponName을 넣어준다.

//+기능1 무기 교체했으니 기존에 들고 있던 무기는 떨군다.
//+기능2 만약 1번 슬롯 무기 들고 있는데 1번 슬롯 무기를 교체했다면 WeaponSwap() 기능을 다시 실행하게 한다.

/*
E키를 눌렀을 때 
감지한 해당 오브젝트가 만약 태그, 혹은 레이어가 DropedWeapon 이라면
총기 데이터를 읽고, enum타입에 따라 해당하는 배열 인덱스에다가 총기 스왑

그러면 E키를 눌렀을 때 감지 레이캐스트를 쏘면 되는 것인가?
*/

namespace FPS.WeaponSO
{
    public class WeaponPickup : MonoBehaviour
    {
        public List<string> weaponNames; //스크립터블 오브젝트의 모든 이름을 저장하기 위함
        public string thisObjectWeaponName;

        private void Start()
        {
            weaponNames = WeaponSettingAssets.instance.GetWeaponNames(); //스크립터블 오브젝트의 이름들 모두 저장. 알아야 소환하지

        }

        public void ChangeWeapon(string weaponName)
        {

        }





    } //class
} //namespace