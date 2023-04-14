using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Exercicio_EstanteMongoDB
{
    [BsonIgnoreExtraElements]
    internal class Book
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("isbn")]
        public string Isbn { get; set; }

        [BsonElement("titulo")]
        public string Title { get; set; }

        [BsonElement("editora")]
        public string Editor { get; set; }

        [BsonElement("Ano de publicacao")]
        public string PublishYear { get; set; }

        [BsonElement("autor")]
        public string Author { get; set; }

        public Book(string isbn, string title, string editor, string publishYear, string author)
        {
            Isbn = isbn;
            Title = title;
            Editor = editor;
            PublishYear = publishYear;
            Author = author;
        }

        public override string ToString()
        {
            return "\nIsbn: "+Isbn+"\nTitulo: "+Title+"\nEditora: "+Editor+"\nAno de publicacao: "+PublishYear+"\nAutor: "+Author;
        }
    }
}
