package api;

import com.amazonaws.regions.Region;
import com.amazonaws.regions.Regions;
import com.amazonaws.services.dynamodbv2.AmazonDynamoDBClient;
import com.amazonaws.services.dynamodbv2.datamodeling.DynamoDBMapper;
import com.amazonaws.services.lambda.runtime.Context;

public class Methods 
{
    public Result<Object> RemoveModerator(OptionId option, Context context) 
    {
        Result<Object> result = new Result<Object>();
        
        try 
        {
            AmazonDynamoDBClient dynamoDBClient = new AmazonDynamoDBClient();
            
            dynamoDBClient.setRegion(Region.getRegion(Regions.EU_WEST_1));
            
            DynamoDBMapper mapper = new DynamoDBMapper(dynamoDBClient);
            
            if (TokenValitation(option, ADMINISTRATOR, mapper)) 
			{
            	if(option.getId() != null && !option.getId().isEmpty())
            	{
            		 User user = new User();
                     
                     user.setUsername(option.getId());
                 
                     mapper.delete(user);
                 
                     result.setIsSuccess(true);
            	}            	 
			} 
			else 
			{
				result.setHasError(true);

				result.setErrorCode(203);
			}
        }
        catch (Exception e) 
        {
            result.setHasError(true);
            
            result.setMessage(e.getMessage());
        }
        
        return result;
    }
    
	 protected boolean TokenValitation(UserCredentials user, int role, DynamoDBMapper mapper)
	    {
	    	return TokenValitation(user, role, mapper, role);
	    }
	    
	    protected boolean TokenValitation(UserCredentials user, int role, DynamoDBMapper mapper, int secondRole)
	    {
	      if(user == null || user.getAccessToken() == null || user.getAccessToken().isEmpty())
	      {
	    	  return false;
	      }
	      
	      try 
	      {
	    	  User savedUser = mapper.load(User.class, user.getUsername());
	    	  
	    	  if(savedUser != null && savedUser.getAccessToken() != null && savedUser.getAccessToken().equals(user.getAccessToken()) && (savedUser.getRole() == role || savedUser.getRole() == secondRole))
	    	  {
	    		  return true;
	    	  }   	  
	      }
	      catch (Exception e)
	      {
	    	  
	      }
	      
	      return false;
	    }
	
	static final int ADMINISTRATOR = 0;
	
	static final int MODERATOR = 1;
}
