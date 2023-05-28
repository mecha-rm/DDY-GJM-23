using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DDY_GJM_23
{
    // A key item.
    public class KeyItem : WorldItem
    {
        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();

            // Id not set.
            if(id == itemId.none)
                id = itemId.key;
        }

        // Give the player the key.
        protected override void GiveItem()
        {
            // Get the player.
            Player player = GameplayManager.Instance.player;

            // Give the player the key.
            player.keyCount++;

            // Destroy the item.
            OnItemGet();
        }
    }
}