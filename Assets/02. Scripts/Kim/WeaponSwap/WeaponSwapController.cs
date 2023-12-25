using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FPS.WeaponHandler;
using FPS.Lee.WeaponDetail; //using 네임스페이스 WeaponFactory를 사용하기 위함, Weapon클래스를 사용하기 위함
using FPS.WeaponSO;

namespace FPS.WeaponSwap
{
    public class WeaponSwapController : MonoBehaviour
    {
        [SerializeField] private List<string> weaponNameList = new List<string>(); //총기 이름만 저장 중 -> 무기 생성에만 관여하는 중이다.
        
        [SerializeField] private Weapon[] weaponSlotArray = new Weapon[7]; //총기를 넣음 0번 주무기 1번 보조무기 등등. 1번~6번. 여기에 ㄹㅇ Weapon클래스로 선언된 무기 들어감


        [SerializeField] private Weapon previousWeapon; // 이전에 활성화된 총기 -> SetActive(false)를 위한 weaponSlotArray에 대한 참조에만 활용된다.
        public WeaponSlot currentWeaponSlot; //무기 교체 로직 switch할 때 SetActive를 false로 할지 바로 총기 들게 true로 할지 부분

        public GameObject weaponPoint; //해당 오브젝트의 Vector3와 각도 받아서 새로운 총기를 만든다.
        public Vector3 currentVector;
        public Quaternion currentQuaternion;

