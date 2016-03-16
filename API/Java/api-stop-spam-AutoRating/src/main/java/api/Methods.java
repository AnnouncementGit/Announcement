package api;

import java.util.ArrayList;
import java.util.Calendar;
import java.util.Collections;
import java.util.Comparator;
import java.util.HashMap;
import java.util.Iterator;
import java.util.LinkedHashMap;
import java.util.LinkedList;
import java.util.List;
import java.util.Map;
import com.amazonaws.regions.Region;
import com.amazonaws.regions.Regions;
import com.amazonaws.services.dynamodbv2.AmazonDynamoDBClient;
import com.amazonaws.services.dynamodbv2.datamodeling.DynamoDBMapper;
import com.amazonaws.services.dynamodbv2.datamodeling.DynamoDBScanExpression;
import com.amazonaws.services.dynamodbv2.model.AttributeValue;
import com.amazonaws.services.dynamodbv2.model.ComparisonOperator;
import com.amazonaws.services.dynamodbv2.model.Condition;
import com.amazonaws.services.lambda.runtime.Context;
import com.amazonaws.services.s3.AmazonS3Client;
import com.twilio.sdk.TwilioRestClient;
import com.twilio.sdk.TwilioRestException;
import com.twilio.sdk.resource.factory.CallFactory;
import com.twilio.sdk.resource.factory.MessageFactory;
import com.twilio.sdk.resource.instance.Call;

public class Methods 
{
	public void AutoRating(Context context) 
	{
	    HashMap<String, SpammerShort> reportsGroupedBySpammer = new HashMap<String, SpammerShort>();
		
		HashMap<String, UserRating> reportsGroupedByUser = new HashMap<String, UserRating>();

		 try 
	        {
			    
			    
			   Calendar today = Calendar.getInstance();
			
			    long currentDate = today.getTimeInMillis();
			    
			    today.add(Calendar.DATE, -PERIOD);
			    
			    long weekAgoDate = today.getTimeInMillis();
			    	
	            AmazonDynamoDBClient dynamoDBClient = new AmazonDynamoDBClient();
	     
	            dynamoDBClient.setRegion(Region.getRegion(Regions.EU_WEST_1));
	            
	            DynamoDBMapper mapper = new DynamoDBMapper(dynamoDBClient);
	            
					Condition condition = new Condition();

					condition.withComparisonOperator(ComparisonOperator.BETWEEN);

					AttributeValue[] attributes = new AttributeValue[2];
					
					attributes[0] = new AttributeValue().withN(String.valueOf(weekAgoDate));
					
					attributes[1] = new AttributeValue().withN(String.valueOf(currentDate));
					
					condition.withAttributeValueList(attributes);

					DynamoDBScanExpression expression = new DynamoDBScanExpression();

					expression.addFilterCondition("CreateDateTime", condition);

					List<Report> reports = mapper.scan(Report.class, expression);
					

					for(int row = 0; row < reports.size(); row++)
					{
						Report report = reports.get(row);
						
						if(!reportsGroupedByUser.containsKey(report.getUserId()))
						{
							UserRating user = new UserRating();
							
							user.setUsername(report.getUserId());
							
							user.setReports(1);
							
							if(report.getIsConfirmed() == CONFIRMED)
							{
							user.setConfirmedReports(1);
							}
							
							reportsGroupedByUser.put(report.getUserId(), user);
						}
						else
						{
							UserRating user = reportsGroupedByUser.get(report.getUserId());

							user.setReports(user.getReports() + 1);
							
							if(report.getIsConfirmed() == CONFIRMED)
							{
							user.setConfirmedReports(user.getConfirmedReports() + 1);
							}
						}
						
						if(report.getIsConfirmed() == CONFIRMED)
						{
						if(!reportsGroupedBySpammer.containsKey(report.getPhoneNumber()))
						{
							 SpammerShort spammer = new SpammerShort();
					           
					         spammer.setPhoneNumber(report.getPhoneNumber());
					           
					         spammer.setSpamCount(1);
					         
					         spammer.setLatitude(report.getLatitude());
								
						     spammer.setLongitude(report.getLongitude());
							
							 reportsGroupedBySpammer.put(report.getPhoneNumber(), spammer);
						}
						else
						{
							SpammerShort spammer = reportsGroupedBySpammer.get(report.getPhoneNumber());
							
							spammer.setLatitude(report.getLatitude());
							
							spammer.setLongitude(report.getLongitude());
							
							spammer.setSpamCount(spammer.getSpamCount() + 1);
						}
						}
					}
					
					List<UserRating> previousTopUsers = mapper.scan(UserRating.class,  new DynamoDBScanExpression());
					
					if(previousTopUsers != null && !previousTopUsers.isEmpty())
					{
						for (UserRating userRating : previousTopUsers) 
						{
							mapper.delete(userRating);
						}
					}
					
                    List<SpammerShort> previousTopSpammers = mapper.scan(SpammerShort.class,  new DynamoDBScanExpression());
					
                    if(previousTopSpammers != null && !previousTopSpammers.isEmpty())
					{
                    	for (SpammerShort spammer : previousTopSpammers) 
						{
							mapper.delete(spammer);
						}
					}

					List<SpammerShort> topSpammers = GetTopSpammers(reportsGroupedBySpammer);
				    
					if(topSpammers != null && !topSpammers.isEmpty())
				    {
						for (SpammerShort spammer : topSpammers) 
						{
							mapper.save(spammer);
						}
				    }
					
					List<UserRating> topUsers = GetTopUsers(reportsGroupedByUser);
					
					if(topUsers != null && !topUsers.isEmpty())
				    {
						for (UserRating user : topUsers) 
						{
							User savedUser = mapper.load(User.class, user.getUsername());
							
							user.setDisplayName(savedUser.getDisplayName());

							mapper.save(user);
						}
				    }
					
					if(topSpammers != null && !topSpammers.isEmpty())
				    {
						for (UserRating user : topUsers) 
						{
							User savedUser = mapper.load(User.class, user.getUsername());
							
							user.setDisplayName(savedUser.getDisplayName());

							mapper.save(user);
						}
				    }
					
					List<Spammer> spammersForCall = new ArrayList<Spammer>();
		
					for (SpammerShort spammer : topSpammers) 
					{
						spammersForCall.add(mapper.load(Spammer.class, spammer.getPhoneNumber()));
					}
					
					TwilioRestClient client = new TwilioRestClient(ACCOUNT_SID, AUTH_TOKEN);
		
		AmazonS3Client s3Client = new AmazonS3Client();
		  
		  s3Client.setRegion(Region.getRegion(Regions.EU_WEST_1));
		  
		if(spammersForCall != null && !spammersForCall.isEmpty())
	    {
			for (Spammer spammer : spammersForCall) 
			{
				Map<String, String> params = new HashMap<String, String>();

				params.put("To", spammer.getPhoneNumber());

				params.put("From", TWILIO_NUMBER);

				String audioRecordUrl = s3Client.getResourceUrl("stop-spam", "audio_records/" + spammer.getAudioRecord() + ".xml");
				
				params.put("Url", audioRecordUrl);
				
				params.put("Method", "GET");
		
				try 
				{
					CallFactory callFactory = client.getAccount().getCallFactory();

					Call call = callFactory.create(params);

					if(call != null)
					{					
					  call.getSid();
					}
				} 
				catch (TwilioRestException e) 
				{

				}
			}
	    }
	       }
	       catch (Exception e) 
	      {
	            
	    }
	}
	
