using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;

using FPS.WeaponSO; //스크립터블 오브젝트 데이터 끌고 오기 위해서 사용

namespace FPS.Lee.WeaponDetail
{
    public class Weapon : MonoBehaviour
    {
        public int damage;
        public float fireRate;

        public Camera mainCamera;

        private float nextFire;

        [Header("Ammo")]
        public int mag;
        public int ammo;
        public int fullMagazine;

        [Header("VFX EFFECT")]
        public GameObject hitVFX;
        public float removeFireHole;

        [Header("UI")]
        public TextMeshProUGUI magText;
        public TextMeshProUGUI ammoText;

        [Header("Animation")]
        public Animation weaponAnimation; //<-- Animation 컴포넌트 필요, 그곳의 Element가 있어야 무기(Clone)을 여기에 넣는게 가능함
        public AnimationClip reloadAnimation;

        [Header("Recoil Settings")]
        /*[Range(0, 1)]
        public float recoilPercent = 0.3f;*/

        [Range(0, 2)]
        public float recoverPercent = 0.7f;

        [Space]
        public float recoilUp = 1f;

        public float recoilBack = 0f;

        private Vector3 originalPosition;
        private Vector3 recoilVelocity = Vector3.zero;

        private float recoilLength;
        private float recoverLength;

        private bool recoiling;
        public bool recovering;

        private PhotonView _photonView;

        //테스트 테스트 테스트 테스트 테스트 테스트 테스트 테스트 테스트 
        [SerializeField] public Ray TEST_RAY;
        public Vector3 End1;
        public Vector3 End2;

        GameObject weaponObjectTest;

        GameObject playerObject;
        Transform weaponPointTest;


        // Start is called before the first frame update
        void Start()
        {
            _photonView = GetComponent<PhotonView>();
            magText = GameObject.Find("MagText").GetComponent<TextMeshProUGUI>();
            ammoText = GameObject.Find("AmmoText").GetComponent<TextMeshProUGUI>();


            originalPosition = transform.localPosition;

            recoilLength = 0;
            recoverLength = 1 / fireRate * recoverPercent;


            //테스트 테스트 테스트 테스트 테스트 테스트 테스트 테스트 테스트 테스트 테스트 테스트 테스트 테스트 테스트 테스트
            weaponObjectTest = this.gameObject; //이 스크립트가 부착 되는 무기오브젝트. Ray쐈을 때 방향을 보기 위함.
            
            playerObject = GameObject.Find("Player"); //플레이어 프리팹
            weaponPointTest = playerObject.transform.Find("WeaponPoint");



            if (weaponPointTest == null || weaponObjectTest == null)
                Debug.LogError("test || weaponObjectTest is null");

            Debug.Log("test is not null");
        }
        
        void Update()
        {
            //180도 돌려서 쏴야하니 -weaponPointTest.forward로 쏴야함
            TEST_RAY = new Ray(weaponPointTest.position, -weaponPointTest.forward); //이거 Player -> WeaponPoint로 넣을거면 test, Weapon 프리팹 자체로 넣을거면 weaponObjectTest 넣는다.
            
            End1 = weaponPointTest.position + -transform.forward * 10f;
            End2 = weaponObjectTest.transform.position + -transform.forward * 10f;

            //Debug.DrawLine(TEST_RAY.origin, End1, Color.yellow);
            Debug.DrawLine(TEST_RAY.origin, End2, Color.cyan);

            if (nextFire > 0)
                nextFire -= Time.deltaTime;

            if (Input.GetButton("Fire1") && nextFire <= 0 && ammo > 0 && weaponAnimation.isPlaying == false)
            {
                nextFire = 1 / fireRate;
                ammo--;

                magText.text = mag.ToString(); //n개의 개수 UI에 수정
                ammoText.text = ammo + "/" + fullMagazine; //mn개의 총알 개수 UI에 수정

                Fire();
            }
            if (Input.GetKeyDown(KeyCode.R) && mag > 0 && ammo < fullMagazine) //30
            {
                Reload();
            }

            if (recoiling)
            {
                Recoil();
            }
            if (recovering)
            {
                Recovering();
            }
        }


