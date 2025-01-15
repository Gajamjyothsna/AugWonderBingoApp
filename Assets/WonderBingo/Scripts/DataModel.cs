using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace WonderBingo
{
    public static class DataModel
    {
        [System.Serializable]
        public enum Continent
        {
            GreatWallOfChina,
            Chichenitza,
            Petra,
            MachuPichu,
            ChristTheRedeemer,
            Colosseum,
            TajMahal
        }

        [System.Serializable]
        public enum SubType
        {
            Terra,
            Chronos
        }

        [System.Serializable]
        public class SubTypeJsonFileName
        {
            public SubType _subType;
            public string _subTypeJsonFileName;
        }

        [System.Serializable]
        public class ContinentJsonClass
        {
            public Continent _continent;
            public List<SubTypeJsonFileName> _subTypeJsonFileNameList;
        }

        [System.Serializable]
        public class ContinentGridImageData
        {
            public Continent _continent;
            public List<SubContinentImageData> subContinentImageDatas;
        }

        [System.Serializable]
        public class GridImageData
        {
            public List<Sprite> _suContinentImage;
        }

        [System.Serializable]
        public class SubContinentImageData
        {
            public SubType _subType;
            public List<GridImageData> _gridImageDatas;
        }

        [System.Serializable]
        public class MemorabiliaModel
        {
            public Continent _continent;
            public Text _memorabiliaTMP;
            public int _memorabiliaCount;
            public Sprite _keychain;
            public Sprite _gridBG;
        }
    }
}
