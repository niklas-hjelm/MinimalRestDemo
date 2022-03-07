using Microsoft.AspNetCore.Mvc;
using MinimalRestDemo.DAL;
using MinimalRestDemo.DAL.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<UserStorage>();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapGet("/users", ([FromServices] UserStorage storage) =>
{
    var users = storage.GetAllUsers();

    if (users.Count <= 0)
    {
        return Results.NotFound();
    }

    return Results.Ok(users);
});

app.MapGet("/users/{id}", ([FromServices] UserStorage storage, int id) =>
{
    var user = storage.GetUser(id);

    return user is not null ? Results.Ok(user): Results.NotFound();
});

app.MapPost("/users", ([FromServices] UserStorage storage, User user) =>
{
    if (user is null)
        return Results.BadRequest();

    return storage.CreateUser(user) ? Results.Ok() : Results.Conflict();
});

app.MapPut("/users/{id}", ([FromServices] UserStorage storage, int id, User user) =>
{
    if (string.IsNullOrEmpty(user.Name) || string.IsNullOrEmpty(user.Email))
        return Results.BadRequest();

    return storage.UpdateUser(id, user) ? Results.Ok(): Results.NotFound();
});

app.MapMethods("/users/name/{id}",  new [] {"PATCH"}, 
    ([FromServices] UserStorage storage, int id, string name) =>
{
    if (string.IsNullOrEmpty(name.Trim()))
    {
        return Results.BadRequest();
    }

    return storage.UpdateUserName(id, name) ?
        Results.Ok(storage.GetUser(id)) : Results.NotFound();
});

app.MapMethods("/users/email/{id}", new[] { "PATCH" }, 
    ([FromServices] UserStorage storage, int id, string email) =>
{
    if (string.IsNullOrEmpty(email.Trim()))
    {
        return Results.BadRequest();
    }

    return storage.UpdateUserEmail(id, email) ?
        Results.Ok(storage.GetUser(id)) : Results.NotFound();
});

app.MapDelete("/users/{id}", ([FromServices] UserStorage storage, int id) =>
{
    return storage.DeleteUser(id) ? Results.Ok() : Results.NotFound();
});


app.Run();
