using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace jfrost_bugtracker.Models
{
    public class ProjectHelper
    {
        private ApplicationDbContext db;

        public ProjectHelper(ApplicationDbContext context)
        {
            this.db = context;
        }

        public bool IsUserOnProj(string userId, int projectId)
        {
            var project = db.Projects.Find(projectId); //looking at all the Projects (each projectId)
            return project.Users.Any(u => u.Id == userId); //return each userId that it finds associated with the projectId
        }

        public IList<ApplicationUser> UsersOnProj(int projectId)
        {
            Projects project = db.Projects.Find(projectId);
            var userList = project.Users.ToList();
            return (userList);
        }

        public bool AddUserToProj(string userId, int projectId)
        {

            if (!IsUserOnProj(userId, projectId))
            {
                ApplicationUser user = db.Users.Find(userId);
                Projects project = db.Projects.Find(projectId);

                project.Users.Add(user);

                try
                {
                    var userAdded = db.SaveChanges();

                    if (userAdded != 0)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                catch
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        //return result;


        public bool RemoveUserFromProj(string userId, int projectId)
        {

            if (IsUserOnProj(userId, projectId))
            {
                ApplicationUser user = db.Users.Find(userId);
                Projects project = db.Projects.Find(projectId);

                project.Users.Remove(user);

                try
                {
                    var userRemoved = db.SaveChanges();

                    if (userRemoved != 0)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                catch
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
            

        //internal void RemoveUserFromProject(string id, List<char> role) // ??
        //{
        //    throw new NotImplementedException();
        //}

        public IList<ApplicationUser> ProjsforUser(string userId)
        {

            //var userIDs = roleManager.FindByName(roleName).Users.Select(r => r.UserId);
            //return userManager.Users.Where(u => userIDs.Contains(u.Id)).ToList();
            return new List<ApplicationUser>();
        }

        public IList<ApplicationUser> UsersNotOnProj(int projectId)
        {

            //var userIDs = System.Web.Security.Roles.GetUsersInRole(roleName);
            //return userManager.Users.Where(u => !userIDs.Contains(u.Id)).ToList();
            return new List<ApplicationUser>();
        }



    }
}