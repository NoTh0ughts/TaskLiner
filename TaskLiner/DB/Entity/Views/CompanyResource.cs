using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskLiner.DB.Entity.Views
{
    public class CompanyResource
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<string> Projects_Names { get; set; }
        public string Owner { get; set; }

        public string ProjectNamesFormatted => string.Join(",\n", Projects_Names);
    }

    public static partial class ToResourceHelper
    {
        public static CompanyResource ToResource(this Company company, string Owner, ICollection<string> projects) => 
            new()
            {
                Id = company.Id,
                Name = company.Name,
                Owner = company.WorkerContracts
                                .Where(x => x.IsOwner == true)
                                .Select(x => x.User.Nickname)
                                .FirstOrDefault(),
                Projects_Names = projects
            };
    }
}
