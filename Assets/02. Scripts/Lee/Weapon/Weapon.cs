using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;

using FPS.WeaponSO; //��ũ���ͺ� ������Ʈ ������ ���� ���� ���ؼ� ���

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
        public Animation weaponAnimation; //<-- Animation ������Ʈ �ʿ�, �װ��� Element�� �־�� ����(Clone)�� ���⿡ �ִ°� ������
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

        //�׽�Ʈ �׽�Ʈ �׽�Ʈ �׽�Ʈ �׽�Ʈ �׽�Ʈ �׽�Ʈ �׽�Ʈ �׽�Ʈ 
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


            //�׽�Ʈ �׽�Ʈ �׽�Ʈ �׽�Ʈ �׽�Ʈ �׽�Ʈ �׽�Ʈ �׽�Ʈ �׽�Ʈ �׽�Ʈ �׽�Ʈ �׽�Ʈ �׽�Ʈ �׽�Ʈ �׽�Ʈ �׽�Ʈ
            weaponObjectTest = this.gameObject; //�� ��ũ��Ʈ�� ���� �Ǵ� ���������Ʈ. Ray���� �� ������ ���� ����.
            
            playerObject = GameObject.Find("Player"); //�÷��̾� ������
            weaponPointTest = playerObject.transform.Find("WeaponPoint");



            if (weaponPointTest == null || weaponObjectTest == null)
                Debug.LogError("test || weaponObjectTest is null");

            Debug.Log("test is not null");
        }
        
        void Update()
        {
            //180�� ������ �����ϴ� -weaponPointTest.forward�� ������
            TEST_RAY = new Ray(weaponPointTest.position, -weaponPointTest.forward); //�̰� Player -> WeaponPoint�� �����Ÿ� test, Weapon ������ ��ü�� �����Ÿ� weaponObjectTest �ִ´�.
            
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

                magText.text = mag.ToString(); //n���� ���� UI�� ����
                ammoText.text = ammo + "/" + fullMagazine; //mn���� �Ѿ� ���� UI�� ����

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
            damage = weaponData.damage; //��
            fireRate = weaponData.fireRate; //�߻� �ֱ�
            mainCamera = GetComponentInParent<Camera>();
            //mainCamera = Camera.main;//����ī�޶� ã��. ���� �ִ� ���� ī�޶� �±� ���� ī�޶� �����ϴ� ���.
            //�ε�... �±װ� MainCamera�� ù ��° Ȱ�� ī�޶� ã�Ƽ� ��� �÷��̾ ���� �����̴� ������ �߻��Ѵ�.

            //Ammo
            mag = weaponData.magazineNum; //�ʱ�ȭ
            ammo = weaponData.ammo; //�ʱ�ȭ
            fullMagazine = weaponData.fullMagazine; //�ʱ�ȭ

            //VFX Effect
            hitVFX = weaponData.hitVFX;
            removeFireHole = weaponData.removeFireHole;

            //UI

            //�ִϸ��̼�
            GameObject playerObject; //Player�� �����Ѵ�
            playerObject = this.gameObject;

            /*
             * ī�޶� ������ �θ��϶� �־�� �ϴ� ����
            //Camera clonedObject = GetComponentInParent<Camera>(); //���� WeaponSwap ��ũ��Ʈ�� �ٴ� ���� Player��,
                                                                    //���⸦ �������ִ� ���� Player�� �ڽ��̶� �̰� �ּ�ó��
            string cloneAnimationFinder = weaponData.weaponNameFinder + "(Clone)";
            Transform weaponTransform = clonedObject.transform.Find(cloneAnimationFinder); //�����ߴµ� (Clone)�����ٰ� �̸��� Ʋ���ϱ� Find�� ���ϳ� ������������
            Debug.Log($"weaponData�������δ�: {weaponData.weaponNameFinder}, cloneAnimationFindr: {cloneAnimationFinder}");
            */

            weaponAnimation = playerObject.GetComponent<Animation>(); //�ѱ� �ִϸ��̼� ������ ��������.
                                                                      //ã�� ������Ʈ���� Animation ������Ʈ�� ã��,
                                                                      //�װ��� Animations�� Element���� �̾Ƽ� weaponAnimation�� �־��ش�.
            //�ѱ� �ִϸ��̼��� ������ ���� �ʱ� ���ؼ� �ִϸ��̼� Ŭ�� �߰�
            //AnimationClip clipAdderW
            //weaponAnimation

            if(weaponAnimation == null)
            {

            }
            else
            {

            }


            //weaponAnimation = GameObject.Find(weaponData.weaponNameFinder); //ak74 �������� ��� ak74�� �־���. �׷��� �ϴ� �ּ�ó�� �ʿ��ϸ� �Ŀ� �ִ´�
            reloadAnimation = weaponData.reloadAnimation; //�̸� ���� �ִϸ��̼� Ŭ�� �־���.

            //RecoilSettings
            recoverPercent = weaponData.recoverPercent;
            recoilUp = weaponData.recoilUp;
            recoilBack = weaponData.recoilBack;
            //recovering = weaponData.recovering; //���� ���� �ּ�ó��

            //Animation�� ������ �ִϸ��̼��� �߰�


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
            //����: UnassignedReferenceException: The variable camera of Weapon has not been assigned.
            //You probably need to assign the camera variable of the Weapon script in the inspector.
            //��... ���� ī�޶󿡼� ������ �ȵ��ݾ�..? -> ������ �÷��̾� �Ѿư��� �����Ǹ� �ʱ�ȭ
            //�׽�Ʈ�� ���� ī�޶� �ֱ� �ߴµ� Bullethole�ΰ� ������ �ȳ��Դ�.
            //Bullethole�� Lee�� EffectSpawner���� ����Ѵ�.
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
