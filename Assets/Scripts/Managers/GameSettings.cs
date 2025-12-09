using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Manager/GameSettings")]

public class GameSettings : ScriptableObject
{
   [SerializeField]
   private string _gameVersion = "0.0.0";
   public string GameVersion {get { return _gameVersion;} }
   [SerializeField]
   private string _nickName = "Wizard";
   public string NickName
   {
       get
       {
           int value = Random.Range(0,100);
           return _nickName + value.ToString();
       }
   }
}
