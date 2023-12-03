namespace FormulaOne.API.Config
{
    public class DbContextConfig
    {
        public int CommandTimeout { get; set; }
        public bool EnableDetailedErrors { get; set; }
        public bool EnableSensitiveDataLogging { get; set; }
    }
}
