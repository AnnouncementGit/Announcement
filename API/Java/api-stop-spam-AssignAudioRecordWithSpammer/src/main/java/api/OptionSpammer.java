package api;

public class OptionSpammer extends UserCredentials 
{
    private String id;
    
    private String audioRecord;

    public String getId() 
    {
        return this.id;
    }

    public void setId(String id) 
    {
        this.id = id;
    }
    
    public String getAudioRecord() 
    {
        return this.audioRecord;
    }

    public void setAudioRecord(String audioRecord) 
    {
        this.audioRecord = audioRecord;
    }
}