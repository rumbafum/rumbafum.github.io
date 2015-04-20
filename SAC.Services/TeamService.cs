using SAC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAC.Services
{
    public class TeamService : IDisposable
    {
        SACServiceContext _context;

        public TeamService(SACServiceContext context)
        {
            _context = context;
        }

        public int TeamExists(string teamName)
        {
            Team team = _context.Teams.Where(t => t.Name == teamName).FirstOrDefault();
            return team != null ? team.Id : -1;
        }

        public IQueryable<Team> GetTeams()
        {
            return _context.Teams;
        }

        public Team AddTeam(string name)
        {
            Team team = _context.Teams.Add(new Team
            {
                Name = name
            });
            _context.SaveChanges();
            return team;
        }

        public void Dispose()
        {
            
        }

    }
}
