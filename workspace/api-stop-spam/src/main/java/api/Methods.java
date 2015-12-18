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


public class Methods 
{
    public Result<String> PushReport(SingleReport report, Context context) 
    {
        Result<String> result = new Result<String>();
        
        try 
        {
            String id = UUID.randomUUID().toString();
            
            ByteArrayInputStream stream = new ByteArrayInputStream(report.getPhoto());
            
            ObjectMetadata meta = new ObjectMetadata();
            
            meta.setContentLength(report.getPhoto().length);
            
            meta.setContentType("image/png");
            
            AmazonS3Client s3Client = new AmazonS3Client();
            
            s3Client.setRegion(Region.getRegion(Regions.EU_WEST_1));
            
            String fileName = id + "-0.png";
            
            s3Client.putObject("stop-spam/reports", fileName, stream, meta);
            
            AmazonDynamoDBClient dynamoDBClient = new AmazonDynamoDBClient();
            
            dynamoDBClient.setRegion(Region.getRegion(Regions.EU_WEST_1));
            
            DynamoDBMapper mapper = new DynamoDBMapper(dynamoDBClient);
            
            report.setId(id);
            
            List<String> photos = new ArrayList<String>();
            
            photos.add(fileName);

            report.setPhotos(photos);
            
            Random random = new Random();
            
            if(random.nextBoolean())
            {
            	report.setPhoneNumber("111-11-11-11");
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
    }

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
            
            meta.setContentType("image/png");
            
            AmazonS3Client s3Client = new AmazonS3Client();
            
            s3Client.setRegion(Region.getRegion(Regions.EU_WEST_1));
            
            String fileName = id + "-" + savedReport.getPhotos().size() + ".png";
            
            s3Client.putObject("stop-spam/reports", fileName, stream, meta);
            
            savedReport.getPhotos().add(fileName);
            
            Random random = new Random();
            
            if(random.nextBoolean())
            {
            	report.setPhoneNumber("111-11-11-11");
            }
            
            mapper.save(savedReport);
            
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
}
