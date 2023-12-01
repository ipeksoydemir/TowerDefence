
namespace ToweDefence.Scenes.Game.BuildingManager
{

    using System;
    using System.Collections;
    using System.Collections.Generic;
    using TMPro;
    using ToweDefence.Scenes.Game.BuildingManager;
    using UnityEngine;
    using UnityEngine.UI;

    public class StoreItem : MonoBehaviour
    {
        [SerializeField] private Image icon;
        [SerializeField] private TextMeshProUGUI title, price;
        [SerializeField] private Button buyButton;
        [SerializeField] private BuildingTypeSO buildingTypeItem;
        public static Action<BuildingTypeSO> OnBuySuccess;
        
        public void SetItem(BuildingTypeSO buildingType)
        {
            buildingTypeItem = buildingType;
            icon.sprite = buildingType.icon;
            price.text = buildingType.price.ToString();
            title.text = buildingType.title.ToString();
            buyButton.onClick.AddListener(SendBuyRequest);
        }
        private void SendBuyRequest()
        {
            
            if(GameManager.Instance.Balance >= buildingTypeItem.price)
            {
                OnBuySuccess?.Invoke(buildingTypeItem);
                GameManager.Instance.Balance -= buildingTypeItem.price;
                // para işlemleri , ses , anim vs
            }
            else
            {

                // parası yetmiyorsa yazı çıkmalı
            }


        }
    }

}