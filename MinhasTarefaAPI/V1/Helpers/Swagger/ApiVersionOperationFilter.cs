﻿using Microsoft.OpenApi.Models;
using MinhasTarefaAPI.V1.Helpers.Swagger;
using Swashbuckle.AspNetCore.Swagger;    // Dependencias obrgatórias para a documentação com versionamento.
using Swashbuckle.AspNetCore.SwaggerGen; // Dependencias obrgatórias para a documentação com versionamento.
using System.Linq;

namespace MinhasTarefaAPI.v1.Helpers.Swagger
{
//    public class ApiVersionOperationFilter : IOperationFilter
//    {



//    //    //public void Apply(Operation operation, OperationFilterContext context)
//    //    //{
//    //    //    var actionApiVersionModel = context.ApiDescription.ActionDescriptor?.GetApiVersion();
//    //    //    if (actionApiVersionModel == null)
//    //    //    {
//    //    //        return;
//    //    //    }

//    //    //    if (actionApiVersionModel.DeclaredApiVersions.Any())
//    //    //    {
//    //    //        operation.Produces = operation.Produces
//    //    //          .SelectMany(p => actionApiVersionModel.DeclaredApiVersions
//    //    //            .Select(version => $"{p};v={version.ToString()}")).ToList();
//    //    //    }
//    //    //    else
//    //    //    {
//    //    //        operation.Produces = operation.Produces
//    //    //          .SelectMany(p => actionApiVersionModel.ImplementedApiVersions.OrderByDescending(v => v)
//    //    //            .Select(version => $"{p};v={version.ToString()}")).ToList();
//    //    //    }
//    //    //}

//    //    public void Apply(OpenApiOperation operation, OperationFilterContext context)
//    //    {
//    //        //var actionApiVersionModel = context.ApiDescription.ActionDescriptor?.GetApiVersion();
//    //        //if (actionApiVersionModel == null)
//    //        //{
//    //        //    return;
//    //        //}

//    //        //if (actionApiVersionModel.DeclaredApiVersions.Any())
//    //        //{
//    //        //    operation.Produces = operation.Produces
//    //        //      .SelectMany(p => actionApiVersionModel.DeclaredApiVersions
//    //        //        .Select(version => $"{p};v={version.ToString()}")).ToList();
//    //        //}
//    //        //else
//    //        //{
//    //        //    operation.Produces = operation.Produces
//    //        //      .SelectMany(p => actionApiVersionModel.ImplementedApiVersions.OrderByDescending(v => v)
//    //        //        .Select(version => $"{p};v={version.ToString()}")).ToList();
//    //        //}
//    //    }
//    //}
}