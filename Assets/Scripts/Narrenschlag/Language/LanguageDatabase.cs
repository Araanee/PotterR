using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace narrenschlag
{
    [CreateAssetMenu(fileName = "New Language Database", menuName = "Narrenschlag/Language Database")]
    public class LanguageDatabase : ScriptableObject
    {
        public List<Word> words = new List<Word>();
    }

    [System.Serializable]
    public class Word
    {
        public int ID;
        public List<string> langWord;
    }

    [System.Serializable]
    public enum Language
    {
        english, german, italian
    }
}