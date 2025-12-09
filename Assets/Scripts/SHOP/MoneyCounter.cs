using UnityEngine;
using UnityEngine.UI;

public class MoneyCounter : MonoBehaviour
{
    //public Text txt;
    public Text CoinsText;

    private void Awake()
    {     
        CoinsText.text=""+PlayerPrefs.GetInt("0",PlayerManager.numberofCoins);   
    }

    private void Update()
    {
        CoinsText.text=""+PlayerPrefs.GetInt("0",PlayerManager.numberofCoins);   
        //CoinsText.text=""+ SaveManager.instance.money;
    }
}
