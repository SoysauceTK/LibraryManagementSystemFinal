using System;
using System.Collections.Generic;
using System.Configuration.Provider;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Xml;

namespace LMS.Security
{
    public class XmlRoleProvider : RoleProvider
    {
        private string _applicationName;
        private readonly object _lock = new object();

        public override string ApplicationName
        {
            get { return _applicationName; }
            set { _applicationName = value; }
        }

        public override void Initialize(string name, System.Collections.Specialized.NameValueCollection config)
        {
            if (config == null)
                throw new ArgumentNullException("config");

            if (string.IsNullOrEmpty(name))
                name = "XmlRoleProvider";

            if (string.IsNullOrEmpty(config["description"]))
            {
                config.Remove("description");
                config.Add("description", "XML Role Provider");
            }

            base.Initialize(name, config);

            ApplicationName = config["applicationName"] ?? HttpContext.Current.Request.ApplicationPath;
        }

        public override string[] GetRolesForUser(string username)
        {
            try
            {
                // Check Staff.xml first
                XmlDocument staffDoc = new XmlDocument();
                string staffPath = HttpContext.Current.Server.MapPath("~/App_Data/Staff.xml");
                staffDoc.Load(staffPath);

                if (staffDoc.SelectSingleNode($"//user[username='{username}']") != null)
                {
                    return new string[] { "Staff" };
                }

                // Check Member.xml
                XmlDocument memberDoc = new XmlDocument();
                string memberPath = HttpContext.Current.Server.MapPath("~/App_Data/Member.xml");
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

        public override void CreateRole(string roleName)
        {
            if (RoleExists(roleName))
                throw new ProviderException("Role already exists.");

            // In our simple implementation, roles are implicit (Staff/Member)
            // So we don't actually need to store them separately
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            // In our implementation, roles are tied to user files
            // So we can't delete roles without affecting users
            if (throwOnPopulatedRole && GetUsersInRole(roleName).Length > 0)
            {
                throw new ProviderException("Cannot delete a populated role.");
            }

            // Roles are implicit in our system (Staff/Member)
            return true;
        }

        public override bool RoleExists(string roleName)
        {
            // We only support Staff and Member roles
            return roleName == "Staff" || roleName == "Member";
        }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            lock (_lock)
            {
                foreach (string roleName in roleNames)
                {
                    if (!RoleExists(roleName))
                        throw new ProviderException("Role not found.");

                    if (roleName == "Staff")
                    {
                        AddUsersToStaffFile(usernames);
                    }
                    else if (roleName == "Member")
                    {
                        // Members are handled through Member.xml during registration
                        throw new ProviderException("Member roles should be assigned through registration.");
                    }
                }
            }
        }

        private void AddUsersToStaffFile(string[] usernames)
        {
            string staffPath = HttpContext.Current.Server.MapPath("~/App_Data/Staff.xml");
            XmlDocument doc = new XmlDocument();

            try
            {
                doc.Load(staffPath);
            }
            catch (System.IO.FileNotFoundException)
            {
                // Create new Staff.xml if it doesn't exist
                XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", null, null);
                doc.AppendChild(dec);
                XmlElement root = doc.CreateElement("users");
                doc.AppendChild(root);
            }

            foreach (string username in usernames)
            {
                if (doc.SelectSingleNode($"//user[username='{username}']") == null)
                {
                    XmlElement user = doc.CreateElement("user");
                    XmlElement usernameElement = doc.CreateElement("username");
                    usernameElement.InnerText = username;
                    user.AppendChild(usernameElement);

                    // Add empty password element (password should be set separately)
                    XmlElement passwordElement = doc.CreateElement("password");
                    passwordElement.InnerText = "";
                    user.AppendChild(passwordElement);

                    doc.DocumentElement.AppendChild(user);
                }
            }

            doc.Save(staffPath);
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            lock (_lock)
            {
                foreach (string roleName in roleNames)
                {
                    if (!RoleExists(roleName))
                        throw new ProviderException("Role not found.");

                    if (roleName == "Staff")
                    {
                        RemoveUsersFromStaffFile(usernames);
                    }
                    else if (roleName == "Member")
                    {
                        // Members can't be removed from Member role in this implementation
                        throw new ProviderException("Cannot remove users from Member role.");
                    }
                }
            }
        }

        private void RemoveUsersFromStaffFile(string[] usernames)
        {
            string staffPath = HttpContext.Current.Server.MapPath("~/App_Data/Staff.xml");
            XmlDocument doc = new XmlDocument();
            doc.Load(staffPath);

            foreach (string username in usernames)
            {
                XmlNode userNode = doc.SelectSingleNode($"//user[username='{username}']");
                if (userNode != null)
                {
                    doc.DocumentElement.RemoveChild(userNode);
                }
            }

            doc.Save(staffPath);
        }

        public override string[] GetUsersInRole(string roleName)
        {
            if (!RoleExists(roleName))
                throw new ProviderException("Role not found.");

            if (roleName == "Staff")
            {
                return GetUsersFromStaffFile();
            }
            else if (roleName == "Member")
            {
                return GetUsersFromMemberFile();
            }

            return new string[0];
        }

        private string[] GetUsersFromStaffFile()
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                string staffPath = HttpContext.Current.Server.MapPath("~/App_Data/Staff.xml");
                doc.Load(staffPath);

                XmlNodeList userNodes = doc.SelectNodes("//user/username");
                string[] users = new string[userNodes.Count];
                for (int i = 0; i < userNodes.Count; i++)
                {
                    users[i] = userNodes[i].InnerText;
                }
                return users;
            }
            catch
            {
                return new string[0];
            }
        }

        private string[] GetUsersFromMemberFile()
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                string memberPath = HttpContext.Current.Server.MapPath("~/App_Data/Member.xml");
                doc.Load(memberPath);

                XmlNodeList userNodes = doc.SelectNodes("//user/username");
                string[] users = new string[userNodes.Count];
                for (int i = 0; i < userNodes.Count; i++)
                {
                    users[i] = userNodes[i].InnerText;
                }
                return users;
            }
            catch
            {
                return new string[0];
            }
        }

        public override string[] GetAllRoles()
        {
            return new string[] { "Staff", "Member" };
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            if (!RoleExists(roleName))
                throw new ProviderException("Role not found.");

            string[] usersInRole = GetUsersInRole(roleName);
            return usersInRole.Where(u => u.Contains(usernameToMatch)).ToArray();
        }
    }
}