package api;

import com.amazonaws.regions.Region;
import com.amazonaws.regions.Regions;
import com.amazonaws.services.dynamodbv2.AmazonDynamoDBClient;
import com.amazonaws.services.dynamodbv2.datamodeling.DynamoDBMapper;
import com.amazonaws.services.dynamodbv2.datamodeling.DynamoDBSaveExpression;
import com.amazonaws.services.dynamodbv2.model.ConditionalCheckFailedException;
import com.amazonaws.services.dynamodbv2.model.ExpectedAttributeValue;
import com.amazonaws.services.lambda.runtime.Context;
import java.util.Date;
import java.util.HashMap;
import java.util.Map;

public class Methods 
{
	public Result<Object> PushModerator(OptionModerator option, Context context) 
	{
		Result<Object> result = new Result<Object>();

		try 
		{
			AmazonDynamoDBClient dynamoDBClient = new AmazonDynamoDBClient();

			dynamoDBClient.setRegion(Region.getRegion(Regions.EU_WEST_1));

			DynamoDBMapper mapper = new DynamoDBMapper(dynamoDBClient);
		
			 if (TokenValitation(option, ADMINISTRATOR, mapper)) 
				{
			
			if (option != null && option.getModeratorUsername() != null && !option.getModeratorUsername().isEmpty() && option.getModeratorPassword() != null && !option.getModeratorPassword().isEmpty()) 
			{
				User user = new User();
				
				user.setUsername(option.getModeratorUsername());
				
				user.setPassword(option.getModeratorPassword());
				
			    user.setRole(MODERATOR);
			
			    user.setCreateDateTime(new Date().getTime());

			    DynamoDBSaveExpression expression = new DynamoDBSaveExpression();
			    
			    Map<String, ExpectedAttributeValue> expected = new HashMap<String, ExpectedAttributeValue>();
			    
			    expected.put("Username", new ExpectedAttributeValue(false));
			    
			    expression.setExpected(expected);

				mapper.save(user, expression);

				result.setIsSuccess(true);
			} 
				}
			 else 
				{
					result.setHasError(true);

					result.setErrorCode(203);
				}
		} 
		catch (ConditionalCheckFailedException e)
		{
			result.setHasError(true);

			result.setMessage(e.getMessage());
			
			result.setErrorCode(201);
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
