package api;

public class OptionReport  extends UserCredentials
{
	 protected String id;
	 
	 protected byte[] photo;
	 
	    protected float latitude;
	    
	    protected float longitude;
	    
	    protected String phoneNumber;
	    
	    public String getId() 
	    {
	        return this.id;
	    }

	    public void setId(String id)
	    {
	        this.id = id;
	    }

	    public String getPhoneNumber() 
	    {
	        return this.phoneNumber;
	    }

	    public void setPhoneNumber(String phoneNumber) 
	    {
	        this.phoneNumber = phoneNumber;
	    }
	    
	    public byte[] getPhoto() 
	    {
	        return this.photo;
	    }

	    public void setPhoto(byte[] photo) 
	    {
	        this.photo = photo;
	    }
	    
	    public float getLatitude() 
	    {
	        return this.latitude;
	    }

	    public void setLatitude(float latitude) 
	    {
	        this.latitude = latitude;
	    }

	    public float getLongitude() 
	    {
	        return this.longitude;
	    }

	    public void setLongitude(float longitude) 
	    {
	        this.longitude = longitude;
	    }

    public OptionReport() 
    {
    	
    }
}