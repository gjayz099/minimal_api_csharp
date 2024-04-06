using NSwag.AspNetCore;
using csharp.db;
using Microsoft.EntityFrameworkCore;
using csharp.models;
using csharp.dtos;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Identity;
using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<dbcontextApp>
(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);





builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen( options => 
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    options.OperationFilter<SecurityRequirementsOperationFilter>();
});




builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApiDocument(config =>
{
    config.DocumentName = "PokemonAPI";
    config.Title = "PokemonAPI v1";
    config.Version = "v1";
});


builder.Services.AddAuthentication();
builder.Services.AddIdentityApiEndpoints<IdentityUser>().AddEntityFrameworkStores<dbcontextApp>();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUi(config =>
    {
        config.DocumentTitle = "PokemonAPI";
        config.Path = "/swagger";
        config.DocumentPath = "/swagger/{documentName}/swagger.json";
        config.DocExpansion = "list";
    });
}


// var pokemons = new List<Pokemon>{
//     new Pokemon {id  = 1, name= "pop", type = "grass", img = "dsadsa"},
//     new Pokemon {id  = 2, name= "pop1", type = "water", img = "dwdada"},
//     new Pokemon {id  = 3, name= "pop2", type = "fire", img = "dsadsadsa"}
// };


RouteGroupBuilder pokemonItems = app.MapGroup("/pokemons");


pokemonItems.MapPost("/",  CreatePokemon);
pokemonItems.MapGet("/", GetPokemon);
pokemonItems.MapGet("/{id}", GetIdPokemon);
pokemonItems.MapPut("/{id}", PutIdPokemon);
pokemonItems.MapDelete("/{id}", DeletePokemon);



static async Task<IResult> CreatePokemon(PokemonDTO pokemonDTO, dbcontextApp dbcontext)
{
    var pokeItem = new Pokemon
    {
        name = pokemonDTO.name,
        type = pokemonDTO.type,
        img = pokemonDTO.img
    };
    dbcontext.Pokemons.Add(pokeItem);

    await dbcontext.SaveChangesAsync();

    pokemonDTO = new PokemonDTO(pokeItem);

    return TypedResults.Created($"/pokemonitems/{pokeItem.id}", pokemonDTO);
};


[Authorize]
static async Task<IResult> GetPokemon(dbcontextApp dbcontext)
{
    return TypedResults.Ok(await dbcontext.Pokemons.Select(x => new PokemonDTO(x)).ToArrayAsync());
}

static async Task<IResult> GetIdPokemon(int id, dbcontextApp dbcontext)
{
    var pokemonid = await dbcontext.Pokemons.FindAsync(id);

    if(pokemonid == null) return TypedResults.NotFound();
   
    return TypedResults.Ok(new PokemonDTO(pokemonid));
}


static async Task<IResult> PutIdPokemon(int id, PokemonDTO pokemonDTO, dbcontextApp dbcontext)
{
    var pokeItem = await dbcontext.Pokemons.FindAsync(id);

    if(pokeItem == null) return TypedResults.NotFound();

    pokeItem.name = pokemonDTO.name;
    pokeItem.type = pokemonDTO.type;
    pokeItem.img = pokemonDTO.img;

    await dbcontext.SaveChangesAsync();

    return TypedResults.Ok(pokeItem);
    
}

static async Task<IResult> DeletePokemon(int id, dbcontextApp dbcontext)
{
    var pokeItem = await dbcontext.Pokemons.FindAsync(id);

    if(pokeItem == null) return TypedResults.NotFound();

    dbcontext.Pokemons.Remove(pokeItem);
    await dbcontext.SaveChangesAsync();
    return TypedResults.Ok("Pokemon Is Deleted");
}


Console.WriteLine("http://localhost:5033/swagger/index.html");

app.Run();
