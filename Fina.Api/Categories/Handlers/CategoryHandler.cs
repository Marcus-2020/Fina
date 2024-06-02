using System.Security.Claims;
using Fina.Api.Common.Data;
using Fina.Core;
using Fina.Core.Categories.Handlers;
using Fina.Core.Categories.Models;
using Fina.Core.Categories.Requests;
using Fina.Core.Common.Responses;
using Microsoft.EntityFrameworkCore;
using ILogger = Serilog.ILogger;

namespace Fina.Api.Categories.Handlers;

public class CategoryHandler : ICategoryHandler
{
    private readonly AppDbContext _context;
    private readonly ILogger _logger;
    
    public CategoryHandler(AppDbContext context, ILogger logger)
    {
        _context = context;
        
        //string userId = principal.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
        
        _logger = logger.ForContext<ICategoryHandler>();
        //_logger.BindProperty("UserId", userId, false, out _);
    }

    public async Task<Response<Category?>> CreateAsync(CreateCategoryRequest request)
    {
        try
        {
            _logger.BindProperty("Request", request, true, out _);
            
            Category category = new()
            {
                Title = request.Title,
                Description = request.Description,
                UserId = request.UserId
            };
            
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
            
            _logger.Information("Categoria {CategoryId} criada com sucesso", category.Id);
            return new Response<Category?>(category,
                StatusCodes.Status201Created,
                message: "Categoria criada com sucesso!");
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Ocorreu um erro ao tentar criar a categoria");
            return new Response<Category?>(null, 
                StatusCodes.Status500InternalServerError, 
                "Ocorreu um erro ao tentar criar a categoria");
        }
    }

    public async Task<Response<Category?>> UpdateAsync(UpdateCategoryRequest request)
    {
        try
        {
            _logger.BindProperty("Request", request, true, out _);
            
            Category? category = await _context.Categories
                .FirstOrDefaultAsync(c => c.Id == request.Id && c.UserId == request.UserId);

            if (category is null)
            {
                _logger.Information("Categoria {CategoryId} não encontrada", request.Id);
                return new Response<Category?>(null,
                    StatusCodes.Status404NotFound,
                    "A categoria não foi encontrada");
            }

            category.Title = request.Title;
            category.Description = request.Description;

            _context.Categories.Update(category);
            await _context.SaveChangesAsync();

            _logger.Information("Cateogira {CategoryId} atualizada com sucesso", request.Id);
            return new Response<Category?>(category,
                message: "Categoria atualizada com sucesso!");
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Ocorreu um erro ao tentar atualizar a categoria {CategoryId}", request.Id);
            return new Response<Category?>(null,
                StatusCodes.Status500InternalServerError,
                "Ocorreu um erro ao tentar atualizar a categoria");
        }
    }

    public async Task<Response<Category?>> DeleteAsync(DeleteCategoryRequest request)
    {
        try
        {
            _logger.BindProperty("Request", request, true, out _);

            Category? category = await _context.Categories
                .FirstOrDefaultAsync(c => c.Id == request.Id && c.UserId == request.UserId);

            if (category is null)
            {
                _logger.Information("Categoria {CategoryId} não encontrada", request.Id);
                return new Response<Category?>(null,
                    StatusCodes.Status404NotFound,
                    "Cateogoria não encontrada");
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            
            _logger.Information("Categoria {CategoryId} excluída com sucesso", request.Id);
            return new Response<Category?>(category,
                message: "Categoria excluída com sucesso!");
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Ocorreu um erro ao tentar excluir a categoria {CategoryId}", request.Id);
            return new Response<Category?>(null,
                StatusCodes.Status500InternalServerError,
                "Ocorreu um erro ao tentar excluir a categoria");
        }
    }

    public async Task<Response<Category?>> GetByIdAsync(GetCategoryByIdRequest request)
    {
        try
        {
            _logger.BindProperty("Request", request, true, out _);
            
            Category? category = await _context.Categories
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == request.Id && c.UserId == request.UserId);

            if (category is null)
            {
                _logger.Information("Categoria {CategoryId} não encontrada", request.Id);
                return new Response<Category?>(null,
                    StatusCodes.Status404NotFound,
                    "Cateogoria não encontrada");
            }

            return new Response<Category?>(category,
                message: "Categoria recuperada com sucesso!");
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Ocorreu um erro ao tentar recuperar a categoria {CategoryId}", request.Id);
            return new Response<Category?>(null,
                StatusCodes.Status500InternalServerError,
                "Ocorreu um erro ao tentar recuperar a categoria");
        }
    }

    public async Task<PagedResponse<List<Category>>> GetAllAsync(GetAllCategoriesRequest request)
    {
        try
        {
            _logger.BindProperty("Request", request, true, out _);

            IQueryable<Category> query = _context.Categories
                .AsNoTracking()
                .Where(c => c.UserId == request.UserId)
                .OrderBy(c => c.Title);

            int totalCount = await query.CountAsync();

            List<Category> categories = await query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync();

            PagedResponse<List<Category>> response = new(categories, 
                totalCount,
                request.PageNumber,
                request.PageSize,
                message: "Categorias recuperadas com sucesso!");
            
            return response;
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Ocorreu um erro ao tentar recuperar as categorias");
            return new PagedResponse<List<Category>>(new(), 
                StatusCodes.Status500InternalServerError,
                "Ocorreu um erro ao tentar recuperar as categorias");
        }
    }
}