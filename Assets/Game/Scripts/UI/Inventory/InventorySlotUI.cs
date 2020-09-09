﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.UI.Inventory
{
    public class InventorySlotUI : MonoBehaviour
    {
        [SerializeField] InventoryItemIcon icon = null;

        public int MaxAcceptable(Sprite item)
        {
            if (GetItem() == null)
            {
                return int.MaxValue;
            }
            return 0;
        }

        public void AddItems(Sprite item, int number)
        {
            icon.SetItem(item);
        }

        public Sprite GetItem()
        {
            return icon.GetItem();
        }

        public int GetNumber()
        {
            return 1;
        }

        public void RemoveItems(int number)
        {
            print(gameObject + " Remove Item" + icon.GetItem());
            icon.SetItem(null);
        }
    }
}
