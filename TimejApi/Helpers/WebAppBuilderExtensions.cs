namespace TimejApi.Helpers
{
    public static class WebAppBuilderExtensions
    {
        public static string GetConnectionString(this WebApplicationBuilder builder)
        {
            builder.Configuration.AddEnvironmentVariables();
            var connection = builder.Configuration.GetValue<string>("TIMEJ_DB_CONN") 
                ?? builder.Configuration.GetConnectionString("Default");

            if (connection is not string)
                throw new ArgumentException("Failed to extract connection string from configuration;" +
                    "Expected 'TIMEJ_DB_CONN' env variable or [ConnectionStrings:Default] prop in settings.");

            return connection;
        }
    }
}
