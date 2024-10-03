using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Html;
//using Microsoft.AspNetCore.Http;
using Agit.Framework.Web;
using TAM.VMS.Domain;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace TAM.VMS.Infrastructure.Navigation
{
    public class MenuBuilder
    {
        private IEnumerable<RolePermission> _acl;
        private IEnumerable<string> _actors;
        private IEnumerable<Menu> _menus;
        private readonly Dictionary<Guid, Menu> _selectedMenus = new Dictionary<Guid, Menu>();

        public MenuBuilder(IEnumerable<RolePermission> acl, IEnumerable<string> actors, IEnumerable<Menu> menus)
        {
            _acl = acl;
            _actors = actors;
            _menus = menus;
        }

        public HtmlString Render()
        {
            Uri absoluteUri = HttpContext.GetAbsoluteUri();
            DeepSelect(absoluteUri.LocalPath);

            var sb = new StringBuilder();

            var roots = _menus.Where(x => x.ParentID == null).OrderBy(x => x.OrderIndex);

            sb.Append(@"<ul class=""page-sidebar-menu page-sidebar-menu-closed"" data-keep-expanded=""false"" data-auto-scroll=""true"" data-slide-speed=""200"">");
            foreach (var root in roots)
            {
                sb.Append(Render(root));
            }
            sb.Append("</ul>");

            //string htmlsidebarmenuold = (sb.ToString()).Replace("class=\"title\">&nbsp;&nbsp;", "class=\"title\">")
            //                            .Replace("\"><i class=\"", "\"><table border=0 style=\"width:90%; display:inline-table; margin-top:1px\"><tr><td style=\"width:13%\"><i class=\"")
            //                            .Replace("\"></i><span class=\"title\">", "\"></i></td><td style=\"width:75%\"><span class=\"title\">")
            //                            .Replace("</span>", "</span></td></tr></table>")
            //                            .Replace("</span></td></tr></table><span class=\"arrow \"></span></td></tr></table>", "</span></td><td style=\"width:1%\"></td></tr></table><span class=\"arrow \"></span>")
            //                            .Replace("</span></td></tr></table></a></li><li class=\"nav-item \">", "</span></td><td style=\"width:1%\"></td></tr></table></a></li><li class=\"nav-item \">")
            //                            .Replace("</span></td></tr></table><span class=\"arrow open\"></span></td></tr></table><span class=\"selected\"></span></td></tr></table>", "</span></td><td></td></tr></table><span class=\"selected\"></span><span class=\"arrow open\"></span>")
            //                            .Replace("</span></td></tr></table><span class=\"selected\"></span></td><td style=\"width:1%\"></td></tr></table>", "</span></td><td style=\"width:1%\"></td></tr></table><span class=\"selected\"></span>")
            //                            .Replace("</span></td></tr></table></a></li><li class=\"nav-item  active open\">", "</span></td><td style=\"width:1%\"></td></tr></table></a></li><li class=\"nav-item  active open\">")
            //                            .Replace("</span></td></tr></table></a></li></ul></li><li class=\"nav-item \"><a href=\"#\" class=\"nav-link nav-toggle\">", "</span></td><td style=\"width:1%\"></td></tr></table></a></li></ul></li><li class=\"nav-item \"><a href=\"#\" class=\"nav-link nav-toggle\">")
            //                            .Replace("</span></td></tr></table></a></li></ul></li></ul>", "</span></td><td style=\"width:1%\"></td></tr></table></a></li></ul></li></ul>")
            //                            .Replace("</span></td></tr></table><span class=\"selected\"></span></td><td style=\"width:1%\"></td></tr></table></a></li></ul>", "</span></td><td style=\"width:1%\"></td></tr></table><span class=\"selected\"></span></a></li></ul>");

            string htmlsidebarmenunew = (sb.ToString())
                                        .Replace("<span class=\"arrow \"></span>", "<div class=\"arrow\" style=\"display:inline-grid; width:10%; float: right;\"><span></span></div>")
                                        .Replace("<span class=\"title\">&nbsp;&nbsp;", "<div class=\"title\" style=\"display:inline-grid; padding-left:3%; width:79%;\"><span>")
                                        .Replace("</span><div class=\"arrow\"", "</span></div><div class=\"arrow\"")
                                        .Replace("</span></a></li><li class=\"nav-item \">", "</span></div></a></li><li class=\"nav-item \">")
                                        .Replace("</span><span class=\"selected\"></span></div>", "</span></div><span class=\"selected\"></span>")
                                        .Replace("</span></a></li><li class=\"nav-item  active open\">", "</span></div></a></li><li class=\"nav-item  active open\">")
                                        .Replace("<span class=\"arrow open\"></span>", "</div><div class=\"arrow open\" style=\"display:inline-grid; width:10%; float: right;\"><span></span></div>")
                                        .Replace("</span></a></li></ul>", "</span></div></a></li></ul>");
            //.Replace("</span><span class=\"arrow \"></span>", "</span></div><div class=\"arrow\" style=\"display:inline-grid; width:10%; float: right;\"><span></span></div>");

            //string test = (sb.ToString()).Replace("<i class=\"fa fa-table\"></i><span class=\"title\">&nbsp;&nbsp;Master Data</span><span class=\"arrow \"></span>", "<div style=\"display:inline-grid; padding-left:2px; width:auto; background-color:lavender;\"><i class=\"fa fa-table\"></i></div><div class=\"title\" style=\"display:inline-grid; padding-left:3%; width:79%;\"><span>Master DataMaster DataMaster Dat Master Data</span></div><div class=\"arrow\" style=\"display:inline-grid; width:10%; float: right;\"><span></span></div>");
            //return new HtmlString(sb.ToString());
            return new HtmlString(htmlsidebarmenunew);
        }

        private string Render(Menu menu)
        {
            var selected = _selectedMenus.ContainsKey(menu.ID);
            var sb = new StringBuilder();
            var childs = _menus.Where(x => x.ParentID == menu.ID).OrderBy(x => x.OrderIndex);
            var hasChild = childs.Count(x => x.Visible == 1 && _actors.Any(y => _acl.Any(z => z.RoleName == y && z.PermissionName == menu.PermissionName))) > 0;

            if (!_actors.Any(x => _acl.Any(y => y.RoleName == x && y.PermissionName == menu.PermissionName)) || menu.Visible != 1) return sb.ToString();

            var urlHelperFactory = HttpContext.Services.GetRequiredService<IUrlHelperFactory>();
            var actionContext = HttpContext.Services.GetRequiredService<IActionContextAccessor>().ActionContext;
            var urlHelper = urlHelperFactory.GetUrlHelper(actionContext);

            sb.Append(string.Format(@"<li class=""nav-item {0}"">", selected ? " active open" : string.Empty));

            sb.Append(string.Format(@"<a href=""{0}"" class=""nav-link {1}"">", !string.IsNullOrEmpty(menu.Url) && !menu.Url.Contains("#") ? urlHelper.Content(menu.Url) : "#", hasChild ? "nav-toggle" : string.Empty));
            sb.Append(string.Format(@"<i class=""{0}""></i>", menu.IconClass));
            sb.Append(string.Format(@"<span class=""title"">&nbsp;&nbsp;{0}</span>", menu.Title));

            if (hasChild)
                sb.Append(string.Format(@"<span class=""arrow {0}""></span>", selected ? "open" : string.Empty));

            if (selected)
                sb.Append(@"<span class=""selected""></span>");

            sb.Append("</a>");

            if (hasChild)
            {
                sb.Append(@"<ul class=""sub-menu"">");
                foreach (var child in childs)
                {
                    sb.Append(Render(child));
                }
                sb.Append("</ul>");
            }

            sb.Append(@"</li>");

            return sb.ToString();
        }
 
        private void DeepSelect(string uri)
        {
            var menu = _menus.FirstOrDefault(x => ( string.IsNullOrEmpty(x.Url) ? "" : x.Url.Replace("~","")) == uri.Split(new string[] { "?" }, StringSplitOptions.None)[0]);

            if (menu == null) return;

            while (menu != null)
            {
                _selectedMenus.Add(menu.ID, menu);

                menu = _menus.FirstOrDefault(x => x.ID == menu.ParentID);
            }
        }
    }
}
