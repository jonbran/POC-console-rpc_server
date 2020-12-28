using console_service.Models;
using console_service.RabbitMQ_Service;

namespace console_service.Patterns.Command
{
    public class InsertTodoItem 
    {
        public object insertTodoItem(object param)
        {
            var item = param as byte[];
            var todoItem = ObjectSerializer.DeSerialize(item, typeof(TodoItem));
            todoItem = this.insertTodoItem(item);

            return todoItem.Serialize(); // ObjectSerialize.Serialize(todoItem);
        }

        public TodoItem insertTodoItem(byte[] message)
        {
            
            TodoItem item = message.DeSerialize(typeof(TodoItem)) as TodoItem;
            var db = new TodoContext();

            var newItem = db.TodoItems.Add(item);
            db.SaveChanges();


            return new TodoItem() {
                Id = newItem.Entity.Id,
                Name = newItem.Entity.Name,
                IsComplete = newItem.Entity.IsComplete
            };
        }
    }
}