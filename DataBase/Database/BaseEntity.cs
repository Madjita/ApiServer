using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace Database.Contexts.Models
{
    public abstract class BaseEntity
    {
        [Key]
        [Column("ID")]
        [JsonProperty(PropertyName = "ID")]
        public virtual int Id { get; set; }
    }
}