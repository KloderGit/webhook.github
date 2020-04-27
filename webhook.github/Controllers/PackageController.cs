using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using webhook.github.DTO;
using webhook.github.Models;

namespace webhook.github.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PackageController : ControllerBase
    {
        private readonly ILogger<PackageController> logger;
        private readonly IConfiguration configuration;

        public PackageController(ILogger<PackageController> logger, IConfiguration configuration)
        {
            this.logger = logger;
            this.configuration = configuration;
        }

        [HttpPost]
        [Route("update")]
        public IActionResult AddOrUpdatePackage([FromBody]WebhookPackageUpdateDTO item)
        {
            var element = WebhookPackageUpdate.Create(item);

            logger.LogInformation($"Получен вебхук с обновлением образа {element.Title}, версия - {element.Version}");

            AddOrUpdatePackageHandler(element);

            return Ok();
        }

        private void AddOrUpdatePackageHandler(WebhookPackageUpdate item)
        {
            var packagesNamesForWatch = configuration.GetSection("Packages").GetChildren().Select(x=>x.Value);

            if (packagesNamesForWatch.Contains(item.Title) != true) return;

            string strCmdText = $"sudo docker pull {item.Repository}/{item.Title}:{item.Version}".ToLower();

            strCmdText.Bash();
        }
    }
}