        //DroppedWeapon과 기존 무기 교체를 위함.
        public Weapon droppedWeapon;
        public WeaponPickup weaponPickup;
        public WeaponSlot droppedWeapon_weaponSlot;
        public LayerMask dropedWeaponLayer;

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
                    currentWeaponSlot = previousWeapon.weaponSlot; //switch문에서 현재 weaponslot에 따라 SetActive를 어케할지 위함.
                    break;
                }
            }

            //E키 입력 감지 -> 네트워크 아닐 때 버전이다.
            if (Input.GetKeyDown(KeyCode.E))
            {
                RaycastHit hit;
                // 카메라의 위치에서 전방으로 레이 발사
                Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
                
                //테스트 DrawRay
                float maxRayDistance = 100f;
                Debug.DrawRay(ray.origin, ray.direction * maxRayDistance, Color.magenta, 1f);


                if (Physics.Raycast(ray, out hit, Mathf.Infinity, dropedWeaponLayer))
                {
                    Debug.Log("레이어는 찾는데 성공함");

                    WeaponPickup weaponPickupComponent = hit.transform.GetComponent<WeaponPickup>();
                    if (weaponPickupComponent != null)
                    {
                        weaponPickup = weaponPickupComponent; //찾은 컴포넌트로 데이터 초기화한다.
                        Debug.Log("WeaponPickup 컴포넌트 찾음");
                    }
                    else
                    {
                        Debug.Log("WeaponPickup 컴포넌트 없음");
                    }


                    // 레이어에 부딪힌 오브젝트의 바로 다음 자식 오브젝트 탐색
                    foreach (Transform child in hit.transform)
                    {

                        // 해당 오브젝트에 붙어 있는 특정 클래스 찾기
                        Weapon droppedWeapon = child.GetComponent<Weapon>();
                        if (droppedWeapon != null)
                        {
                            droppedWeapon_weaponSlot = droppedWeapon.weaponSlot;
                            this.droppedWeapon = droppedWeapon;

                            Debug.Log("상대의 Weapon 클래스 찾는데 성공했음. 다음 ㄱㄱ");
                            // 클래스를 찾았으면, 여기서 처리
                            // 예: yourClassComponent.YourMethod();


                            PickupDroppedWeapon(); //무기 교체 시작
                        }
                        else
                        {
                            Debug.Log("WeaponPickup 검색 실패");
                        }
                    }
                }
            }
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

            //Weapon 인벤이 비어있다면 -> Weapon 생성
            if (weaponSlotArray[index] == null)
            {
                weaponSlotArray[index] = WeaponFactory.CreateWeapon(weaponNameList[index]); //무기 생성
                if(previousWeapon != null)
                    previousWeapon.gameObject.SetActive(false);
            }
            //Weapon 인벤이 비어있지 않다면 무기교체 -> 비활성화된(false) Weapon 활성화(true)
            else if (weaponSlotArray[index] != null)
            {
                previousWeapon.gameObject.SetActive(false);
                weaponSlotArray[index].gameObject.SetActive(true); //총기를 바꿨으니 해당 총기를 숨김 처리한다.

            }
            previousWeapon = weaponSlotArray[index]; //SetActive를 false로 만들어주기 위해서 이전 무기 저장
        }

        void PickupDroppedWeapon()
        {
            droppedWeapon_weaponSlot = weaponPickup.GetWeaponSlotData();
            //★★★★그 전에 weaponPickup을 Ray로 찾은 weaponPickup으로 초기화 해야 정확한 데이터가 들어갈 것임.★★★★

            //droppedWeapon;
            Weapon Temp;
            Temp = droppedWeapon;

            // 현재 슬롯에 있는 무기를 임시 변수에 저장 ----> 이렇게 해야 주무기를 바꾸려고 하면 주무기가 교체된다.
            Weapon currentWeaponInSlot = weaponSlotArray[(int)droppedWeapon_weaponSlot];

            // 떨어진 무기를 WeaponPoint의 자식으로 만들고 위치와 회전을 조정합니다.
            GameObject droppedWeaponObject = droppedWeapon.gameObject; //그냥 게임 오브젝트롤 교체해야 초기화를 하는 수고를 덜 수 있다.
            droppedWeaponObject.transform.SetParent(weaponPoint.transform); //weaponPoint의 자식으로 만든다.
            droppedWeaponObject.transform.localPosition = Vector3.zero;
            droppedWeaponObject.transform.localRotation = Quaternion.identity;

            // 떨어진 무기를 현재 슬롯에 설정합니다.
            weaponSlotArray[(int)droppedWeapon_weaponSlot] = droppedWeapon;

            // 현재 슬롯에 있던 무기를 떨어진 무기의 위치로 이동시킵니다.
            if (currentWeaponInSlot != null)
            {
                GameObject currentWeaponObject = currentWeaponInSlot.gameObject;
                Transform originalParent = weaponPickup.transform; // 떨어진 무기의 원래 부모
                currentWeaponObject.transform.SetParent(originalParent); // 부모 설정
                currentWeaponObject.transform.localPosition = Vector3.zero; // 로컬 위치 조정
                currentWeaponObject.transform.localRotation = Quaternion.identity; // 로컬 회전 조정
                currentWeaponObject.SetActive(false); // 일단 비활성화
                //★★★★★★그리고 컴포넌트 꺼야함.
            }

            // 새로운 무기 활성화 -> 이거는 currentWeaponSlot에 따라 SetActive 여부를 정한다. 같으면 true 다르면 false상태로 둔다.
            droppedWeaponObject.SetActive(true);

            // 이전에 활성화된 무기 업데이트
            previousWeapon = droppedWeapon;
            //ㅁㄴㅇㄹ

            //★★★★★★★컴포넌트 켜는거 하자. 

            //이후에 이거를 슬롯에 따라 변경되게 한다.
            /*
            switch (droppedWeapon_weaponSlot)
            {
                case WeaponSlot.MainArm: //1번 슬롯, 0번 리스트
                    Temp = weaponSlotArray[0];
                    weaponSlotArray[0] = weaponPickup.ChangeWeapon(Temp);
                    break;

                case WeaponSlot.SubArm: //2번 슬롯, 1번 리스트
                    Temp = weaponSlotArray[1];
                    weaponSlotArray[1] = weaponPickup.ChangeWeapon(Temp);
                    break;

                case WeaponSlot.Melee: //3번 슬롯, 2번 리스트
                    Temp = weaponSlotArray[2];
                    weaponSlotArray[2] = weaponPickup.ChangeWeapon(Temp);
                    break;

                case WeaponSlot.Explosive: //4번 슬롯, 3번 리스트
                    Temp = weaponSlotArray[3];
                    weaponSlotArray[3] = weaponPickup.ChangeWeapon(Temp);
                    break;

                case WeaponSlot.SpecialWeapon: //5번 슬롯, 4번 리스트
                    Temp = weaponSlotArray[4];
                    weaponSlotArray[4] = weaponPickup.ChangeWeapon(Temp);
                    break;

                case WeaponSlot.Grappling: //6번 슬롯, 5번 리스트
                    Temp = weaponSlotArray[5];
                    weaponSlotArray[5] = weaponPickup.ChangeWeapon(Temp);
                    break;


            } //switch  */
        } //PickupDroppedWeapon




    } //class
} //namespace
