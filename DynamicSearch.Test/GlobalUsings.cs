global using System;
global using System.IO;
global using System.Text;
global using System.Net.Http;
global using System.Threading.Tasks;
global using System.Collections.Generic;
global using Microsoft.AspNetCore.Hosting;
global using Microsoft.AspNetCore.Mvc.Testing;
global using Microsoft.Extensions.Configuration;

global using Xunit;
global using Xunit.Abstractions;
global using Newtonsoft.Json;
global using Newtonsoft.Json.Linq;
global using Testcontainers.PostgreSql;
global using DotNet.Testcontainers.Builders;
global using DotNet.Testcontainers.Networks;

global using DynamicSearch.EfCore.Model;
global using DynamicSearch.Test.Integration;

global using Core.Api;
global using Core.Application.Model;