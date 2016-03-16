package api;

import com.amazonaws.services.dynamodbv2.datamodeling.DynamoDBAttribute;
import com.amazonaws.services.dynamodbv2.datamodeling.DynamoDBHashKey;
import com.amazonaws.services.dynamodbv2.datamodeling.DynamoDBTable;

@DynamoDBTable(tableName="Users")
public class User 
{
    @DynamoDBHashKey(attributeName="Username")
    private String username;
    
    @DynamoDBAttribute(attributeName="DisplayName")
    private String displayName;
    
    @DynamoDBAttribute(attributeName="Password")
    private String password;
    
    @DynamoDBAttribute(attributeName="CreateDateTime")
    private long createDateTime;
    
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
    
    public String getDisplayName() 
    {
        return this.displayName;
    }

    public void setDisplayName(String displayName) 
    {
        this.displayName = displayName;
    }
    
    public String getPassword() 
    {
        return this.password;
    }

    public void setPassword(String password) 
    {
        this.password = password;
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
    
    public long getCreateDateTime() 
    {
        return this.createDateTime;
    }

    public void setCreateDateTime(long createDateTime) 
    {
        this.createDateTime = createDateTime;
    }
}