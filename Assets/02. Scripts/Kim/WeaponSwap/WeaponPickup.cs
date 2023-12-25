using UnityEngine;
using System.Collections.Generic;
using FPS.WeaponSO;
using FPS.Lee.WeaponDetail; //Weapon Ŭ������ ����ϱ� ����
using FPS.WeaponHandler;

//Todo
//��ũ���ͺ� ������Ʈ���� enum WeaoinSlot�� string weaponName�� �޾Ƽ� �����ϴ� ������Ʈ. �׸��� ��� ���ܸԾ����� �˸��� ���� �����ո� ����
//��ȣ�ۿ� �ϸ� WeaponSlot�� �̿��ؼ� WeaponSwap�� WeaponList�� �� �� �ε����� ������ �����ϰ�, �� ������ ���� weaponName�� �־��ش�.

//+���1 ���� ��ü������ ������ ��� �ִ� ����� ������.
//+���2 ���� 1�� ���� ���� ��� �ִµ� 1�� ���� ���⸦ ��ü�ߴٸ� WeaponSwap() ����� �ٽ� �����ϰ� �Ѵ�.

//2Ʈ
/*
EŰ�� ������ �� 
������ �ش� ������Ʈ�� ���� �±�, Ȥ�� ���̾ DropedWeapon �̶��
�ѱ� �����͸� �а�, enumŸ�Կ� ���� �ش��ϴ� �迭 �ε������ٰ� �ѱ� ����

�׷��� EŰ�� ������ �� ���� ����ĳ��Ʈ�� ��� �Ǵ� ���ΰ�?
*/

//3Ʈ
/*
������ ����� ��ü�Ϸ��� 
1. weaponSlotArray[int index]�� �����͸� �����ͼ� Weapon temp�� �����ϰ�, 
2. ���� �� ������Ʈ�� �������� Weapon dropedWeapon�� ��ȯ�� ���� �׳༮���� �ʱ�ȭ�ϰ� ������Ѵ�.
3. �׸��� ������Ʈ�� �ӽ� ������ Weapon temp�� �����ͷ� �ٲ�����Ѵ�.
*/

//4Ʈ
/*
�׷����� Weapon dropedWeapon�� �����ʹ� ��� ó���Ұ���? 
WeaponFactory�� CreateWeapon���� �ѱ� ���� + �⺻ ������ ó���� Start���� �� �ؾ��ҵ�. + �ٴڿ� �ִ� ��ó�� ȸ���� ��ȸ�Ǹ� �ϱ�
�׷��� CreateWeapon�� �Ű������� weaponName�� �޾Ƽ� ������ش�.

*/

/// <summary> 
/// �ش� ��ũ��Ʈ �� ������ ������Ʈ ����Ϸ���
/// 1. ��ũ��Ʈ ������ ������Ʈ�� ���� ScriptableObject �ڱ�
/// 2. 
/// 
/// </summary>

namespace FPS.WeaponSwap
{
    public class WeaponPickup : MonoBehaviour
    {
        public List<string> weaponNames; //��ũ���ͺ� ������Ʈ�� ��� �̸��� �����ϱ� ���� -> �Ⱦ��Ű�����

        [SerializeField] private WeaponDataSO droppedItemSO; //ScriptableObject�� �����ϴ� ����. ���⼭ �̸� �̾ƿ���, �װɷ� Create�ϰ� �ϸ� ���� ������?
        public Weapon droppedWeapon;
        public Weapon temp_droppedWeapon;
        public WeaponSlot droppedWeapon_WeaponSlot;

        private void Start()
        {
            weaponNames = WeaponSettingAssets.instance.GetWeaponNames(); //��ũ���ͺ� ������Ʈ�� �̸��� ��� ����. �˾ƾ� ��ȯ�ϱ� ����. -> ���߰��� �ʿ������ ������
            //��� ���°� Ȯ������.
            
            droppedWeapon = WeaponFactory.CreateDropedWeapon(droppedItemSO.weaponName, this.gameObject); //���� ��ũ���ͺ� ������Ʈ�� �̸��� �� ��ũ��Ʈ �ִ� ������Ʈ ������� �����, �����ʹ� WeaponUISetup �Լ��ҷ��� �˾Ƽ� �ʱ�ȭ
            droppedWeapon_WeaponSlot = droppedItemSO.weaponSlot;
            temp_droppedWeapon = droppedWeapon;
        }

        //WeaponSwapController�� E�� ������ �� �ߵ��ǰ� �� �Լ�
        //DropedWeapon�� Slot�� �߿�����, �Ű������� �޴� �༮�� �߿����� �ʴ�.

        //�ϴ� �ӽ� Weapon�� ������ ���� DropedWeapon����, DropedWeapon�� �Ű����� �����ͷ� �ٲٰ�, ����� TempDropedItem ��ȯ 
        //

        //���� ���Ⱑ �� �� �������� ������.
        public WeaponSlot GetWeaponSlotData()
        {
            return droppedWeapon.weaponSlot;
        }

        //�� ������ ó���� �ϰ� ���⸦ �ٲ۴�.
        public Weapon ChangeWeapon(Weapon previousWeapon)
        {
            droppedWeapon = previousWeapon;

            return temp_droppedWeapon;
        }



    } //class
} //namespace