using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarSelection : MonoBehaviour
{ 
    public Text coinsT;
    [Header("Navigation Buttons")]
    [SerializeField]private Button previousButton;
    [SerializeField]private Button nextButton;

    [Header("Play/Buy Buttons")]
    [SerializeField] private Button play;
    [SerializeField] private Button buy;
    [SerializeField] private Text priceText;

    [Header("Car attributes")]
    [SerializeField]private int[] carPrices;
    private int currentCar;
   

    private void Awake()
    {
        coinsT.text=""+PlayerPrefs.GetInt("0",PlayerManager.numberofCoins);
        SelectCar(0);
        
    }

    private void SelectCar(int _index)
    {
        
        play.gameObject.SetActive(true);
        buy.gameObject.SetActive(false);
        previousButton.interactable = (_index!=0);
        nextButton.interactable=(_index!=transform.childCount-1);

        for (int i =0; i< transform.childCount;i++)
        {
            transform.GetChild(i).gameObject.SetActive(i == _index);
        }
        if(SaveManager.instance.carsUnlocked[_index])
        {
            play.gameObject.SetActive(true);
            buy.gameObject.SetActive(false);
        }
        else
        {
            play.gameObject.SetActive(false);
            buy.gameObject.SetActive(true);
            priceText.text=carPrices[_index]+"";
            buy.interactable=(SaveManager.instance.money>=carPrices[currentCar]);
        }
    }

    public void ChangeCar(int _change)
    {
        currentCar+=_change;
        if (currentCar > transform.childCount - 1)
            currentCar = 0;
        else if (currentCar < 0)
            currentCar = transform.childCount - 1;

        SaveManager.instance.currentCar = currentCar;
        SaveManager.instance.Save();
        SelectCar(currentCar);
    }

    public void buyCar()
    {
        SaveManager.instance.money-=carPrices[currentCar];
        SaveManager.instance.carsUnlocked[currentCar]=true;
        

    }
}
