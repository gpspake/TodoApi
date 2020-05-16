using System.Collections.Generic;

public class TodoList
{
    public long Id { get; set; }
    public string Name { get; set; }
    
    public virtual ICollection<TodoItem> TodoItems { get; set; }
}