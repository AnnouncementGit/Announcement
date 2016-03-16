package api;

import com.amazonaws.services.dynamodbv2.datamodeling.DynamoDBAttribute;
import com.amazonaws.services.dynamodbv2.datamodeling.DynamoDBHashKey;
import com.amazonaws.services.dynamodbv2.datamodeling.DynamoDBTable;

@DynamoDBTable(tableName="Spammers")
public class Spammer 
{
	@DynamoDBHashKey(attributeName="PhoneNumber")
    private String phoneNumber;
    
	@DynamoDBAttribute(attributeName="AudioRecord")
    private String audioRecord;
    
    @DynamoDBAttribute(attributeName="AllReports")
    private int allReports;
    
    @DynamoDBAttribute(attributeName="CreateDateTime")
    private long createDateTime;
    
    @DynamoDBAttribute(attributeName="Latitude")
    protected float latitude;
    
    @DynamoDBAttribute(attributeName="Longitude")
    protected float longitude;
    
    public String getPhoneNumber() 
    {
        return this.phoneNumber;
    }

    public void setPhoneNumber(String phoneNumber) 
    {
        this.phoneNumber = phoneNumber;
    }

    public String getAudioRecord()
    {
        return this.audioRecord;
    }

    public void setAudioRecord(String audioRecord) 
    {
        this.audioRecord = audioRecord;
    }

    public int getAllReports() 
    {
        return this.allReports;
    }

    public void setAllReports(int allReports) 
    {
        this.allReports = allReports;
    }
    
    public long getCreateDateTime() 
    {
        return this.createDateTime;
    }

    public void setCreateDateTime(long createDateTime) 
    {
        this.createDateTime = createDateTime;
    }
    
    public float getLatitude() 
    {
        return this.latitude;
    }

    public void setLatitude(float latitude) 
    {
        this.latitude = latitude;
    }

    public float getLongitude() 
    {
        return this.longitude;
    }

    public void setLongitude(float longitude) 
    {
        this.longitude = longitude;
    }
}