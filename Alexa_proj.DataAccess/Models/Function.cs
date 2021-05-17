using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Alexa_proj.Data_Control.Models
{
#nullable enable

    public class Function
        {
            [ForeignKey("ExecutableModel")]
            [Column("function_id")]
            public int FunctionId { get; set; }

            /// <summary>
            /// The Executable entity, that this function relates to
            /// </summary>
            public ExecutableModel Executable { get; set; }

            /// <summary>
            /// The Id of the Executable entity, that this function refers to
            /// </summary>
            public int ExecutableId { get; set; }

            /// <summary>
            /// An abstract name of a function
            /// </summary>
            [MaxLength(40)]
            [Column("function_name")]
            public string FunctionName { get; set; }

            /// <summary>
            /// A function's static endpoint 
            /// For Example: api.openweather.api.openweathermap.org/data/2.5/weather?q={city name}&appid={API key}
            /// </summary>
            [MaxLength(250)]
            [Column("function_endpoint")]
            public string? FunctionEndpoint { get; set; }

            /// <summary>
            /// The result of a static API call
            /// </summary>
            [Column("function_result")]
            public FunctionResult? FunctionResult { get; set; }
        }

#nullable disable


}
