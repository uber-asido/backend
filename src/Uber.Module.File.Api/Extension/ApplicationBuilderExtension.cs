using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNetCore.Builder;
using System.IO;
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

            var uploadFile = builder.Action("UploadFile");
            uploadFile.Parameter<Stream>("file");
            uploadFile.ReturnsFromEntitySet<UploadHistory>(nameof(UploadHistory));
        });

        return app;
    }
}
