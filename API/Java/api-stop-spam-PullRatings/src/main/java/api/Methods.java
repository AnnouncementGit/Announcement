package api;

import java.util.List;

import com.amazonaws.regions.Region;
import com.amazonaws.regions.Regions;
import com.amazonaws.services.dynamodbv2.AmazonDynamoDBClient;
import com.amazonaws.services.dynamodbv2.datamodeling.DynamoDBMapper;
import com.amazonaws.services.dynamodbv2.datamodeling.DynamoDBScanExpression;
import com.amazonaws.services.lambda.runtime.Context;

public class Methods 
{
    public Result<Ratings> PullRatings(Context context) 
    {
        Result<Ratings> result = new Result<Ratings>();
        
        try 
       {
            AmazonDynamoDBClient dynamoDBClient = new AmazonDynamoDBClient();
            
           dynamoDBClient.setRegion(Region.getRegion(Regions.EU_WEST_1));
            
            Ratings ratings = new Ratings();
            
            ratings.setTopUsers(new DynamoDBMapper(dynamoDBClient).scan(UserRating.class, new DynamoDBScanExpression()));
           
            List<SpammerShort> spammers = new DynamoDBMapper(dynamoDBClient).scan(SpammerShort.class, new DynamoDBScanExpression());

            ratings.setTopSpammers(spammers);
            
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
}
