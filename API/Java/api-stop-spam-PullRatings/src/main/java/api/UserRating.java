package api;

import com.amazonaws.services.dynamodbv2.datamodeling.DynamoDBAttribute;
import com.amazonaws.services.dynamodbv2.datamodeling.DynamoDBHashKey;
import com.amazonaws.services.dynamodbv2.datamodeling.DynamoDBTable;

@DynamoDBTable(tableName="TopUsers")
public class UserRating 
{    
    @DynamoDBHashKey(attributeName="Username")
    private String username;
    
    @DynamoDBAttribute(attributeName="DisplayName")
    private String displayName;
    
    @DynamoDBAttribute(attributeName="Reports")
    private int reports;
    
    @DynamoDBAttribute(attributeName="ConfirmedReports")
    private int confirmedReports;
   
    public String getUsername() 
    {
        return this.username;
    }

    public void setUsername(String username) 
    {
        this.username = username;
    }
    
    public int getReports() 
    {
        return this.reports;
    }

    public void setReports(int reports) 
    {
        this.reports = reports;
    }
    
    public int getConfirmedReports() 
    {
        return this.confirmedReports;
    }

    public void setConfirmedReports(int confirmedReports) 
    {
        this.confirmedReports = confirmedReports;
    }
    
    public String getDisplayName() 
    {
        return this.displayName;
    }

    public void setDisplayName(String displayName) 
    {
        this.displayName = displayName;
    }
}