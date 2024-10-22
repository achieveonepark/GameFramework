using SQLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data
{
    private static SQLiteConnection _db;
    
    public static void SetDB(string path)
    {
        _db = new SQLiteConnection(path);
    }

    public static void CreateTable<T>()
    {
        _db.CreateTable<T>();
    }

    public static void DropTable<T>()
    {
        _db.DropTable<T>();
    }

    public static T Get<T> (object id) where T : new()
    {
        var data = _db.Get<T>(id);

        return data;
    }

    public static bool Exists(string tableName, object id)
    {
        string query = $"SELECT COUNT(1) FROM {tableName} WHERE ID = ?";
        int count = (int)_db.ExecuteScalar<object>(query, id);
        return count > 0;
    }
    
    public static bool TryGetValue<TValue, TKey>(string tableName, TKey id, out TValue result) where TValue : new()
    {
        string query = $"SELECT COUNT(1) FROM {tableName} WHERE ID = ?";
        int count = _db.ExecuteScalar<int>(query, id);
        if(count > 0)
        {
            result = Get<TValue>(id);
            return true;
        }
        result = default(TValue);
        return false;
    }

    public static void Insert<T> (T data) where T : new()
    {
        _db.InsertOrReplace(data);
    }
}
