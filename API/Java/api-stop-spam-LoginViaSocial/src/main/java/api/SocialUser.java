package api;


public class SocialUser 
{
    private String userId;
    
    private String token;
    
    private String displayName;

    public String getUserId() 
    {
        return this.userId;
    }

    public void setUserId(String userId) 
    {
        this.userId = userId;
    }
    
    public String getToken() 
    {
        return this.token;
    }

    public void setToken(String token) 
    {
        this.token = token;
    }

    public String getDisplayName() 
    {
        return this.displayName;
    }

    public void setDisplayName(String displayName) 
    {
        this.displayName = displayName;
    }
}