package api;

public class OptionModerator  extends UserCredentials
{
    private String moderatorUsername;
    
    private String moderatorPassword;

    public String getModeratorUsername() 
    {
        return this.moderatorUsername;
    }
    
    public void setModeratorUsername(String moderatorUsername) 
    {
        this.moderatorUsername = moderatorUsername;
    }
    
    public String getModeratorPassword() 
    {
        return this.moderatorPassword;
    }

    public void setModeratorPassword(String moderatorPassword) 
    {
        this.moderatorPassword = moderatorPassword;
    }

    public OptionModerator() 
    {
    	
    }
}