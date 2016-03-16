package api;

import com.amazonaws.services.dynamodbv2.datamodeling.DynamoDBAttribute;
import com.amazonaws.services.dynamodbv2.datamodeling.DynamoDBHashKey;
import com.amazonaws.services.dynamodbv2.datamodeling.DynamoDBTable;

import java.util.List;

@DynamoDBTable(tableName="Reports")
public class Report 
{
    @DynamoDBHashKey(attributeName="Id")
    protected String id;
    
    @DynamoDBAttribute(attributeName="PhoneNumber")
    protected String phoneNumber;
    
    @DynamoDBAttribute(attributeName="Latitude")
    protected float latitude;
    
    @DynamoDBAttribute(attributeName="Longitude")
    protected float longitude;
    
    @DynamoDBAttribute(attributeName="Photos")
    protected List<String> photos;
    
    @DynamoDBAttribute(attributeName="CreateDateTime")
    protected long createDateTime;
    
    @DynamoDBAttribute(attributeName="IsConfirmed")
    protected int isConfirmed;
    
    @DynamoDBAttribute(attributeName="UserId")
    protected String userId;

    public String getId() 
    {
        return this.id;
    }

    public void setId(String id)
    {
        this.id = id;
    }

    public String getPhoneNumber() 
    {
        return this.phoneNumber;
    }

    public void setPhoneNumber(String phoneNumber) 
    {
        this.phoneNumber = phoneNumber;
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

    public List<String> getPhotos() 
    {
        return this.photos;
    }

    public void setPhotos(List<String> photos) 
    {
        this.photos = photos;
    }

    public long getCreateDateTime() 
    {
        return this.createDateTime;
    }

    public void setCreateDateTime(long createDateTime) 
    {
        this.createDateTime = createDateTime;
    }

    public int getIsConfirmed() 
    {
        return this.isConfirmed;
    }

    public void setIsConfirmed(int isConfirmed) 
    {
        this.isConfirmed = isConfirmed;
    }
    
    public String getUserId() 
    {
        return this.userId;
    }

    public void setUserId(String userId) 
    {
        this.userId = userId;
    }
}