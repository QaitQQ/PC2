using System;
using System.Collections.Generic;

using static Object_Description.DB_Access_Struct;

namespace CRMLibs
{
    [System.Serializable]
    public class Partner
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Аddress { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public List<СontactPerson> СontacPersons { get; set; }
        public int EmployeeID { get; set; }
        public List<Event> Events { get; set; }
        public int DirectoryID { get; set; }
        public int StatusID { get; set; }
    }
    [System.Serializable]
    public class Event
    {
        public int Id { get; set; }
        public string Сontent { get; set; }
        public int TypeID { get; set; }
        public DateTime DatePlanned { get; set; }
        public DateTime DateОccurred { get; set; }
        public int СontactPersonID { get; set; }
        public int PartnerID { get; set; }
    }
    [System.Serializable]
    public class СontactPerson
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int PartnerID { get; set; }
        public string Phone { get; set; }
    }
    [System.Serializable]
    public class Directory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ParentId { get; set; }
    }
    [System.Serializable]
    public class Status
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class EventType
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class Task
    {

        public int Id { get; set; }
        public string Сontent { get; set; }
        public DateTime DatePlanned { get; set; }
        public DateTime DateОccurred { get; set; }
        public User Implementer { get; set; }
        public User Curator { get; set; }
        public Status Status { get; set; }
    }



}
