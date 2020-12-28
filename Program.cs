using System;
using console_service.Patterns.Command;
using console_service.RabbitMQ_Service;

namespace console_service
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            // Insert
            var insert = new InsertTodoItem();
            var command = new Command(null, insert.insertTodoItem);
            var server = new Server("rpc_insertTodoItem", command);

            // Get
            var get = new GetTodoItem();
            var getCommand = new Command(null, get.Get);
            var getServer = new Server("rpc_getTodoItem", getCommand);

            // GetAll
            var getAllCommand = new Command(null, get.GetAllTodoItems);
            var getAllServer = new Server("rpc_getAllTodoItems", getAllCommand);

            server.connect();
            getServer.connect();
            getAllServer.connect();

            Console.ReadLine();
        }
    }
}
