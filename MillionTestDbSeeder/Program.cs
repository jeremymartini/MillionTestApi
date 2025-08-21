// See https://aka.ms/new-console-template for more information

using Bogus;
using Bogus.DataSets;
using MillionTestApi.DTOS;
using MillionTestApi.Settings;
using MongoDB.Bson;
using MongoDB.Driver;

try
{
    Console.Write("Por favor, introduzca el numero de registros que desea crear: ");
    int records = int.Parse(Console.ReadLine()!);

    var owner = new Faker<OwnerDto>("es_MX")
        .StrictMode(true)
        .RuleFor(x => x.Id, f => ObjectId.GenerateNewId())
        .RuleFor(x => x.Name, f => $"{f.Name.FirstName()} {f.Name.LastName()}")
        .RuleFor(x => x.Address, f => f.Address.FullAddress())
        .RuleFor(x => x.Photo, f => f.Internet.Avatar())
        .RuleFor(x => x.Birthday, f => f.Date.Past(80, DateTime.Now.AddYears(-18)).ToString());

    Console.WriteLine("Creando los duenos...");
    List<OwnerDto> fakeOwners = owner.Generate(records);
    Console.WriteLine($"{records} duenos creados!");


    int codeInternal = 1;
    List<PropertyDto> fakeProperties = new();
    foreach (var fakeOwner in fakeOwners)
    {
        var property = new Faker<PropertyDto>("es_MX")
            .StrictMode(true)
            .RuleFor(x => x.Id, f => ObjectId.GenerateNewId())
            .RuleFor(x => x.IdOwner, f => fakeOwner.Id)
            .RuleFor(x => x.Name, f => $"{f.Company.CatchPhrase()} {f.Lorem.Word()}")
            .RuleFor(x => x.Address, f => f.Address.FullAddress())
            .RuleFor(x => x.Price, f => f.Finance.Amount(100000, 1000000))
            .RuleFor(x => x.CodeInternal, f => codeInternal++)
            .RuleFor(x => x.Year, f => f.Date.Past(10, DateTime.Now).Year);

        fakeProperties.Add(property.Generate());

        Console.WriteLine($"Propiedad falsa generada para dueno: {fakeOwner.Name}");
    }

    Console.WriteLine("Todas las propiedades falsas se han generado!");

    List<PropertyImageDto> fakePropertyImages = new();
    foreach (var fakeProperty in fakeProperties)
    {
        var propertyImage = new Faker<PropertyImageDto>("es_MX")
            .StrictMode(true)
            .RuleFor(x => x.Id, f => ObjectId.GenerateNewId())
            .RuleFor(x => x.IdProperty, f => fakeProperty.Id)
            .RuleFor(x => x.File, f => f.Image.PicsumUrl(600, 400, true))
            .RuleFor(x => x.Enabled, f => true);

        fakePropertyImages.Add(propertyImage.Generate());

        Console.WriteLine($"Imagen falsa generada para propiedad: {fakeProperty.Name}");
    }

    Console.WriteLine("Todas las imagenes falsas de propiedades se han generado!");

    List<PropertyTraceDto> fakePropertyTraces = new();
    foreach (var fakeProperty in fakeProperties)
    {
        var propertyTrace = new Faker<PropertyTraceDto>("es_MX")
            .StrictMode(true)
            .RuleFor(x => x.Id, f => ObjectId.GenerateNewId())
            .RuleFor(x => x.IdProperty, f => fakeProperty.Id)
            .RuleFor(x => x.Name, f => $"{f.Company.CatchPhrase()} {f.Lorem.Word()}")
            .RuleFor(x => x.DateSale, f => f.Date.Past(f.PickRandom(1, 10), DateTime.Now).ToString())
            .RuleFor(x => x.Value, f => f.Finance.Amount(100000, 1000000))
            .RuleFor(x => x.Tax, f => f.Finance.Amount(1000, 10000));

        fakePropertyTraces.Add(propertyTrace.Generate());

        Console.WriteLine($"Historial falso generado para propiedad: {fakeProperty.Name}");
    }

    Console.WriteLine();
    Console.WriteLine("Todas los hitoriales falsos de propiedades se han generado!");
    Console.WriteLine("============================================================");
    Console.WriteLine("Procederemos a llenar la base de datos con la informacion falsa generada...");
    Console.WriteLine();

    var client = new MongoClient("mongodb://localhost:27017");
    var database = client.GetDatabase("million_test_db");

    Console.WriteLine("Insertando duenos falsos en la base de datos...");
    var ownersCollection = database.GetCollection<OwnerDto>("owner");
    await ownersCollection.InsertManyAsync(fakeOwners);
    Console.WriteLine("Todos los duenos falsos han sido insertados en la base de datos!");
    Console.WriteLine();

    Console.WriteLine("Insertando propiedades falsas en la base de datos...");
    var propertyCollection = database.GetCollection<PropertyDto>("property");
    await propertyCollection.InsertManyAsync(fakeProperties);
    Console.WriteLine("Todos las propiedades falsas han sido insertadas en la base de datos!");
    Console.WriteLine();

    Console.WriteLine("Insertando imagenes de propiedades falsas en la base de datos...");
    var propertyImageCollection = database.GetCollection<PropertyImageDto>("property_image");
    await propertyImageCollection.InsertManyAsync(fakePropertyImages);
    Console.WriteLine("Todos las imagenes de propiedades falsas han sido insertadas en la base de datos!");
    Console.WriteLine();

    Console.WriteLine("Insertando historiales de venta de propiedades falsas en la base de datos...");
    var propertyTraceCollection = database.GetCollection<PropertyTraceDto>("property_trace");
    await propertyTraceCollection.InsertManyAsync(fakePropertyTraces);
    Console.WriteLine("Todos los historiales de propiedades falsas han sido insertados en la base de datos!");
    Console.WriteLine();

    Console.WriteLine("Has terminado de sembrar tu base de datos.");
    Console.WriteLine("Presiona enter para cerrar la consola.");
    Console.ReadLine();


}
catch (Exception ex)
{
    Console.WriteLine("Error al iniciar la aplicacion: " + ex.Message);
    return;
}