	private static List<SpammerShort> GetTopSpammers(HashMap<String, SpammerShort> reportsGroupedBySpammer)
	{
		  reportsGroupedBySpammer = SortSpammers(reportsGroupedBySpammer);
		
	      Iterator iterator = reportsGroupedBySpammer.entrySet().iterator();
	      
	      List<SpammerShort> spammers = new ArrayList<SpammerShort>();
	      
	      while(iterator.hasNext()) 
	      {
	           Map.Entry<String, SpammerShort> entry = (Map.Entry<String, SpammerShort>)iterator.next();

	           spammers.add(entry.getValue());

	           if(spammers.size() >= 10)
	           {
	        	   break;
	           }
	      }
	      
	      return spammers;
	}
	
	private static List<UserRating> GetTopUsers(HashMap<String, UserRating> reportsGroupedByUser)
	{
		 reportsGroupedByUser = SortUsers(reportsGroupedByUser);
			
	      Iterator iterator1 = reportsGroupedByUser.entrySet().iterator();
	      
	      List<UserRating> users = new ArrayList<UserRating>();
	      
	      while(iterator1.hasNext()) 
	      {
	           Map.Entry<String, UserRating> entry = (Map.Entry<String, UserRating>)iterator1.next();

	           users.add(entry.getValue());
	           
	           System.out.println(entry.getValue().getUsername() + " Reports : " + entry.getValue().getReports() + " Confirmed Reports : " + entry.getValue().getConfirmedReports());
	           
	           if(users.size() >= 10)
	           {
	        	   break;
	           }
	      }
	      
	      return users;
	}
	
	 private static HashMap SortUsers(HashMap map) 
	 { 
	       List list = new LinkedList(map.entrySet());
	 
	       Collections.sort(list, new Comparator() 
	       {
	    	   public int compare(Object o1, Object o2) 
	            {
	    		   UserRating  user1 = ((Map.Entry<String, UserRating> )o1).getValue();
	            	
	               UserRating  user2 = ((Map.Entry<String, UserRating> )o2).getValue();

	               return -(user1.getConfirmedReports() - user2.getConfirmedReports());
	            }
	       });

	       HashMap sortedHashMap = new LinkedHashMap();
	       
	       for (Iterator it = list.iterator(); it.hasNext();) 
	       {
	              Map.Entry entry = (Map.Entry) it.next();
	              
	              sortedHashMap.put(entry.getKey(), entry.getValue());
	       } 
	       return sortedHashMap;
	  }
	 
	 private static HashMap SortSpammers(HashMap map) 
	 { 
	       List list = new LinkedList(map.entrySet());
	 
	       Collections.sort(list, new Comparator() 
	       {
	            public int compare(Object o1, Object o2) 
	            {
	            	SpammerShort  spammer1 = ((Map.Entry<String, SpammerShort> )o1).getValue();
	            	
	            	SpammerShort  spammer2 = ((Map.Entry<String, SpammerShort> )o2).getValue();

	               return -(spammer1.getSpamCount() - spammer2.getSpamCount());
	            }
	       });

	       HashMap sortedHashMap = new LinkedHashMap();
	       
	       for (Iterator it = list.iterator(); it.hasNext();) 
	       {
	              Map.Entry entry = (Map.Entry) it.next();
	              
	              sortedHashMap.put(entry.getKey(), entry.getValue());
	       } 
	       return sortedHashMap;
	  }

	public static final String ACCOUNT_SID = "AC3feba6670dbe8b00d757de9213318cea";

	public static final String AUTH_TOKEN = "cc88e3c492f77a2eda1611042eeef905";
	
	public static final String TWILIO_NUMBER= "+48718811019";
	
	public static final int PERIOD = 5;
	
	public static final int CONFIRMED = 1;
}
