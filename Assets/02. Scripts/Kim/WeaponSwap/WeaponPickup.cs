using UnityEngine;
using FPS.WeaponSO;

//Todo
//��ũ���ͺ� ������Ʈ���� enum WeaponType�� string weaponName�� �޾Ƽ� �����ϴ� ������Ʈ. �׸��� ��� ���ܸԾ����� �˸��� ���� �����ո� ����
//��ȣ�ۿ� �ϸ� WeaponType�� �̿��ؼ� WeaponSwap�� WeaponList�� �� �� �ε����� ������ �����ϰ�, �� ������ ���� weaponName�� �־��ش�.

//+���1 ���� ��ü������ ������ ��� �ִ� ����� ������.
//+���2 ���� 1�� ���� ���� ��� �ִµ� 1�� ���� ���⸦ ��ü�ߴٸ� WeaponSwap() ����� �ٽ� �����ϰ� �Ѵ�.

namespace FPS.WeaponSwap
{
    public class WeaponPickup : MonoBehaviour
    {

        WeaponDataSO weaponData = WeaponSettingAssets.instance["weaponName"];




    }
}