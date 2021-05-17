using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Alexa_proj.Data_Control.Models
{
#nullable enable

    public class Keyword
    {
        /// <summary>
        /// Auto-generated Id
        /// </summary>
        [Column("keyword_id")]
        public int KeywordId { get; set; }

        /// <summary>
        /// The Executable entity, that this keyword relates to
        /// </summary>
        public ExecutableModel Executable { get; set; }

        /// <summary>
        /// The Id of an Executable entity, that this keyword refers to
        /// </summary>
        public int ExecutableId { get; set; }

        /// <summary>
        /// Keyword value
        /// </summary>
        [MaxLength(40)]
        [Column("keyword_value")]
        public string KeywordValue { get; set; }

        /// <summary>
        /// Whether the keyword is critical or not
        /// </summary>
        public bool IsCritical { get; set; }


    }

#nullable disable
}

