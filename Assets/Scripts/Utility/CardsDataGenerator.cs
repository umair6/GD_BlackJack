using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

public class CardsDataGenerator : EditorWindow
{
    private string spriteFolderPath = "Assets/Resources/Sprites/Cards"; // Change if needed
    private string outputFolderPath = "Assets/Resources/Data/Cards";
    

    [MenuItem("Tools/Generate CardData Assets")]
    public static void ShowWindow()
    {
        GetWindow<CardsDataGenerator>("CardsData Generator");
    }

    void OnGUI()
    {
        GUILayout.Label("CardsData Generator", EditorStyles.boldLabel);
        spriteFolderPath = EditorGUILayout.TextField("Sprite Folder", spriteFolderPath);
        outputFolderPath = EditorGUILayout.TextField("Output Folder", outputFolderPath);

        if (GUILayout.Button("Generate All CardData Assets"))
        {
            GenerateCardDataAssets();
        }
    }

    private void GenerateCardDataAssets()
    {
        if (!Directory.Exists(outputFolderPath))
        {
            Directory.CreateDirectory(outputFolderPath);
            AssetDatabase.Refresh();
        }

        string[] spriteGuids = AssetDatabase.FindAssets("t:Sprite", new[] { spriteFolderPath });
        int count = 0;

        foreach (string guid in spriteGuids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path);
            string fileName = Path.GetFileNameWithoutExtension(path);

            if (!TryParseCardInfo(fileName, out int score, out bool isAce))
            {
                Debug.LogWarning($"Failed to parse card: {fileName}");
                continue;
            }

            CardSO card = ScriptableObject.CreateInstance<CardSO>();
            card.cardSprite = sprite;
            card.cardValue = score;
            card.isAce = isAce;

            string assetPath = $"{outputFolderPath}/{fileName}.asset";
            AssetDatabase.CreateAsset(card, assetPath);
            count++;
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log($"Generated {count} CardData assets.");
    }

    private bool TryParseCardInfo(string fileName, out int score, out bool isAce)
    {
        score = 0; isAce = false;
        if (fileName.Length < 2) return false;
        string filteredName = fileName;
        filteredName = filteredName.Replace("cardHearts", "");
        filteredName = filteredName.Replace("cardSpades", "");
        filteredName = filteredName.Replace("cardDiamonds", "");
        filteredName = filteredName.Replace("cardClubs", "");

        // Parse value
        switch (filteredName)
        {
            case "2":
            case "3":
            case "4":
            case "5":
            case "6":
            case "7":
            case "8":
            case "9":
                {
                    score = int.Parse(filteredName);
                    break;
                }
            case "10":
            case "J":
            case "Q":
            case "K":
                {
                    score = 10;
                    break;
                }
            case "A":
                {
                    score = 11;
                    isAce = true;
                    break;
                }
            default: return false;
        }

        return true;
    }
}
