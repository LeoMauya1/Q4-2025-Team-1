using Unity.VisualScripting;
using UnityEngine;



public class SaveDataController : MonoBehaviour
{


    public static SaveDataController Instance;
    public SaveDataAsset defaultData;
    private SavaData _currentData;
    public ref SavaData currentData => ref _currentData;

    public string filename;

    private void Awake()
    {
        Instance = this;
        Load();
    }


    private void OnDestroy()
    {
        save();
    }


    public void save()
    {
        Serializer.Save(_currentData, $"{Application.persistentDataPath}/SavaData", filename);
    }

    public void Load()
    {
        _currentData = Serializer.Load($"{Application.persistentDataPath}/SavaData",filename,defaultData.value);
    }
}