using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using util;


namespace DDY_GJM_23
{
    // Loads an area from a file.
    public class AreaLoader : MonoBehaviour
    {
        // Loads the data on start.
        public bool loadOnStart = true;

        // Gives the area the values if this component is set.
        [Tooltip("Set the area if you want the program to fill in the values.")]
        public WorldArea area;

        // The world sector.
        public WorldArea.worldSector sector = WorldArea.worldSector.unknown;

        // The number of the area.
        public int areaNumber = -1;

        // The reference position for loaded content.
        [Tooltip("The offset of the tile positions.")]
        public Vector3 posOffset = new Vector3(-13.5F, 6.5F, 0);

        // The spacing for placing tiles.
        [Tooltip("Spacing for tiles [x = col, y = row]")]
        public Vector2 spacing = new Vector2(1, -1);

        [Header("Files")]

        // The file name (include the file extension).
        [Tooltip("The file name. Make sure to include the file extension (.csv).")]
        public string fileName = "";

        // The three folder paths that are used to check for files.
        // Do NOT have them end in a slash.
        [Tooltip("The primary search location. Include the slash at the end of the path.")]
        public string folderPath1 = "Assets/Resources/Data/Areas/";

        [Tooltip("The secondary search location. Include the slash at the end of the path.")]
        public string folderPath2 = "Resources/Data/Areas/";

        [Tooltip("The tertiary search location. Include the slash at the end of the path.")]
        public string folderPath3 = "Data/Areas/";


        [Header("Parents")]
        // The parent for the loaded tiles.
        public Transform tilesParent = null;

        [Header("Prefabs")]

        // GRASS
        public WorldTile grassFloorA;
        public WorldTile grassWallA;

        // METAL
        public WorldTile metalFloorA;
        public WorldTile metalWallA;

        // Start is called before the first frame update
        void Start()
        {
            // Gets the componnet if it's not set.
            if (area == null)
                area = GetComponent<WorldArea>();

            // Load the data on start.
            if (loadOnStart)
                LoadData();
        }

        // Loads the data from the set file.
        public bool LoadData()
        {
            // The data file being loaded.
            string file = "";
            string filePath = "";

            // Gets set to 'true' if the file exists at the provided path.
            bool exists = false;

            // Attempt 1 (Folder Path 1)
            if(File.Exists(folderPath1 + fileName))
            {
                filePath = folderPath1;
                exists = true;
            }
            else if (File.Exists(folderPath2 + fileName))
            {
                filePath = folderPath2;
                exists = true;
            }
            else if (File.Exists(folderPath3 + fileName))
            {
                filePath = folderPath3;
                exists = true;
            }

            // File doesn't exist.
            if(!exists)
            {
                Debug.LogError("The file does not exist at the specified path.");
                return false;
            }

            // Form the file.
            file = filePath + fileName;

            // Reads all lines in the provided file.
            string[] lines = File.ReadAllLines(@file);

            // The character for splitting elements.
            char splitChar = ',';

            // Goes through each line.
            for (int i = 0; i < lines.Length; i++)
            {
                // Grabs the current line and splits it by the split character.
                string[] entries = lines[i].Split(splitChar);

                // 0 - Area Code
                if (i == 0)
                {
                    // The area code.
                    string areaCode = entries[0];

                    // The sector and number.
                    string areaSectorStr = "";
                    string areaNumberStr = "";

                    // The sector and the number string.
                    if(areaCode.Length == 3) // Correct length.
                    {
                        // Gets the sector.
                        areaSectorStr = areaCode.Substring(0, 1);
                        sector = WorldArea.GetWorldSector(areaSectorStr[0]);

                        // Gets the area number.
                        areaNumberStr = areaCode.Substring(1);
                        areaNumber = int.Parse(areaNumberStr);

                        // Tries to parse to convert the string to an integer.
                        if(!int.TryParse(areaNumberStr, out areaNumber))
                        {
                            Debug.LogAssertion("Area number could not be read.");
                            areaNumber = 0;
                        }
                    }
                    else
                    {
                        sector = WorldArea.worldSector.unknown;
                        areaNumber = 0;
                    }
                }
                // 1 - Tiles
                else
                {
                    // Goes through each entry.
                    for(int j = 0; j < entries.Length; j++)
                    {
                        // Gets the tile code.
                        string tc = entries[j];

                        // If the tile code is empty, is set to "0" or "00A", nothing should be put here.
                        // So go onto the next entry.
                        if(tc.Length != 3 || tc == "00A")
                            continue;

                        // The first 2 characters are the number, and the third character is the type.
                        int number;
                        string type;

                        // Converts the tile number from a string to an int.
                        int.TryParse(tc.Substring(0, 2), out number);

                        // Gets the type as a string.
                        type = tc.Substring(2, 1);


                        // TILE LOADING //

                        // Instantiates the tile.
                        WorldTile newTile = InstantiateTile(number, type[0]);

                        // No tile was generated, so continue.
                        if (newTile == null)
                            continue;


                        // TILE POSITIONING //

                        // The tile position.
                        Vector3 tilePos = posOffset;
                        
                        // The row and column. Var 'col' is -1 since the first line was the area ID.
                        int row = i - 1;
                        int col = j;

                        // Calculate the tile position.
                        tilePos.x += spacing.x * col;
                        tilePos.y += spacing.y * row;

                        // Set the tile's position.
                        newTile.transform.position = tilePos;

                        // Give it the tiles parent.
                        newTile.transform.parent = tilesParent;
                    } 
                }
            }

            // Set the area information.
            // This doesn't appear to work if this function is called in the Start() function, as it...
            // Likely gets overwritten by other things.
            if(area != null)
            {
                area.sector = sector;
                area.areaNumber = areaNumber;
            }    

            // Data loaded successfully.
            return true;
        }

        // Instantiates a tile.
        public WorldTile InstantiateTile(int number, char type)
        {
            // The original tile.
            WorldTile origTile = null;

            // The instantiated tile.
            WorldTile copyTile;

            // Checking the tile number.
            switch(number)
            {
                case 1: // Grass Floor
                    // Checking the tile type.
                    switch (type)
                    {
                        case 'A':
                        case 'a':
                        default:
                            origTile = grassFloorA;
                            break;
                    }

                    break;

                case 2: // Grass Wall
                    // Checking the tile type.
                    switch (type)
                    {
                        case 'A':
                        case 'a':
                        default:
                            origTile = grassWallA;
                            break;
                    }

                    break;

                case 3: // Metal Floor
                    // Checking the tile type.
                    switch (type)
                    {
                        case 'A':
                        case 'a':
                        default:
                            origTile = metalFloorA;
                            break;
                    }
                    break;

                case 4: // Metal Wall
                    // Checking the tile type.
                    switch (type)
                    {
                        case 'A':
                        case 'a':
                        default:
                            origTile = metalWallA;
                            break;
                    }
                    break;
            }

            // Checks if the original tile was set or not.
            if(origTile != null) // Set
            {
                // Copies the tile and returns it.
                copyTile = Instantiate(origTile);
                return copyTile;
            }
            else // Not set.
            {
                return null;
            }
        }
    }
}