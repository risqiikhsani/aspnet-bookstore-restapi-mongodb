using BookStoreApi.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace BookStoreApi.Services;

public class BooksService
{
    private readonly IMongoCollection<Book> _booksCollection;
    // IMongoDatabase: Represents the Mongo database for running operations. 
    // This tutorial uses the generic GetCollection<TDocument>(collection) method on the interface to gain access to data in a specific collection. Run CRUD operations against the collection after this method is called. In the GetCollection<TDocument>(collection) method call:
    // collection represents the collection name.
    // TDocument represents the CLR object type stored in the collection.



    public BooksService(IOptions<BookStoreDatabaseSettings> bookStoreDatabaseSettings)
    {
        var mongoClient = new MongoClient(bookStoreDatabaseSettings.Value.ConnectionString);
        // MongoClient: Reads the server instance for running database operations. The constructor of this class is provided in the MongoDB connection string:

        var mongoDatabase = mongoClient.GetDatabase(bookStoreDatabaseSettings.Value.DatabaseName);

        _booksCollection = mongoDatabase.GetCollection<Book>(bookStoreDatabaseSettings.Value.BooksCollectionName);
        //  GetCollection<TDocument>(collection) returns a MongoCollection object representing the collection. In this tutorial, the following methods are invoked on the collection:
        // DeleteOneAsync: Deletes a single document matching the provided search criteria.
        // Find<TDocument>: Returns all documents in the collection matching the provided search criteria.
        // InsertOneAsync: Inserts the provided object as a new document in the collection.
        // ReplaceOneAsync: Replaces the single document matching the provided search criteria with the provided object.

    }

    // GetCollection<TDocument>(collection) method call:
    // collection represents the collection name.
    // TDocument represents the CLR object type stored in the collection.

    // GetCollection<TDocument>(collection) returns a MongoCollection object representing the collection. In this tutorial, the following methods are invoked on the collection:

    // DeleteOneAsync: Deletes a single document matching the provided search criteria.
    // Find<TDocument>: Returns all documents in the collection matching the provided search criteria.
    // InsertOneAsync: Inserts the provided object as a new document in the collection.
    // ReplaceOneAsync: Replaces the single document matching the provided search criteria with the provided object.


    public async Task<List<Book>> GetAsync() =>
        await _booksCollection.Find(_ => true).ToListAsync();

    public async Task<Book?> GetAsync(string id) =>
        await _booksCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task CreateAsync(Book newBook) =>
        await _booksCollection.InsertOneAsync(newBook);

    public async Task UpdateAsync(string id, Book updatedBook) =>
        await _booksCollection.ReplaceOneAsync(x => x.Id == id, updatedBook);

    public async Task RemoveAsync(string id) =>
        await _booksCollection.DeleteOneAsync(x => x.Id == id);
}