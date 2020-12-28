using System;
using System.Text;
using console_service.Models;
using console_service.RabbitMQ_Service;

namespace console_service.Patterns.Command
{
    public class GetTodoItem
    {
        public object Get(object obj) {
            var message = obj as byte[];

            var id = long.Parse(Encoding.UTF8.GetString(message));
            var db = new TodoContext();
            
            var item = db.TodoItems.Find(id);
            return item.Serialize();
        }

        public object GetAllTodoItems(object obj = null) {
            var db = new TodoContext();

            var items = db.TodoItems;

            return items.Serialize();
        }
    }

}