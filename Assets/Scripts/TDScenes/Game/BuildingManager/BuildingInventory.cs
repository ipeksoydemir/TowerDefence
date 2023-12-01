using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using Tools.HSM;
using Tools.ObserverPattern;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

namespace ToweDefence.Scenes.Game.BuildingManager
{
    public class BuildingInventory : MonoBehaviour,IObserver
    {
       
        [SerializeField] private List<InventoryItem> inventoryItemList=new List<InventoryItem>();
        [SerializeField] private BuildingManager buildingManager;
        [SerializeField] private AssetReference inventoryItem;
        [SerializeField] private Transform content;
        private GameObject buildingBtnTemp;
       // public static Action<BuildingTypeSO> ActiveBuildingTypeChanged;
        [SerializeField] private Sprite[] buttonIconStatus;
        [SerializeField] private Image buttonImage;
        [SerializeField] private TextMeshProUGUI balance;
        [SerializeField] private TextMeshProUGUI health;
        private void OnEnable()
        {
            StoreItem.OnBuySuccess += HandleBuyAction;
            
        }
       
        public void ChangeStatus()
        {
           bool currentStatus= buildingManager.gameObject.activeSelf;
           buttonImage.sprite = buttonIconStatus[currentStatus?0:1];
           buildingManager.gameObject.SetActive(!currentStatus);
        }
        
        private void HandleBuyAction(BuildingTypeSO boughtItem)
        {
           
            
            var inventoryItem = inventoryItemList?.FirstOrDefault(item => item.GetInventoryItemType() == boughtItem);

            if (inventoryItem == null)
            {
                if (buildingBtnTemp == null)
                {
                    Debug.LogError("Failed to load cell prefab.");
                    return;
                }
                Transform inventoryBtnTransform = Instantiate(buildingBtnTemp.transform, content);
                inventoryItem= inventoryBtnTransform.GetComponent<InventoryItem>();
                inventoryItem.SetItem(boughtItem);
                inventoryItemList.Add(inventoryItem);
               
            }
            inventoryItem.CountUpdated();

        }
        private void Start()
        {
            balance.text = GameManager.Instance.Balance.ToString();
            health.text = GameManager.Instance.Health.ToString();
            ChangeStatus();
        }
     
        private async Task Awake()
        {
            GameManager.Instance.Attach(this);
         
            buildingBtnTemp = await inventoryItem.LoadAssetAsync<GameObject>().Task;
            if (buildingBtnTemp == null)
            {
                Debug.LogError("Failed to load cell prefab.");
                return;
            }
          
          
            int index = 0;
           
        //    ActiveBuildingTypeChanged?.Invoke(buildingManager.GetActiveBuildingType());
        }

        public void HandleNotification()
        {
            // para değişti
            balance.text = GameManager.Instance.Balance.ToString();
            health.text = GameManager.Instance.Health.ToString() ;
        }
    }


}
