[System.Serializable]
public class SaveData
{
    public int progress;
    public bool SoundOn;

    public SaveData(CoreData data)
    {
        this.progress = data.progress;
        this.SoundOn = data.SoundOn;
    }
}
