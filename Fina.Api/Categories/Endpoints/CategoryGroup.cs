using Fina.Core.Categories.Handlers;
using Fina.Core.Categories.Models;
using Fina.Core.Categories.Requests;
using Fina.Core.Common.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Fina.Api.Categories.Endpoints;

public static class CategoryGroup
{
    public static void MapCategoryEndpoints(this RouteGroupBuilder builder)
    {
        builder.MapPost("", CreateCategoryEndpoint.HandleAsync)
            .WithName("CreateCategory")
            .WithDescription("Cria uma nova categoria.")
            .WithDisplayName("Criar Categoria")
            .WithTags("Categories")
            .Accepts<CreateCategoryRequest>("application/json")
            .Produces<Response<Category?>>()
            .WithOpenApi();

        builder.MapPut("", UpdateCategoryEndpoint.HandleAsync)
            .WithName("UpdateCategory")
            .WithDescription("Atualiza uma categoria existente.")
            .WithDisplayName("Atualizar Categoria")
            .WithTags("Categories")
            .Accepts<UpdateCategoryRequest>("application/json")
            .Produces<Response<Category?>>()
            .WithOpenApi();
        
        builder.MapDelete("{id}", DeleteCategoryEndpoint.HandleAsync)
            .WithName("DeleteCategory")
            .WithDescription("Deleta uma categoria existente.")
            .WithDisplayName("Deletar Categoria")
            .WithTags("Categories")
            .Produces<Response<Category?>>()
            .WithOpenApi();

        builder.MapGet("{id}", GetCategoryByIdEndpoint.HandleAsync)
            .WithName("GetCategoryById")
            .WithDescription("Obtém uma categoria pelo ID.")
            .WithDisplayName("Obter Categoria por ID")
            .WithTags("Categories")
            .Produces<Response<Category?>>()
            .WithOpenApi();

        builder.MapGet("", GetAllCategoriesEndpoint.HandleAsync)
            .WithName("GetAllCategories")
            .WithDescription("Obtém todas as categorias.")
            .WithDisplayName("Obter Todas as Categorias")
            .WithTags("Categories")
            .Produces<PagedResponse<List<Category>>>()
            .WithOpenApi();
    }
}