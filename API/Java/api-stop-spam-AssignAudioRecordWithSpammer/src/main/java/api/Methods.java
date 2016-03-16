package api;

import com.amazonaws.regions.Region;
import com.amazonaws.regions.Regions;
import com.amazonaws.services.dynamodbv2.AmazonDynamoDBClient;
import com.amazonaws.services.dynamodbv2.datamodeling.DynamoDBMapper;
import com.amazonaws.services.lambda.runtime.Context;
import com.amazonaws.services.s3.AmazonS3Client;
import com.amazonaws.services.s3.model.ObjectMetadata;
import java.io.ByteArrayInputStream;
import java.io.ByteArrayOutputStream;
import javax.xml.parsers.DocumentBuilder;
import javax.xml.parsers.DocumentBuilderFactory;
import javax.xml.transform.Source;
import javax.xml.transform.TransformerFactory;
import javax.xml.transform.dom.DOMSource;
import javax.xml.transform.stream.StreamResult;
import org.w3c.dom.Document;
import org.w3c.dom.Element;

public class Methods 
{
    public Result<String> AssignAudioRecordWithSpammer(OptionSpammer option, Context context) 
    {
        Result<String> result = new Result<String>();
        
        try 
        {
        	 AmazonDynamoDBClient dynamoDBClient = new AmazonDynamoDBClient();
             
             dynamoDBClient.setRegion(Region.getRegion(Regions.EU_WEST_1));
             
             DynamoDBMapper mapper = new DynamoDBMapper(dynamoDBClient);
        	
        	if (TokenValitation(option, ADMINISTRATOR, mapper, MODERATOR)) 
			{
        	    if(option.getId() != null && !option.getId().isEmpty())
        	    {
        	    	  Spammer savedSpammer = mapper.load(Spammer.class, option.getId());
        	          
        	    	  if(savedSpammer != null && option.getAudioRecord() != null && !option.getAudioRecord().isEmpty())
        	    	  {
        	    		  AmazonS3Client s3Client = new AmazonS3Client();
        	    		  
        	    		  s3Client.setRegion(Region.getRegion(Regions.EU_WEST_1));
        	    		  
        	    		  String audioRecordPath = "stop-spam/audio_records";
        	    		  
        	    		  String audioRecordUrl = s3Client.getResourceUrl("stop-spam", "audio_records/" + option.getAudioRecord());
        	    		  
        	    		  byte[] bytes = GetXmlWrapedAudioRecord(audioRecordUrl); 
        	    		  
        	    		   ByteArrayInputStream stream = new ByteArrayInputStream(bytes);
        	               
        	               ObjectMetadata meta = new ObjectMetadata();
        	               
        	               meta.setContentLength(bytes.length);
        	               
        	               meta.setContentType("text/xml");

        	               String fileName = option.getAudioRecord() + ".xml";
        	               
        	               s3Client.putObject(audioRecordPath, fileName, stream, meta);

        	              savedSpammer.setAudioRecord(option.getAudioRecord());
        	              
        	              mapper.save(savedSpammer);
        	              
        	              result.setIsSuccess(true);
        	    	  }   
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
    
    protected byte[] GetXmlWrapedAudioRecord(String audioRecordUrl)
    {
    	byte[] result = null;
		
		    	  DocumentBuilderFactory factory = DocumentBuilderFactory.newInstance();
		    	  
		    	  DocumentBuilder builder;
		    	  
				try 
				{
					builder = factory.newDocumentBuilder();
					
					 Document document = builder.newDocument();
	    		     
					 document.setXmlStandalone(true);
					 
			    	  Element rootElement = document.createElement("Response");
			    	  
			    	  document.appendChild(rootElement);
			    	  
			    	  Element company = document.createElement("Play");
			    	  
			    	  company.setTextContent(audioRecordUrl);

			    	  rootElement.appendChild(company);
			    	  
			    	  ByteArrayOutputStream outputStream = new ByteArrayOutputStream();
			    	  
			    	  Source xmlSource = new DOMSource(document);
			    	  
			    	  StreamResult outputTarget = new StreamResult(outputStream);

				      TransformerFactory.newInstance().newTransformer().transform(xmlSource, outputTarget);
				      
				      result = outputStream.toByteArray();

				} 
				catch (Exception e) 
				{
					
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
