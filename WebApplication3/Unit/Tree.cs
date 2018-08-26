using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication3.Framework.Models;

namespace WebApplication3.Unit
{
    public class Node
    {

        [JsonProperty(PropertyName = "children")]
        public List<Node> Children { get; set; }

        //[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Node Parent { get; set; }
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "pId")]
        public string ParentId { get; set; }

        public string state { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        public string url { get; set; }

        public int roleId { get; set; }
    }
    public class Tree
    {
        public static IEnumerable<Node> RawCollectionToTree(List<Role> collection)
        {
            var treeDictionary = new Dictionary<string, Node>();
            collection.ForEach(x => treeDictionary.Add(x.MenuId, new Node { Id = x.MenuId, ParentId = x.Pid, Name = x.RoleName ,url=x.Url,roleId=x.RoleId}));

            foreach (var item in treeDictionary.Values)
            {
                if (!string.IsNullOrEmpty(item.ParentId))
                {
                    Node proposedParent;

                    if (treeDictionary.TryGetValue(item.ParentId, out proposedParent))
                    {
                        item.Parent = proposedParent;
                        if (item.Parent.Children == null) {
                            item.Parent.Children = new List<Node>();
                        }
                        proposedParent.Children.Add(item);
                    }
                   
                }

            }
            return treeDictionary.Values.Where(x => x.Parent == null);
        }

    }

}
