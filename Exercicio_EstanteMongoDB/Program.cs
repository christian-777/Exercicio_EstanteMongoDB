using System.ComponentModel.Design;
using Amazon.SecurityToken.Model;
using Exercicio_EstanteMongoDB;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Driver;

internal class Program
{
    private static void Main(string[] args)
    {
        MongoClient mongo = new MongoClient("mongodb://localhost:27017");

        var dataBase = mongo.GetDatabase("Estante");
        var collectionLivro = dataBase.GetCollection<BsonDocument>("Livro");
        var collectionEmprestado = dataBase.GetCollection<BsonDocument>("Emprestado");
        var collectionLendo = dataBase.GetCollection<BsonDocument>("Lendo");

        int esc;

        do
        {
            Console.WriteLine("escolha: ");
            Console.WriteLine("1- Cadastrar livro");
            Console.WriteLine("2- Listar livros na estante");
            Console.WriteLine("3- Editar atributo de um livro");
            Console.WriteLine("4- Procurar por um livro na estante");
            Console.WriteLine("5- Emprestar livro");
            Console.WriteLine("6- Devolver livro (emprestado/lendo)");
            Console.WriteLine("7- Ler livro");
            Console.WriteLine("0- Sair");
            esc= int.Parse(Console.ReadLine());

            switch(esc)
            {
                case 0:
                    Console.WriteLine("Saindo...");
                    Thread.Sleep(1000);
                    System.Environment.Exit(0);
                    break;

                case 1:
                    InsertInLivro(collectionLivro);
                    break;

                case 2:
                    FindInLivro(collectionLivro);
                    break;

                case 3:
                    UpdateInLivro(collectionLivro);
                    break;

                case 4:
                    FindOneInLivro(collectionLivro);
                    break;

                case 5:
                    SplitBook(collectionLivro, collectionEmprestado);
                    break;

                case 6:
                    Console.WriteLine("devolver de [1]- emprestado [2]- lendo");
                    var choice= Console.ReadLine();
                    if (choice == "1")
                    {
                        SplitBook(collectionEmprestado, collectionLivro);
                    }
                    if (choice == "2")
                    {
                        SplitBook(collectionLendo, collectionLivro);
                    }
                    break;

                case 7:
                    SplitBook(collectionLivro, collectionLendo);
                    break;
            }

        } while (true);
       
    }

    private static void SplitBook(IMongoCollection<BsonDocument> collectionLivro, IMongoCollection<BsonDocument> collectionEmprestado)
    {
        Console.WriteLine("digite o nome do livro que deseja encontrar: ");
        var search = Console.ReadLine();

        var book = collectionLivro.Find(new BsonDocument()).First();
        collectionEmprestado.InsertOne(book);
        collectionLivro.DeleteOne(Builders<BsonDocument>.Filter.Eq("_id", (ObjectId)book.GetValue(0)));
    }

    private static void FindOneInLivro(IMongoCollection<BsonDocument> collectionLivro)
    {
        Console.WriteLine("digite o nome do livro que deseja encontrar: ");
        var search= Console.ReadLine();

        var books = collectionLivro.Find(new BsonDocument()).ToList();
        var aux = books.Where(book => BsonSerializer.Deserialize<Book>(book).Title == search).ToList();
        aux.ForEach(book => Console.WriteLine(BsonSerializer.Deserialize<Book>(book).ToString()));
        
    }

    private static void UpdateInLivro(IMongoCollection<BsonDocument> collectionLivro)
    {
        Console.WriteLine("Digite o titulo do livro que deseja modificar: ");
        string search= Console.ReadLine();
        var filter = Builders<BsonDocument>.Filter.Eq("titulo", search);

        int choice;
        do
        {
            Console.WriteLine("Escolha o atributo que deseja editar deste livro: ");
            Console.WriteLine("1- isbn");
            Console.WriteLine("2- titulo");
            Console.WriteLine("3- editora");
            Console.WriteLine("4- ano de publicacao");
            Console.WriteLine("5- autor");
            Console.WriteLine("0- sair");
            choice= int.Parse(Console.ReadLine());

            switch (choice)
            {
                case 0:
                    Console.WriteLine("Saindo...");
                    Thread.Sleep(1000);
                    break;

                case 1:
                    Console.WriteLine("novo isbn:");
                    var isbn=Console.ReadLine();

                    var update = Builders<BsonDocument>.Update.Set("isbn", isbn);

                    collectionLivro.UpdateMany(filter, update);
                    break;

                case 2:
                    Console.WriteLine("novo titulo:");
                    var title = Console.ReadLine();

                    var updatetitle = Builders<BsonDocument>.Update.Set("titulo", title);

                    collectionLivro.UpdateMany(filter, updatetitle);
                    break;

                case 3:
                    Console.WriteLine("nova editora:");
                    var editor = Console.ReadLine();

                    var updateeditor = Builders<BsonDocument>.Update.Set("editra", editor);

                    collectionLivro.UpdateMany(filter, updateeditor);
                    break;

                case 4:
                    Console.WriteLine("novo ano de publicacao:");
                    var publishYear = Console.ReadLine();

                    var updatePublishYear = Builders<BsonDocument>.Update.Set("Ano de publicacao", publishYear);

                    collectionLivro.UpdateMany(filter, updatePublishYear);
                    break;

                case 5:
                    Console.WriteLine("novo autor:");
                    var author = Console.ReadLine();

                    var updateAuhor = Builders<BsonDocument>.Update.Set("autor", author);

                    collectionLivro.UpdateMany(filter, updateAuhor);
                    break;
            }

        } while (choice!=0);
    }

    private static void FindInLivro(IMongoCollection<BsonDocument> collectionLivro)
    {
        var books = collectionLivro.Find(new BsonDocument()).ToList();
        books.ForEach(book => Console.WriteLine(BsonSerializer.Deserialize<Book>(book).ToString()));
        
    }

    private static void InsertInLivro(IMongoCollection<BsonDocument> collectionLivro)
    {
        Console.WriteLine("Isbn: ");
        string isbn = Console.ReadLine();

        Console.WriteLine("Titulo: ");
        string titulo = Console.ReadLine();

        Console.WriteLine("Editora: ");
        string editora = Console.ReadLine();

        Console.WriteLine("Ano publicacao: ");
        string anoPublicacao = Console.ReadLine();

        string nameAuthhor;

        Console.WriteLine("nome do autor: ");
        nameAuthhor = Console.ReadLine();


        var insert = new BsonDocument
        {
            { "isbn", isbn },
            { "titulo", titulo },
            { "editora", editora },
            { "Ano de publicacao", anoPublicacao },
            { "autor", nameAuthhor }
        };

        collectionLivro.InsertOne(insert);
    }
}