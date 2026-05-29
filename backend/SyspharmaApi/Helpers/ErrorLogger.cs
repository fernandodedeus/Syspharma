using SyspharmaApi.Context;

namespace SyspharmaApi.Helpers
{
    public static class ErrorLogger
    {
        public static async Task Log(SyspharmaContext context, string title, string description)
        {
            try
            {
                await context.Errors.AddAsync(new Models.Error()
                {
                    Title = title,
                    Description = description,
                    ErrorAt = DateTime.UtcNow.AddHours(-3)
                });
                await context.SaveChangesAsync();
            }
            catch { }
        }
    }
}