        public void WeaponUISetup(WeaponDataSO weaponData)
        {
            //Elementry Variable
            damage = weaponData.damage; //뎀
            fireRate = weaponData.fireRate; //발사 주기
            mainCamera = GetComponentInParent<Camera>();
            //mainCamera = Camera.main;//메인카메라 찾기. 씬에 있는 메인 카메라 태그 가진 카메라에 접근하는 방법.
            //인데... 태그가 MainCamera인 첫 번째 활성 카메라를 찾아서 모든 플레이어가 같이 움직이는 문제가 발생한다.

            //Ammo
            mag = weaponData.magazineNum; //초기화
            ammo = weaponData.ammo; //초기화
            fullMagazine = weaponData.fullMagazine; //초기화

            //VFX Effect
            hitVFX = weaponData.hitVFX;
            removeFireHole = weaponData.removeFireHole;

            //UI

            //애니메이션
            GameObject playerObject; //Player를 참조한다
            playerObject = this.gameObject;

            /*
             * 카메라가 무기의 부모일때 넣어야 하는 내용
            //Camera clonedObject = GetComponentInParent<Camera>(); //이제 WeaponSwap 스크립트가 붙는 곳이 Player고,
                                                                    //무기를 생성해주는 곳이 Player의 자식이라 이거 주석처리
            string cloneAnimationFinder = weaponData.weaponNameFinder + "(Clone)";
            Transform weaponTransform = clonedObject.transform.Find(cloneAnimationFinder); //설마했는데 (Clone)빠졌다고 이름이 틀리니까 Find를 못하네 ㅋㅋㅋㅋㅋㅋ
            Debug.Log($"weaponData네임파인더: {weaponData.weaponNameFinder}, cloneAnimationFindr: {cloneAnimationFinder}");
            */

            weaponAnimation = playerObject.GetComponent<Animation>(); //총기 애니메이션 없으면 에러난다.
                                                                      //찾은 오브젝트에서 Animation 컴포넌트를 찾고,
                                                                      //그곳의 Animations의 Element에서 뽑아서 weaponAnimation에 넣어준다.
            //총기 애니메이션을 일일이 넣지 않기 위해서 애니메이션 클립 추가
            //AnimationClip clipAdderW
            //weaponAnimation

            if(weaponAnimation == null)
            {

            }
            else
            {

            }


            //weaponAnimation = GameObject.Find(weaponData.weaponNameFinder); //ak74 프리팹의 경우 ak74를 넣었다. 그래서 일단 주석처리 필요하면 후에 넣는다
            reloadAnimation = weaponData.reloadAnimation; //미리 만든 애니메이션 클립 넣어줌.

            //RecoilSettings
            recoverPercent = weaponData.recoverPercent;
            recoilUp = weaponData.recoilUp;
            recoilBack = weaponData.recoilBack;
            //recovering = weaponData.recovering; //에러 떠서 주석처리

            //Animation에 재장전 애니메이션을 추가


        }

        void Reload()
        {
            weaponAnimation.Play(reloadAnimation.name);
            if (mag > 0)
            {
                mag--;

                ammo = fullMagazine;
            }
            magText.text = mag.ToString();
            ammoText.text = ammo + "/" + fullMagazine;
        }

        public void Fire()
        {
            recoiling = true;
            recovering = false;

            //Ray ray = new Ray(mainCamera.transform.position, mainCamera.transform.forward); 
            //오류: UnassignedReferenceException: The variable camera of Weapon has not been assigned.
            //You probably need to assign the camera variable of the Weapon script in the inspector.
            //흠... 메인 카메라에서 나오면 안되잖아..? -> 생성된 플레이어 쫓아가게 생성되면 초기화
            //테스트로 메인 카메라에 넣긴 했는데 Bullethole인가 뭔가가 안나왔다.
            //Bullethole은 Lee의 EffectSpawner에서 담당한다.
            /*
             *  NullReferenceException: Object reference not set to an instance of an object
                FPS.Lee.WeaponDetail.Weapon.Fire () (at Assets/02. Scripts/Lee/Weapon.cs:186)
                FPS.Lee.WeaponDetail.Weapon.Update () (at Assets/02. Scripts/Lee/Weapon.cs:92)
             */
            RaycastHit hit;
            int playermask = 1 << gameObject.layer;
            

            int enemy = LayerMask.GetMask("Enemy");

            Debug.DrawLine(TEST_RAY.origin, End1, Color.yellow);
            Debug.DrawLine(TEST_RAY.origin, End2, Color.red);



            //if (Physics.Raycast(TEST_RAY, out hit, 100f, playermask))
            if (Physics.Raycast(TEST_RAY, out hit, playermask))
            {
                Debug.DrawLine(TEST_RAY.origin, End1, Color.red);


                if (hit.transform.TryGetComponent(out Health health))
                {
                    Debug.Log("EnemyHealth Down");
                    health.TakeDamageClientRpc(damage);
                }
            }
            else if (Physics.Raycast(TEST_RAY.origin, TEST_RAY.direction, out hit, 100f, ~playermask))
            {
                Debug.DrawLine(TEST_RAY.origin, End2, Color.blue);


                Debug.Log("Effect Blah Blah");
                EffectSpawner.instance.SpawnBulletEffectClientRpc(hit.point, hit.normal);

            }

            else if (Physics.Raycast(TEST_RAY.origin, TEST_RAY.direction, out hit, enemy))
            {
                Debug.Log("Enemy!!!!!!!!!!!!!1");

            }


                return;

        }

        void Recoil()
        {
            Vector3 finalPosition = new Vector3(originalPosition.x,
                                                originalPosition.y + recoilUp,
                                                originalPosition.z + recoilBack);

            transform.localPosition = Vector3.SmoothDamp(transform.localPosition,
                                                         finalPosition,
                                                         ref recoilVelocity,
                                                         recoilLength);
            if (transform.localPosition == finalPosition)
            {
                recoiling = false;
                recovering = true;
            }
        }
        void Recovering()
        {
            Vector3 finalPosition = originalPosition;

            transform.localPosition = Vector3.SmoothDamp(transform.localPosition,
                                                         finalPosition,
                                                         ref recoilVelocity,
                                                         recoverLength);
            if (transform.localPosition == finalPosition)
            {
                recoiling = false;
                recovering = false;
            }
        }
    }//class
}//namespace
