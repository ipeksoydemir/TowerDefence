using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using Tools.ObserverPattern;
using ToweDefence.Scenes.Game.BuildingManager;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class Store : MonoBehaviour,IObserver
{


    [SerializeField] private AssetReference storeItem;
    [SerializeField] private List<BuildingTypeSO> buildingTypeSOList;
    [SerializeField] private Transform content;
    [SerializeField] private TextMeshProUGUI balance;
    [SerializeField] private TextMeshProUGUI count;
    public void HandleNotification()
    {// para değişti
        balance.text = GameManager.Instance.Balance.ToString();
        //  throw new System.NotImplementedException();
    }
    private void OnEnable()
    {
        balance.text = GameManager.Instance.Balance.ToString();
    }
    private async Task Awake()
    {
        GameManager.Instance.Attach(this);
        var buildingBtnTemp = await storeItem.LoadAssetAsync<GameObject>().Task;
        if (storeItem == null)
        {
            Debug.LogError("Failed to load cell prefab.");
            return;
        }

        count.text = buildingTypeSOList.Count.ToString();
        int index = 0;
        foreach (var buildingTypeSo in buildingTypeSOList)
        {
            Transform storeBtnTransform = Instantiate(buildingBtnTemp.transform, content);
            StoreItem storeItem = storeBtnTransform.GetComponent<StoreItem>();
            storeItem.SetItem(buildingTypeSo);

            // storeda eklencek inventoryItemList.Add(inventoryItem);
            index++;
        }
        
    }
}
