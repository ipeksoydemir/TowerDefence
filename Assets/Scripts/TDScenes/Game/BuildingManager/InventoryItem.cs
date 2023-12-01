using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using ToweDefence.Scenes.Game.BuildingManager;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] public Button button,rotateButton;
    [SerializeField] private TMP_Text count;
    [SerializeField] private GameObject selector;
    [SerializeField] private BuildingTypeSO buildingTypeItem;
    public static Action<BuildingTypeSO> RotateRequest;
    public void SetItem(BuildingTypeSO buildingType)
    {
        button.onClick.AddListener(ChangeActiveBuildingType);
        buildingTypeItem = buildingType;
        icon.sprite = buildingTypeItem.icon;
        count.text = buildingTypeItem.count.ToString();
        rotateButton.onClick.AddListener(SendRotateRequest);
        BuildingManager.CountDecrease += CountDecrease;
    }

    void ChangeActiveBuildingType()
    {
        BuildingManager.Instance.SetActiveBuildingType(buildingTypeItem);
    }

    private void SendRotateRequest()
    {
        icon.transform.Rotate(0, 0, -90);
        RotateRequest?.Invoke(buildingTypeItem);
    }

    private void OnEnable()
    {
        selector.SetActive(false);
      BuildingManager.ActiveBuildingTypeChanged += UpdateSelector;
    }

    public void UpdateSelector(BuildingTypeSO selected)
    {
        selector.SetActive(selected==buildingTypeItem);
     //   button.interactable=(selected != buildingTypeItem);
    }
    public BuildingTypeSO GetInventoryItemType()
    {
        return buildingTypeItem;
    }
    public void CountUpdated()
    {
        buildingTypeItem.count++;
        count.text = buildingTypeItem.count.ToString();
    }
    public void CountDecrease()
    {
        if(BuildingManager.Instance.GetActiveBuildingType()==buildingTypeItem)
        {
            buildingTypeItem.count--;
            count.text = buildingTypeItem.count.ToString();
           
        }
    }
}
