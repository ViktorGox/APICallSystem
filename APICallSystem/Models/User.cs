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

        public override string ToString()
        {
            return Id + " - " + Name + " - " + Email; 
        }
    }
}
