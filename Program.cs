using Microsoft.EntityFrameworkCore;
using minimalapi_crud;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Connect to PostgreSQL Database
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<NoteDb>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/", () => "Welcome to Notes API!");

app.MapPost("/notes/", async (Note n, NoteDb db) =>
{
    db.Notes.Add(n);
    await db.SaveChangesAsync();

    return Results.Created($"/notes/{n.Id}", n);
});

app.MapGet("/notes/{id:int}", async (int id, NoteDb db) =>
{
    return await db.Notes.FindAsync(id)
            is Note n
                ? Results.Ok(n)
                : Results.NotFound();
});

app.MapGet("/notes", async (NoteDb db) => await db.Notes.ToListAsync());

app.MapPut("/notes/{id:int}", async (int id, Note n, NoteDb db) =>
{
    var note = await db.Notes.FindAsync(id);

    if (note is null) return Results.NotFound();

    //found, so update with incoming note n.
    note.Text = n.Text;
    note.Done = n.Done;
    await db.SaveChangesAsync();
    return Results.Ok(note);
});

app.MapDelete("/notes/{id:int}", async (int id, NoteDb db) => {

    var note = await db.Notes.FindAsync(id);
    if (note is not null)
    {
        db.Notes.Remove(note);
        await db.SaveChangesAsync();
    }
    return Results.NoContent();
});

await app.RunAsync();

