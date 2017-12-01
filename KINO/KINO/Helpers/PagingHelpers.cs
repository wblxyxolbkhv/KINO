using KINO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace KINO.Helpers
{
    public static class PagingHelpers
    {
        public static MvcHtmlString PageLinks(this HtmlHelper html,
            PageInfo pageInfo, Func<int, string> pageUrl, bool createButtons)
        {
            StringBuilder result = new StringBuilder();

            if (pageInfo.TotalPages == 1)
                return null;
            if (createButtons)
            {
                for (int i = 1; i <= pageInfo.TotalPages; i++)
                {
                    TagBuilder tag = new TagBuilder("input");
                    tag.MergeAttribute("type", "submit");
                    tag.MergeAttribute("name", "page");
                    tag.MergeAttribute("value", i.ToString());
                    // если текущая страница, то выделяем ее,
                    // например, добавляя класс
                    if (i == pageInfo.PageNumber)
                    {
                        tag.AddCssClass("selected");
                        tag.AddCssClass("btn-primary");
                    }
                    tag.AddCssClass("btn btn-default");
                    result.Append(tag.ToString());
                }
            }
            else
            {
                List<TagBuilder> links = new List<TagBuilder>();
                for (int i = 1; i <= pageInfo.TotalPages; i++)
                {
                    TagBuilder tag = new TagBuilder("a");
                    tag.MergeAttribute("href", pageUrl(i));
                    tag.InnerHtml = i.ToString();
                    tag.MergeAttribute("style", "display:inline-block");
                    // если текущая страница, то выделяем ее,
                    // например, добавляя класс
                    if (i == pageInfo.PageNumber)
                    {
                        tag.AddCssClass("selected");
                        tag.AddCssClass("btn-primary");
                    }
                    tag.AddCssClass("btn btn-default");
                    links.Add(tag);
                }
                int index = links.IndexOf(links.Find(x => x.InnerHtml == pageInfo.PageNumber.ToString()));
                for(int i = 1; i < index-1; i++)
                {
                    links.Remove(links[i]);
                }
                if (index != -1 && index - 2 > 0)
                {
                    TagBuilder spanTag = new TagBuilder("span");
                    spanTag.SetInnerText(" ... ");
                    spanTag.MergeAttribute("style", "font-weight:bold; font-size:20px;");
                    //spanTag.MergeAttribute("onclick", String.Format("expandPages(this,{0}", pageUrl(pageInfo.PageNumber)));

                    links.Insert(index - 2, spanTag);
                }
                index = links.IndexOf(links.Find(x => x.InnerHtml == pageInfo.PageNumber.ToString()));
                for(int i = index + 2; i< links.Count-1; i++)
                {
                    if (index + 2 == links.Count)
                        break;
                    links.Remove(links[i]);
                }
                if (index != -1 && index + 2 < Convert.ToInt32(links.Last().InnerHtml) - 1)
                {
                    TagBuilder spanTag = new TagBuilder("span");
                    spanTag.SetInnerText(" ... ");
                    spanTag.MergeAttribute("style", "font-weight:bold; font-size:20px;");
                    //spanTag.MergeAttribute("onclick", String.Format("expandPages(this,{0}", pageUrl(pageInfo.PageNumber)));

                    links.Insert(index + 2, spanTag);
                }

                foreach(var tag in links)
                {
                    result.Append(tag);
                }
                
            }
            return MvcHtmlString.Create(result.ToString());
        }
    }
}