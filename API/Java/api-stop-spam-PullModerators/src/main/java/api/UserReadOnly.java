package api;

import com.amazonaws.services.dynamodbv2.datamodeling.DynamoDBHashKey;
import com.amazonaws.services.dynamodbv2.datamodeling.DynamoDBTable;

@DynamoDBTable(tableName="Users")
public class UserReadOnly
{
    @DynamoDBHashKey(attributeName="Username")
    private String username;
       
    public String getUsername() 
    {
        return this.username;
    }

    public void setUsername(String username) 
    {
        this.username = username;
    }
    
    public UserReadOnly() 
    {
    	
    }
}