using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FPS.WeaponHandler;
using FPS.Lee.WeaponDetail; //using 네임스페이스 WeaponFactory를 사용하기 위함

namespace FPS.WeaponSwap
{
    public class WeaponSwapController : MonoBehaviour
    {
        [SerializeField] private List<string> weaponNameList = new List<string>(); //총기 이름만 저장 중
        
        [SerializeField] private Weapon[] testArray = new Weapon[7]; //여기에 총기를 넣음 0번 주무기 1번 보조무기 등등. 1번~6번


        [SerializeField] private Weapon previousWeapon; // 현재 활성화된 총기

        public GameObject weaponPoint; //해당 오브젝트의 Vector3와 각도 받아서 새로운 총기를 만든다.
        public Vector3 currentVector;
        public Quaternion currentQuaternion;

        //public WeaponFactory weaponFactory; //총기 생성을 위해 클래스를 직접 참조하게 만들었다.
                                            //Start부분에 FindObjectType으로 초기화하게 했음. 만약 작동 안할 경우 에디터 상에서 참조하기.
        
        
        private void Start()
        {
            //기본 총기 인벤토리 만듦. 나중 가면 오브젝트와 상호작용하면 해당하는 슬롯에 무기를 때려넣어서 무기 생성하게 하면 된다.
            weaponNameList.Clear(); //시작했는데 무슨 이상한 쓰레기 값이 들어가서 일단 초기화 하고 넣는다.
            weaponNameList.Add("(AR) AK74");
            weaponNameList.Add("(HG) M1911");
            weaponNameList.Add("(SG) Bennelli_M4");
            weaponNameList.Add("(EX) RPG7");
            weaponNameList.Add("(SP) FlashGrenade");
            weaponNameList.Add("(MG) M249");
            weaponNameList.Add("(SMG) Uzi"); //weaponNameList.Count = 7;
            //weaponList에 Add될 때마다 숫자 키 패드에 하나씩 무기가 들어가고, 불러올 수 있다.


            weaponPoint = GameObject.Find("WeaponPoint");
            if(weaponPoint == null)
            {
                Debug.LogError("Create the Object in Player Object's Child and Name it 'WeaponPoint'");
            }

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
            // 키패드 입력 감지. 키패드 1번일 때 i = 0
            for (int i = 0; i < weaponNameList.Count; i++)
            {
                if (Input.GetKeyDown((KeyCode)((int)KeyCode.Alpha1 + i))) //KeyCode,Alpha1 키패드 1번을 눌렀는지 확인. 
                {
                    SwitchWeapon(i);
                    break;
                }
            }

            //if (Input.GetKeyCode("E"))
            /*
            // Sodo Code
            //if 버튼을 눌렀는데, 대상의 트리거/레이어가 ㅇㅇ이라면 WeaponPickup의 ChangeWeapon 메서드를 실행
            //매개변수로 보내는 데이터는 weaponHolderList이고, 그 리스트들 싹다 읽은 다음에 그 클래스에서 새로운 리스트를 만들고,
            //해당하는 인덱스의 정보만 변경한다.
            //그리고 반환해준 것으로 여기 있는 weaponHolderList를 바꾸면 무기 교체 완료
            */
        }

        void SwitchWeapon(int index) //1, 2, 3번 인덱스니까 거기에 맞는
        {
            if (index < 0 || index >= weaponNameList.Count) //인덱스 범위를 넘으면 그냥 반환 (실행 안됨)
                return;

            if (testArray[index] == null)
            {
                testArray[index] = WeaponFactory.CreateWeapon(weaponNameList[index]); //무기 생성
                if(previousWeapon != null)
                    previousWeapon.gameObject.SetActive(false);
            }
            else if (testArray[index] != null)
            {
                previousWeapon.gameObject.SetActive(false);
                testArray[index].gameObject.SetActive(true); //총기를 바꿨으니 해당 총기를 숨김 처리한다.

            }
            previousWeapon = testArray[index]; //WeaponName으로 총기 생성
        }
    } //class
} //namespace
