using UnityEngine;

[CreateAssetMenu(fileName = "CoreData", menuName = "Core Data")]
public class CoreData : ScriptableObject
{
    public int progress = 0;
    public bool SoundOn = true;

    public void ChangeFromSaveToCoreData(SaveData data)
    {
        progress = data.progress;
        SoundOn = data.SoundOn;
    }
}
