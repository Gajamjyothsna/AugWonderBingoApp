using UnityEngine;
using System.Collections.Generic;
using System;
namespace WonderBingo
{
    [CreateAssetMenu(fileName = "NewContinentGridData", menuName = "ScriptableObjects/NewContinentGridData")]
    public class ContinentGridData : ScriptableObject
    {
        public List<Grid> grids;
    }

    [System.Serializable]
    public class Grid
    {
        public QuestionData[] questions;
    }

    [System.Serializable]
    public class QuestionData
    {
        public int index;
        public string question;
        public string answer;
        public bool questionalreadyShown;
        public string oneLiner;
    }
}
