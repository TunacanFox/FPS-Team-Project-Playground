using FPS.WeaponSO;
//using FPS.Datum;
using UnityEngine;

//enum �� �����͸� �̿��Ͽ� Ű ���� ��ġ�ϴ� �Լ��� ȣ�����ش�.
//ScriptableObject�� ����� CardDataSO�� enum������ ����
public class WeaponTypeManager : MonoBehaviour
{
    public GameObject weaponPrefab;//ī�� ������ ���� �⺻ ������
    Transform weaponSettingHandImageTransform;


    // ī���� ���� ȿ���� �����ϴ� �޼ҵ�
    public void ExecuteEffect(string weaponName)
    {
        //WeaponDataSO cardData = WeaponSettingAssets.instance[cardName]; //���߿� �����Ѵ�~
    }

    public void ActivateWeaponEffect(WeaponType effectType, Vector3 dragEndPosition) //ī��� ���� �� ���Ŷ� dragEndPosition ���ֵ� ����
    {
        //weaponData�� ���� ��ġ�ϴ� �޼��带 ȣ���Ѵ�.

        // ���⼭ cardData�� ����Ͽ� ī���� ���� ȿ���� �����մϴ�.
        // ���� ���, ī�尡 �����̸� Ư�� �ൿ�� �ϰų�, 
        // �� ī��� Ư�� ��ġ�� ���� �����ϴ� ���� ������ ������ �� �ֽ��ϴ�.
        switch (effectType)
        {
            case WeaponType.MainArm:
                Debug.Log("Acorn");
                GameObject cardInstance = Instantiate(weaponPrefab, dragEndPosition, Quaternion.identity); //ī�� �� �� �� �����ѰŶ� ���ֵ� ����
                //�̷� ������ ������ ��ȯ ����
                //Ȥ�� �Լ� �θ��簡
                break;

            case WeaponType.SideArm:
                Debug.Log("SideArm");
                break;

            case WeaponType.Melee:
                Debug.Log("Melee");
                break;

            case WeaponType.Explosive:
                Debug.Log("Explosive");
                break;

            case WeaponType.SpecialWeapon:
                Debug.Log("SpecialWeapon");
                break;

            case WeaponType.Grappling:
                Debug.Log("Grappling");
                break;


                // ��Ÿ ȿ�� ó��...
        }
    }

    #region weaponData�� ���� ȣ��Ǵ� �޼���
    private void ShotgunPallet(WeaponDataSO weaponData)
    {
        // ���� ó�� �Ӹ��� ���� �� ������ ���� ���
    }

    private void DestroyWall(WeaponDataSO cardDatweaponDataa)
    {
        // �� �μ��� ������ �����ε� ���� ���� ������ ������ ������ ���� ���
    }

    // ��Ÿ ī�� ȿ�� ���� �޼ҵ��...

    #endregion
}
