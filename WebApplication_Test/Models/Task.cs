namespace WebApplication_Test.Models
{
    public class Task
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Priority { get; set; }
        public DateTime CreatedDate {  get; set; }
        public bool isDone { get; set; }
    }
}
