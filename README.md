# kr.bbon.AspNetCore Package

[![](https://img.shields.io/nuget/v/kr.bbon.AspNetCore)](https://www.nuget.org/packages/kr.bbon.AspNetCore) [![](https://img.shields.io/nuget/dt/kr.bbon.AspNetCore)](https://www.nuget.org/packages/kr.bbon.AspNetCore) [![publish to nuget](https://github.com/bbonkr/kr.bbon.AspNetCore/actions/workflows/dotnet.yml/badge.svg)](https://github.com/bbonkr/kr.bbon.AspNetCore/actions/workflows/dotnet.yml)

## π’ Overview

.NET 5 μμ API μΉ μμ©νλ‘κ·Έλ¨ νλ‘μ νΈ <small>(webapi ννλ¦Ώ κΈ°μ€)</small>λ₯Ό μμν  λ, λ°λ³΅μ μΌλ‘ μμ±νλ μ¬ν­μ ν¨ν€μ§λ‘ μ λ¦¬νμ΅λλ€.


## π Namespace

### kr.bbon.AspNetCore

μΉ μμ©νλ‘κ·Έλ¨μμ μ¬μ©λλ κΈ°λ₯κ³Ό νμ₯ λ©μλλ₯Ό ν¬ν¨ν©λλ€.

### kr.bbon.AspNetCore.Filters

ASP.NET Core Mvc Filtersλ₯Ό μ§μνλ κΈ°λ₯μ ν¬ν¨ν©λλ€.

### kr.bbon.AspNetCore.Mvc

ASP.NET Core MVCλ₯Ό μ§μνλ κΈ°λ₯μ ν¬ν¨ν©λλ€.

## π― Features

### Exception classes

#### HttpStatusException class


HTTP μμΈλ₯Ό νννκΈ° μν΄ μ¬μ©λ©λλ€.

> [kr.bbon.Core](https://github.com/bbonkr/kr.bbon.AspNetCore) ν¨ν€μ§λ‘ μ΄μ λμμ΅λλ€.

```csharp
// Exception handling in action method
try
{
    // ...
}
catch(HttpStatusException ex)
{
    return StatusCode(ex.StatusCode, ex.Message, ex.GetDetails());
}
catch(Exception ex)
{
    // ...
}
```

#### HttpStatusException<TDeatails> class

HTTP μμΈμ μμΈ μ λ³΄λ₯Ό νννκΈ° μν΄ μ¬μ©λ©λλ€.

> [kr.bbon.Core](https://github.com/bbonkr/kr.bbon.AspNetCore) ν¨ν€μ§λ‘ μ΄μ λμμ΅λλ€.

```csharp
throw new HttpStatusException(HttpStatusCode.BadRequest, new SomeDetails
{
    Id = "err111",
    Message = "AAA μμ±μ κ°μ΄ μ μκ° μλλλ€.",
});
```

#### SomethingWrongException

μ¬μ©μ μ μ μμΈλ₯Ό νννκΈ° μν΄ μ¬μ©ν©λλ€.

> [kr.bbon.Core](https://github.com/bbonkr/kr.bbon.AspNetCore) ν¨ν€μ§λ‘ μ΄μ λμμ΅λλ€.

```csharp
// Exception handling in action method 
try
{
    // ...
}
catch(SomethingWrongException ex)
{
    return StatusCode(HttpStatusCode.Forbidden, ex.Message, ex.GetDetails());
}
catch(Exception ex)
{
    // ...
}
```

#### SomethingWrongException<TDetails>

μ¬μ©μ μ μ μμΈμ μμΈ μ λ³΄λ₯Ό νννκΈ° μν΄ μ¬μ©ν©λλ€.

> [kr.bbon.Core](https://github.com/bbonkr/kr.bbon.AspNetCore) ν¨ν€μ§λ‘ μ΄μ λμμ΅λλ€.

```csharp
throw new SomethingWrongException("λ°μ΄ν°λ₯Ό μ²λ¦¬νμ§ λͺ»νμ΅λλ€.", new SomeDetails 
{
    Id = "err111",
    Message = "AAA μμ±μ κ°μ΄ μ μκ° μλλλ€.",
});
```

### Extension methods

#### IApplicationBuilder.UseSwaggerUIWithApiVersioning() method

IApplicationBuilder μΈν°νμ΄μ€μ UseSwaggerUIWithApiVersioning λ©μλλ₯Ό μΆκ°ν©λλ€.

Swagger λ₯Ό μ¬μ©νκΈ° μν΄ νμν μ½λ μ‘°κ°μ΄ μ λ¦¬λμ΄ μμ΅λλ€.

```csharp
// Configure() on Startup.cs
app.UseSwaggerUIWithApiVersioning();
```

#### Object.ToJson<T>() method

Object ν΄λμ€μ ToJson λ©μλλ₯Ό μΆκ°ν©λλ€.

κ°μ²΄μ μΈμ€ν΄μ€λ₯Ό JSON λ¬Έμμ΄λ‘ μ§λ ¬ννλ κΈ°λ₯μ μ κ³΅ν©λλ€.

> [kr.bbon.Core](https://github.com/bbonkr/kr.bbon.AspNetCore) ν¨ν€μ§λ‘ μ΄μ λμμ΅λλ€.

```csharp
var sample = getSampleModel();
sample.ToJson();
```

JsonSerializerOptions λ§€κ°λ³μκ° μ§μ λμ§ μμ κ²½μ° μλ κΈ°λ³Έκ°μ μ¬μ©ν©λλ€.

```csharp
var defaultOptions = new JsonSerializerOptions
{
    WriteIndented = true,
    Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
};
```

#### IServiceCollection.AddApiVersioningAndSwaggerGen() method

IServiceCollection μΈν°νμ΄μ€μ AddApiVersioningAndSwaggerGen λ©μλλ₯Ό μΆκ°ν©λλ€.

ApiVersioning, Swagger λ₯Ό μ¬μ©νκΈ° μν΄ νμν μ½λμ‘°κ°μ΄ μ λ¦¬λμ΄ μμ΅λλ€.


```csharp
// ConfigureService() on Startup.cs
var defaultApiVersion = new ApiVersion(1, 0);
services.Configure<AppOptions>(Configuration.GetSection(AppOptions.Name));

services.AddApiVersioningAndSwaggerGen(defaultApiVersion);
```

### Filter classes

#### ApiExceptionHandlerFilter class

μ»¨νΈλ‘€λ¬μμ μ²λ¦¬λμ§ μμ μμΈκ° λ°μνλ©΄ μμΈλ₯Ό μ²λ¦¬νλ νν°λ₯Ό μ κ³΅ν©λλ€.

μμΈ μ²λ¦¬ νν°λ₯Ό μ»¨νΈλ‘€λ¬μμ μ¬μ©νλ €λ©΄ μλμ κ°μ΄ ν΄λμ€ νΉμ±μΌλ‘ μΆκ°ν©λλ€.

```csharp
[ApiExceptionHandlerFilter]
// ...
public class SomeController : ApiControllerBase 
{
    // ...
}
```

μμΈ μ²λ¦¬ νν°λ₯Ό μ‘μμμ μ¬μ©νλ €λ©΄ μλμ κ°μ΄ ν΄λμ€ νΉμ±μΌλ‘ μΆκ°ν©λλ€.

```csharp
[HttpGet]
[ApiExceptionHandlerFilter]
public IActionResult SomeAction()
{
    // ...
}
```

μμΈ μ²λ¦¬ νν°λ₯Ό μ μ­μΌλ‘ μ¬μ©νλ €λ©΄ μλμ κ°μ΄ κ΅¬μ±ν©λλ€.

```csharp
// ConfigureService() on Startup.cs
services.AddControllers(options => 
{
    options.Filters.Add<ApiExceptionHandlerFilter>();
});
```

### Model classes

#### ApiResponseModel classes

λμΌν μλ΅μ μ κ³΅νκΈ° μν΄ μ¬μ©λλ μλ΅ λͺ¨λΈμλλ€.

μλμ κ°μ νμμΌλ‘ HTTP μλ΅ λ³Έλ¬Έμ μ κ³΅ν©λλ€.

```
{
    statusCode: number
    message: string
    data: T
}
```

#### ErrorModel class

μ¬μ©μ μ μ μ€λ₯λ₯Ό ννν©λλ€.

> [kr.bbon.Core](https://github.com/bbonkr/kr.bbon.AspNetCore) ν¨ν€μ§λ‘ μ΄μ λμμ΅λλ€.

```csharp
var error = new ErrorModel
{
    Code = "Some code",
    Message = "Some message",
    InnerError = new ErrorModel
    {
        Code = "Some inner code",
        Message = "Some inner message",
    }
});
```

### ControllerBase classes

#### ApiControllerBase class

μΉ μμ©νλ‘κ·Έλ¨ μλ΅μ λμΌνκ² μ κ³΅νκΈ° μν μ½λμ‘°κ°μ΄ μ λ¦¬λμ΄ μμ΅λλ€.

> β  ApiControllerBase ν΄λμ€λ₯Ό μμνλ ApiController λ `ApiVersionAttribute`, `AreaAttribute`, `RouteAttribute` λ₯Ό νμμ μΌλ‘ μ¬μ©ν΄μΌ ν©λλ€.

```csharp
[ApiVersion(DefaultValues.ApiVersion)]
[ApiController]
[Area(DefaultValues.AreaName)]
[Route(DefaultValues.RouteTemplate)]
[ApiExceptionHandlerFilter]
public class WeatherForecastController : ApiControllerBase 
{
    // ...
}
```

μ»¨νΈλ‘€λ¬μ μ‘μ λ©μλμμ μλμ κ°μ΄ μλ΅μ κ΅¬μ±ν©λλ€.

```csharp
var responseData = GetResponseData();

return StatusCode(HttpStatusCode.OK, responseData)
```

HTTP μλ΅λ³Έλ¬Έμ μλμ κ°μ΄ μ κ³΅λ©λλ€.

```json
{
    "statusCode": 200,
    "message": null,
    "data": {
        // ...responseData
    }
}
```

### Etc classes

<a id="app-options-class"></a>
#### AppOptions class

μμ© νλ‘κ·Έλ¨ κ΅¬μ±κ°μ ννν©λλ€.

appsettings.json μ μλμ κ°μ΄ κ΅¬μ±νκ³ , μλΉμ€ κ΅¬μ±μμ κ΅¬μ±κ°μ μ½μ΄μ μ²λ¦¬ν©λλ€.

```json
// appsettings.json 
{
    "App": {
        "Title": "Awesome api",
        "Description": "My awesome api !!"
    },
    // ...
}
```

```csharp
// ConfigureService() on Startup.cs
services.Configure<AppOptions>(Configuration.GetSection(AppOptions.Name));
```

<a id="configure-swagger-options-base-class"></a>
#### ConfigureSwaggerOptionsBase class

Swagger κ΅¬μ±κ°μ μν κΈ°λ³Έ ν΄λμ€μλλ€.

μλμ κ°μ΄ `ConfigureSwaggerOptionsBase` λ₯Ό μμνλ λ³ΈμΈμ `ConfigureSwaggerOptions` ν΄λμ€λ₯Ό μ μνμΈμ.

```csharp
public class MyConfigureSwaggerOptions :  ConfigureSwaggerOptionsBase {
    public MyConfigureSwaggerOptions(IApiVersionDescriptionProvider provider, IOptionsMonitor<AppOptions> optionsAccessor)
        : base(provider) 
    {
        options = optionsAccessor.CurrentValue;
    }

    public override string AppTitle => options.Title;

    public override string AppDescription => options.Description;

    private readonly AppOptions options;
}
```

#### ConfigureSwaggerOptions class

[ConfigureSwaggerOptionsBase](#configure-swagger-options-base-class) ν΄λμ€λ₯Ό κ΅¬ννλ ν΄λμ€μλλ€.

### Constants 

#### DefaultValues class

RouteTemplate: 
  ν΄λμ€ λΌμ°νΈ ννλ¦Ώμ κΈ°λ³Έκ°μλλ€. `[area]/v{version:apiVersion}/[controller]`

ApiVersion:
  κΈ°λ³Έ λ²μ  λ¬Έμμ΄ μλλ€. `1.0`

AreaName:
  μμ­ κΈ°λ³Έκ°μλλ€. `api`