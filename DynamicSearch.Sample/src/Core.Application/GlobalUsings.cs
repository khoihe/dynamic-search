global using System;
global using System.Dynamic;
global using System.Threading;
global using System.Threading.Tasks;
global using System.Reflection;
global using System.Linq.Expressions;
global using System.Collections.Generic;
global using Microsoft.Extensions.DependencyInjection;

global using MediatR;

global using DynamicSearch.EfCore.Model;
global using DynamicSearch.EfCore.Service;
global using DynamicSearch.EfCore.Extension;
global using DynamicSearch.EfCore.Interface;
global using GenericRepository.Lib.Interface;
global using Kernel.Lib.Constant;

global using Core.Domain.Entity;
global using Core.Application.Model;
global using Core.Application.Service;
global using Core.Application.Command;
global using Core.Application.Service.Interface;
global using Core.Application.Repository.Interface;