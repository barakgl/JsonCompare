using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp2
{
    class JsonCmp
    {
        static int IdErrors = 0;
        static int ValueErrors = 0;
        static int MissingElementsErrors = 0;

        public int[] summ()
        {
            int[] res = { IdErrors, ValueErrors, MissingElementsErrors };
            return res;
        }

        public Rootobject DeserializeJson(String json)
        {
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };
            return JsonConvert.DeserializeObject<Rootobject>(json, settings);
        }

        public String SerializeJson(Rootobject json)
        {
            return JsonConvert.SerializeObject(json,Formatting.Indented);
        }

        public Boolean cmp(Rootobject json1, Rootobject json2)
        {         
            return json1.cmp(json2);    
        }

        /// <summary>
        ///  the root element in json file
        /// </summary>
        public class Rootobject
        { 
            /* json structure */
            public Best_Path[] best_path { get; set; }
            public Nodes_List[] nodes_list { get; set; }

            public Boolean cmp(Rootobject other)
            {
                Boolean res = true;
                /* find offset */

                int offset = Math.Abs(this.best_path[0].id - other.best_path[0].id);

                /* loop all nodes in 'best_path' */
                foreach (var tup in this.best_path.Zip(other.best_path, Tuple.Create))
                {
                    res &= tup.Item1.cmp(tup.Item2, offset);
                }

                /* checks if 'best_path' are equal in length */
                if (this.best_path.Length > other.best_path.Length)
                {
                    for (int i = other.best_path.Length; i < this.best_path.Length; i++)
                    {
                        res &= false;
                        this.best_path[i].setAllErrorsTrue();
                    }
                }
                else if (this.best_path.Length < other.best_path.Length)
                {
                    for (int i = this.best_path.Length; i < other.best_path.Length; i++)
                    {
                        res &= false;
                        other.best_path[i].setAllErrorsTrue();
                    }
                }

                /* loop all nodes in 'nodes_list' */
                foreach (var tup in this.nodes_list.Zip(other.nodes_list, Tuple.Create))
                {
                    res &= tup.Item1.cmp(tup.Item2, offset);
                }

                /* checks if 'nodes_list' are equal in length */
                if (this.nodes_list.Length > other.nodes_list.Length)
                {
                    for (int i = other.nodes_list.Length; i < this.nodes_list.Length; i++)
                    {
                        res &= false;
                        this.nodes_list[i].setAllErrorsTrue();
                    }
                }
                else if (this.nodes_list.Length < other.nodes_list.Length)
                {
                    for (int i = this.nodes_list.Length; i < other.nodes_list.Length; i++)
                    {
                        res &= false;
                        other.nodes_list[i].setAllErrorsTrue();
                    }
                }
                return res;
            }
        }

        public class Best_Path
        {
            /* json structure */
            public int id { get; set; }
            public int index { get; set; }

            /* error array*/
            Boolean[] error = new Boolean[2];
            public Boolean[] getErrors() { return this.error; }

            public void setAllErrorsTrue()
            {
                this.error[0] = true;
                this.error[1] = true;
                MissingElementsErrors++;
            }

            public Boolean cmp(Best_Path other, int offset = 0)
            {
                Boolean res = true;

                /* compare id */
                if (Math.Abs(this.id - other.id) != offset)
                {
                    res = false;
                    this.error[0] = true;
                    other.error[0] = true;  
                    IdErrors++;
                }

                /* compare index */
                if (this.index != other.index)
                {
                    res = false;
                    this.error[1] = true;
                    other.error[1] = true;
                    ValueErrors++;
                }
                return res;
            }
        }

        public class Nodes_List
        {
            /* json structure */
            public int id { get; set; }
            public int index { get; set; }
            public Edge[] edges { get; set; }

            /* error array*/
            Boolean[] error = new Boolean[2];

            public Boolean[] getErrors() { return this.error; }

            public void setAllErrorsTrue()
            {
                this.error[0] = true;
                this.error[1] = true;
                foreach (var edge in this.edges)
                    edge.setAllErrorsTrue();
                MissingElementsErrors++;
            }

            public Boolean cmp(Nodes_List other, int offset = 0)
            {
                Boolean res = true;

                /* compare id */
                if (Math.Abs(this.id - other.id) != offset)
                {
                    res = false;
                    this.error[0] = true;
                    other.error[0] = true;
                    IdErrors++;
                }

                /* compare index */
                if (this.index != other.index)
                {
                    res = false;
                    this.error[1] = true;
                    other.error[1] = true;
                    ValueErrors++;
                }

                /* loop all Edges in 'nodes_list' */
                foreach (var tup in this.edges.Zip(other.edges, Tuple.Create))
                {
                    res &= tup.Item1.cmp(tup.Item2, offset);
                }

                /* checks if 'nodes_list' are equal in length */
                if (this.edges.Length > other.edges.Length)
                {
                    for (int i = other.edges.Length; i < this.edges.Length; i++)
                    {
                        res &= false;
                        this.edges[i].setAllErrorsTrue();
                    }
                }
                else if (this.edges.Length < other.edges.Length)
                {
                    for (int i = this.edges.Length; i < other.edges.Length; i++)
                    {
                        res &= false;
                        other.edges[i].setAllErrorsTrue();
                    }
                }

                return res;
            }
        }

        public class Edge
        {
            /* json structure */
            public int[] edge_nodes;
            public float weight_1;
            public float weight_2;

            /* error array*/
            Boolean[] error = new Boolean[4];

            public Boolean[] getErrors() { return this.error; }

            public void setAllErrorsTrue()
            {
                this.error[0] = true;
                this.error[1] = true;
                this.error[2] = true;
                this.error[3] = true;
                MissingElementsErrors++;      
            }

            public Boolean cmp(Edge other, int offset = 0)
            {
                Boolean res = true;

                if (Math.Abs(this.edge_nodes[0] - other.edge_nodes[0]) != 0)
                {
                    res = false;
                    this.error[0] = true;
                    other.error[0] = true;  
                    IdErrors++;
                }

                if (Math.Abs(this.edge_nodes[1] - other.edge_nodes[1]) != 0)
                {
                    res = false;
                    this.error[1] = true;
                    other.error[1] = true;
                    ValueErrors++;
                }

                if (this.weight_1 != other.weight_1)
                {
                    res = false;
                    this.error[2] = true;
                    other.error[2] = true;
                    ValueErrors++;
                }

                if (this.weight_2 != other.weight_2)
                {
                    res = false;
                    this.error[3] = true;
                    other.error[3] = true;
                    ValueErrors++;
                }
                return res;
            }
        }
    }
}

