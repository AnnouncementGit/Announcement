package api;

import com.amazonaws.services.dynamodbv2.datamodeling.DynamoDBAttribute;
import com.amazonaws.services.dynamodbv2.datamodeling.DynamoDBHashKey;
import com.amazonaws.services.dynamodbv2.datamodeling.DynamoDBTable;

@DynamoDBTable(tableName="Spammers")
public class Spammer 
{
    protected String id;
    
    protected String phoneNumber;
    
    protected String audioRecord;
    
    protected int reportsCount;

    public String getId() 
    {
        return this.id;
    }

    @DynamoDBHashKey(attributeName="Id")
    public void setId(String id) 
    {
        this.id = id;
    }

    @DynamoDBAttribute(attributeName="Username")
    public String getPhoneNumber() 
    {
        return this.phoneNumber;
    }

    public void setPhoneNumber(String phoneNumber) 
    {
        this.phoneNumber = phoneNumber;
    }

    @DynamoDBAttribute(attributeName="AudioRecord")
    public String getAudioRecord()
    {
        return this.audioRecord;
    }

    public void setAudioRecord(String audioRecord) 
    {
        this.audioRecord = audioRecord;
    }

    @DynamoDBAttribute(attributeName="ReportsCount")
    public int getReportsCount() 
    {
        return this.reportsCount;
    }

    public void setReportsCount(int reportsCount) 
    {
        this.reportsCount = reportsCount;
    }

    public Spammer() 
    {
    	
    }

    public Spammer(String id, String phoneNumber) 
    {
        this.id = id;
        
        this.phoneNumber = phoneNumber;
    }
}