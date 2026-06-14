# Curso .NET 8/9 - Macoratti
Repositório referente ao curso do Macoratti para ASP.Net do .NET 8/9.

Segue abaixo alguns códigos importantes e de fácil acesso:
## Extension Method para erros customizados:

Criamos uma classe de modelo ErrorDetails que vão conter as informações do erro:
```csharp
using System.Text.Json;

namespace ApiCatalogo.Models
{
    public class ErrorDetails
    {
        public int StatusCode { get; set; }
        public string? Message { get; set; }
        public string? StackTrace { get; set; }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
```

Depois criamos o Extension Method de **IApplicationBuilder**

```csharp
using System.Net;
using ApiCatalogo.Models;
using Microsoft.AspNetCore.Diagnostics;

namespace ApiCatalogo.Extensions
{
    public static class ApiExceptionMiddlewareExtensions
    {
        public static void ConfigureExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                   context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                   context.Response.ContentType = "application/json" ;

                   var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                   if(contextFeature != null)
                    {
                        await context.Response.WriteAsync(new ErrorDetails()
                        {
                           StatusCode = context.Response.StatusCode,
                           Message = contextFeature.Error.Message,
                           StackTrace = contextFeature.Error.StackTrace
                        }.ToString());
                    }
                });
            });
        }
    }
}
```

Após isso, chamamos **app.ConfigureExceptionHandler()** em nosso **Program.cs**

```csharp
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    app.ConfigureExceptionHandler(); //<- Nesta linha
}
```
