using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FPS.WeaponHandler;
using FPS.Lee.WeaponDetail; //using 네임스페이스 WeaponFactory를 사용하기 위함

namespace FPS.WeaponSwap
{
    public class WeaponSwap : MonoBehaviour
    {
        private List<string> weaponList = new List<string>(); //총기 이름만 가지고 검색 때려서 총기 가져올 거다
        [SerializeField] private Weapon currentWeapon; // 현재 활성화된 총기

        public GameObject weaponPoint; //해당 오브젝트의 Vector3와 각도 받아서 새로운 총기를 만든다.
        public Vector3 currentVector;
        public Quaternion currentQuaternion;

        //public WeaponFactory weaponFactory; //총기 생성을 위해 클래스를 직접 참조하게 만들었다.
                                            //Start부분에 FindObjectType으로 초기화하게 했음. 만약 작동 안할 경우 에디터 상에서 참조하기.
        

        private void Start()
        {
            //기본 총기 인벤토리 만듦. 나중 가면 오브젝트와 상호작용하면 해당하는 슬롯에 무기를 때려넣어서 무기 생성하게 하면 된다.
            weaponList.Clear(); //시작했는데 무슨 이상한 쓰레기 값이 들어가서 일단 초기화 하고 넣는다.
            weaponList.Add("(AR) AK74");
            weaponList.Add("(HG) M1911");
            weaponList.Add("(SG) Bennelli_M4");
            weaponList.Add("(EX) RPG7");
            weaponList.Add("(SP) FlashGrenade");
            weaponList.Add("(MG) M249");
            weaponList.Add("(SMG) Uzi");
            //weaponList에 Add될 때마다 숫자 키 패드에 하나씩 무기가 들어가고, 불러올 수 있다.

            weaponPoint = GameObject.Find("WeaponPoint");

            /*
            weaponFactory = FindObjectOfType<WeaponFactory>(); //WeaponFactory 스크립트가 적용된 오브젝트를 씬에서 찾아서 참조

            if (weaponFactory == null || weaponList == null)
            {
                Debug.LogError("WeaponFactory or weaponList is not assigned in WeaponSwap");
            }
            */
        }

        void Update()
        {
            // 키패드 입력 감지
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
            if (index < 0 || index >= weaponList.Count) //인덱스 범위를 넘으면 그냥 반환 (실행 안됨)
                return;

            if (currentWeapon != null)
            {
                Destroy(currentWeapon); //총기를 바꿨으니 기존의 총기를 제거
            }

            //Debug.Log($"weaponList: {weaponList[index]}, index: {index}"); //생성할 총기의 이름과 인덱스를 출력해본다.

            currentWeapon = WeaponFactory.CreateWeapon(weaponList[index]); //총기 생성
        }

    }


}
