
using System.IO;
using UnityEditor.ShaderGraph.Serialization;
using UnityEngine;
using static UnityEngine.InputSystem.Controls.DiscreteButtonControl;
public static class Serializer
{
    public static void Save <T>(T value, string directory, string fileName)
    {
        if(!Directory.Exists(directory))
            Directory.CreateDirectory(directory);





        string fullPath = Path.Combine(directory, fileName);
        FileMode writeMode = File.Exists(fullPath) ? FileMode.Truncate : FileMode.Create;


         using FileStream stream = new(fullPath,writeMode);
         using StreamWriter writer = new(stream);

        string json = JsonUtility.ToJson(value);
        writer.Write(json);
        writer.Flush();
        Debug.Log($"Object was saved to {fullPath} successfully! as {json}");
    }

    public static T Load<T>(string directory, string fileName, T defaultValue)
    {
        if (!Directory.Exists(directory))
            Directory.CreateDirectory(directory);
        string fullPath = Path.Combine(directory, fileName);



        if (!File.Exists(fullPath))
            return defaultValue;


           
        FileMode readMode = FileMode.Open;
        using FileStream stream = new(fullPath, readMode);
        using StreamReader reader = new(stream);

       string json = reader.ReadToEnd();
        T value = JsonUtility.FromJson<T>(json);


        Debug.Log($"Object was successfully loaded from {fullPath} as {value}");
        return value;
    }
}