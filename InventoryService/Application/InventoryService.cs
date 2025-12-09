using AutoMapper;
using AutoMapper.QueryableExtensions;
using Contracts.Events;
using InventoryService.Application.Common;
using InventoryService.Application.DTOs;
using InventoryService.Application.Interfaces;
using InventoryService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace InventoryService.Application;

public class InventoryService : IInventoryService
{
    private readonly IInventoryDbContext _inventoryDbContext;
    private readonly IMapper _mapper;
    
    public InventoryService(IInventoryDbContext inventoryDbContext, IMapper mapper)
    {
        _inventoryDbContext = inventoryDbContext;
        _mapper = mapper;
    }

    public async Task<Result<List<ProductDto>>> GetProductsAsync()
    {
        var products = await _inventoryDbContext.Products.ProjectTo<ProductDto>(_mapper.ConfigurationProvider).ToListAsync();
        return await Result<List<ProductDto>>.SuccessAsync(products);
    }

    public async Task<Result<Guid>> CreateProductAsync(CreateProductRequest request)
    {
        try
        {
            var product = _mapper.Map<Product>(request);
            await _inventoryDbContext.Products.AddAsync(product);
            await _inventoryDbContext.SaveChangesAsync();
            return await Result<Guid>.SuccessAsync(product.Id);
        }
        catch (Exception ex)
        {
            return await Result<Guid>.FailureAsync(ex.Message);
        }
    }

    public async Task<Result<None>> UpdateProductQuantity(UpdateQuantityRequest request, Guid productId)
    {
        try
        {
            var product = _inventoryDbContext.Products.Find(productId);
            if (product == null)
            {
                return await Result<None>.FailureAsync("Product not found");
            }
            product.Quantity = request.Quantity;
            await _inventoryDbContext.SaveChangesAsync();
            return await Result<None>.SuccessAsync();
        }
        catch (Exception ex)
        {
            return await Result<None>.FailureAsync(ex.Message);
        }
    }

    public async Task<Result<long>> ReserveItemsAsync(IReadOnlyList<OrderItemDto> itemDtos)
    {
        try
        {
            var totalPrice = 0;
            foreach (var item in itemDtos)
            {
                var itemResult = await ReserveItemAsync(item);
                if (!itemResult.Succeeded)
                {
                    return await Result<long>.FailureAsync("Ошибка");
                }
                totalPrice += item.Quantity;
            }
            await _inventoryDbContext.SaveChangesAsync();
            return await Result<long>.SuccessAsync(totalPrice);
        }
        catch (Exception ex)
        {
            return await Result<long>.FailureAsync(ex.Message);
        }
    }
    
    private async Task<Result<long>> ReserveItemAsync(OrderItemDto request)
    {
        var product = _inventoryDbContext.Products.Find(request.ProductId);
        if (product == null)
        {
            return await Result<long>.FailureAsync("Product not found");
        }

        if (product.Quantity >= request.Quantity)
        {
            product.Quantity -= request.Quantity;
        }
        return await Result<long>.SuccessAsync();
    }
}