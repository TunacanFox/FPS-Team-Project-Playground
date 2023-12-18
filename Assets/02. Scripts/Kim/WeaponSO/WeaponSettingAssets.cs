using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FPS.WeaponSO
{
    public class WeaponSettingAssets : MonoBehaviour
    {
        public static WeaponSettingAssets instance;

        //�׽�Ʈ��
        public List<string> GetAllWeaponNames()
        {
            return _weaponDataSOSettings.Keys.ToList();
        }

        //public WeaponDataSO this[string name] => _weaponDataSOSettings[name]; //Ű Ȯ���� ���� �Ʒ��� get �κ� �߰��� ������ ����
        [SerializeField] private Dictionary<string, WeaponDataSO> _weaponDataSOSettings;
        [SerializeField] private List<WeaponDataSO> _weaponDataSOList; //ScriptableObject���� ����Ǵ� ����Ʈ

        public WeaponDataSO this[string name] //this WeaponSettingAssets �̱���
        {
            get
            {
                Debug.Log("Accessing WeaponSettingAssets with key: " + name);
                if (_weaponDataSOSettings.ContainsKey(name)) //Weapon data not found for key: AAA 
                {
                    return _weaponDataSOSettings[name];
                }
                else
                {
                    Debug.LogError("Weapon data not found for key: " + name);
                    return null;
                }
            }
        }


        private void Awake() //�� �༮�� ������ ���� ���ư��� �ʱ�ȭ�� �����ľ߸� �Ѵ�.
        {
            //�̰��� Debug.Log���� �ѱⰡ �������� ���� ��� �Ѽ� Ȯ���ϴ� �뵵��

            //Debug.Log("WeaponSettingAssets.cs Awake has been Start");
            instance = this; //�����: instance �� nulll, _weaponDataSOSrttings�� null
            //Debug.Log("�ν��Ͻ�: " + instance); //����ϸ� WeaponSettingAssets�� ��µȴ�.

            _weaponDataSOSettings = new Dictionary<string, WeaponDataSO>(); // Count = 4, �� = null
            //_weaponDataSOSettings Count = 0~4�ǵ��� �� ���� foreach�� �� ����.  �̸��� �ٸ��� �� ����.
            foreach (var data in _weaponDataSOList)
            { //data = AAA, data.name = AAA, _weaponDataSOSettings.Count = 0
                _weaponDataSOSettings.Add(data.name, data); //_skillCastSettingList�� ��� �ϳ��ϳ��� ���⿡ Add��
                
                //Debug.Log("Added Weapon data to dictionary: " + data.name); //��ųʸ� Ű�� �����ϴ��� ����Ͽ� Ȯ��.
                //Debug.Log("_weaponDataSOSettings: " + _weaponDataSOSettings); //check

            }
            _weaponDataSOList = null; //���� ����
        }
    }


}