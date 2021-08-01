using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    #region Variables
    public Item item;
    public SpriteRenderer sr;
    #endregion

    #region Start
    void Start()
    {
        sr.sprite = item.texture;
    }
    #endregion

    #region Adding items to inv
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
             bool wasPickedUp = Inventory.instance.Add(item);
            if (wasPickedUp)
                Destroy(gameObject);
        }
    }
    #endregion
}
