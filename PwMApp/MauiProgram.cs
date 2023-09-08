using Microsoft.Extensions.Logging;
namespace PwMApp
{
    public static class Cash 
    {
        public static List<Item> Items { get; set; }

        public class Item 
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }     
        }
    }
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });
#if DEBUG
    		builder.Logging.AddDebug();
#endif
            return builder.Build();
        }
    }
}
