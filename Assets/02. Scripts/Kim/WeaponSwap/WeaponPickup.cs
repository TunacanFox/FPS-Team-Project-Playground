using UnityEngine;
using System.Collections.Generic;
using FPS.WeaponSO;
using FPS.Lee.WeaponDetail; //Weapon 클래스를 사용하기 위함
using FPS.WeaponHandler;

//Todo
//스크립터블 오브젝트에서 enum WeaoinSlot과 string weaponName을 받아서 저장하는 오브젝트. 그리고 어떻게 생겨먹었는지 알리기 위한 프리팹만 저장
//상호작용 하면 WeaponSlot을 이용해서 WeaponSwap의 WeaponList의 몇 번 인덱스에 넣을지 지정하고, 그 지정한 곳에 weaponName을 넣어준다.

//+기능1 무기 교체했으니 기존에 들고 있던 무기는 떨군다.
//+기능2 만약 1번 슬롯 무기 들고 있는데 1번 슬롯 무기를 교체했다면 WeaponSwap() 기능을 다시 실행하게 한다.

//2트
/*
E키를 눌렀을 때 
감지한 해당 오브젝트가 만약 태그, 혹은 레이어가 DropedWeapon 이라면
총기 데이터를 읽고, enum타입에 따라 해당하는 배열 인덱스에다가 총기 스왑

그러면 E키를 눌렀을 때 감지 레이캐스트를 쏘면 되는 것인가?
*/

//3트
/*
떨어진 무기와 교체하려면 
1. weaponSlotArray[int index]의 데이터를 가져와서 Weapon temp에 저장하고, 
2. 현재 이 오브젝트의 데이터인 Weapon dropedWeapon을 반환한 다음 그녀석으로 초기화하게 해줘야한다.
3. 그리고 오브젝트는 임시 저장한 Weapon temp의 데이터로 바꿔줘야한다.
*/

//4트
/*
그러려면 Weapon dropedWeapon의 데이터는 어떻게 처리할거임? 
WeaponFactory의 CreateWeapon으로 총기 제작 + 기본 데이터 처리는 Start했을 때 해야할듯. + 바닥에 있는 것처럼 회전도 기회되면 하기
그런데 CreateWeapon은 매개변수로 weaponName을 받아서 만들어준다.

*/

/// <summary> 
/// 해당 스크립트 및 부착한 오브젝트 사용하려면
/// 1. 스크립트 부착한 오브젝트에 가서 ScriptableObject 박기
/// 2. 
/// 
/// </summary>

namespace FPS.WeaponSwap
{
    public class WeaponPickup : MonoBehaviour
    {
        public List<string> weaponNames; //스크립터블 오브젝트의 모든 이름을 저장하기 위함 -> 안쓸거같은데

        [SerializeField] private WeaponDataSO droppedItemSO; //ScriptableObject를 저장하는 변수. 여기서 이름 뽑아오고, 그걸로 Create하게 하면 되지 않을까?
        public Weapon droppedWeapon;
        public Weapon temp_droppedWeapon;
        public WeaponSlot droppedWeapon_WeaponSlot;

        private void Start()
        {
            weaponNames = WeaponSettingAssets.instance.GetWeaponNames(); //스크립터블 오브젝트의 이름들 모두 저장. 알아야 소환하기 때문. -> 나중가면 필요없을거 같은데
            //모두 들어가는거 확인했음.
            
            droppedWeapon = WeaponFactory.CreateDropedWeapon(droppedItemSO.weaponName, this.gameObject); //넣은 스크립터블 오브젝트의 이름과 이 스크립트 있는 오브젝트 기반으로 만들고, 데이터는 WeaponUISetup 함수불러서 알아서 초기화
            droppedWeapon_WeaponSlot = droppedItemSO.weaponSlot;
            temp_droppedWeapon = droppedWeapon;
        }

        //WeaponSwapController가 E를 눌렀을 때 발동되게 할 함수
        //DropedWeapon의 Slot이 중요하지, 매개변수로 받는 녀석이 중요하진 않다.

        //일단 임시 Weapon형 변수에 기존 DropedWeapon저장, DropedWeapon을 매개변수 데이터로 바꾸고, 줘야할 TempDropedItem 반환 
        //

        //먼저 무기가 몇 번 슬롯인지 보낸다.
        public WeaponSlot GetWeaponSlotData()
        {
            return droppedWeapon.weaponSlot;
        }

        //그 다음에 처리를 하고 무기를 바꾼다.
        public Weapon ChangeWeapon(Weapon previousWeapon)
        {
            droppedWeapon = previousWeapon;

            return temp_droppedWeapon;
        }



    } //class
} //namespace