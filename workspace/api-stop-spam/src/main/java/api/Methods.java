package api;

import com.amazonaws.regions.Region;
import com.amazonaws.regions.Regions;
import com.amazonaws.services.dynamodbv2.AmazonDynamoDBClient;
import com.amazonaws.services.dynamodbv2.datamodeling.DynamoDBMapper;
import com.amazonaws.services.dynamodbv2.datamodeling.DynamoDBScanExpression;
import com.amazonaws.services.lambda.runtime.Context;
import com.amazonaws.services.s3.AmazonS3Client;
import com.amazonaws.services.s3.model.ObjectMetadata;
import java.io.ByteArrayInputStream;
import java.util.ArrayList;
import java.util.List;
import java.util.Random;
import java.util.UUID;
import java.util.regex.Matcher;
import java.util.regex.Pattern;

import org.bytedeco.javacpp.*;
import static org.bytedeco.javacpp.lept.*;
import static org.bytedeco.javacpp.tesseract.*;


public class Methods 
{
	public Result<Integer> PushReport(SingleReport report, Context context) 
    {
        Result<Integer> result = new Result<Integer>();
        
        TessBaseAPI tessBaseAPI = new TessBaseAPI();
        
        result.setValue(tessBaseAPI.Init("src/main/resources/tessdata", "eng"));  
       
        return result;
    }
	
    /*public Result<String> PushReport(SingleReport report, Context context) 
    {
        Result<String> result = new Result<String>();
        
        try 
        {
            String id = UUID.randomUUID().toString();
            
            ByteArrayInputStream stream = new ByteArrayInputStream(report.getPhoto());
            
            ObjectMetadata meta = new ObjectMetadata();
            
            meta.setContentLength(report.getPhoto().length);
            
            meta.setContentType("image/jpeg");
            
            AmazonS3Client s3Client = new AmazonS3Client();
            
            s3Client.setRegion(Region.getRegion(Regions.EU_WEST_1));
            
            String fileName = id + "-0.jpg";
            
            s3Client.putObject("stop-spam/reports", fileName, stream, meta);
            
            AmazonDynamoDBClient dynamoDBClient = new AmazonDynamoDBClient();
            
            dynamoDBClient.setRegion(Region.getRegion(Regions.EU_WEST_1));
            
            DynamoDBMapper mapper = new DynamoDBMapper(dynamoDBClient);
            
            report.setId(id);
            
            List<String> photos = new ArrayList<String>();
            
            photos.add(fileName);

            report.setPhotos(photos);
            
            String phoneNumber = ScanPhoneNumber(report.getPhoto());
            
            if(phoneNumber != null)
            {
            	report.setPhoneNumber(phoneNumber);
            }

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
        catch (Exception e)
        {
            result.setHasError(true);
            
            result.setMessage(e.getMessage());
        }
        
        return result;
    }*/

    public Result<String> PushReportContinue(SingleReport report, Context context) 
    {
        Result<String> result = new Result<String>();
        
        try 
        {
            AmazonDynamoDBClient dynamoDBClient = new AmazonDynamoDBClient();
            
            dynamoDBClient.setRegion(Region.getRegion(Regions.EU_WEST_1));
            
            DynamoDBMapper mapper = new DynamoDBMapper(dynamoDBClient);
            
            Report savedReport = mapper.load(Report.class, report.getId());
            
            String id = savedReport.getId();
            
            ByteArrayInputStream stream = new ByteArrayInputStream(report.getPhoto());
            
            ObjectMetadata meta = new ObjectMetadata();
            
            meta.setContentLength(report.getPhoto().length);
            
            meta.setContentType("image/jpeg");
            
            AmazonS3Client s3Client = new AmazonS3Client();
            
            s3Client.setRegion(Region.getRegion(Regions.EU_WEST_1));
            
            String fileName = id + "-" + savedReport.getPhotos().size() + ".jpg";
            
            s3Client.putObject("stop-spam/reports", fileName, stream, meta);
            
            savedReport.getPhotos().add(fileName);
            
            String phoneNumber = ScanPhoneNumber(report.getPhoto());
            
            if(phoneNumber != null)
            {
            	savedReport.setPhoneNumber(phoneNumber);
            }
            
            mapper.save(savedReport);
            
            result.setValue(id);
            
            if(savedReport.getPhoneNumber() == null || savedReport.getPhoneNumber() == "")
            {
            	result.setIsSuccess(false);
            }
            else
            {
            	result.setIsSuccess(true);
            }
        }        
        catch (Exception e) 
        {
            result.setHasError(true);
            
            result.setMessage(e.getMessage());
        }

        return result;
    }

    public Result<String> PushModerator(ModeratorRegistration moderator, Context context) 
    {
        Result<String> result = new Result<String>();
        
        try 
        {
            AmazonDynamoDBClient dynamoDBClient = new AmazonDynamoDBClient();
            
            dynamoDBClient.setRegion(Region.getRegion(Regions.EU_WEST_1));
            
            DynamoDBMapper mapper = new DynamoDBMapper(dynamoDBClient);
            
            mapper.save(moderator);
            
            result.setValue(moderator.getId());
            
            result.setIsSuccess(true);
        }
        catch (Exception e)
        {
            result.setHasError(true);
            
            result.setMessage(e.getMessage());
        }
        
        return result;
    }

