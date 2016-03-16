package api;

import com.amazonaws.services.dynamodbv2.datamodeling.DynamoDBAttribute;
import com.amazonaws.services.dynamodbv2.datamodeling.DynamoDBHashKey;
import com.amazonaws.services.dynamodbv2.datamodeling.DynamoDBTable;

@DynamoDBTable(tableName="TopSpammers")
public class SpammerShort
{
	@DynamoDBHashKey(attributeName="PhoneNumber")
    private String phoneNumber;

    @DynamoDBAttribute(attributeName="SpamCount")
    private int spamCount;
   
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

    public int getSpamCount() 
    {
        return this.spamCount;
    }

    public void setSpamCount(int spamCount) 
    {
        this.spamCount = spamCount;
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