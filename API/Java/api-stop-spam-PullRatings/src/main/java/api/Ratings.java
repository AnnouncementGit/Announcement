package api;

import java.util.List;

public class Ratings 
{
    private List<UserRating> topUsers;
    
    private List<SpammerShort> topSpammers;

    public List<UserRating> getTopUsers() 
    {
        return this.topUsers;
    }

    public void setTopUsers(List<UserRating> topUsers) 
    {
        this.topUsers = topUsers;
    }

    public List<SpammerShort> getTopSpammers() 
    {
        return this.topSpammers;
    }

    public void setTopSpammers(List<SpammerShort> topSpammers) 
    {
        this.topSpammers = topSpammers;
    }
}