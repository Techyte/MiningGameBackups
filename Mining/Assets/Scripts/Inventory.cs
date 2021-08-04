using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    #region Singleton
    //The inventory Singleton 
    public static Inventory instance;

    void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of Inventory was found!");
            return;
        }
        instance = this;
    }

    #endregion

    #region Variables
    //Variables
    public delegate void OnItemChenged();
    public OnItemChenged onItemChangedCallback;

    public int space = 20;

    public List<Item> items = new List<Item>();
    #endregion

    #region Adding items
    //Function to add items to the inventory (can be used anywhere)
    public bool Add(Item item)
    {
        if (items.Count >= space)
        {
            print("Not Enough Room");
            return false;
            
        }
        items.Add(item);

        return true;
    }
    #endregion

    #region Removing items
    //Function to remove items from the inventory (can be called from anywhere)
    public void Remove(Item item) 
    {
        items.Remove(item);

        if (onItemChangedCallback != null)
        {
            onItemChangedCallback.Invoke();
        }
        print(items);
    }
    #endregion
}
