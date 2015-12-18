package api;

import com.amazonaws.services.dynamodbv2.datamodeling.DynamoDBAttribute;
import com.amazonaws.services.dynamodbv2.datamodeling.DynamoDBHashKey;
import com.amazonaws.services.dynamodbv2.datamodeling.DynamoDBTable;
import java.util.List;

@DynamoDBTable(tableName="NotConfirmedReports")
public class Report 
{
    protected String id;
    
    protected String phoneNumber;
    
    protected float latitude;
    
    protected float longitude;
    
    protected List<String> photos;

    @DynamoDBHashKey(attributeName="Id")
    public String getId() 
    {
        return this.id;
    }

    public void setId(String id)
    {
        this.id = id;
    }

    @DynamoDBAttribute(attributeName="PhoneNumber")
    public String getPhoneNumber() 
    {
        return this.phoneNumber;
    }

    public void setPhoneNumber(String phoneNumber) 
    {
        this.phoneNumber = phoneNumber;
    }

    @DynamoDBAttribute(attributeName="Latitude")
    public float getLatitude() 
    {
        return this.latitude;
    }

    public void setLatitude(float latitude) 
    {
        this.latitude = latitude;
    }

    @DynamoDBAttribute(attributeName="Longitude")
    public float getLongitude() 
    {
        return this.longitude;
    }

    public void setLongitude(float longitude) 
    {
        this.longitude = longitude;
    }

    @DynamoDBAttribute(attributeName="Photos")
    public List<String> getPhotos() 
    {
        return this.photos;
    }

    public void setPhotos(List<String> photos) 
    {
        this.photos = photos;
    }

    public Report() 
    {
    	
    }

    public Report(String id, String phoneNumber, float latitude, float longitude, List<String> photos) 
    {
        this.id = id;
        
        this.phoneNumber = phoneNumber;
        
        this.latitude = latitude;
        
        this.longitude = longitude;
        
        this.photos = photos;
    }
}