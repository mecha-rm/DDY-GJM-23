using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DDY_GJM_23
{
    // The results data.
    public class ResultsData : MonoBehaviour
    {
        // Determines if the player was alive when the game ended.
        public bool survived = false;

        // The amount of deaths the player had.
        public int deaths = -1;

        // Number of scraps.
        public int scrapsTotal = -1;

        // The amount of times the base was visited.
        public int baseVisits = -1;

        // Number of keys used.
        public int keysUsed = -1;

        // Number of health packs used (does not include base visits).
        public int healthItemsUsed = -1;

        // The session length.
        public float gameLength = -1;

        [Header("Goals")]

        // The scrap goal.
        public int scrapGoal = 0;

        // Gets set to 'true' if the scrap goal has been reached.
        public bool scrapGoalReached = false;

        [Header("Weapons")]
        // Got the slow gun.
        public bool gotGunSlow = false;

        // Got the mid (standard) gun.
        public bool gotGunMid = false;

        // Got the fast gun.
        public bool gotGunFast = false;

        // Got the land upgrade (faster movement speed).
        public bool gotRunPower = false;

        // Got the swim upgrdade (faster water movement + poison immunity).
        public bool gotSwimPower = false;
    }
}