using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DDY_GJM_23
{
    // A health item.
    public class HealthItem : WorldItem
    {
        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();

            id = itemId.health;
        }

        // Give the player the key.
        protected override void GiveItem()
        {
            // Get the player.
            Player player = GameplayManager.Instance.player;

            // Give the player the key.
            player.healCount++;

            // Destroy the item.
            OnItemGet();
        }
    }
}