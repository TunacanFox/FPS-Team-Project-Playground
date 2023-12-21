using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FPS.WeaponSO
{
    public class WeaponSettingAssets : MonoBehaviour
    {
        public static WeaponSettingAssets instance;

        //public WeaponDataSO this[string name] => _weaponDataSOSettings[name]; //Ű Ȯ���� ���� �Ʒ��� get �κ� �߰��� ������ ����
        [SerializeField] private Dictionary<string, WeaponDataSO> _weaponDataSOSettings;
        [SerializeField] private List<WeaponDataSO> _weaponDataSOList; //ScriptableObject���� ����Ǵ� ����Ʈ. (����Ǹ� ���������ȴ�.)

        [SerializeField] private List<string> _weaponNames; //Test: ��� ������ �̸����� ������ �׽�Ʈ �ϱ� ����

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

        //�̰��� Debug.Log���� �ѱⰡ �������� ���� ��� �Ѽ� Ȯ���ϴ� �뵵��
        private void Awake() //�� �༮�� ������ ���� ���ư��� �ʱ�ȭ�� �����ľ߸� �Ѵ�.
        {
            
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

            //���� Scriptable Object���� �̸��� �����ϴ� ����Ʈ �ʱ�ȭ
            _weaponNames = new List<string>();
            foreach (var weaponData in _weaponDataSOList)
            {
                _weaponNames.Add(weaponData.name);
            }

            /*
            //WeaponNames �����ϴ� ����Ʈ�� �� ������ Ȯ�ο�
            foreach (var name in _weaponNames)
            {
                Debug.Log($"�׽�Ʈ: {name}");
            }
            */
            _weaponDataSOList = null; //�� ��������� ���� ����
        }

        //�׽�Ʈ�� ��� Ű (���� �̸�) ����Ʈ�� ��ȯ����
        public List<string> GetWeaponNames()
        {
            return _weaponNames;
        }

    }
}