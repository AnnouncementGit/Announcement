package api;

import com.amazonaws.services.dynamodbv2.datamodeling.DynamoDBAttribute;
import com.amazonaws.services.dynamodbv2.datamodeling.DynamoDBHashKey;
import com.amazonaws.services.dynamodbv2.datamodeling.DynamoDBTable;

@DynamoDBTable(tableName="Users")
public class UserCredentials 
{
    @DynamoDBHashKey(attributeName="Username")
    private String username;
    
    @DynamoDBAttribute(attributeName="Role")
    private int role;
    
    @DynamoDBAttribute(attributeName="AccessToken")
    private String accessToken;

    public String getUsername() 
    {
        return this.username;
    }

    public void setUsername(String username) 
    {
        this.username = username;
    }
    
    public int getRole() 
    {
        return this.role;
    }

    public void setRole(int role) 
    {
        this.role = role;
    }
    
    public String getAccessToken() 
    {
        return this.accessToken;
    }

    public void setAccessToken(String accessToken) 
    {
        this.accessToken = accessToken;
    }
    
    public UserCredentials() 
    {
    	
    }
}