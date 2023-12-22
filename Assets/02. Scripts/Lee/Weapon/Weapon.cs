using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;

using FPS.WeaponSO; //什滴験斗鷺 神崎詮闘 汽戚斗 怪壱 神奄 是背辞 紫遂

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
        public Animation weaponAnimation; //<-- Animation 陳匂獲闘 琶推, 益員税 Element亜 赤嬢醤 巷奄(Clone)聖 食奄拭 隔澗惟 亜管敗
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

        //砺什闘 砺什闘 砺什闘 砺什闘 砺什闘 砺什闘 砺什闘 砺什闘 砺什闘 
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


            //砺什闘 砺什闘 砺什闘 砺什闘 砺什闘 砺什闘 砺什闘 砺什闘 砺什闘 砺什闘 砺什闘 砺什闘 砺什闘 砺什闘 砺什闘 砺什闘
            weaponObjectTest = this.gameObject; //戚 什滴験闘亜 採鐸 鞠澗 巷奄神崎詮闘. Ray恕聖 凶 号狽聖 左奄 是敗.
            
            playerObject = GameObject.Find("Player"); //巴傾戚嬢 覗軒噸
            weaponPointTest = playerObject.transform.Find("WeaponPoint");



            if (weaponPointTest == null || weaponObjectTest == null)
                Debug.LogError("test || weaponObjectTest is null");

            Debug.Log("test is not null");
        }
        
        void Update()
        {
            //180亀 宜形辞 女醤馬艦 -weaponPointTest.forward稽 女醤敗
            TEST_RAY = new Ray(weaponPointTest.position, -weaponPointTest.forward); //戚暗 Player -> WeaponPoint稽 隔聖暗檎 test, Weapon 覗軒噸 切端稽 隔聖暗檎 weaponObjectTest 隔澗陥.
            
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

                magText.text = mag.ToString(); //n鯵税 鯵呪 UI拭 呪舛
                ammoText.text = ammo + "/" + fullMagazine; //mn鯵税 恥硝 鯵呪 UI拭 呪舛

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
            damage = weaponData.damage; //稀
            fireRate = weaponData.fireRate; //降紫 爽奄
            mainCamera = GetComponentInParent<Camera>();
            //mainCamera = Camera.main;//五昔朝五虞 達奄. 樟拭 赤澗 五昔 朝五虞 殿益 亜遭 朝五虞拭 羨悦馬澗 号狛.
            //昔汽... 殿益亜 MainCamera昔 湛 腰属 醗失 朝五虞研 達焼辞 乞窮 巴傾戚嬢亜 旭戚 崇送戚澗 庚薦亜 降持廃陥.

            //Ammo
            mag = weaponData.magazineNum; //段奄鉢
            ammo = weaponData.ammo; //段奄鉢
            fullMagazine = weaponData.fullMagazine; //段奄鉢

            //VFX Effect
            hitVFX = weaponData.hitVFX;
            removeFireHole = weaponData.removeFireHole;

            //UI

            //蕉艦五戚芝
            GameObject playerObject; //Player研 凧繕廃陥
            playerObject = this.gameObject;

            /*
             * 朝五虞亜 巷奄税 採乞析凶 隔嬢醤 馬澗 鎧遂
            //Camera clonedObject = GetComponentInParent<Camera>(); //戚薦 WeaponSwap 什滴験闘亜 細澗 員戚 Player壱,
                                                                    //巷奄研 持失背爽澗 員戚 Player税 切縦戚虞 戚暗 爽汐坦軒
            string cloneAnimationFinder = weaponData.weaponNameFinder + "(Clone)";
            Transform weaponTransform = clonedObject.transform.Find(cloneAnimationFinder); //竺原梅澗汽 (Clone)匙然陥壱 戚硯戚 堂軒艦猿 Find研 公馬革 せせせせせせ
            Debug.Log($"weaponData革績督昔希: {weaponData.weaponNameFinder}, cloneAnimationFindr: {cloneAnimationFinder}");
            */

            weaponAnimation = playerObject.GetComponent<Animation>(); //恥奄 蕉艦五戚芝 蒸生檎 拭君貝陥.
                                                                      //達精 神崎詮闘拭辞 Animation 陳匂獲闘研 達壱,
                                                                      //益員税 Animations税 Element拭辞 嗣焼辞 weaponAnimation拭 隔嬢層陥.
            //恥奄 蕉艦五戚芝聖 析析戚 隔走 省奄 是背辞 蕉艦五戚芝 適験 蓄亜
            //AnimationClip clipAdderW
            //weaponAnimation

            if(weaponAnimation == null)
            {

            }
            else
            {

            }


            //weaponAnimation = GameObject.Find(weaponData.weaponNameFinder); //ak74 覗軒噸税 井酔 ak74研 隔醸陥. 益掘辞 析舘 爽汐坦軒 琶推馬檎 板拭 隔澗陥
            reloadAnimation = weaponData.reloadAnimation; //耕軒 幻窮 蕉艦五戚芝 適験 隔嬢捜.

            //RecoilSettings
            recoverPercent = weaponData.recoverPercent;
            recoilUp = weaponData.recoilUp;
            recoilBack = weaponData.recoilBack;
            //recovering = weaponData.recovering; //拭君 彊辞 爽汐坦軒

            //Animation拭 仙舌穿 蕉艦五戚芝聖 蓄亜


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
            //神嫌: UnassignedReferenceException: The variable camera of Weapon has not been assigned.
            //You probably need to assign the camera variable of the Weapon script in the inspector.
            //緋... 五昔 朝五虞拭辞 蟹神檎 照鞠摂焼..? -> 持失吉 巴傾戚嬢 耐焼亜惟 持失鞠檎 段奄鉢
            //砺什闘稽 五昔 朝五虞拭 隔延 梅澗汽 Bullethole昔亜 杭亜亜 照蟹尽陥.
            //Bullethole精 Lee税 EffectSpawner拭辞 眼雁廃陥.
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
