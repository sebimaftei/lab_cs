namespace ProductsManagement.Common.Logging;

public static class LogEvents
{
    public static int ProductCreationStarted = 2001;
    public static int ProductValidationFailed = 2002;
    public static int ProductCreationCompleted = 2003;
    public static int DatabaseOperationStarted = 2004;
    public static int DatabaseOperationCompleted = 2005;
    public static int CacheOperationPerformed = 2006;
    public static int SKUValidationPerformed = 2007;
    public static int StockValidationPerformed = 2008;
    public static int ProductValidationCompleted = 2009;
    public static int DatabaseOperationFailed = 2010;
}