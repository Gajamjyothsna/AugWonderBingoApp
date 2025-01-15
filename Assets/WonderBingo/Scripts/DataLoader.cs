using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.NetworkInformation;
using UnityEngine;

namespace WonderBingo
{
    public class DataLoader : MonoBehaviour
    {
        public List<DataModel.ContinentJsonClass> _continentJsonClassList;

        public ContinentGridData gridData; // Reference to the ScriptableObject

        public string _jsonPath;

        public void LoadData(int _continentIndex, int _subTypeIndex)
        {
            _jsonPath = _continentJsonClassList[_continentIndex]._subTypeJsonFileNameList[_subTypeIndex]._subTypeJsonFileName;
            Debug.Log("JsonPath" + _jsonPath);
            TextAsset jsonFile = Resources.Load<TextAsset>(_jsonPath);
            if (jsonFile != null)
            {
                string json = jsonFile.text;
                JsonUtility.FromJsonOverwrite(json, gridData);
                Debug.Log("Grid data loaded successfully!");
            }
            else
            {
                Debug.LogError("JSON file not found in Resources folder!");
            }
        }
    }

}
