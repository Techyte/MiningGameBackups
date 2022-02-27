using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    #region Variables
    //Variables
    public Item item;
    public SpriteRenderer sr;
    #endregion

    #region Start
    //Start Function
    void Start()
    {
        sr.sprite = item.texture;
    }
    #endregion

    #region Adding items to inv
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //If it collids with the player then add it to the inventory
        if (collision.gameObject.tag == "Player")
        {
            bool wasPickedUp = Inventory.instance.Add(item);
            if (wasPickedUp)
                Destroy(gameObject);
        }
    }
    #endregion
}
