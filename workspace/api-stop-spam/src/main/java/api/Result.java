package api;

public class Result<T> 
{
    protected boolean isSuccess;
    
    protected T value;
    
    protected boolean hasError;
    
    protected String message;

    public boolean getHasError() 
    {
        return this.hasError;
    }

    public void setHasError(boolean hasError) 
    {
        this.hasError = hasError;
    }

    public String getMessage() 
    {
        return this.message;
    }

    public void setMessage(String message) 
    {
        this.message = message;
    }

    public boolean getIsSuccess()
    {
        return this.isSuccess;
    }

    public void setIsSuccess(boolean isSuccess) 
    {
        this.isSuccess = isSuccess;
    }

    public T getValue() 
    {
        return this.value;
    }

    public void setValue(T value) 
    {
        this.value = value;
    }
}