namespace UserAuthentication
{
    // todo: refactor to config/azure appsettings overwrite
    public static class Constants
    {
        public const string Audience = "localhost:44359";
        public const string Issuer = Audience;
        public const string Secret = "development_secret_this_is_replaced_in_build";
    }
}
