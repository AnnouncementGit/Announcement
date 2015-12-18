package api;

import api.Report;
import com.amazonaws.services.dynamodbv2.datamodeling.DynamoDBIgnore;

public class SingleReport extends Report 
{
    protected byte[] photo;

    @DynamoDBIgnore
    public byte[] getPhoto() 
    {
        return this.photo;
    }

    public void setPhoto(byte[] photo) 
    {
        this.photo = photo;
    }

    public SingleReport() 
    {
    	
    }

    public SingleReport(float latitude, float longitude, byte[] photo) 
    {
        this.latitude = latitude;
        
        this.longitude = longitude;
        
        this.photo = photo;
    }
}