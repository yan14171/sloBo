using System.ComponentModel.DataAnnotations.Schema;

namespace Alexa_proj.Data_Control.Models
{
#nullable enable

    public class FunctionResult
        {
            [Column("result_id")]
            public int FunctionResultId { get; set; }

            /// <summary>
            /// The Function entity, that this result is of
            /// </summary>
            public Function Function { get; set; }

            /// <summary>
            /// The Id of the Function entity, that this result is of
            /// </summary>
            public int FunctionId { get; set; }

            /// <summary>
            /// A static JSON result of a call to the API endpoint, given in the Function Entity, 
            /// that this result relates to 
            /// </summary>
            [Column("result_value")]
            public string? ResultValue { get; set; }
        }

#nullable disable
}
