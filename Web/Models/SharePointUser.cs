using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Web.Models
{
    public class SharePointUser
    {
        public string Id { get; private set; }
        public string Name { get; private set; }
        public string Email { get; private set; }
        public List<string> Groups { get; set; }

        public SharePointUser(string id, string name, string email)
        {
            Id = id;
            Name = name;
            Email = email;
        }

        public SharePointUser(string id, string name, string email, List<string> groups)
        {
            Id = id;
            Name = name;
            Email = email;
            Groups = groups;
            //foreach (var group in groups)
            //{
            //    Groups.Add(group);
            //}
        }
    }
}
