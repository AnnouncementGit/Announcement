package api;

import api.Spammer;
import api.User;
import java.util.List;

public class Ratings 
{
    private List<User> topUsers;
    
    private List<Spammer> topSpammers;

    public List<User> getTopUsers() 
    {
        return this.topUsers;
    }

    public void setTopUsers(List<User> topUsers) 
    {
        this.topUsers = topUsers;
    }

    public List<Spammer> getTopSpammers() 
    {
        return this.topSpammers;
    }

    public void setTopSpammers(List<Spammer> topSpammers) 
    {
        this.topSpammers = topSpammers;
    }

    public Ratings() 
    {
    	
    }

    public Ratings(List<User> topUsers, List<Spammer> topSpammers) 
    {
        this.topUsers = topUsers;
        
        this.topSpammers = topSpammers;
    }
}