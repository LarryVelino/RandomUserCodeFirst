namespace RandomUserCodeFirst.Model
{
    using System.ComponentModel.DataAnnotations;

    public class User
    {
        public string Cell { get; set; }

        public string Email { get; set; }

        public string Name { get; set; }

        public string Phone { get; set; }

        [Key]
        public int UserId { get; set; }
    }
}