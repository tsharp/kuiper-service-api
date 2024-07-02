using Kuiper.Clustering.ServiceApi.Resources;
using Kuiper.Clustering.ServiceApi.Security;
using Kuiper.Clustering.ServiceApi.Storage;
using System;
using System.IO.Compression;
using System.Text;

namespace Kuiper.Clustering.ServiceApi.Middware.Configuration
{
    public class CaDiscoveryMiddleware : IMiddleware
    {
        protected readonly IKeyValueStore configStore;
        protected readonly KuiperEndpointConfiguration config;

        public CaDiscoveryMiddleware(IKeyValueStore configStore, KuiperEndpointConfiguration config)
        {
            this.config = config;
            this.configStore = configStore;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var caList = await configStore.ScanAsync<CertificateAuthority>("/security.kuiper-sys.com/v1/kuiper-system/certificateauthority");

            var certificates = caList
                .Where(ca => ca.Data.EnableDiscovery)
                .Select(ca => new PemCertificateData()
                {
                    FriendlyName = ca.Data.FriendlyName,
                    EncodedCertificate = ca.Data.Certificate
                });

            if (!certificates.Any())
            {
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                return;
            }

            if (context.GetRouteValue("pathInfo") is string pathInfo && 
                pathInfo.Equals("download", StringComparison.InvariantCultureIgnoreCase))
            {
                // Create Zip File
                using (var zipStream = new MemoryStream())
                {
                    using (var zip = new ZipArchive(zipStream, ZipArchiveMode.Create, true))
                    {
                        foreach (var ca in certificates)
                        {
                            var entry = zip.CreateEntry($"{ca.FriendlyName}.cer");

                            using (var entryStream = entry.Open())
                            {
                                await entryStream.WriteAsync(Encoding.ASCII.GetBytes(ca.PemCertificate));
                                entryStream.Flush();
                            }
                        }

                        zipStream.Flush();
                    }

                    zipStream.Position = 0;
                    var uniqueId = Guid.NewGuid().ToString().Replace("-", "");
                    context.Response.ContentType = "application/octet-stream";
                    context.Response.Headers.Add("Content-Disposition", $"attachment; filename=Kuiper.CaCertificates.{uniqueId}.zip");
                    context.Response.Headers.Add("Content-Length", zipStream.Length.ToString());

                    await zipStream.CopyToAsync(context.Response.Body);
                }

                return;
            }

            await context.Response.WriteAsJsonAsync(certificates);
        }
    }
}
