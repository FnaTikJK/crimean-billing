namespace API.Infrastructure;

public static class PhoneConverter
{
    public static string? ToPhoneWithoutRegMask(string? phoneNumberWithMask)
    {
        if (phoneNumberWithMask == null)
            return null;
        
        var len = phoneNumberWithMask.Length;
        if (len < 10)
            throw new Exception("Incorrect phone format");
        return phoneNumberWithMask.Substring(len - 10);
    }
}