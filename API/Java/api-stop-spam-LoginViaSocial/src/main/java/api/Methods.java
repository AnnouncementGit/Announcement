package api;

import java.util.Date;
import java.util.UUID;
import com.amazonaws.regions.Region;
import com.amazonaws.regions.Regions;
import com.amazonaws.services.dynamodbv2.AmazonDynamoDBClient;
import com.amazonaws.services.dynamodbv2.datamodeling.DynamoDBMapper;
import com.amazonaws.services.lambda.runtime.Context;

public class Methods 
{
	public Result<UserCredentials> LoginViaSocial(SocialUser user, Context context) 
    {
		  Result<UserCredentials> result = new Result<UserCredentials>();
	        
	        try 
	        {
	        	if (user != null && user.getUserId() != null && !user.getUserId().isEmpty() && user.getToken() != null && !user.getToken().isEmpty()) 
				{
	        		 AmazonDynamoDBClient dynamoDBClient = new AmazonDynamoDBClient();
	                 
	                 dynamoDBClient.setRegion(Region.getRegion(Regions.EU_WEST_1));
	                 
	                 DynamoDBMapper mapper = new DynamoDBMapper(dynamoDBClient);
	                 
	                 User savedUser = mapper.load(User.class, user.getUserId());
	                 
	                 if(savedUser != null)
	                 {
	                	 if(savedUser.getAccessToken() == null || savedUser.getAccessToken().isEmpty())
	                	 {
	                		 savedUser.setAccessToken(UUID.randomUUID().toString());
	                	 }
	                	 
	                	 mapper.save(savedUser);
	                 }
	                 else
	                 {
	                	    savedUser = new User();
	        				
	                	    savedUser.setUsername(user.getUserId());
	        				
	                	    savedUser.setDisplayName(user.getDisplayName());
	                	    
	                	    savedUser.setRole(USER);
	        			
	                	    savedUser.setAccessToken(UUID.randomUUID().toString());
	                		
	                	    savedUser.setCreateDateTime(new Date().getTime());
	                		
	                		mapper.save(savedUser);
	                 }
	                 
	                 UserCredentials credentials = new UserCredentials();
                	 
                	 credentials.setUsername(savedUser.getUsername());
                	 
                	 credentials.setRole(savedUser.getRole());
                	 
                	 credentials.setAccessToken(savedUser.getAccessToken());
                	 
                	 result.setValue(credentials);
                	 
                	 result.setIsSuccess(true);
				}
	        	 else
                 {
                	 result.setHasError(true);
         	            
         	         result.setErrorCode(202);
                 }
	        }
	        catch (Exception e)
	        {
	            result.setHasError(true);
	            
	            result.setMessage(e.getMessage());
	        }
	        
	        return result;
    }
	
	static final int USER = 2;
}