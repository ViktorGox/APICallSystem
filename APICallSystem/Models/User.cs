namespace APICallSystem.BackEnd
{
    /// <summary>
    /// Dummy class.
    /// </summary>
    internal class User
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        public User(string id, string name, string email)
        {
            Id = id;
            Name = name;
            Email = email;
        }

        public override string ToString()
        {
            return Id + " - " + Name + " - " + Email; 
        }
    }
}
