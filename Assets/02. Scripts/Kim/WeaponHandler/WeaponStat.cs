using FPS.WeaponSO;
using UnityEngine;

namespace FPS.WeaponHandler
{
    public class WeaponStat : MonoBehaviour
    {
        //enum Ÿ���� WeaponEffectType weaponType �̰Ŵ� namespace�� �ҷ��ͼ� ���� �ȴ�.

        //�������� ������
        public string weaponName; //���� ����
        public float damage; //��
        public float range; //��Ÿ�
        public float fireRate; //�߻� �ֱ�
        public int magazineSize; //źâ ũ��
        public int totalMagazine; //�־����� �� źâ ū��

        public GameObject weaponPrefab;


        public void Setup(WeaponDataSO weaponData)
        {
            //�ѱ� ���� �ʱ�ȭ -> WeaponFactory���� �� ��ũ��Ʈ �ٿ��ְ� �ϰ�, �� �Լ� �ҷ��� �ʱ�ȭ�ϰ� �Ѵ�.
            weaponName = weaponData.weaponName;
            damage = weaponData.damage;
            range = weaponData.range;
            fireRate = weaponData.fireRate;
            magazineSize = weaponData.magazineNum;
            totalMagazine = weaponData.fullMagazine;

            weaponPrefab = weaponData.weaponPrefab;
        }

        public void TestPrint() //�� �� ���� üũ �� (���� �����غ��� �����Ϳ��� Ȯ���ص� �����ϳ�)
        {
            Debug.Log($"weaponName: {weaponName}");
            Debug.Log($"damage: {damage}");
            Debug.Log($"range: {range}");
            Debug.Log($"fireRate: {fireRate}");
            Debug.Log($"magazineSize: {magazineSize}");
            Debug.Log($"totalMagazine: {totalMagazine}");
        }

    }
}