using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using MagicVilla_CouponAPI.Configs;
using MagicVilla_CouponAPI.Data;
using MagicVilla_CouponAPI.DTOs;
using MagicVilla_CouponAPI.Models;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(MappingConfig));
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/api/coupon", (ILogger<Program> _logger) =>
{
    _logger.Log(LogLevel.Information, "Getting all Coupons");
    return Results.Ok(new ResponseDTO
    {
        Result = CouponStore.Coupons.OrderBy(x => x.Id),
        IsSuccess = true,
        StatusCode = HttpStatusCode.OK
    });
}).WithName("GetCoupons")
.Produces<ResponseDTO>(200);

app.MapGet("/api/coupon/{id}", (int id) =>
{
    return Results.Ok(new ResponseDTO
    {
        IsSuccess = true,
        StatusCode = HttpStatusCode.OK,
        Result = CouponStore.Coupons.FirstOrDefault(x => x.Id == id)
    });
}).WithName("GetCoupon")
.Produces<ResponseDTO>(200);

app.MapPost("/api/coupon", (IMapper _mapper,
    IValidator<CouponDTO> _validation,
    CouponDTO couponDTO) =>
{
    ResponseDTO responseDTO = new();

    ValidationResult validationResult = _validation.ValidateAsync(couponDTO).GetAwaiter().GetResult();
    if (!validationResult.IsValid)
    {
        responseDTO.IsSuccess = false;
        responseDTO.StatusCode = HttpStatusCode.BadRequest;
        responseDTO.ErrorMessages.Add(validationResult.Errors.FirstOrDefault().ToString());
        return Results.BadRequest(responseDTO);
    }

    Coupon coupon = _mapper.Map<Coupon>(couponDTO);
    coupon.Id = CouponStore.Coupons.OrderByDescending(x => x.Id).FirstOrDefault().Id + 1;
    coupon.Created = DateTime.UtcNow;
    coupon.IsActive = true;

    CouponStore.Coupons.Add(coupon);

    responseDTO.IsSuccess = true;
    responseDTO.StatusCode = HttpStatusCode.BadRequest;
    responseDTO.Result = coupon;

    //return Results.CreatedAtRoute("GetCoupon", new { id = coupon.Id }, responseDTO);
    return Results.Ok(responseDTO);
}).WithName("CreateCoupon")
.Produces<ResponseDTO>(200)
.Produces(400);

app.MapPut("/api/coupon/{id}", (IValidator<CouponDTO> _validation,
    int id,
    CouponDTO couponDTO) =>
{
    ResponseDTO responseDTO = new();

    Coupon coupon = CouponStore.Coupons.FirstOrDefault(x => x.Id == id);
    if (coupon is null)
    {
        responseDTO.IsSuccess = false;
        responseDTO.StatusCode = HttpStatusCode.NotFound;
        responseDTO.ErrorMessages.Add($"Object by Id {id} Not Found.");
        return Results.NotFound(responseDTO);
    }

    ValidationResult validationResult = _validation.ValidateAsync(couponDTO).GetAwaiter().GetResult();
    if (!validationResult.IsValid)
    {
        responseDTO.IsSuccess = false;
        responseDTO.StatusCode = HttpStatusCode.BadRequest;
        responseDTO.ErrorMessages.Add(validationResult.Errors.FirstOrDefault().ToString());
        return Results.BadRequest(responseDTO);
    }

    coupon.Name = couponDTO.Name;
    coupon.Percent = couponDTO.Percent;
    coupon.LastUpdated = DateTime.UtcNow;

    responseDTO.IsSuccess = true;
    responseDTO.StatusCode = HttpStatusCode.BadRequest;
    responseDTO.Result = coupon;

    return Results.Ok(responseDTO);
}).WithName("UpdateCoupon")
.Produces<ResponseDTO>(200)
.Produces(400)
.Produces(404);

app.MapDelete("/api/coupon/{id}", (int id) =>
{
    ResponseDTO responseDTO = new();
    Coupon coupon = CouponStore.Coupons.FirstOrDefault(x => x.Id == id);
    if (coupon is null)
    {
        responseDTO.IsSuccess = false;
        responseDTO.StatusCode = HttpStatusCode.NotFound;
        responseDTO.ErrorMessages.Add($"Object by Id {id} Not Found.");
        return Results.NotFound(responseDTO);
    }
    CouponStore.Coupons.Remove(coupon);

    responseDTO.IsSuccess = true;
    responseDTO.StatusCode = HttpStatusCode.BadRequest;
    responseDTO.Result = coupon;

    return Results.Ok(responseDTO);
}).WithName("DeleteCoupon")
.Produces<Coupon>(200)
.Produces(404);

app.UseHttpsRedirection();
app.Run();