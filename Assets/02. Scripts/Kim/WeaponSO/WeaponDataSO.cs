using System;
using UnityEngine;

namespace FPS.WeaponSO
{
    public enum WeaponSlot
    {
        MainArm, //1�� ���� ���� ���� ����
        SubArm, //2�� ���� ���� ���� ������
        Melee, //3�� ���� ���� ���� ����
        Explosive, //4�� ���� ���߹�, ���̼���ź ���
        SpecialWeapon, //5�� ���� Ư������: ��ô���� ������, ��帧, ������ ������
        Grappling, //6�� ����: �θ��� �׷��ø� ��!!
    }

    [CreateAssetMenu(fileName = "Weapon", menuName = "WeaponSO")]
    public class WeaponDataSO : ScriptableObject
    {
        /// <summary>
        /// ��� ���: 
        /// Ÿ�Կ� ���� ��ġ�ϴ� �Լ��� ȣ���ϰ� �� �� �ִ�. ex) �׷��ø� �� �Լ� �ҷ�����, ���� ���� �ҷ�����,
        /// ��ô��, �� �߿� ���� ����ź�̸� ȭ�� ���� ��ȯ ����
        /// ���Ÿ� ���� �ƴϸ� ����
        /// </summary>

        [Header("WeaponSlot")]
        public WeaponSlot weaponSlot;

        [Header("WeaponName")]
        public string weaponName; //��ũ���ͺ� ������Ʈ�� �θ��� ���� �̸�
        public string weaponNameFinder;//Find�� �ϱ� ���� �̸�

        //�������� ������
        [Header("Elementry Variable")]
        public int damage; //��
        public float fireRate; //�߻� �ֱ�
        //Main Camera�� �˾Ƽ� Find�� Weapon.cs�� �־�����

        public float range; //��Ÿ�

        [Header("Ammo")]
        public int ammo; //�� �� �ִ� �Ѿ� ����
        public int fullMagazine; //�ѿ� ���� �ִ� �Ѿ� ����
        public int magazineNum; //źâ ����

        [Header("Prefab")]
        public GameObject weaponPrefab;

        [Header("VFX EFFECT")]
        public GameObject hitVFX;
        public float removeFireHole;

        [Header("Animation")]
        //public Animation animationForWeapon; //ak74 �������� ��� ak74�� �־���. �ش� �ѱ��� �������� �־�� �ϴ� �� �ϴ�.
        //�׷��� Weapon.cs���� Find�̿��ؼ� �־�����
        public AnimationClip reloadAnimation; //�̸� ���� �ִϸ��̼� Ŭ���� �ִ´�.

        //�ݵ�
        [Header("Recoil Settings")]
        public float recoverPercent;
        public float recoilUp;
        public float recoilBack;
        public bool recovering;

        // ��Ÿ �ʿ��� �ѱ� Ư���� �߰�...

    }
}