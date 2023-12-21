using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FPS.WeaponSO
{
    public class WeaponSettingAssets : MonoBehaviour
    {
        public static WeaponSettingAssets instance;

        //public WeaponDataSO this[string name] => _weaponDataSOSettings[name]; //키 확인을 위해 아래에 get 부분 추가된 것으로 변경
        [SerializeField] private Dictionary<string, WeaponDataSO> _weaponDataSOSettings;
        [SerializeField] private List<WeaponDataSO> _weaponDataSOList; //ScriptableObject들이 저장되는 리스트. (실행되면 참조해제된다.)

        [SerializeField] private List<string> _weaponNames; //Test: 모든 무기의 이름들이 들어가는지 테스트 하기 위함

        public WeaponDataSO this[string name] //this WeaponSettingAssets 싱글톤
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

        //이곳의 Debug.Log들은 총기가 존재하지 않을 경우 켜서 확인하는 용도다
        private void Awake() //이 녀석이 무조건 먼저 돌아가서 초기화를 끝마쳐야만 한다.
        {
            
            //Debug.Log("WeaponSettingAssets.cs Awake has been Start");
            instance = this; //디버깅: instance 값 nulll, _weaponDataSOSrttings값 null
            //Debug.Log("인스턴스: " + instance); //출력하면 WeaponSettingAssets이 출력된다.

            _weaponDataSOSettings = new Dictionary<string, WeaponDataSO>(); // Count = 4, 값 = null
            //_weaponDataSOSettings Count = 0~4의도된 곳 까지 foreach문 잘 돈다.  이름도 다르고 잘 들어간다.
            foreach (var data in _weaponDataSOList)
            { //data = AAA, data.name = AAA, _weaponDataSOSettings.Count = 0
                _weaponDataSOSettings.Add(data.name, data); //_skillCastSettingList의 요소 하나하나를 여기에 Add함
                
                //Debug.Log("Added Weapon data to dictionary: " + data.name); //딕셔너리 키가 존재하는지 출력하여 확인.
                //Debug.Log("_weaponDataSOSettings: " + _weaponDataSOSettings); //check

            }

            //들어온 Scriptable Object들의 이름을 저장하는 리스트 초기화
            _weaponNames = new List<string>();
            foreach (var weaponData in _weaponDataSOList)
            {
                _weaponNames.Add(weaponData.name);
            }

            /*
            //WeaponNames 저장하는 리스트에 잘 들어갔는지 확인용
            foreach (var name in _weaponNames)
            {
                Debug.Log($"테스트: {name}");
            }
            */
            _weaponDataSOList = null; //다 사용했으니 참조 해제
        }

        //테스트용 모든 키 (무기 이름) 리스트에 반환해줌
        public List<string> GetWeaponNames()
        {
            return _weaponNames;
        }

    }
}