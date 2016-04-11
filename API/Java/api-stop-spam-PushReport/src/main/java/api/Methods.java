package api;

import com.amazonaws.regions.Region;
import com.amazonaws.regions.Regions;
import com.amazonaws.services.dynamodbv2.AmazonDynamoDBClient;
import com.amazonaws.services.dynamodbv2.datamodeling.DynamoDBMapper;
import com.amazonaws.services.lambda.runtime.Context;
import com.amazonaws.services.s3.AmazonS3Client;
import com.amazonaws.services.s3.model.ObjectMetadata;
import java.io.ByteArrayInputStream;
import java.util.ArrayList;
import java.util.Date;
import java.util.List;
import java.util.UUID;
import java.util.regex.Matcher;
import java.util.regex.Pattern;
import org.bytedeco.javacpp.*;
import org.bytedeco.javacpp.lept.PIX;
import org.bytedeco.javacpp.tesseract.TessBaseAPI;
import static org.bytedeco.javacpp.lept.*;

public class Methods 
{
    public Result<String> PushReport(OptionReport option, Context context) 
    {
        Result<String> result = new Result<String>();
        
        try 
        {
        	 AmazonDynamoDBClient dynamoDBClient = new AmazonDynamoDBClient();
             
             dynamoDBClient.setRegion(Region.getRegion(Regions.EU_WEST_1));
             
             DynamoDBMapper mapper = new DynamoDBMapper(dynamoDBClient);
        	
        	if (TokenValitation(option, USER, mapper)) 
			{
        	
            String id = UUID.randomUUID().toString();
            
            ByteArrayInputStream stream = new ByteArrayInputStream(option.getPhoto());
            
            ObjectMetadata meta = new ObjectMetadata();
            
            meta.setContentLength(option.getPhoto().length);
            
            meta.setContentType("image/jpeg");
            
            AmazonS3Client s3Client = new AmazonS3Client();
            
            s3Client.setRegion(Region.getRegion(Regions.EU_WEST_1));
            
            String fileName = id + "-0.jpg";
            
            s3Client.putObject("stop-spam/reports", fileName, stream, meta);
            
            Report report = new Report();
            
            report.setLatitude(option.getLatitude());
            
            report.setLongitude(option.getLongitude());
            
            report.setId(id);
            
            report.setUserId(option.getUsername());
            
            report.setIsConfirmed(NOTCONFIRMED);
            
            List<String> photos = new ArrayList<String>();
            
            photos.add(fileName);

            report.setPhotos(photos);
            
            String phoneNumber = ScanPhoneNumber(option.getPhoto());
            
            if(phoneNumber != null)
            {
            	report.setPhoneNumber(phoneNumber);
            }
            
            report.setCreateDateTime(new Date().getTime());

            mapper.save(report);
            
            result.setValue(id);
            
            if(report.getPhoneNumber() == null || report.getPhoneNumber() == "")
            {
            	result.setIsSuccess(false);
            }
            else
            {
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
    
    protected String ScanPhoneNumber(byte[] imageBytes)
    {
    	String result = null;
    	
    	if(imageBytes == null)
    	{
    		return null;
    	}
    	
    	try
    	{
    		BytePointer outText;

            TessBaseAPI tessBaseAPI = new TessBaseAPI();
     
            if (tessBaseAPI.Init(this.getClass().getResource("/tessdata").getPath(), "eng") != 0) 
            {
                System.err.println("Could not initialize tesseract.");
                
                System.exit(1);
            }

            PIX image = pixReadMem(imageBytes, imageBytes.length);
            
            tessBaseAPI.SetImage(image);
           

            outText = tessBaseAPI.GetUTF8Text();
            
			String[] patterns = {
					"\\+\\d{2}[-\\.\\s]*\\([-\\.\\s]*\\d{3}[-\\.\\s]*\\)[-\\.\\s]*\\d{3}[-\\.\\s]*\\d{2}[-\\.\\s]*\\d{2}",
					"\\d{2}[-\\.\\s]*\\([-\\.\\s]*\\d{3}[-\\.\\s]*\\)[-\\.\\s]*\\d{3}[-\\.\\s]*\\d{3}[-\\.\\s]*\\d{1}",
					"\\+\\d{2}[-\\.\\s]*\\d{3}[-\\.\\s]*\\d{3}[-\\.\\s]*\\d{3}[-\\.\\s]*\\d{1}",
					"\\+\\d{2}[-\\.\\s]*\\d{3}[-\\.\\s]*\\d{3}[-\\.\\s]*\\d{3}[-\\.\\s]*\\d{1}",
					"\\+\\d{2}[-\\.\\s]*\\([-\\.\\s]*\\d{3}[-\\.\\s]*\\)[-\\.\\s]*\\d{7}",
					"\\([-\\.\\s]*\\d{3}[-\\.\\s]*\\)[-\\.\\s]*\\d{3}[-\\.\\s]*\\d{2}[-\\.\\s]*\\d{2}",
					"\\d{3}[-\\.\\s]*\\d{3}[-\\.\\s]*\\d{3}[-\\.\\s]*\\d{1}",
					"\\d{3}[-\\.\\s]*\\d{3}[-\\.\\s]*\\d{2}[-\\.\\s]*\\d{2}",
					"\\+\\d{2}[-\\.\\s]*\\(\\d{3}\\)[-\\.\\s]*\\d{3}[-\\.\\s]*\\d{2}[-\\.\\s]*\\d{2}",
					"\\(\\d{3}\\)[-\\.\\s]*\\d{2}[-\\.\\s]*\\d{3}[-\\.\\s]*\\d{3}[-\\.\\s]*\\d{1}",
					"\\d{2}[-\\.\\s]*\\(\\d{3}\\)[-\\.\\s]*\\d{3}[-\\.\\s]*\\d{3}[-\\.\\s]*\\d{1}",
					"\\+\\d{2}\\(\\d{3}\\)[-\\.\\s]*\\d{3}[-\\.\\s]*\\d{3}[-\\.\\s]*\\d{1}",
					"\\+\\d{2}[-\\.\\s]*\\d{3}[-\\.\\s]*\\d{3}[-\\.\\s]*\\d{2}[-\\.\\s]*\\d{2}",
					"\\+\\d{2}[-\\.\\s]*\\d{3}[-\\.\\s]*\\d{3}[-\\.\\s]*\\d{3}[-\\.\\s]*\\d{1}",
					"\\(\\d{3}\\)[-\\.\\s]*\\d{3}[-\\.\\s]*\\d{2}[-\\.\\s]*\\d{2}",
					"\\(\\d{3}\\)[-\\.\\s]*\\d{2}[-\\.\\s]*\\d{3}[-\\.\\s]*\\d{2}",
					"\\(\\d{3}\\)[-\\.\\s]*\\d{3}[-\\.\\s]*\\d{4}",
					"\\d{3}[-\\.\\s]*\\d{3}[-\\.\\s]*\\d{3}[-\\.\\s]*\\d{1}",
					"\\d{3}[-\\.\\s]*\\d{3}[-\\.\\s]*\\d{2}[-\\.\\s]*\\d{2}",
					"\\d{3}[-\\.\\s]*\\d{3}[-\\.\\s]*\\d{2}[-\\.\\s]*\\d{2}", "\\d{10}",
					"\\d{3}[-\\.\\s]*\\d{2}[-\\.\\s]*\\d{2}", "\\d{7}" };
            

            String text = outText.getString();
            
            for (String pattern : patterns) 
            {
            	Pattern p = Pattern.compile(pattern);
            	
            	Matcher m = p.matcher(text);
            	
            	while (m.find()) 
                {
                	String phoneNumber = m.group();
                	
                	if(phoneNumber != null && !phoneNumber.isEmpty())
                	{
                		result = phoneNumber;
                		
                		tessBaseAPI.End();
                        
                        outText.deallocate();
                        
                        pixDestroy(image);
                		
                		return result;
                	}
                }	
			}
            
            tessBaseAPI.End();
            
            outText.deallocate();
            
            pixDestroy(image);
    	}
    	catch(Exception e)
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
    
    		static final int USER = 2;

    		static final int NOTCONFIRMED = -1;
}
