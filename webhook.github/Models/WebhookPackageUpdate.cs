using webhook.github.DTO;

namespace webhook.github.Models
{
    public class WebhookPackageUpdate
    {
        public string Title { get; set; }
        public string Repository { get; set; }
        public string Version { get; set; }

        public static WebhookPackageUpdate Create(WebhookPackageUpdateDTO dto)
        {
            return new WebhookPackageUpdate
            {
                Title = dto.Package.Name,
                Repository = dto.Package.Registry.Url
                            .Substring(8).ToLower(),
                Version = dto.Package.PackageVersion.Version
            };
        }
    }
}