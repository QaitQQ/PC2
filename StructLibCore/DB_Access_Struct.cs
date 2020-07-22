namespace Object_Description
{
    public class DB_Access_Struct
    {
        [System.Serializable]
        public class User
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Pass { get; set; }
        }
    }
}