    public Result<Object> RemoveModerator(String id, Context context) 
    {
        Result<Object> result = new Result<Object>();
        
        try 
        {
            AmazonDynamoDBClient dynamoDBClient = new AmazonDynamoDBClient();
            
            dynamoDBClient.setRegion(Region.getRegion(Regions.EU_WEST_1));
            
            DynamoDBMapper mapper = new DynamoDBMapper(dynamoDBClient);
            
            Moderator moderator = new Moderator();
            
            moderator.setId(id);
            
            mapper.delete(moderator);
            
            result.setIsSuccess(true);
        }
        catch (Exception e) 
        {
            result.setHasError(true);
            
            result.setMessage(e.getMessage());
        }
        
        return result;
    }

    public Result<List<Moderator>> PullModerators(Context context) 
    {
        Result<List<Moderator>> result = new Result<List<Moderator>>();
        
        try 
        {
            AmazonDynamoDBClient dynamoDBClient = new AmazonDynamoDBClient();
            
            dynamoDBClient.setRegion(Region.getRegion(Regions.EU_WEST_1));
            
            result.setValue(new DynamoDBMapper(dynamoDBClient).scan(Moderator.class, new DynamoDBScanExpression()));
            
            result.setIsSuccess(true);
        }
        catch (Exception e) 
        {
            result.setHasError(true);
            
            result.setMessage(e.getMessage());
        }
        
        return result;
    }

    public Result<Ratings> PullRatings(Context context) 
    {
        Result<Ratings> result = new Result<Ratings>();
        
        try 
        {
            AmazonDynamoDBClient dynamoDBClient = new AmazonDynamoDBClient();
            
            dynamoDBClient.setRegion(Region.getRegion(Regions.EU_WEST_1));
            
            Ratings ratings = new Ratings();
            
            ratings.setTopUsers(new DynamoDBMapper(dynamoDBClient).scan(User.class, new DynamoDBScanExpression()));
           
            ratings.setTopSpammers(new DynamoDBMapper(dynamoDBClient).scan(Spammer.class, new DynamoDBScanExpression()));
            
            result.setValue(ratings);
            
            result.setIsSuccess(true);
        }
        catch (Exception e)
        {
            result.setHasError(true);
            
            result.setMessage(e.getMessage());
        }
        
        return result;
    }

    public Result<String> ConfirmReport(Report report, Context context) 
    {
         Result<String> result = new Result<String>();
         
         try 
         {
             AmazonDynamoDBClient dynamoDBClient = new AmazonDynamoDBClient();
             
             dynamoDBClient.setRegion(Region.getRegion(Regions.EU_WEST_1));
             
             DynamoDBMapper mapper = new DynamoDBMapper(dynamoDBClient);
             
             mapper.delete(report);
             
             result.setIsSuccess(true);
         }
         catch (Exception e) 
         {
             result.setHasError(true);
             
             result.setMessage(e.getMessage());
         }
         
         return result;
    }

    public Result<Object> RejectReport(String id, Context context) 
    {
    	 Result<Object> result = new Result<Object>();
         
         try 
         {
             AmazonDynamoDBClient dynamoDBClient = new AmazonDynamoDBClient();
             
             dynamoDBClient.setRegion(Region.getRegion(Regions.EU_WEST_1));
             
             DynamoDBMapper mapper = new DynamoDBMapper(dynamoDBClient);
             
             Report report = mapper.load(Report.class, id);
             
             mapper.delete(report);
             
             
             AmazonS3Client s3Client = new AmazonS3Client();
             
             s3Client.setRegion(Region.getRegion(Regions.EU_WEST_1));
             
             List<String> photos = report.getPhotos();
             
             if(photos != null)
             {
            	for (String photoName : photos) 
            	{
            		s3Client.deleteObject("stop-spam/reports", photoName);
				}
             }

             result.setIsSuccess(true);
         }
         catch (Exception e) 
         {
             result.setHasError(true);
             
             result.setMessage(e.getMessage());
         }
         
         return result;
    }

    public Result<List<Report>> PullReports(Context context) 
    {
        Result<List<Report>> result = new Result<List<Report>>();
        
        try 
        {
            AmazonDynamoDBClient dynamoDBClient = new AmazonDynamoDBClient();
            
            dynamoDBClient.setRegion(Region.getRegion(Regions.EU_WEST_1));
            
            result.setValue(new DynamoDBMapper(dynamoDBClient).scan(Report.class, new DynamoDBScanExpression()));
            
            result.setIsSuccess(true);
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
     
            if (tessBaseAPI.Init("src/main/resources/tessdata", "eng") != 0) 
            {
                System.err.println("Could not initialize tesseract.");
                
                System.exit(1);
            }

            PIX image = pixReadMem(imageBytes, imageBytes.length);
            
            tessBaseAPI.SetImage(image);
           

            outText = tessBaseAPI.GetUTF8Text();
            
            Pattern p = Pattern.compile("\\d{3}[-\\.\\s]\\d{2}[-\\.\\s]\\d{2}");
            
            Matcher m = p.matcher(outText.getString());
            
            while (m.find()) 
            {
            	String phoneNumber = m.group();
            	
            	if(phoneNumber != null && !phoneNumber.isEmpty())
            	{
            		result = phoneNumber;
            		
            		break;
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
}
