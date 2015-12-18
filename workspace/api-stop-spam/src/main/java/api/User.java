package api;

import com.amazonaws.services.dynamodbv2.datamodeling.DynamoDBAttribute;
import com.amazonaws.services.dynamodbv2.datamodeling.DynamoDBHashKey;
import com.amazonaws.services.dynamodbv2.datamodeling.DynamoDBTable;

@DynamoDBTable(tableName="Users")
public class User 
{
    protected String id;
    
    protected String username;

    @DynamoDBHashKey(attributeName="Id")
    public String getId() 
    {
        return this.id;
    }

    public void setId(String id) 
    {
        this.id = id;
    }

    @DynamoDBAttribute(attributeName="Username")
    public String getUsername() 
    {
        return this.username;
    }

    public void setUsername(String username) 
    {
        this.username = username;
    }

    public User() 
    {
    	
    }

    public User(String id, String username) 
    {
        this.id = id;
        
        this.username = username;
    }
}