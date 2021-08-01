using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    #region Singleton
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
    public delegate void OnItemChenged();
    public OnItemChenged onItemChangedCallback;

    public int space = 20;

    public List<Item> items = new List<Item>();
    #endregion

    #region Adding items
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
