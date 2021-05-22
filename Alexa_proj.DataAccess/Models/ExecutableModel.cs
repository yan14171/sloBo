using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net.Http;
using System.Threading.Tasks;

namespace Alexa_proj.Data_Control.Models
{
#nullable enable

    public class ExecutableModel
    {
        /// <summary>
        /// Primary key of an Executable table
        /// </summary>
        [Column("executable_id")]
        public int ExecutableModelId { get; set; }

        /// <summary>
        /// Name of an Executable
        /// </summary>
        [MaxLength(40)]
        [Column("executable_name")]
        public string ExecutableName { get; set; }

        /// <summary>
        /// List of keywords, that relate to this executable
        /// </summary>
        [Column("keywords")]
        public List<Keyword> Keywords { get; set; } = new List<Keyword>();

        /// <summary>
        /// A function, that was bound to this executable
        /// </summary>
        [Column("executable_function")]
        public Function ExecutableFunction { get; set; }

        public virtual Task Execute()
        {
            throw new System.NotImplementedException();
        }

        public async virtual Task<T> GetInfo<T> () where T : class
        {
            var client = new HttpClient();
            var response =
                await client.GetAsync(this.ExecutableFunction.FunctionEndpoint);
            T Report = JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync());
            return Report;
        }
    }

#nullable disable
}
