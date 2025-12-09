using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace narrenschlag
{
    public class LanguageManager : MonoBehaviour
    {
        [Tooltip("The data has to come from SOMEWHERE... ^^")]
        public LanguageDatabase database;
        [Space]
        [Tooltip("Language is loaded via the 'PlayerPrefs'\n\nIf you want to change it in the editor use the 'Set Language' below!")]
        public Language curLanguage;

        //Array is converted into a dictionary what reduces the time and performance needed to find
        //the word after it has been set up. So it maybe takes a few frames when you change the language
        //in the menu but in the long run it saves a lot performance on runtime!
        private Dictionary<int, string> dic = new Dictionary<int, string>();
        public void UpdateDictionary()
        {
            dic.Clear();

            foreach (Word w in database.words)
                dic.Add(w.ID, w.langWord[(int)curLanguage]);
        }

        public string GetWord(int ID)
        {
            if (dic.ContainsKey(ID))
                return dic[ID];

            return null;
        }

        public void SetLanguage(Language l)
        {
            PlayerPrefs.SetInt("Language", (int)l);

            //Disable if you want that the language only updates after reloading or loading a new scene!
            UpdateLanguage();
        }

        public Language GetLanguage()
        {
            return (Language)PlayerPrefs.GetInt("Language");
        }

        public void UpdateLanguage()
        {
            bool updateDic = curLanguage != GetLanguage();

            curLanguage = GetLanguage();

            //Only update the dictionary if it is a different language
            if(updateDic)
                UpdateDictionary();
        }

        //Editor part where you can set the language in the editor for... debug stuff?
        #region SetLanguage
        [Header("Set Language")]
        public Language langToSet;
        public bool setLang;
        void OnDrawGizmos()
        {
            if (setLang)
            {
                setLang = false;
                SetLanguage(langToSet);
            }
        }
        #endregion

        //Creates a singleton so every script in the scene can easily access the script
        #region singleton
        public static LanguageManager singleton;
        void Awake()
        {
            singleton = this;
            UpdateLanguage();
        }
        #endregion

        //If you want to take a look at the old stuff of this code here it is
        #region legacy code (outdated because of performance)
        public string Legacy_GetWord(int ID)
        {
            for (int i = 0; i < database.words.Count; i++)
            {
                if (database.words[i].ID == ID)
                    return database.words[i].langWord[(int)curLanguage];
            }

            return null;
        }
        #endregion
    }
}