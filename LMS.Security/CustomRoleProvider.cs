// CustomRoleProvider.cs in LMS.Security namespace
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Security;
using System.Xml;

namespace LMS.Security
{
    public class CustomRoleProvider : RoleProvider
    {
        public override string ApplicationName { get; set; }

        public override string[] GetRolesForUser(string username)
        {
            try
            {
                // Check Staff.xml first
                XmlDocument staffDoc = new XmlDocument();
                string staffPath = System.Web.Hosting.HostingEnvironment.MapPath("~/App_Data/Staff.xml");
                staffDoc.Load(staffPath);

                if (staffDoc.SelectSingleNode($"//user[username='{username}']") != null)
                {
                    return new string[] { "Staff" };
                }

                // Check Member.xml
                XmlDocument memberDoc = new XmlDocument();
                string memberPath = System.Web.Hosting.HostingEnvironment.MapPath("~/App_Data/Member.xml");
                memberDoc.Load(memberPath);

                if (memberDoc.SelectSingleNode($"//user[username='{username}']") != null)
                {
                    return new string[] { "Member" };
                }

                return new string[0];
            }
            catch
            {
                return new string[0];
            }
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            var roles = GetRolesForUser(username);
            return roles.Contains(roleName);
        }

        // Other required methods with minimal implementation
        public override void CreateRole(string roleName) { throw new NotImplementedException(); }
        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole) { throw new NotImplementedException(); }
        public override bool RoleExists(string roleName) { throw new NotImplementedException(); }
        public override void AddUsersToRoles(string[] usernames, string[] roleNames) { throw new NotImplementedException(); }
        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames) { throw new NotImplementedException(); }
        public override string[] GetUsersInRole(string roleName) { throw new NotImplementedException(); }
        public override string[] GetAllRoles() { throw new NotImplementedException(); }
        public override string[] FindUsersInRole(string roleName, string usernameToMatch) { throw new NotImplementedException(); }
    }
}