# kr.bbon.AspNetCore Package

[![](https://img.shields.io/nuget/v/kr.bbon.AspNetCore)](https://www.nuget.org/packages/kr.bbon.AspNetCore) [![](https://img.shields.io/nuget/dt/kr.bbon.AspNetCore)](https://www.nuget.org/packages/kr.bbon.AspNetCore) [![publish to nuget](https://github.com/bbonkr/kr.bbon.AspNetCore/actions/workflows/dotnet.yml/badge.svg)](https://github.com/bbonkr/kr.bbon.AspNetCore/actions/workflows/dotnet.yml)

## 📢 Overview

.NET 5 에서 API 웹 응용프로그램 프로젝트 <small>(webapi 템플릿 기준)</small>를 시작할 때, 반복적으로 작성하던 사항을 패키지로 정리했습니다.


## 🌈 Namespace

### kr.bbon.AspNetCore

웹 응용프로그램에서 사용되는 기능과 확장 메서드를 포함합니다.

> [kr.bbon.Core](https://www.nuget.org/packages/kr.bbon.Core) 패키지로 이동되었습니다.

### kr.bbon.AspNetCore.Filters

ASP.NET Core Mvc Filters를 지원하는 기능을 포함합니다.

### kr.bbon.AspNetCore.Mvc

ASP.NET Core MVC를 지원하는 기능을 포함합니다.

## 🎯 Features

### Exception classes

#### HttpStatusException class

> [kr.bbon.Core](https://www.nuget.org/packages/kr.bbon.Core) 패키지로 이동되었습니다.

HTTP 예외를 표현하기 위해 사용됩니다.

> [kr.bbon.Core](https://github.com/bbonkr/kr.bbon.AspNetCore) 패키지로 이전되었습니다.

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

> [kr.bbon.Core](https://www.nuget.org/packages/kr.bbon.Core) 패키지로 이동되었습니다.

HTTP 예외와 상세 정보를 표현하기 위해 사용됩니다.

> [kr.bbon.Core](https://github.com/bbonkr/kr.bbon.AspNetCore) 패키지로 이전되었습니다.

```csharp
throw new HttpStatusException(HttpStatusCode.BadRequest, new SomeDetails
{
    Id = "err111",
    Message = "AAA 속성의 값이 정수가 아닙니다.",
});
```

#### SomethingWrongException

> [kr.bbon.Core](https://www.nuget.org/packages/kr.bbon.Core) 패키지로 이동되었습니다.

사용자 정의 예외를 표현하기 위해 사용합니다.

> [kr.bbon.Core](https://github.com/bbonkr/kr.bbon.AspNetCore) 패키지로 이전되었습니다.

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

> [kr.bbon.Core](https://www.nuget.org/packages/kr.bbon.Core) 패키지로 이동되었습니다.

사용자 정의 예외와 상세 정보를 표현하기 위해 사용합니다.

> [kr.bbon.Core](https://github.com/bbonkr/kr.bbon.AspNetCore) 패키지로 이전되었습니다.

```csharp
throw new SomethingWrongException("데이터를 처리하지 못했습니다.", new SomeDetails 
{
    Id = "err111",
    Message = "AAA 속성의 값이 정수가 아닙니다.",
});
```

### Extension methods

#### IApplicationBuilder.UseSwaggerUIWithApiVersioning() method

IApplicationBuilder 인터페이스에 UseSwaggerUIWithApiVersioning 메서드를 추가합니다.

Swagger 를 사용하기 위해 필요한 코드 조각이 정리되어 있습니다.

```csharp
// Configure() on Startup.cs
app.UseSwaggerUIWithApiVersioning();
```

#### Object.ToJson<T>() method

Object 클래스에 ToJson 메서드를 추가합니다.

객체의 인스턴스를 JSON 문자열로 직렬화하는 기능을 제공합니다.

> [kr.bbon.Core](https://github.com/bbonkr/kr.bbon.AspNetCore) 패키지로 이전되었습니다.

```csharp
var sample = getSampleModel();
sample.ToJson();
```

JsonSerializerOptions 매개변수가 지정되지 않은 경우 아래 기본값을 사용합니다.

```csharp
var defaultOptions = new JsonSerializerOptions
{
    WriteIndented = true,
    Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
};
```

#### IServiceCollection.AddApiVersioningAndSwaggerGen() method

IServiceCollection 인터페이스에 AddApiVersioningAndSwaggerGen 메서드를 추가합니다.

ApiVersioning, Swagger 를 사용하기 위해 필요한 코드조각이 정리되어 있습니다.


```csharp
// ConfigureService() on Startup.cs
var defaultApiVersion = new ApiVersion(1, 0);
services.Configure<AppOptions>(Configuration.GetSection(AppOptions.Name));

services.AddApiVersioningAndSwaggerGen(defaultApiVersion);
```

### Filter classes

#### ApiExceptionHandlerFilter class

컨트롤러에서 처리되지 않은 예외가 발생하면 예외를 처리하는 필터를 제공합니다.

예외 처리 필터를 컨트롤러에서 사용하려면 아래와 같이 클래스 특성으로 추가합니다.

```csharp
[ApiExceptionHandlerFilter]
// ...
public class SomeController : ApiControllerBase 
{
    // ...
}
```

예외 처리 필터를 액션에서 사용하려면 아래와 같이 클래스 특성으로 추가합니다.

```csharp
[HttpGet]
[ApiExceptionHandlerFilter]
public IActionResult SomeAction()
{
    // ...
}
```

예외 처리 필터를 전역으로 사용하려면 아래와 같이 구성합니다.

```csharp
// ConfigureService() on Startup.cs
services.AddControllers(options => 
{
    options.Filters.Add<ApiExceptionHandlerFilter>();
});
```

### Model classes

#### ApiResponseModel classes

동일한 응답을 제공하기 위해 사용되는 응답 모델입니다.

아래와 같은 형식으로 HTTP 응답 본문을 제공합니다.

```
{
    statusCode: number
    message: string
    instance: string
    path: string
    method: string
    data: T
}
```

#### ErrorModel class

사용자 정의 오류를 표현합니다.

> [kr.bbon.Core](https://github.com/bbonkr/kr.bbon.AspNetCore) 패키지로 이전되었습니다.

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

웹 응용프로그램 응답을 동일하게 제공하기 위한 코드조각이 정리되어 있습니다.

> ⚠ ApiControllerBase 클래스를 상속하는 ApiController 는 `ApiVersionAttribute`, `AreaAttribute`, `RouteAttribute` 를 필수적으로 사용해야 합니다.

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

컨트롤러의 액션 메서드에서 아래와 같이 응답을 구성합니다.

```csharp
var responseData = GetResponseData();

return StatusCode(HttpStatusCode.OK, responseData)
```

HTTP 응답본문은 아래와 같이 제공됩니다.

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

응용 프로그램 구성값을 표현합니다.

appsettings.json 을 아래와 같이 구성하고, 서비스 구성에서 구성값을 읽어서 처리합니다.

OpenApiInfo 클래스도 동일한 값으로 구성됩니다.

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

Swagger 구성값을 위한 기본 클래스입니다.

아래와 같이 `ConfigureSwaggerOptionsBase` 를 상속하는 본인의 `ConfigureSwaggerOptions` 클래스를 정의하세요.

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

[ConfigureSwaggerOptionsBase](#configure-swagger-options-base-class) 클래스를 구현하는 클래스입니다.

### Constants 

#### DefaultValues class

RouteTemplate: 
  클래스 라우트 템플릿의 기본값입니다. `[area]/v{version:apiVersion}/[controller]`

ApiVersion:
  기본 버전 문자열 입니다. `1.0`

AreaName:
  영역 기본값입니다. `api`