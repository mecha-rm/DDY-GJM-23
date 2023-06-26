using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DDY_GJM_23
{
    // The gameplay manager.
    public class GameplayManager : MonoBehaviour
    {
        // The gameplay manager instance.
        private static GameplayManager instance;

        // Gets set to 'true' when the singleton is initialized.
        private bool initialized = false;

        // The player.
        public Player player;

        // The total number of scraps the player has created.
        public int scrapsTotal = 0;

        [Header("World")]

        // The game world.
        public World world;

        // The world camera.
        public WorldCamera worldCamera;

        // The home base of the game.
        public HomeBase homeBase;

        [Header("World/Item Drops")]

        // The weapon uses item prebab.
        public WeaponUsesItem weaponUsesItemPrefab;


        [Header("Canvas")]

        // The gameplay UI.
        public GameplayUI gameUI;

        // The dialogue for the game.
        public Tutorial tutorial;

        [Header("Other")]

        // The game timer (in seconds) - 5 Minutes (300 Seconds)
        public float timer = 300.0F;

        // The amoutn of time that has passed since the game has started.
        public float elapsedGameTime = 0.0F;

        // Pauses the timers if set to 'true'.
        public bool pausedTimers = false;

        // Pauses the game if set to 'true'.
        private bool pausedGame = false;

        // Constructor
        private GameplayManager()
        {
            // ...
        }

        // Awake is called when the script is being loaded
        void Awake()
        {
            // If the instance hasn't been set, set it to this object.
            if (instance == null)
            {
                instance = this;
            }
            // If the instance isn't this, destroy the game object.
            else if (instance != this)
            {
                Destroy(gameObject);
            }

            // Run code for initialization.
            if (!initialized)
            {
                initialized = true;
            }
        }

        // Start is called just before any of the Update methods is called the first time
        private void Start()
        {
            // Find the player.
            if (player == null)
                player = FindObjectOfType<Player>(true);


            // WORLD
            // Finds the world component.
            if (world == null)
                world = FindObjectOfType<World>(true);

            // Find the world camera.
            if (worldCamera == null)
                worldCamera = FindObjectOfType<WorldCamera>(true);

            // Find the home base.
            if (homeBase == null)
                homeBase = FindObjectOfType<HomeBase>(true);

            // UI
            // The game UI.
            if (gameUI == null)
                gameUI = FindObjectOfType<GameplayUI>(true);


            // Dialogue not set.
            if (tutorial == null)
            {
                tutorial = FindObjectOfType<Tutorial>(true);

                // Adds the component to the game manager.
                if (tutorial == null)
                    tutorial = gameObject.AddComponent<Tutorial>();
            }


            //// If the tutorial should be used.
            //if(GameSettings.Instance.useTutorial)
            //{
            //    OpenTextBox(tutorial.GetDebugText());
            //}
        }

        // Gets the instance.
        public static GameplayManager Instance
        {
            get
            {
                // Checks if the instance exists.
                if (instance == null)
                {
                    // Tries to find the instance.
                    instance = FindObjectOfType<GameplayManager>(true);


                    // The instance doesn't already exist.
                    if (instance == null)
                    {
                        // Generate the instance.
                        GameObject go = new GameObject("Gameplay Manager (singleton)");
                        instance = go.AddComponent<GameplayManager>();
                    }

                }

                // Return the instance.
                return instance;
            }
        }

        // Returns 'true' if the object has been initialized.
        public bool Initialized
        {
            get
            {
                return initialized;
            }
        }

        // GAME WORLD
        // Spawns an item at the provided position.
        public void SpawnItem(Vector3 itemPos)
        {
            // TODO: implement chance rate.

            // If the weapon item prefab isn't set.
            if (weaponUsesItemPrefab != null)
            {
                WeaponUsesItem item = Instantiate(weaponUsesItemPrefab);
                item.transform.position = itemPos;
            }
        }


        // WINDOWS

        // MAP
        // Checks if the map is open.
        public bool IsMapOpen()
        {
            return gameUI.IsMapOpen();
        }

        // Opens the game map.
        public void OpenMap()
        {
            gameUI.OpenMap();
        }

        // Closes the game map.
        public void CloseMap()
        {
            gameUI.CloseMap();
        }

        // Toggles the map.
        public void ToggleMap()
        {
            gameUI.ToggleMap();
        }

        // TUTORIAL //
        // Opens the text box.
        public void OpenTextBox(List<string> pages)
        {
            gameUI.OpenTextBox(pages);
        }

        // Closes the text box.
        // If 'clearPages' is true, the text box elements are cleared out.
        public void CloseTextBox(bool clearPages = false)
        {
            gameUI.CloseTextBox(clearPages);
        }


        // SETTINGS //
        // Opens the game settings.
        public void OpenSettings()
        {
            gameUI.OpenSettings();
        }

        // Close the game settings.
        public void CloseSettings()
        {
            gameUI.CloseSettings();
        }

        // TIME
        // Returns the game timer, formatted.
        public string GetTimerFormatted(bool roundUp = true)
        {
            // Gets the time and rounds it up to the nearest whole number.
            float time = (roundUp) ? Mathf.Ceil(timer) : timer;

            // Formats the time.
            string formatted = StringFormatter.FormatTime(time, false, true, false);

            // Returns the formatted time.
            return formatted;
        }


        // Called to end the game.
        protected void OnTimeOver()
        {
            // NOTE: if the player is in a base area when the timer is over, have it count.
            // TODO: implement time over effects.

            ToResultsScene();
        }

        // Sets the game to be paused.
        public void SetPausedGame(bool paused)
        {
            pausedGame = paused;

            // TODO: check for animations not bound by time scale.
            // Makes changes based on if the game is paused or not.
            if (pausedGame)
            {
                Time.timeScale = 0; // Paused
                player.enableInputs = false;

            }
            else
            {
                Time.timeScale = 1; // Normal
                player.enableInputs = true;
            }
        }

        // Called to end the game.
        public void ToResultsScene()
        {
            // ...

            // Checks if the player is in the base.
            bool inBase = homeBase.IsPlayerInBase();

            // If the player isn't in the base.
            if(!inBase)
            {
                // Grabs the current area.
                WorldArea currArea = world.GetCurrentArea();

                // Area was set.
                if (currArea != null)
                {
                    // If the player is in a white sector, then it counts as being safe.
                    inBase = currArea.sector == WorldArea.worldSector.white;
                }
            }

            // If the player is in the base, count the scraps they have on hand. 
            if (inBase)
            {
                scrapsTotal += player.scrapCount;
                player.scrapCount = 0;
            }
                

            // Creates the results data to be read in on the results screen.
            GameObject resultsObject = new GameObject("Results Data");
            DontDestroyOnLoad(resultsObject);
            ResultsData results = resultsObject.AddComponent<ResultsData>();

            // Sets if the player was alive or not.
            results.survived = inBase;

            // Set the number of deaths.
            results.deaths = player.deaths;

            // Scrap count.
            results.scrapsTotal = scrapsTotal;

            // Visits.
            results.baseVisits = homeBase.visits;

            // Keys used.
            results.keysUsed = player.keysUsed;

            // Heals used.
            results.healthItemsUsed = player.healsUsed;

            // Game length.
            results.gameLength = elapsedGameTime;

            // Weapons
            results.gotGunSlow = player.HasWeapon(player.gunSlow);
            results.gotGunMid = player.HasWeapon(player.gunMid);
            results.gotGunFast = player.HasWeapon(player.gunFast);
            results.gotRunPower = player.HasWeapon(player.runPower);
            results.gotSwimPower = player.HasWeapon(player.swimPower);

            // Loads the results scene.
            SceneManager.LoadScene("ResultsScene");
        }

        // Update is called once per frame
        void Update()
        {
            // The timer isn't paused.
            if(!pausedTimers)
            {
                // Time remaining.
                if(timer > 0.0F)
                {
                    // Reduce the timer.
                    timer -= Time.deltaTime;

                    // Time over.
                    if(timer <= 0.0F)
                    {
                        // Keep timer at 0.
                        timer = 0.0F;

                        // Call time over function.
                        OnTimeOver();
                    }
                }

                // Add to the game time.
                elapsedGameTime += Time.deltaTime;
            }
        }
    }
}