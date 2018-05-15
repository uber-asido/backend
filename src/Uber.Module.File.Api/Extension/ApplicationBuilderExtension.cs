using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNetCore.Builder;
using Uber.Core.OData;
using Uber.Module.File.Abstraction.Model;

public static class ApplicationBuilderExtension
{
    public static IApplicationBuilder UseFileApi(this IApplicationBuilder app, ODataModelBuilder odataBuilder)
    {
        odataBuilder.AddNamespace("File", builder =>
        {
            builder.AddEnumType<UploadStatus>();

            var uploadHistory = builder.AddEntitySet<UploadHistory>();
            uploadHistory.EntityType.HasKey(e => e.Key);
        });

        return app;
    }
}
