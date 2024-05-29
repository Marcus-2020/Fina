using Fina.Api.Categories.Data;
using Fina.Api.Common.Data;
using Fina.Core.Categories.Handlers;
using Fina.Core.Categories.Models;
using Fina.Core.Categories.Requests;
using Fina.Core.Common.Requests;
using Fina.Core.Common.Responses;
using Microsoft.EntityFrameworkCore;

namespace Fina.Api.Categories.Handlers;

public class CategoryHandler : ICategoryHandler
{
    private readonly Serilog.ILogger _logger = Serilog.Log.ForContext<CategoryHandler>();
    private readonly AppDbContext _context;

    public CategoryHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Response<Category?>> CreateAsync(CreateCategoryRequest request)
    {
        var logger = _logger.ForContext("Request", request, true);
        try
        {
            _logger.Information("Criando categoria {Name}", request.Title);
            
            var category = new Category
            {
                Title = request.Title,
                Description = request.Description,
                UserId = request.UserId
            };

            _context.Categories.Add(category);

            await _context.SaveChangesAsync();
            
            _logger.Information("Categoria {Name} criada com sucesso", request.Title);

            return new Response<Category?>(category,
                StatusCodes.Status200OK, 
                "Categoria criada com sucesso.");
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Erro ao criar a categoria {Name}", request.Title);
            return new Response<Category?>(null,
                StatusCodes.Status500InternalServerError,
                $"Erro ao criar a categoria");
        }
        finally
        {
            await _context.DisposeAsync();
        }
    }

    public async Task<Response<Category?>> UpdateAsync(UpdateCategoryRequest request)
    {
        var logger = _logger.ForContext("Request", request, true);
        try
        {
            _logger.Information("Atualizando categoria {Id}", request.Id);
            
            var category = await _context.Categories.FindAsync(request.Id);

            if (category == null)
            {
                _logger.Warning("Categoria {Id} não encontrada", request.Id);
                return new Response<Category?>(null,
                    StatusCodes.Status404NotFound,
                    "Categoria não encontrada.");
            }
            
            if (category.UserId != request.UserId)
            {
                ErrorNotFromTheUser<Category>(request.Id, request);
            }

            category.Title = request.Title;
            category.Description = request.Description;

            await _context.SaveChangesAsync();
            
            _logger.Information("Categoria {Id} atualizada com sucesso", request.Id);

            return new Response<Category?>(category,
                StatusCodes.Status200OK, 
                "Categoria atualizada com sucesso.");
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Erro ao atualizar a categoria {Id}", request.Id);
            return new Response<Category?>(null,
                StatusCodes.Status500InternalServerError,
                $"Erro ao atualizar a categoria");
        }
        finally
        {
            await _context.DisposeAsync();
        }
    }

    public async Task<Response<Category?>> DeleteAsync(DeleteCategoryRequest request)
    {
        var logger = _logger.ForContext("Request", request, true);
        try
        {
            _logger.Information("Deletando categoria {Id}", request.Id);
            
            var category = await _context.Categories.FindAsync(request.Id);

            if (category == null)
            {
                _logger.Warning("Categoria {Id} não encontrada", request.Id);
                return new Response<Category?>(null,
                    StatusCodes.Status404NotFound,
                    "Categoria não encontrada.");
            }
            
            if (category.UserId != request.UserId)
            {
                ErrorNotFromTheUser<Category>(request.Id, request);
            }

            _context.Categories.Remove(category);

            await _context.SaveChangesAsync();
            
            _logger.Information("Categoria {Id} deletada com sucesso", request.Id);

            return new Response<Category?>(category,
                StatusCodes.Status200OK, 
                "Categoria deletada com sucesso.");
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Erro ao deletar a categoria {Id}", request.Id);
            return new Response<Category?>(null,
                StatusCodes.Status500InternalServerError,
                $"Erro ao deletar a categoria");
        }
        finally
        {
            await _context.DisposeAsync();
        }
    }

    public async Task<Response<Category?>> GetByIdAsync(GetCategoryByIdRequest request)
    {
        var logger = _logger.ForContext("Request", request, true);
        try
        {
            _logger.Information("Buscando categoria {Id}", request.Id);
            
            var category = await _context.Categories
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == request.Id);

            if (category == null)
            {
                _logger.Warning("Categoria {Id} não encontrada", request.Id);
                return new Response<Category?>(null,
                    StatusCodes.Status404NotFound,
                    "Categoria não encontrada.");
            }
            
            if (category.UserId != request.UserId)
            {
                ErrorNotFromTheUser<Category>(request.Id, request);
            }
            
            _logger.Information("Categoria {Id} encontrada com sucesso", request.Id);

            return new Response<Category?>(category,
                StatusCodes.Status200OK, 
                "Categoria encontrada com sucesso.");
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Erro ao buscar a categoria {Id}", request.Id);
            return new Response<Category?>(null,
                StatusCodes.Status500InternalServerError,
                $"Erro ao buscar a categoria");
        }
        finally
        {
            await _context.DisposeAsync();
        }
    }

    public async Task<PagedResponse<List<Category>>> GetAllAsync(GetAllCategoriesRequest request)
    {
        var logger = _logger.ForContext("Request", request, true);
        try
        {
            _logger.Information("Buscando todas as categorias");
            
            var categories = await _context.Categories
                .Where(x => x.UserId == request.UserId)
                .Skip(request.PageNumber * request.PageSize)
                .Take(request.PageSize)
                .AsNoTracking()
                .ToListAsync();

            _logger.Information("Categorias encontradas com sucesso");

            return new PagedResponse<List<Category>>(categories,
                StatusCodes.Status200OK, 
                "Categorias encontradas com sucesso.");
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Erro ao buscar as categorias");
            return new PagedResponse<List<Category>>(new(),
                StatusCodes.Status500InternalServerError,
                $"Erro ao buscar as categorias");
        }
        finally
        {
            await _context.DisposeAsync();
        }
    }
    
    private Response<T?> ErrorNotFromTheUser<T>(long categoryId, Request request) where T : class
    {
        _logger.Warning("Categoria {Id} não pertence ao usuário {UserId}", categoryId, request.UserId);
        return new Response<T?>(null,
            StatusCodes.Status403Forbidden,
            "Categoria não pertence ao usuário.");
    }
}