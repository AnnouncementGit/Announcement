package api;

import com.amazonaws.regions.Region;
import com.amazonaws.regions.Regions;
import com.amazonaws.services.dynamodbv2.AmazonDynamoDBClient;
import com.amazonaws.services.dynamodbv2.datamodeling.DynamoDBMapper;
import com.amazonaws.services.lambda.runtime.Context;

import java.util.Date;

public class Methods 
{
    public Result<String> ConfirmReport(OptionReport option, Context context) 
    {
         Result<String> result = new Result<String>();
         
         try 
         {
        	 AmazonDynamoDBClient dynamoDBClient = new AmazonDynamoDBClient();
        	 
        	 dynamoDBClient.setRegion(Region.getRegion(Regions.EU_WEST_1));
             
             DynamoDBMapper mapper = new DynamoDBMapper(dynamoDBClient);
             
             if (TokenValitation(option, ADMINISTRATOR, mapper, MODERATOR)) 
  			{ 
             
             Report savedReport = mapper.load(Report.class, option.getId());
             
             savedReport.setPhoneNumber(option.getPhoneNumber());
             
             savedReport.setIsConfirmed(CONFIRMED);
             
             mapper.save(savedReport);
             
             Spammer spammer = mapper.load(Spammer.class, option.getPhoneNumber());
             
             if(spammer != null)
             {
            	 spammer.setAllReports(spammer.getAllReports() + 1);
            	 
            	 spammer.setLatitude(savedReport.getLatitude());
            	 
            	 spammer.setLongitude(savedReport.getLongitude());
            	 
            	 mapper.save(spammer);
             }
             else
             {
            	  spammer = new Spammer();
                  
            	  spammer.setPhoneNumber(option.getPhoneNumber());
            	  
            	  spammer.setCreateDateTime(new Date().getTime());
            	  
            	  spammer.setAllReports(1);
            	  
            	  spammer.setAudioRecord(DEFAULT_AUDIO);
            	  
            	  spammer.setLatitude(savedReport.getLatitude());
             	 
             	  spammer.setLongitude(savedReport.getLongitude());
            	  
                  mapper.save(spammer);
             }

             result.setIsSuccess(true);
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

    		static final int CONFIRMED = 1;

    		static final String DEFAULT_AUDIO = "default.mp3";
}
