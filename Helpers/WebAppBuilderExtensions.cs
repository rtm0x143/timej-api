namespace TimejApi.Helpers
{
    public static class WebAppBuilderExtensions
    {
        public static string GetConnectionString(this WebApplicationBuilder builder)
        {
            builder.Configuration.AddEnvironmentVariables();
            var connection = builder.Environment.IsDevelopment()
                ? builder.Configuration.GetConnectionString("Default")
                : builder.Configuration.GetValue<string>("TIMEJ_DB_CONN");

            if (connection is not string)
                throw new ArgumentException("Failed to extract connection string from configuration;" +
                    "Expected [ConnectionStrings:Default] prop or 'TIMEJ_DB_CONN' env variable.");

            return connection;
        }
    }
}
