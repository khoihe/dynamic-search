namespace Kernel.Lib.Constant;

public static class CacheKeys
{
    public static string FIXED_RATE = "fixed_rate_limit_{0}";
    public static string LEAKY_BUCKET = "leaky_bucket_rate_limit_{0}";

    public static string TOKEN_BUCKET = "token_bucket_rate_limit_{0}";
    public static string TOKEN_BUCKET_TOKENS = "token_bucket_rate_limit_{0}_tokens";
    public static string TOKEN_BUCKET_TIMESTAMP = "token_bucket_rate_limit_{0}_timestamp";
}