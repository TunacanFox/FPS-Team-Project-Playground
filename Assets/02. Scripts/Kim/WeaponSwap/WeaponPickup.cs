using UnityEngine;
using FPS.WeaponSO;
using System.Collections.Generic;
using Unity.VisualScripting;

//Todo
//��ũ���ͺ� ������Ʈ���� enum WeaoinSlot�� string weaponName�� �޾Ƽ� �����ϴ� ������Ʈ. �׸��� ��� ���ܸԾ����� �˸��� ���� �����ո� ����
//��ȣ�ۿ� �ϸ� WeaponSlot�� �̿��ؼ� WeaponSwap�� WeaponList�� �� �� �ε����� ������ �����ϰ�, �� ������ ���� weaponName�� �־��ش�.

//+���1 ���� ��ü������ ������ ��� �ִ� ����� ������.
//+���2 ���� 1�� ���� ���� ��� �ִµ� 1�� ���� ���⸦ ��ü�ߴٸ� WeaponSwap() ����� �ٽ� �����ϰ� �Ѵ�.

/*
EŰ�� ������ �� 
������ �ش� ������Ʈ�� ���� �±�, Ȥ�� ���̾ DropedWeapon �̶��
�ѱ� �����͸� �а�, enumŸ�Կ� ���� �ش��ϴ� �迭 �ε������ٰ� �ѱ� ����

�׷��� EŰ�� ������ �� ���� ����ĳ��Ʈ�� ��� �Ǵ� ���ΰ�?
*/

namespace FPS.WeaponSO
{
    public class WeaponPickup : MonoBehaviour
    {
        public List<string> weaponNames; //��ũ���ͺ� ������Ʈ�� ��� �̸��� �����ϱ� ����
        public string thisObjectWeaponName;

        private void Start()
        {
            weaponNames = WeaponSettingAssets.instance.GetWeaponNames(); //��ũ���ͺ� ������Ʈ�� �̸��� ��� ����. �˾ƾ� ��ȯ����

        }

        public void ChangeWeapon(string weaponName)
        {

        }





    } //class
} //namespace