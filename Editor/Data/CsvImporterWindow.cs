using SQLite;
using System.IO;
using UnityEditor;
using UnityEngine;
using Gilzoide.SqliteAsset.Csv;

public class CsvImporterWindow : EditorWindow
{
    private static CsvImporterWindow _instance;
    private string _tableName;
    private string _csvContent;
    private string _csvPath;
    private DefaultAsset _dbAsset;

    [MenuItem("GameFramework/Data/CsvImporter")]
    static void ShowWindow()
    {
        _instance = GetWindow<CsvImporterWindow>("CSV Importer");
        _instance.minSize = _instance.maxSize = new Vector2(400, 300);
    }

    void OnEnable()
    {
        ValidateAssets();
    }

    private void OnGUI()
    {
        DrawFilePath();
        DrawCsvContent();
        DrawInsertButton();
    }

    private void DrawFilePath()
    {
        ValidateAssets();

        EditorGUI.BeginChangeCheck();
        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("SQLite DB", EditorStyles.boldLabel);
        _dbAsset = (DefaultAsset)EditorGUILayout.ObjectField("Database Asset", _dbAsset, typeof(DefaultAsset), false);

        EditorGUILayout.Space(5);
        using (new EditorGUILayout.HorizontalScope())
        {
            EditorGUILayout.LabelField("CSV File Path", GUILayout.Width(150));

            GUI.enabled = false;
            EditorGUILayout.TextField(_csvPath.Replace(Application.dataPath, ""));
            GUI.enabled = true;

            if (GUILayout.Button("Select"))
            {
                _csvPath = EditorUtility.OpenFilePanel("CSV File", Application.dataPath, "csv");
            }
        }
        _tableName = EditorGUILayout.TextField("Table Name", _tableName);
        if (EditorGUI.EndChangeCheck())
        {
            EditorPrefs.SetString("csvAssetPath", _csvPath);
            SaveAssetPath("dbAssetPath", _dbAsset);
        }
    }

    private void DrawCsvContent()
    {
        EditorGUILayout.Space(30);
        EditorGUILayout.LabelField("데이터 미리보기", EditorStyles.boldLabel);
        GUI.enabled = false;
        EditorGUILayout.TextField(_csvContent);
        GUI.enabled = true;
    }

    private void DrawInsertButton()
    {
        GUILayout.FlexibleSpace();
        GUI.enabled = string.IsNullOrEmpty(_csvPath) is false && _dbAsset != null && string.IsNullOrEmpty(_tableName) is false;
        if(GUILayout.Button("Insert!"))
        {
            var reader = File.OpenText(_csvPath);
            var dbAssetPath = AssetDatabase.GetAssetPath(_dbAsset);
            var db = new SQLiteConnection(dbAssetPath);
            db.ImportCsvToTable(_tableName, reader);

            EditorUtility.DisplayDialog("CSV Importer", $"{_tableName}이 db에 포함되었습니다.", "확인");
        }
        GUI.enabled = true;
    }

    private void SaveAssetPath(string key, UnityEngine.Object asset)
    {
        string path = AssetDatabase.GetAssetPath(asset);
        EditorPrefs.SetString(key, path);
    }

    private void ValidateAssets()
    {
        _csvPath ??= EditorPrefs.GetString("csvAssetPath", "");
        _dbAsset ??= AssetDatabase.LoadAssetAtPath<DefaultAsset>(EditorPrefs.GetString("dbAssetPath", ""));
    }
}